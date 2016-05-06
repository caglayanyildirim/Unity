using System;
using System.Collections.Generic;
using Frequency.Framework;
using Frequency.Framework.DataAccess;
using Frequency.Unity.Common.Module.Security;

namespace Frequency.Unity.Common.Module.EventManagement
{
    public class Event : IAuditable
    {
        #region Constructor

        private readonly IRepository<Event> repository;
        private readonly IModuleContext context;

        protected Event() { }
        public Event(IRepository<Event> repository, IModuleContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        #endregion

        #region Properties

        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual int Size { get; protected set; }
        public virtual int TargetCount { get; protected set; }
        public virtual int CurrentCount { get; protected set; }
        public virtual Date StartDate { get; protected set; }
        public virtual Date EndDate { get; protected set; }
        public virtual bool IsPrivate { get; protected set; }
        public virtual bool IsActive { get; protected set; }
        public virtual bool IsDeleted { get; protected set; }
        public virtual AuditInfo AuditInfo { get; protected set; }
        public virtual bool IsCompleted { get { return TargetCount == CurrentCount; } }

        #endregion

        protected internal virtual Event With(string name, string description, List<NewEventAccount> eventAccounts, int target, Date startDate, Date endDate,bool isPrivate)
        {
            Name = name;
            Description = description;
            Size = eventAccounts.Count;
            TargetCount = target;
            StartDate = startDate;
            EndDate = endDate;
            IsPrivate = isPrivate;

            repository.Insert(this);

            foreach (var eventAccount in eventAccounts)
            {
                context.New<EventAccount>().With(this, eventAccount);
            }

            return this;
        }

        protected internal virtual void AddEventAccount(NewEventAccount eventAccount)
        {
            context.New<EventAccount>().With(this, eventAccount);
        }

        protected internal virtual void MarkAsPrivate()
        {
            IsPrivate = true;
        }

        protected internal virtual void MarkAsPublic()
        {
            IsPrivate = false;
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

    public class Events : Query<Event>
    {
        public Events(IModuleContext context)
        : base(context)
        { }
    }
}
