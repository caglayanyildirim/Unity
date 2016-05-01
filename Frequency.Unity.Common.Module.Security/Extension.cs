using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Frequency.Unity.Common.Module.Security
{
    public static class Extension
    {
        public static string EncryptPassword(this string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new SecurityException.PasswordCannotBeEmpty();
            }

            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            var result = md5.Hash;
            var strBuilder = new StringBuilder();
            foreach (var t in result)
            {
                strBuilder.Append(t.ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static bool PasswordIsValid(this string password)
        {
            if (!string.IsNullOrEmpty(password) && password.Length >= 8 && password.Any(char.IsLetter) && password.Any(char.IsDigit))
            {
                return true;
            }

            throw new SecurityException.PasswordIsNotValid();
        }
    }
}