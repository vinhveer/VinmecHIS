using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Authentication.Helper
{
    public class MD5Helper
    {
        public static string Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                // Chuyển chuỗi thành byte array
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Hash dữ liệu
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển byte array thành chuỗi hexadecimal
                StringBuilder sb = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}