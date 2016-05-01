using System;
using System.Collections.Generic;
using Frequency.Framework;
using Frequency.Framework.DataAccess;
using Frequency.Framework.Security;

namespace Frequency.Unity.Common.Module.Security
{
	public class AccountToken : IAuditable, ISession
    {
		private readonly IRepository<AccountToken> repository;

		private readonly IModuleContext context;

		protected AccountToken() { }
		public AccountToken(IRepository<AccountToken> repository, IModuleContext context)
		{
			this.repository = repository;
			this.context = context;
		}

		public virtual int Id { get; protected set; }
		public virtual Account Account { get; protected set; }
		public virtual AppToken Token { get; protected set; }
		public virtual bool IsActive { get; protected set; }
		public virtual DateTime ExpireDateTime { get; protected set; }
		public virtual AuditInfo AuditInfo { get; protected set; }

		protected internal virtual AccountToken With(Account account, DateTime expireDate)
		{
			Account = account;
			Token = context.System.NewAppToken();
			IsActive = true;
			ExpireDateTime = expireDate;

			repository.Insert(this);

			return this;
		}

		protected internal virtual void MarkAsPassive()
		{
			IsActive = false;
		}

		private void Validate()
		{
			if (ExpireDateTime < context.System.Now)
			{
				throw new SecurityException.AccountTokenExpired();
			}

			repository.ForceUpdate(this);
		}

		#region Api Mappings

		#region Security

		IAccount ISession.Account { get { return Account; } }
		
		string ISession.Host
		{
			get
			{
				 
				return string.Format("{0} @ {1}", Account.DisplayName, 
					context.Request.Host.Address //todo: login sirasinda request.host gonderilmeli ve bu tabloda kaydedilmeli
				);
			}
		}

		void ISession.Validate() { Validate(); }

		#endregion

		#endregion

		#region Web Service Mappings
        

		#endregion
	}

	public class AccountTokens : Query<AccountToken>
	{
		public AccountTokens(IModuleContext context)
			: base(context) { }

		internal AccountToken SingleByToken(AppToken token)
		{
			return SingleBy(at => at.Token == token && at.IsActive);
		}

		internal List<AccountToken> ByAccount(Account account)
		{
			return By(at => at.Account == account && at.IsActive);
		}
	}
}