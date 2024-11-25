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
using System.Data.Entity.Infrastructure;

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

        [HttpPost]
        public ActionResult PatientSignUp(string username, string firstname, string lastname,
            string gender, string dateofbirth, string address, string password,
            string email, string phonenumber, string emergencycontact)
        {
            try
            {
                if (_db.PATIENTACCOUNTs.Any(p => p.PATIENT_USERNAME == username))
                {
                    ViewBag.Error = "Tên đăng nhập đã tồn tại.";
                    return View();
                }
                var dob = DateTime.Parse(dateofbirth);
                Debug.WriteLine($"Parsed Date of Birth: {dob}");
                Debug.WriteLine($"Type of Date of Birth: {dob.GetType()}");

                // Lưu bệnh nhân
                var newPatient = new PATIENT
                {
                    FIRST_NAME = firstname,
                    LAST_NAME = lastname,
                    DATE_OF_BIRTH = dob,
                    GENDER = gender,
                    PATIENT_EMAIL = email,
                    PATIENT_PHONE = phonenumber,
                    PATIENT_ADDRESS = address,
                    EMERGENCY_CONTACT = emergencycontact,
                    REGISTRATION_DATE = DateTime.Now,
                };

                _db.PATIENTs.Add(newPatient);
                _db.SaveChanges();


                // Tạo tài khoản
                var newAccount = new PATIENTACCOUNT
                {
                    PATIENT_ID = newPatient.PATIENT_ID,
                    PATIENT_USERNAME = username,
                    PATIENT_PASSWORD = MD5Helper.Hash(password)
                };

                _db.PATIENTACCOUNTs.Add(newAccount);
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Ghi log lỗi chi tiết
                Debug.WriteLine($"DbUpdateException: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    if (ex.InnerException.InnerException != null)
                    {
                        Debug.WriteLine($"Inner Inner Exception: {ex.InnerException.InnerException.Message}");
                    }
                }
                ViewBag.Error = "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng kiểm tra thông tin nhập.";
                return View();
            }
            catch (Exception ex)
            {
                // Bắt các lỗi khác
                Debug.WriteLine($"Exception: {ex.Message}");
                ViewBag.Error = "Đã xảy ra lỗi không xác định. Vui lòng thử lại.";
                return View();
            }
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