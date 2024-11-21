using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Authentication.Models.Data;
using Authentication.Helper;
using System.Web;
using System.Web.Helpers;
using System.Diagnostics;

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
        public ActionResult PatientSignUp(string username, string firstname, string lastname, string gender, DateTime dateofbirth, string address, string password, string email, string phonenumber)
        {
            if (_db.PATIENTACCOUNTs.Any(p => p.PATIENT_USERNAME == username))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại.";
                return View();
            }

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

            _db.PATIENTs.Add(newPatient);
            _db.SaveChanges();

            long newPatientId = newPatient.PATIENT_ID;

            var newAccount = new PATIENTACCOUNT
            {
                PATIENT_ID = newPatientId,
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

        public ActionResult EmployeeSignIn()
        {
            var patientAccounts = _db.PATIENTACCOUNTs.ToList();

            // Print data to the Output pane
            foreach (var account in patientAccounts)
            {
                Debug.WriteLine($"ID: {account.PATIENT_ID}, Username: {account.PATIENT_USERNAME}");
            }

            return View();
        }

        [HttpPost]
        public ActionResult EmployeeSignIn(string username, string password)
        {
            string hashedPassword = MD5Helper.Hash(password);

            var employeeAccount = _db.EMPLOYEEACCOUNTs.SingleOrDefault(x => x.EMPLOYEE_USERNAME == username && x.EMPLOYEE_PASSWORD == hashedPassword);

            //Debug.WriteLine("hashedPassword:" + hashedPassword);
            if (employeeAccount != null)
            {
                var employee = _db.EMPLOYEEs.SingleOrDefault(e => e.EMPLOYEE_ID == employeeAccount.EMPLOYEE_ID);
                if (employee != null)
                {
                    var employeeRole = employee.ROLE_NAME;

                    // Mã hóa PatientId và mã hóa URL
                    string encryptedEmployeeId = employeeAccount.EMPLOYEE_ID.ToString();

                    // Sinh RequestVerificationToken
                    string requestVerificationToken = GenerateRequestVerificationToken();
                    Debug.WriteLine("Role_name ", employee.ROLE_NAME);
                    string port = "";

                    if (employee.ROLE_NAME == "Doctor")
                    {
                        port = "44386";
                    }
                    else if (employee.ROLE_NAME == "Pharmacist")
                    {
                        port = "44394";
                    }
                    else if (employee.ROLE_NAME == "Receptionist")
                    {
                        port = "44316";
                    } 
                    else if (employee.ROLE_NAME == "Admin")
                    {
                        port = "44306";
                    }    
                    // URL chuyển hướng
                    string redirectUrl = $"https://localhost:{port}/Authentication/AuthenticationRequest?employeeid={encryptedEmployeeId}&token={requestVerificationToken}";

                    // Chuyển hướng
                    return Redirect(redirectUrl);
                }
            }
            ViewBag.Error = "Tên đăng nhập và mật khẩu không chính xác";
            return View();
        }
    }
}
