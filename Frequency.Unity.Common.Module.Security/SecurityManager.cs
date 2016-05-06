using System;
using Frequency.Framework;
using Frequency.Framework.Logging;
using Frequency.Framework.Security;
using Frequency.Unity.Common.Module.Security.Enums;
using Frequency.Unity.Common.Module.SharedData;

namespace Frequency.Unity.Common.Module.Security
{
	public class SecurityManager :IAuditManager, ISessionManager
	{
        #region Constructor

        private readonly IModuleContext context;
		private readonly CommonDataManager commonDataManager;
        private readonly ILogger logger;

		public SecurityManager(IModuleContext context, CommonDataManager commonDataManager)
		{
			this.context = context;
			this.commonDataManager = commonDataManager;

            logger = context.Logger.Get<SecurityManager>();
		}

        #endregion

        public Account Register(string displayName, Email email)
		{
		    if (displayName == null) { throw new SecurityException.DisplayNameCannotBeEmpty(); }
            if (email.IsDefault()) { throw new SecurityException.EmailCannotBeEmpty(); }

			return context.New<Account>().With(displayName, email);
		}
        public void CreateSecurityCode(Email email)
        {
            if (email.IsDefault()) { throw new SecurityException.EmailCannotBeEmpty(); }

            var account = CheckAccount(email);

            if (account != null)
            {
                var expireSetting = commonDataManager.GetSecurityCodeExpireMinute();
                var securityCode = context.New<SecurityCode>().With(account, TimeSpan.FromMinutes(expireSetting));
                
            }
            else
            {
                
            }
        }

        public void VerifySecurityCode(Email email, string securityCode)
        {
            if (email.IsDefault()) { throw new SecurityException.EmailCannotBeEmpty(); }
            if (securityCode.IsNullOrEmpty()) { throw new SecurityException.SecurityCodeCannotBeEmpty(); }

            var account = CheckAccount(email);
            if (account == null)
            {
                throw new SecurityException.SecurityCodeOrAccountNotFound();
            }

            account.VerifySecurityCode(securityCode);
        }

        public void ChangePassword(string oldPassword, string newPassword)
		{
		    var account = context.GetCurrentAccountToken().Account;
		    if (account.Status == AccountStatus.Blocked)
		    {
		        throw new SecurityException.AccountIsBlocked();
		    }
                
            account.ChangePassword(oldPassword, newPassword);
		}

        public AccountToken SavePassword(Email email, string securityCode, string password)
        {
            if (email.IsDefault()) { throw new SecurityException.EmailCannotBeEmpty(); }
            if (securityCode.IsNullOrEmpty()) { throw new SecurityException.SecurityCodeCannotBeEmpty(); }
            if (password.IsNullOrEmpty()) { throw new SecurityException.PasswordCannotBeEmpty(); }

            var account = CheckAccount(email);
            if (account == null)
            {
                throw new SecurityException.SecurityCodeOrAccountNotFound();
            }

            if (account.Status == AccountStatus.Passive)
            {

                
            }

            account.Activate(securityCode, password);

            return account.CreateToken();
        }


        public AccountToken Login(Email email, string password)
		{
			if (email.IsDefault()) { throw new SecurityException.EmailCannotBeEmpty(); }
			if (password.IsNullOrEmpty()) { throw new SecurityException.PasswordCannotBeEmpty(); }

			var account = context.Query<Accounts>().SingleBy(email, password);
			if (account == null)
			{
				throw new SecurityException.AccountNotFound();
			}

			return account.CreateToken();
		}

		public AccountToken LoginByToken(AppToken appToken)
		{
			var accountToken = context.Query<AccountTokens>().SingleByToken(appToken);
			if (accountToken == null)
			{
				throw new SecurityException.TokenNotFound();
			}

			accountToken.MarkAsPassive();
			return accountToken.Account.CreateToken();
		}

		public void Logout()
		{
			context.GetCurrentAccountToken().MarkAsPassive();
		}

		private Account CheckAccount(Email email)
		{
			var account = context.Query<Accounts>().SingleBy(email);
			if (account != null)
			{
				if (account.Status == AccountStatus.Blocked)
				{
					throw new SecurityException.AccountIsBlocked();
				}
			}

			return account;
		}

		private ISession GetSession(AppToken token)
		{
			var accountToken = context.Query<AccountTokens>().SingleByToken(token);
			if (accountToken != null)
			{
				return accountToken;
			}

			return null;
		}

		#region Web Service Mappings

		

		#endregion

		#region Api Mappings

		#region Security

		void IAuditManager.SaveRequest(Guid requestId, AppToken appToken, string serviceInstance, string serviceName, string serviceHeaders, string serviceParameters) { }
		void IAuditManager.SaveSuccessfulResponse(Guid requestId, string serviceResponse) { }
		void IAuditManager.SaveFailedResponse(Guid requestId, Exception exception) { }
		int IAuditManager.GetDailyRequestCount(AppToken appToken, string serviceInstance, string serviceName, Date date) { return 0; }
		int IAuditManager.GetMaxDailyRequestCount(AppToken appToken, string serviceInstance, string serviceName) { return 1; }

		ISession ISessionManager.GetSession(AppToken appToken) { return GetSession(appToken); }

		#endregion

		#endregion
	}

	public static class ModuleContextExtensions
	{
		public static AccountToken GetCurrentAccountToken(this IModuleContext source)
		{
			AccountToken token = source.Session as AccountToken;
			if (token != null)
			{
				return token;
			}

			throw new SecurityException.SessionIsNotValid();
		}
	}
}