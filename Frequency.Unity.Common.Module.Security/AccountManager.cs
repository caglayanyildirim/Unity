using System;
using Frequency.Framework;
using Frequency.Unity.Common.Module.Security.Service;

namespace Frequency.Unity.Common.Module.Security
{
    public class AccountManager : IAccountManagerService
    {
        #region Constructor

        private readonly IModuleContext context;

        public AccountManager(IModuleContext context)
        {
            this.context = context;
        }

        #endregion

        #region Web Service Mappings

        #region Common
        IAccountInfo IAccountManagerService.CreateAccount(string displayName, Email email)
        {
            return context.New<Account>().With(displayName, email);
        }
        #endregion

        #endregion
    }
}