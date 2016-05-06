using System;
using Frequency.Framework.Service;
using Frequency.Unity.Common.Module.SharedData;

namespace Frequency.Unity.Common.Module.Security
{
    public static class SecurityException
    {
        public class GivenPasswordDoesNotMatchWithAccountPassword : ServiceException
        {
            public GivenPasswordDoesNotMatchWithAccountPassword()
                : base(ResultCodes.Security.Err(0))
            { }
        }

        public class AccountNotFound : ServiceException
        {
            public AccountNotFound()
                : base(ResultCodes.Security.Err(1))
            { }
        }

        public class DuplicateEmailFound : ServiceException
        {
            public DuplicateEmailFound(Email email)
                : base(ResultCodes.Security.Err(2), email)
            { }
        }

        public class AccountIsPassive : ServiceException
        {
            public AccountIsPassive()
                : base(ResultCodes.Security.Err(3))
            { }
        }

        public class AccountIsBlocked : ServiceException
        {
            public AccountIsBlocked()
                : base(ResultCodes.Security.Err(4))
            { }
        }

        public class SecurityCodeOrAccountNotFound : ServiceException
        {
            public SecurityCodeOrAccountNotFound()
                : base(ResultCodes.Security.Err(5))
            { }
        }

        public class AccountTokenExpired : ServiceException
        {
            public AccountTokenExpired()
                : base(ResultCodes.Security.Err(6))
            { }
        }

        public class PasswordCannotBeEmpty : ServiceException
        {
            public PasswordCannotBeEmpty()
                : base(ResultCodes.Security.Err(7))
            { }
        }

        public class PasswordIsNotValid : ServiceException
        {
            public PasswordIsNotValid()
                : base(ResultCodes.Security.Err(8))
            { }
        }

        public class TokenNotFound : ServiceException
        {
            public TokenNotFound()
                : base(ResultCodes.Security.Err(9))
            { }
        }

        public class PasswordsCannotBeTheSame : ServiceException
        {
            public PasswordsCannotBeTheSame()
                : base(ResultCodes.Security.Err(10))
            { }
        }

        public class EmailCannotBeEmpty : ServiceException
        {
            public EmailCannotBeEmpty()
                : base(ResultCodes.Security.Err(11))
            { }
        }

        public class SecurityCodeCannotBeEmpty : ServiceException
        {
            public SecurityCodeCannotBeEmpty()
                : base(ResultCodes.Security.Err(12))
            { }
        }

        public class RequiredParameterIsMissing : ServiceException
        {
            public RequiredParameterIsMissing()
                : base(ResultCodes.Security.Err(13))
            { }
        }

        public class DisplayNameCannotBeEmpty : ServiceException
        {
            public DisplayNameCannotBeEmpty()
                : base(ResultCodes.Security.Err(14))
            { }
        }

        public class ServiceApplicationCannotBeEmpty : ServiceException
        {
            public ServiceApplicationCannotBeEmpty()
                : base(ResultCodes.Security.Err(15))
            { }
        }
        public class SessionIsNotValid : ServiceException
        {
            public SessionIsNotValid()
                : base(ResultCodes.Security.Err(16))
            { }
        }
        public class DuplicateNameFound : ServiceException
        {
            public DuplicateNameFound(string name)
                : base(ResultCodes.Security.Err(17), name)
            { }
        }
    }
}