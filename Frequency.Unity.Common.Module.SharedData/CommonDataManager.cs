using System;
using Frequency.Framework;

namespace Frequency.Unity.Common.Module.SharedData
{
    public class CommonDataManager
    {
        private readonly IModuleContext context;

        public CommonDataManager(IModuleContext context)
        {
            this.context = context;
        }
        
        [Internal]
        public int GetAccountTokenExpireMinute()
        {
            return context.Settings.Get<int>("GetAccountTokenExpireMinute");
        }
    }
}