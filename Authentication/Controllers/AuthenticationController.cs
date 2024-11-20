using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Authentication.Models.Data;
using Authentication.Helper;
using System.Web;

namespace Authentication.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationDbContext _db = new AuthenticationDbContext();

        public ActionResult PatientSignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PatientSignIn(string username, string password)
        {
            string hashedPassword = MD5Helper.Hash(password);

            var patientAccount = _db.PATIENTACCOUNTs.SingleOrDefault(x => x.PATIENT_USERNAME == username && x.PATIENT_PASSWORD == hashedPassword);

            if (patientAccount != null)
            {
                // Mã hóa PatientId và mã hóa URL
                string encryptedPatientId = patientAccount.PATIENT_ID.ToString();

                // Sinh RequestVerificationToken
                string requestVerificationToken = GenerateRequestVerificationToken();

                // URL chuyển hướng
                string redirectUrl = $"https://localhost:44327/Authentication/AuthenticationRequest?patientid={encryptedPatientId}&token={requestVerificationToken}";

                // Chuyển hướng
                return Redirect(redirectUrl);
            }

            ViewBag.Error = "Tên đăng nhập và mật khẩu không chính xác";
            return View();
        }


        public ActionResult PatientSignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PatientSignUp(string username, string password, int patientId)
        {
            if (_db.PATIENTACCOUNTs.Any(p => p.PATIENT_USERNAME == username))
            {
                ViewBag.Error = "Username already exists.";
                return View();
            }

            var newAccount = new PATIENTACCOUNT
            {
                PATIENT_ID = patientId,
                PATIENT_USERNAME = username,
                PATIENT_PASSWORD = MD5Helper.Hash(password)
            };

            _db.PATIENTACCOUNTs.Add(newAccount);
            _db.SaveChanges();
            return RedirectToAction("PatientSignIn");
        }

        private string Encrypt(string plainText)
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

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        private string GenerateRequestVerificationToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }
    }
}
