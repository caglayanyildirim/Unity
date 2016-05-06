using System;

namespace Frequency.Unity.Common.Module.Security.Service
{
    public interface IAccountManagerService
    {
        IAccountInfo CreateAccount(string displayName, Email email);
    }

    public interface IAccountInfo
    {
        int Id { get; }
    }
}