﻿using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class AuthenticationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AuthenticationRequest(string employeeid, string token)
        {
            try
            {
                // Kiểm tra input
                if (string.IsNullOrEmpty(employeeid) || string.IsNullOrEmpty(token))
                {
                    return View();
                }

                // Giải mã Base64 URL-safe
                string decodedEmployeeId = DecodeBase64UrlSafe(employeeid);
                string decryptedEmployeeId = Decrypt(decodedEmployeeId);

                // Giải mã token
                string decodedToken = DecodeBase64UrlSafe(token);

                // Kiểm tra giải mã
                if (string.IsNullOrEmpty(decryptedEmployeeId))
                {
                    return Content("Giải mã thất bại.");
                }

                // Lưu thông tin
                Session["EmployeeId"] = decryptedEmployeeId;
                ViewBag.Token = decodedToken;

                return View();
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return Content($"Lỗi xác thực: {ex.Message}");
            }
        }

        private string Decrypt(string cipherText)
        {
            try
            {
                const string keyBase64 = "qSLCtgCSolEzY5VVMyyZWy90qET4huVXG9XBLRSa10s=";
                const string ivBase64 = "jBEQDaG7vfNC4enrNFveaQ==";
                byte[] key = Convert.FromBase64String(keyBase64);
                byte[] iv = Convert.FromBase64String(ivBase64);

                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var decryptor = aes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipherText);
                        byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return Encoding.UTF8.GetString(plainBytes);
                    }
                }
            }
            catch (Exception)
            {
                // Ghi log lỗi chi tiết nếu cần
                return null;
            }
        }

        // Giải mã Base64 URL-safe
        private string DecodeBase64UrlSafe(string input)
        {
            // Thêm padding nếu cần
            input = input.PadRight(input.Length + (4 - input.Length % 4) % 4, '=')
                .Replace('-', '+')
                .Replace('_', '/');

            byte[] decodedBytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(decodedBytes);
        }
    }
}