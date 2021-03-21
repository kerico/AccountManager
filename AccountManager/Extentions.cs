using System;
using System.Text;

namespace AccountManager
{
    public static class Extentions
    {
        public static string Encrypt(this string value)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return value;
            }
        }
        public static string Decrypt(this string value)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(value);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return value;
            }
        }
    }
}
