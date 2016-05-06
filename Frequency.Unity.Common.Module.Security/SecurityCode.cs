using System;
using Frequency.Framework;
using Frequency.Framework.DataAccess;
using Frequency.Unity.Common.Module.Security.Enums;
using Frequency.Unity.Common.Module.SharedData;

namespace Frequency.Unity.Common.Module.Security
{
    public class SecurityCode : IAuditable
	{
        #region Constructor

        private readonly IRepository<SecurityCode> repository;
		private readonly IModuleContext context;
		private readonly IRandomSequenceService sequenceService;

		protected SecurityCode() { }

		public SecurityCode(IRepository<SecurityCode> repository, IModuleContext context, IRandomSequenceService sequenceService)
		{
			this.repository = repository;
			this.context = context;
			this.sequenceService = sequenceService;
		}

        #endregion

        #region Properties

        public virtual int Id { get; protected set; }
		public virtual Account Account { get; protected set; }
		public virtual string Code { get; protected set; }
		public virtual SecurityCodeStatus Status { get; protected set; }
		public virtual DateTime ExpireDate { get; protected set; }
		public virtual AuditInfo AuditInfo { get; protected set; }

        #endregion

        protected internal virtual SecurityCode With(Account account, TimeSpan lifetime) { return With(account, context.System.Now.Add(lifetime)); }
		protected internal virtual SecurityCode With(Account account, DateTime expireDate)
		{
			Account = account;
			Code = sequenceService.GetSequence(sequenceService.Numbers, 6);
			Status = SecurityCodeStatus.Unused;
			ExpireDate = expireDate;

			repository.Insert(this);

			return this;
		}

		protected internal virtual void MarkAsUsed()
		{
			Status = SecurityCodeStatus.Used;
		}
	}

	public class SecurityCodes : Query<SecurityCode>
	{
		private new readonly IModuleContext context;

		public SecurityCodes(IModuleContext context)
			: base(context)
		{
			this.context = context;
		}

		internal SecurityCode SingleBy(Account account, string code)
		{
			return SingleBy(
				s => s.Account == account &&
				s.Code == code && s.ExpireDate > context.System.Now &&
				s.Status == SecurityCodeStatus.Unused);
		}
	}
}