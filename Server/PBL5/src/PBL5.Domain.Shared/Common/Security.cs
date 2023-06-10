using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PBL5.Common
{
    public class Security
    {
        public static string GetMD5(string input)
        {
            string str_md5 = "";
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(input);
            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            mang = my_md5.ComputeHash(mang);
            foreach (byte b in mang)
            {
                str_md5 += b.ToString("x2");
            }
            return str_md5;
        }
        public static string GenerateRandomPassword()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            string password = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            
            return password;
        }
    }
}