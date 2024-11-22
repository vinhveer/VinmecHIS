using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Admin.Models.Data;

namespace Admin.Controllers
{
    public class EMPLOYEEsController : Controller
    {
        private AdminDbContext db = new AdminDbContext();

        // GET: EMPLOYEEs
        public ActionResult Index(int page = 1)
        {
            int pageSize = 10;
            var eMPLOYEEs = db.EMPLOYEEs.AsQueryable();
            eMPLOYEEs = eMPLOYEEs.OrderBy(bn => bn.EMPLOYEE_ID);
            var currentRecords = eMPLOYEEs.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)eMPLOYEEs.Count() / pageSize);
            //ViewBag.CurrentRecords = currentRecords.Count();
            ViewBag.STT = (page - 1) * pageSize;
            return View(currentRecords);
        }

        // GET: EMPLOYEEs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = db.EMPLOYEEs.Find(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(eMPLOYEE);
        }

        private void CreateEmployeeAccount(EMPLOYEE eMPLOYEE)
        {
            // Sinh username tự động dựa trên vai trò
            string role = eMPLOYEE.ROLE_NAME.ToLower();
            string prefix = "";

            switch (role)
            {
                case "doctor":
                    prefix = "doctor";
                    break;
                case "pharmacist":
                    prefix = "pharmacist";
                    break;
                case "receptionist":
                    prefix = "Receptionist";
                    break;
                // Thêm các trường hợp khác nếu cần
                default:
                    prefix = "employee";
                    break;
            }

            // Lấy số thứ tự lớn nhất hiện tại cho role
            int nextNumber = db.EMPLOYEEACCOUNTs
                               .Where(a => a.EMPLOYEE_USERNAME.StartsWith(prefix))
                               .Count() + 1;

            string username = $"{prefix}{nextNumber:D3}";

            // Mật khẩu đã mã hóa
            string encryptedPassword = "414e7c8ede73a0c2c3d17699134f4080";

            EMPLOYEEACCOUNT newAccount = new EMPLOYEEACCOUNT
            {
                EMPLOYEE_ID = eMPLOYEE.EMPLOYEE_ID,
                EMPLOYEE_USERNAME = username,
                EMPLOYEE_PASSWORD = encryptedPassword
            };

            // Thêm tài khoản vào cơ sở dữ liệu
            db.EMPLOYEEACCOUNTs.Add(newAccount);
            db.SaveChanges();
        }



        // GET: EMPLOYEEs/Create
        public ActionResult Create()
        {
            ViewBag.EMPLOYEE_ID = new SelectList(db.EMPLOYEEACCOUNTs, "EMPLOYEE_ID", "EMPLOYEE_USERNAME");
            return View();
        }

        // POST: EMPLOYEEs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EMPLOYEE_ID,FIRST_NAME,LAST_NAME,DATE_OF_BIRTH,GENDER,EMPLOYEE_EMAIL,EMPLOYEE_PHONE,EMPLOYEE_ADDRESS,ROLE_NAME,HIRE_DATE,EMPLOYEE_ROOM,DEPARTMENT")] EMPLOYEE eMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                // Thêm nhân viên vào cơ sở dữ liệu
                db.EMPLOYEEs.Add(eMPLOYEE);
                db.SaveChanges();

                // Tạo tài khoản cho nhân viên mới
                CreateEmployeeAccount(eMPLOYEE);

                return RedirectToAction("Index");
            }

            ViewBag.EMPLOYEE_ID = new SelectList(db.EMPLOYEEACCOUNTs, "EMPLOYEE_ID", "EMPLOYEE_USERNAME", eMPLOYEE.EMPLOYEE_ID);
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEEs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = db.EMPLOYEEs.Find(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.EMPLOYEE_ID = new SelectList(db.EMPLOYEEACCOUNTs, "EMPLOYEE_ID", "EMPLOYEE_USERNAME", eMPLOYEE.EMPLOYEE_ID);
            return View(eMPLOYEE);
        }

        // POST: EMPLOYEEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EMPLOYEE_ID,FIRST_NAME,LAST_NAME,DATE_OF_BIRTH,GENDER,EMPLOYEE_EMAIL,EMPLOYEE_PHONE,EMPLOYEE_ADDRESS,ROLE_NAME,HIRE_DATE,EMPLOYEE_ROOM,DEPARTMENT")] EMPLOYEE eMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eMPLOYEE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EMPLOYEE_ID = new SelectList(db.EMPLOYEEACCOUNTs, "EMPLOYEE_ID", "EMPLOYEE_USERNAME", eMPLOYEE.EMPLOYEE_ID);
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEEs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = db.EMPLOYEEs.Find(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(eMPLOYEE);
        }

        // POST: EMPLOYEEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            // Lấy thông tin nhân viên
            EMPLOYEE eMPLOYEE = db.EMPLOYEEs.Find(id);

            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }

            // Xóa các bản ghi liên quan trong bảng EmployeeAccount
            var employeeAccounts = db.EMPLOYEEACCOUNTs.Where(a => a.EMPLOYEE_ID == id).ToList();
            if (employeeAccounts.Any())
            {
                db.EMPLOYEEACCOUNTs.RemoveRange(employeeAccounts);
            }

            // Xóa các bản ghi liên quan trong bảng Appointment
            var appointments = db.APPOINTMENTs.Where(a => a.EMPLOYEE_ID == id).ToList();
            if (appointments.Any())
            {
                db.APPOINTMENTs.RemoveRange(appointments);
            }

            // Xóa các bản ghi liên quan trong bảng MedicalRecord
            var medicalRecords = db.MEDICALRECORDs.Where(m => m.EMPLOYEE_ID == id).ToList();
            foreach (var medicalRecord in medicalRecords)
            {
                // Xóa các bản ghi trong bảng Prescription liên quan tới MedicalRecord
                var prescriptions = db.PRESCRIPTIONs.Where(p => p.MEDICAL_RECORD_ID == medicalRecord.MEDICAL_RECORD_ID).ToList();
                if (prescriptions.Any())
                {
                    db.PRESCRIPTIONs.RemoveRange(prescriptions);
                }

                // Xóa các bản ghi trong bảng Invoice liên quan tới MedicalRecord
                var invoices = db.INVOICEs.Where(i => i.MEDICAL_RECORD_ID == medicalRecord.MEDICAL_RECORD_ID).ToList();
                if (invoices.Any())
                {
                    db.INVOICEs.RemoveRange(invoices);
                }

                // Sau khi xóa Prescription và Invoice, xóa MedicalRecord
                db.MEDICALRECORDs.Remove(medicalRecord);
            }

            // Xóa các bản ghi liên quan trong bảng Invoice nếu có (Invoice không chỉ phụ thuộc vào MedicalRecord)
            var invoicesForEmployee = db.INVOICEs.Where(i => i.EMPLOYEE_ID == id).ToList();
            if (invoicesForEmployee.Any())
            {
                db.INVOICEs.RemoveRange(invoicesForEmployee);
            }

            // Xóa bản ghi trong bảng Employee
            db.EMPLOYEEs.Remove(eMPLOYEE);

            // Lưu thay đổi vào cơ sở dữ liệu
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Nếu có lỗi xảy ra, bạn có thể ghi lại log hoặc xử lý thông báo lỗi
                ModelState.AddModelError("", $"Không thể xóa nhân viên do có lỗi: {ex.Message}");
                return View("Delete", eMPLOYEE);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
