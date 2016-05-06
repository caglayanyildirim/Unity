using System;
using System.Collections.Generic;
using Frequency.Framework;
using Frequency.Framework.DataAccess;
using Frequency.Framework.Security;
using Frequency.Unity.Common.Module.Security.Enums;
using Frequency.Unity.Common.Module.SharedData;

namespace Frequency.Unity.Common.Module.Security
{
	public class Account : IAuditable, IAccount
	{
	    const int MAX_DISPLAY_NAME_LENGHT = 20;
	    const int MIN_DISPLAY_NAME_LENGHT = 3;
        #region Constructor

        private readonly IRepository<Account> repository;
		private readonly IModuleContext context;
		private readonly CommonDataManager commonDataManager;

		protected Account() { }

		public Account(IRepository<Account> repository, IModuleContext context, CommonDataManager commonDataManager)
		{
			this.repository = repository;
			this.context = context;
			this.commonDataManager = commonDataManager;
		}

        #endregion

        #region Properties

        public virtual int Id { get; protected set; }
		public virtual string DisplayName { get; protected set; }
		public virtual Email Email { get; protected set; }
		public virtual string Password { get; protected set; }
		public virtual AccountStatus Status { get; protected set; }
		public virtual AuditInfo AuditInfo { get; protected set; }

        #endregion

        protected internal virtual Account With(string displayName, Email email)
		{
			if (context.Query<Accounts>().CountBy(email) > 0)
			{
				throw new SecurityException.DuplicateEmailFound(email);
			}
            var dName = displayName.Trim();
		    if (dName.IndexOf(" ", StringComparison.Ordinal)!=-1 || dName.Length > MAX_DISPLAY_NAME_LENGHT || dName.Length < MIN_DISPLAY_NAME_LENGHT)
		    {
                throw new SharedDataException.InvalidData("displayName", displayName);
            }

            if (context.Query<Accounts>().CountBy(displayName) > 0)
			{
				throw new SecurityException.DuplicateNameFound(displayName);
			}

			DisplayName = displayName;
			Email = email;
			Password = string.Empty;
			Status = AccountStatus.Passive;

			repository.Insert(this);

			return this;
		}

		public virtual List<AccountToken> GetTokens()
		{
			return context.Query<AccountTokens>().ByAccount(this);
		}

		protected internal virtual void ChangePassword(string oldPassword, string newPassword)
		{
			if (string.IsNullOrEmpty(oldPassword))
			{
				throw new SharedDataException.RequiredParameterIsMissing("oldPassword");
			}

			if (string.IsNullOrEmpty(newPassword))
			{
				throw new SharedDataException.RequiredParameterIsMissing("newPassword");
			}

			if (oldPassword.EncryptPassword() == newPassword.EncryptPassword())
			{
				throw new SecurityException.PasswordsCannotBeTheSame();
			}

			if (oldPassword.EncryptPassword() != Password)
			{
				throw new SecurityException.GivenPasswordDoesNotMatchWithAccountPassword();
			}

			SetPassword(newPassword);

			foreach (var accountToken in GetTokens())
			{
				if (accountToken == context.Session) { continue; }

				accountToken.MarkAsPassive();
			}
		}

		private void SetPassword(string password)
		{
			if (password.PasswordIsValid())
			{
				Password = password.EncryptPassword();
			}
		}
        public virtual void Activate(string securityCode, string password)
        {
            var secCode = GetSecurityCode(securityCode);
            secCode.MarkAsUsed();

            SetPassword(password);
            ChangeStatus(AccountStatus.Active);
        }

        protected internal virtual AccountToken CreateToken()
		{
			switch (Status)
			{
				case AccountStatus.Passive:
					throw new SecurityException.AccountIsPassive();
				case AccountStatus.Blocked:
					throw new SecurityException.AccountIsBlocked();
			}

			return context.New<AccountToken>().With(this, context.System.Now.AddMinutes(commonDataManager.GetAccountTokenExpireMinute()));
		}
        protected internal virtual void VerifySecurityCode(string securityCode)
        {
            GetSecurityCode(securityCode);
        }

        private SecurityCode GetSecurityCode(string securityCode)
        {
            var secCode = context.Query<SecurityCodes>().SingleBy(this, securityCode);
            if (secCode == null)
            {
                throw new SecurityException.SecurityCodeOrAccountNotFound();
            }

            return secCode;
        }

        private void ChangeStatus(AccountStatus status)
		{
			Status = status;
		}

		public override string ToString()
		{
			return DisplayName;
		}

		#region Api Mappings

		#region Security

		bool IAccount.HasAccess(IResource resource) { return true; }

		#endregion

		#endregion
	}

	public class Accounts : Query<Account>
	{
		public Accounts(IModuleContext context)
			: base(context) { }

		internal Account SingleBy(Email email)
		{
			return SingleBy(a => a.Email == email);
		}

		internal int CountBy(Email email)
		{
			return CountBy(a => a.Email == email);
		}
		internal int CountBy(string displayName)
		{
			return CountBy(a => a.DisplayName == displayName);
		}

		internal Account SingleBy(Email email, string password)
		{
			return SingleBy(a => a.Email == email && a.Password == password.EncryptPassword());
		}
	}
}