using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Doctor.Models.Data;
using Doctor.Models.Views;

namespace Doctor.Controllers
{
    public class DoctorController : Controller
    {
        private readonly DoctorDbContext _db = new DoctorDbContext();

        public DoctorController()
        {
            _db = new DoctorDbContext();
        }

        // GET: Tạo hồ sơ bệnh án
        public ActionResult CreateMedicalRecord()
        {
            try
            {
                var employee_id = Session["EmployeeId"]; // Lấy ID từ session đăng nhập
                
                ViewBag.employee_id = employee_id;
                ViewBag.examination_date = DateTime.Now;
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMedicalRecord([Bind(Include = "PATIENT_ID,EMPLOYEE_ID,EXAMINATION_DATE,DIAGNOSIS,TREATMENT,FOLLOW_UP_DATE,ADDITIONAL_NOTES,HOSPITAL_FEES")] MEDICALRECORD medicalRecord)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.MEDICALRECORDs.Add(medicalRecord);
                    await _db.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Hồ sơ bệnh án đã được tạo thành công!";
                    return RedirectToAction("MedicalRecordDetails", new { id = medicalRecord.MEDICAL_RECORD_ID });
                }
                return View(medicalRecord);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo hồ sơ: " + ex.Message;
                return View(medicalRecord);
            }
        }

        // GET: Tìm hồ sơ bệnh án theo ID bệnh nhân
        public async Task<ActionResult> MedicalRecordByPatientId(long? patientId)
        {
            try
            {
                if (patientId == null)
                {
                    TempData["ErrorMessage"] = "Vui lòng nhập ID bệnh nhân.";
                    return RedirectToAction("Index");
                }

                var medicalRecords = await _db.MEDICALRECORDs
                    .Include(m => m.PATIENT)
                    .Include(m => m.EMPLOYEE)
                    .Where(m => m.PATIENT_ID == patientId)
                    .ToListAsync();

                if (!medicalRecords.Any())
                {
                    TempData["ErrorMessage"] = "Không tìm thấy hồ sơ bệnh án cho bệnh nhân này.";
                    return RedirectToAction("Index");
                }

                // Nếu có nhiều hồ sơ, chuyển đến trang danh sách
                if (medicalRecords.Count > 1)
                {
                    return View("MedicalRecordsList", medicalRecords);
                }

                // Nếu chỉ có 1 hồ sơ, chuyển đến trang chi tiết
                return RedirectToAction("MedicalRecordDetails", new { id = medicalRecords.First().MEDICAL_RECORD_ID });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Xem chi tiết hồ sơ bệnh án
        public async Task<ActionResult> MedicalRecordDetails(long? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["ErrorMessage"] = "Vui lòng chọn hồ sơ bệnh án.";
                    return RedirectToAction("Index");
                }

                var medicalRecord = await _db.MEDICALRECORDs
                    .Include(m => m.PATIENT)
                    .Include(m => m.EMPLOYEE)
                    .FirstOrDefaultAsync(m => m.MEDICAL_RECORD_ID == id);

                if (medicalRecord == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy hồ sơ bệnh án.";
                    return RedirectToAction("Index");
                }

                return View(medicalRecord);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var medicalRecords = await _db.MEDICALRECORDs
                        .AsNoTracking()
                        .Include(m => m.PATIENT)
                        .Include(m => m.EMPLOYEE)
                        .Select(m => new MedicalRecordViewModel
                        {
                            MEDICAL_RECORD_ID = m.MEDICAL_RECORD_ID,
                            PatientName = m.PATIENT.LAST_NAME + m.PATIENT.FIRST_NAME,
                            EXAMINATION_DATE = m.EXAMINATION_DATE,
                            DIAGNOSIS = m.DIAGNOSIS,
                            PATIENT_ID = m.PATIENT_ID,
                            DoctorName = m.EMPLOYEE.LAST_NAME + m.EMPLOYEE.FIRST_NAME
                        })
                        .OrderByDescending(m => m.EXAMINATION_DATE)
                        .ToListAsync();
                return View(medicalRecords);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Tạo đơn thuốc
        public async Task<ActionResult> CreatePrescription(long? medicalRecordId)
        {
            try
            {
                if (medicalRecordId == null)
                {
                    TempData["ErrorMessage"] = "Vui lòng chọn hồ sơ bệnh án.";
                    return RedirectToAction("Index");
                }

                var medicalRecord = await _db.MEDICALRECORDs
                    .Include(m => m.PATIENT)
                    .FirstOrDefaultAsync(m => m.MEDICAL_RECORD_ID == medicalRecordId);

                if (medicalRecord == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy hồ sơ bệnh án.";
                    return RedirectToAction("Index");
                }

                ViewBag.MedicalRecord = medicalRecord;
                ViewBag.Medicines = new SelectList(await _db.MEDICINEs.OrderBy(m => m.MEDICINE_NAME).ToListAsync(),
                                                "MEDICINE_ID", "MEDICINE_NAME");

                return View(new PRESCRIPTIONDETAIL { MEDICAL_RECORD_ID = medicalRecordId.Value });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePrescription(PRESCRIPTIONDETAIL prescriptionDetail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.PRESCRIPTIONDETAILs.Add(prescriptionDetail);
                    await _db.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Đơn thuốc đã được thêm thành công!";
                    return RedirectToAction("MedicalRecordDetails", new { id = prescriptionDetail.MEDICAL_RECORD_ID });
                }

                // Nếu ModelState không hợp lệ, load lại dữ liệu cho view
                var medicalRecord = await _db.MEDICALRECORDs
                    .Include(m => m.PATIENT)
                    .FirstOrDefaultAsync(m => m.MEDICAL_RECORD_ID == prescriptionDetail.MEDICAL_RECORD_ID);

                ViewBag.MedicalRecord = medicalRecord;
                ViewBag.Medicines = new SelectList(_db.MEDICINEs.OrderBy(m => m.MEDICINE_NAME),
                                                "MEDICINE_ID", "MEDICINE_NAME");
                return View(prescriptionDetail);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Danh sách đơn thuốc
        public async Task<ActionResult> Prescriptions()
        {
            try
            {
                var prescriptions = await _db.PRESCRIPTIONDETAILs
                    .Include(p => p.MEDICINE)
                    .Include(p => p.PRESCRIPTION.MEDICALRECORD.PATIENT)
                    .Select(p => new PrescriptionViewModel  // Tạo ViewModel để hiển thị
                    {
                        MedicalRecordId = p.MEDICAL_RECORD_ID,
                        PrescribedQuantity = p.PRESCRIBED_QUANTITY,
                        Dosage = p.DOSAGE,
                        MedicineName = p.MEDICINE.MEDICINE_NAME,
                        PatientName = p.PRESCRIPTION.MEDICALRECORD.PATIENT.LAST_NAME,
                        ExaminationDate = p.PRESCRIPTION.MEDICALRECORD.EXAMINATION_DATE
                    })
                    .OrderByDescending(p => p.ExaminationDate)
                    .ToListAsync();

                return View(prescriptions);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
