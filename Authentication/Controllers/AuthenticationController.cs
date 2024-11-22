using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Authentication.Models.Data;
using Authentication.Helper;
using System.Diagnostics;
using System.Web;
using System.Collections.Generic;

namespace Authentication.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationDbContext _db = new AuthenticationDbContext();

        // Từ điển ánh xạ vai trò và port
        private static readonly Dictionary<string, string> RolePorts = new Dictionary<string, string>
        {
            { "Doctor", "44386" },
            { "Pharmacist", "44394" },
            { "Receptionist", "44316" },
            { "Admin", "44306" }
        };

        public ActionResult PatientSignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PatientSignIn(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            string hashedPassword = MD5Helper.Hash(password);

            var patientAccount = _db.PATIENTACCOUNTs.SingleOrDefault(
                x => x.PATIENT_USERNAME == username && x.PATIENT_PASSWORD == hashedPassword);

            if (patientAccount != null)
            {
                // Mã hóa an toàn
                string encryptedPatientId = EncryptAndSafeEncode(patientAccount.PATIENT_ID.ToString());
                string safeToken = GenerateSafeToken();

                // URL chuyển hướng
                string redirectUrl = $"https://localhost:44327/Authentication/AuthenticationRequest?patientid={encryptedPatientId}&token={safeToken}";

                return Redirect(redirectUrl);
            }

            ViewBag.Error = "Tên đăng nhập và mật khẩu không chính xác";
            return View();
        }

        public ActionResult PatientSignUp()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult PatientSignUp(string username, string firstname, string lastname, string gender, DateTime dateofbirth, string address, string password, string email, string phonenumber)
        //{
        //    if (_db.PATIENTACCOUNTs.Any(p => p.PATIENT_USERNAME == username))
        //    {
        //        ViewBag.Error = "Tên đăng nhập đã tồn tại.";
        //        return View();
        //    }

        public ActionResult PatientSignUp(string username, string firstname, string lastname,
            string gender, DateTime dateofbirth, string address, string password,
            string email, string phonenumber)
        {
            // Kiểm tra trùng username
            if (_db.PATIENTACCOUNTs.Any(p => p.PATIENT_USERNAME == username))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại.";
                return View();
            }

        //    var newPatient = new PATIENT
        //    {
        //        FIRST_NAME_ = firstname,
        //        LAST_NAME_ = lastname,
        //        DATE_OF_BIRTH_ = dateofbirth,
        //        C_GENDER_ = gender,
        //        PATIENT_EMAIL = email,
        //        PATIENT_PHONE = phonenumber,
        //        PATIENT_ADDRESS = address,
        //    };
            // Tạo bệnh nhân mới
            var newPatient = new PATIENT
            {
                FIRST_NAME = firstname,
                LAST_NAME = lastname,
                DATE_OF_BIRTH = dateofbirth,
                GENDER = gender,
                PATIENT_EMAIL = email,
                PATIENT_PHONE = phonenumber,
                PATIENT_ADDRESS = address,
            };

        //    _db.PATIENTs.Add(newPatient);
        //    _db.SaveChanges();
            // Lưu bệnh nhân
            _db.PATIENTs.Add(newPatient);
            _db.SaveChanges();

        //    long newPatientId = newPatient.PATIENT_ID;

        //    var newAccount = new PATIENTACCOUNT
        //    {
        //        PATIENT_ID = newPatientId,
        //        PATIENT_USERNAME = username,
        //        PATIENT_PASSWORD = MD5Helper.Hash(password) 
        //    };
            // Tạo tài khoản
            var newAccount = new PATIENTACCOUNT
            {
                PATIENT_ID = newPatient.PATIENT_ID,
                PATIENT_USERNAME = username,
                PATIENT_PASSWORD = MD5Helper.Hash(password)
            };

        //    _db.PATIENTACCOUNTs.Add(newAccount);
        //    _db.SaveChanges(); 
        //    return RedirectToAction("PatientSignIn");
        //}
            _db.PATIENTACCOUNTs.Add(newAccount);
            _db.SaveChanges();

            return RedirectToAction("PatientSignIn");
        }

        public ActionResult EmployeeSignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeSignIn(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            string hashedPassword = MD5Helper.Hash(password);

            var employeeAccount = _db.EMPLOYEEACCOUNTs.SingleOrDefault(
                x => x.EMPLOYEE_USERNAME == username && x.EMPLOYEE_PASSWORD == hashedPassword);

            if (employeeAccount != null)
            {
                var employee = _db.EMPLOYEEs.SingleOrDefault(e => e.EMPLOYEE_ID == employeeAccount.EMPLOYEE_ID);
                if (employee != null)
                {
                    // Mã hóa EmployeeId
                    string encryptedEmployeeId = EncryptAndSafeEncode(employeeAccount.EMPLOYEE_ID.ToString());
                    string safeToken = GenerateSafeToken();

                    // Lấy port
                    if (!RolePorts.TryGetValue(employee.ROLE_NAME, out string port))
                    {
                        ViewBag.Error = "Vai trò không hợp lệ.";
                        return View();
                    }

                    // URL chuyển hướng
                    string redirectUrl = $"https://localhost:{port}/Authentication/AuthenticationRequest?employeeid={encryptedEmployeeId}&token={safeToken}";

                    return Redirect(redirectUrl);
                }
            }

            ViewBag.Error = "Tên đăng nhập và mật khẩu không chính xác";
            return View();
        }

        // Mã hóa và mã hóa an toàn URL
        private string EncryptAndSafeEncode(string plainText)
        {
            string encryptedText = Encrypt(plainText);
            return ToBase64UrlSafe(Encoding.UTF8.GetBytes(encryptedText));
        }

        // Sinh token an toàn
        private string GenerateSafeToken()
        {
            string token = GenerateRequestVerificationToken();
            return ToBase64UrlSafe(Encoding.UTF8.GetBytes(token));
        }

        // Các phương thức mã hóa và hỗ trợ không thay đổi
        private string Encrypt(string plainText)
        {
            // Giữ nguyên logic mã hóa
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

        // Mã hóa Base64 URL-safe
        private string ToBase64UrlSafe(byte[] input)
        {
            return Convert.ToBase64String(input)
                .TrimEnd('=')           // Loại bỏ padding
                .Replace('+', '-')      // Thay thế + bằng -
                .Replace('/', '_');     // Thay thế / bằng _
        }
    }
}