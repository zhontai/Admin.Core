using System.Text;
using System.Security.Cryptography;

namespace Admin.Core.Common.Helpers
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public class MD5Encrypt
    {
        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Encrypt16(string password)
        {
            if (password.IsNull())
                return null;

            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(Encoding.UTF8.GetBytes(password)).ToHex();
            }
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Encrypt32(string password = "")
        {
            if (password.IsNull())
                return null;

            using (var md5 = MD5.Create())
            {
                string pwd = string.Empty;
                byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                foreach (var item in s)
                {
                    pwd = string.Concat(pwd, item.ToString("X"));
                }
                return pwd;
            }
        }

        /// <summary>
        /// 64位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Encrypt64(string password)
        {
            if (password.IsNull())
                return null;

            using(var md5 = MD5.Create())
            {
                byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                return s.ToBase64();
            }
        }
    }
}
