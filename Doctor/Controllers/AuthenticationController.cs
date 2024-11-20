using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Doctor.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AuthenticationRequest(string employeeid, string token)
        {
            if (string.IsNullOrEmpty(employeeid) || string.IsNullOrEmpty(token))
            {
                return Content("Thông tin đăng nhập không hợp lệ.");
            }

            //// Decode URL để khôi phục dấu '+'
            //string decodedPatientId = HttpUtility.UrlDecode(patientid);

            //// Giải mã chuỗi đã decode
            //string decryptedPatientId = Decrypt(decodedPatientId);

            //if (string.IsNullOrEmpty(decryptedPatientId))
            //{
            //    return Content("Giải mã thất bại.");
            //}

            ViewBag.EmployeeId = employeeid;
            ViewBag.Token = token;

            Session["EmployeeId"] = employeeid;
            return View();
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
            catch
            {
                return null;
            }
        }
    }
}
