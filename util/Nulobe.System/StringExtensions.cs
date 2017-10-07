using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
        public static string Base64Encode(this string str)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Capitalize(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException();
            }

            return str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length - 1);
        }

        public static string Decapitalize(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException();
            }

            return str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1);
        }
    }
}
