using Frequency.Framework;
using Frequency.Unity.Common.Module.Security;
using Frequency.Unity.Common.Module.SharedData;

namespace Frequency.Unity.Common.Module.EventManagement
{
    public class EventManager
    {
        #region Constructor

        private readonly IModuleContext context;

        public EventManager(IModuleContext context)
        {
            this.context = context;
        }
        
        #endregion
    }

    public struct NewEventAccount
    {
        internal Account Account { get; private set; }
        internal int TargetCount { get; private set; }

        public NewEventAccount(Account account, int targetCount)
        {
            if (targetCount == default(int)) { throw new SharedDataException.ValueShouldBeGreaterThan("targetCount",0); }
            if (account == default(Account)) { throw new SharedDataException.RequiredParameterIsMissing("account"); }
            Account = account;
            TargetCount = targetCount;
        }
    }
}