using Admin.Filters;
using Admin.Models.Data;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Admin.Controllers
{
    [Authenticate]
    public class PatientsController : Controller
    {
        private AdminDbContext db = new AdminDbContext();

        // GET: PATIENTs
        //public ActionResult Index()
        //{
        //    var pATIENTs = db.PATIENTs.Include(p => p.PATIENTACCOUNT);
        //    return View(pATIENTs.ToList());
        //}
        public ActionResult Index(int page = 1)
        {
            int pageSize = 10;
            var pATIENTs = db.PATIENTs.AsQueryable();
            pATIENTs = pATIENTs.OrderBy(bn => bn.PATIENT_ID);
            var currentRecords = pATIENTs.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)pATIENTs.Count() / pageSize);
            //ViewBag.CurrentRecords = currentRecords.Count();
            ViewBag.STT = (page - 1) * pageSize;
            return View(currentRecords);
        }

        // GET: PATIENTs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PATIENT pATIENT = db.PATIENTs.Find(id);
            if (pATIENT == null)
            {
                return HttpNotFound();
            }
            return View(pATIENT);
        }

        // GET: PATIENTs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PATIENT pATIENT = db.PATIENTs.Find(id);
            if (pATIENT == null)
            {
                return HttpNotFound();
            }
            ViewBag.PATIENT_ID = new SelectList(db.PATIENTACCOUNTs, "PATIENT_ID", "PATIENT_USERNAME", pATIENT.PATIENT_ID);
            return View(pATIENT);
        }

        // POST: PATIENTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PATIENT_ID,FIRST_NAME,LAST_NAME,DATE_OF_BIRTH,GENDER,PATIENT_EMAIL,PATIENT_PHONE,PATIENT_ADDRESS,EMERGENCY_CONTACT,REGISTRATION_DATE")] PATIENT pATIENT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pATIENT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PATIENT_ID = new SelectList(db.PATIENTACCOUNTs, "PATIENT_ID", "PATIENT_USERNAME", pATIENT.PATIENT_ID);
            return View(pATIENT);
        }

        // GET: PATIENTs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PATIENT pATIENT = db.PATIENTs.Find(id);
            if (pATIENT == null)
            {
                return HttpNotFound();
            }
            return View(pATIENT);
        }

        // POST: PATIENTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            // Lấy thông tin bệnh nhân
            PATIENT pATIENT = db.PATIENTs.Find(id);

            if (pATIENT == null)
            {
                return HttpNotFound();
            }

            // Xóa các bản ghi liên quan trong bảng Appointment
            var appointments = db.APPOINTMENTs.Where(a => a.PATIENT_ID == id).ToList();
            if (appointments.Any())
            {
                db.APPOINTMENTs.RemoveRange(appointments);
            }

            // Xóa các bản ghi liên quan trong bảng MedicalRecord
            var medicalRecords = db.MEDICALRECORDs.Where(m => m.PATIENT_ID == id).ToList();
            foreach (var medicalRecord in medicalRecords)
            {
                // Xóa các bản ghi trong bảng Prescription liên quan tới MedicalRecord
                var prescriptions = db.PRESCRIPTIONs.Where(p => p.MEDICAL_RECORD_ID == medicalRecord.MEDICAL_RECORD_ID).ToList();
                if (prescriptions.Any())
                {
                    // Lưu trữ danh sách Medical_Record_ID từ Prescription
                    var medicalRecordIds = prescriptions.Select(p => p.MEDICAL_RECORD_ID).ToList();

                    // Xóa các bản ghi trong bảng PrescriptionDetail liên quan tới Prescription
                    var prescriptionDetails = db.PRESCRIPTIONDETAILs
                        .Where(pd => medicalRecordIds.Contains(pd.MEDICAL_RECORD_ID)).ToList();
                    if (prescriptionDetails.Any())
                    {
                        db.PRESCRIPTIONDETAILs.RemoveRange(prescriptionDetails);
                    }

                    // Sau khi xóa PrescriptionDetail, xóa Prescription
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

            // Xóa các bản ghi liên quan trong bảng PatientAccount (nếu có)
            var patientAccount = db.PATIENTACCOUNTs.Where(pa => pa.PATIENT_ID == id).ToList();
            if (patientAccount.Any())
            {
                db.PATIENTACCOUNTs.RemoveRange(patientAccount);
            }

            // Xóa bản ghi của bệnh nhân trong bảng Patient
            db.PATIENTs.Remove(pATIENT);

            // Lưu thay đổi vào cơ sở dữ liệu
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Nếu có lỗi xảy ra, bạn có thể ghi lại log hoặc xử lý thông báo lỗi
                ModelState.AddModelError("", $"Không thể xóa bệnh nhân do có lỗi: {ex.Message}");
                return View("Delete", pATIENT);
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
