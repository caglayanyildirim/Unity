using Frequency.Framework;
using Frequency.Framework.DataAccess;
using Frequency.Unity.Common.Module.Security;

namespace Frequency.Unity.Common.Module.EventManagement
{
    public class EventAccount : IAuditable
    {
        #region Constructor

        private readonly IRepository<EventAccount> repository;
        private readonly IModuleContext context;

        protected EventAccount() { }
        public EventAccount(IRepository<EventAccount> repository, IModuleContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        #endregion

        #region Properties

        public virtual int Id { get; protected set; }
        public virtual Event Event { get; protected set; }
        public virtual Account Account { get; protected set; }
        public virtual int TargetCount { get; protected set; }
        public virtual int CurrentCount { get; protected set; }
        public virtual bool IsActive { get; protected set; }
        public virtual bool IsDeleted { get; protected set; }
        public virtual AuditInfo AuditInfo { get; protected set; }
        public virtual bool IsCompleted { get { return TargetCount == CurrentCount; } }

        #endregion

        protected internal virtual EventAccount With(Event @event, NewEventAccount eventAccount)
        {
            Event = @event;
            Account = eventAccount.Account;
            TargetCount = eventAccount.TargetCount;

            repository.Insert(this);

            return this;
        }

        protected internal int IncreaseCurrentCount()
        {
            return CurrentCount++;
        }

        protected internal void MarkAsPassive()
        {
            IsActive = false;
        }

        protected internal void MarkAsActive()
        {
            IsActive = false;
        }

        protected internal void Delete()
        {
            IsDeleted = true;
        }
    }

    public class EventAccounts : Query<EventAccount>
    {
        public EventAccounts(IModuleContext context)
        : base(context)
        { }
    }
}
