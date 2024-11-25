using Doctor.Filters;
using Doctor.Models.Data;
using Doctor.Models.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Doctor.Controllers
{
    [Authenticate]
    public class DoctorController : Controller
    {
        private readonly DoctorDbContext _db = new DoctorDbContext();

        // GET: Tạo hồ sơ bệnh án
        public ActionResult CreateMedicalRecord(string patientId)
        {
            try
            {
                var employee_id = Session["EmployeeId"];
                ViewBag.patient_id = patientId;
                ViewBag.employee_id = employee_id;
                ViewBag.examination_date = DateTime.Now;

                if (!long.TryParse(patientId, out long long_patient_id))
                {
                    TempData["ErrorMessage"] = "ID bệnh nhân không hợp lệ.";
                    return RedirectToAction("Index");
                }

                if (!long.TryParse(Session["EmployeeId"].ToString(), out long long_employee_id))
                {
                    TempData["ErrorMessage"] = "ID nhân viên không hợp lệ.";
                    return RedirectToAction("Index");
                }


                ViewBag.patient_name = _db.PATIENTs.Find(long_patient_id).LAST_NAME + " " + _db.PATIENTs.Find(long_patient_id).FIRST_NAME;
                ViewBag.doctor_name = _db.EMPLOYEEs.Find(long_employee_id).LAST_NAME + " " + _db.EMPLOYEEs.Find(long_employee_id).FIRST_NAME;

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateMedicalRecord([Bind(Include = "PATIENT_ID,EMPLOYEE_ID,EXAMINATION_DATE,DIAGNOSIS,TREATMENT,FOLLOW_UP_DATE,ADDITIONAL_NOTES,HOSPITAL_FEES")] MEDICALRECORD medicalRecord, string patientId, string ED)
        {
            medicalRecord.EMPLOYEE_ID = Convert.ToInt64(Session["EmployeeId"]);
            medicalRecord.EXAMINATION_DATE = DateTime.Parse(ED);
            medicalRecord.PATIENT_ID = Convert.ToInt64(patientId);

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
        // Controller
        public async Task<ActionResult> MedicalRecordDetails(long? medicalRecordId)
        {
            // Kiểm tra giá trị null hoặc không hợp lệ
            if (medicalRecordId == null || medicalRecordId <= 0)
            {
                return RedirectToAction("Index");
            }

            // Truy vấn hồ sơ bệnh án
            var medicalRecord = await _db.MEDICALRECORDs
                .Include(m => m.PATIENT)
                .Include(m => m.EMPLOYEE)
                .Where(m => m.MEDICAL_RECORD_ID == medicalRecordId)
                .Select(m => new MedicalRecordDetailsModel
                {
                    MEDICAL_RECORD_ID = m.MEDICAL_RECORD_ID,
                    PatientName = (m.PATIENT == null
                                    ? "Không có thông tin"
                                    : (m.PATIENT.LAST_NAME + " " + m.PATIENT.FIRST_NAME).Trim()),
                    DoctorName = (m.EMPLOYEE == null
                                    ? "Không có thông tin"
                                    : (m.EMPLOYEE.LAST_NAME + " " + m.EMPLOYEE.FIRST_NAME).Trim()),
                    EXAMINATION_DATE = m.EXAMINATION_DATE,
                    DIAGNOSIS = m.DIAGNOSIS,
                    TREATMENT = m.TREATMENT,
                    ADDITIONAL_NOTES = m.ADDITIONAL_NOTES,
                    HOSPITAL_FEES = m.HOSPITAL_FEES
                })
                .FirstOrDefaultAsync();

            // Nếu không tìm thấy hồ sơ
            if (medicalRecord == null)
            {
                TempData["Error"] = "Không tìm thấy hồ sơ bệnh án.";
                return RedirectToAction("Index");
            }

            // Trả về View với dữ liệu hồ sơ
            return View(medicalRecord);
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

        // GET: Hiển thị giao diện tạo đơn thuốc
        public async Task<ActionResult> CreatePrescription(long? medicalRecordId)
        {
            if (medicalRecordId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn hồ sơ bệnh án.";
                return RedirectToAction("Index");
            }

            var medicalRecord = await _db.MEDICALRECORDs
                .Include(m => m.PATIENT) // Bao gồm thông tin bệnh nhân
                .FirstOrDefaultAsync(m => m.MEDICAL_RECORD_ID == medicalRecordId);

            if (medicalRecord == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hồ sơ bệnh án.";
                return RedirectToAction("Index");
            }

            // Truyền các thông tin cần thiết vào ViewBag
            ViewBag.MedicalRecord = medicalRecord;
            ViewBag.PatientName = medicalRecord.PATIENT.LAST_NAME + medicalRecord.PATIENT.FIRST_NAME;
            ViewBag.ExaminationDate = medicalRecord.EXAMINATION_DATE.ToString("dd/MM/yyyy");

            // Trả về view cho phép tạo đơn thuốc
            return View();
        }


        // GET: Lấy danh sách thuốc (API hỗ trợ tìm kiếm)
        public async Task<ActionResult> SearchMedicines(string keyword = "")
        {
            var medicines = await _db.MEDICINEs
                .Where(m => string.IsNullOrEmpty(keyword) || m.MEDICINE_NAME.Contains(keyword))
                .OrderBy(m => m.MEDICINE_NAME)
                .Select(m => new
                {
                    m.MEDICINE_ID,
                    m.MEDICINE_NAME
                })
                .ToListAsync();

            return Json(medicines, JsonRequestBehavior.AllowGet);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePrescription(string selectedMedicinesJson, long medicalRecordId, string note)
        {
            if (string.IsNullOrWhiteSpace(selectedMedicinesJson))
            {
                TempData["ErrorMessage"] = "Danh sách thuốc không được để trống.";
                return RedirectToAction("CreatePrescription", new { medicalRecordId });
            }

            try
            {
                // Deserialize JSON thành danh sách PrescriptionDetailInput
                var selectedMedicines = JsonConvert.DeserializeObject<List<PrescriptionDetailInput>>(selectedMedicinesJson);
                if (selectedMedicines == null || !selectedMedicines.Any())
                {
                    TempData["ErrorMessage"] = "Danh sách thuốc không hợp lệ.";
                    return RedirectToAction("CreatePrescription", new { medicalRecordId });
                }

                // Kiểm tra thông tin cho đơn thuốc
                var prescription = new PRESCRIPTION
                {
                    MEDICAL_RECORD_ID = medicalRecordId,
                    NOTES = note,
                    PRESCRIPTION_DATE = DateTime.Now
                };

                // Thêm đơn thuốc vào DB trước khi thêm các PrescriptionDetails
                _db.PRESCRIPTIONs.Add(prescription);

                // Duyệt qua danh sách thuốc được chọn
                foreach (var medicineInput in selectedMedicines)
                {
                    // Kiểm tra thuốc có tồn tại không
                    var existingMedicine = await _db.MEDICINEs.FindAsync(medicineInput.MedicineId);
                    if (existingMedicine == null)
                    {
                        TempData["ErrorMessage"] = $"Thuốc với ID {medicineInput.MedicineId} không tồn tại.";
                        return RedirectToAction("CreatePrescription", new { medicalRecordId });
                    }

                    // Kiểm tra trùng lặp
                    var existingPrescription = await _db.PRESCRIPTIONDETAILs
                        .FirstOrDefaultAsync(p => p.MEDICAL_RECORD_ID == medicalRecordId && p.MEDICINE_ID == medicineInput.MedicineId);
                    if (existingPrescription != null)
                    {
                        TempData["ErrorMessage"] = $"Thuốc {existingMedicine.MEDICINE_NAME} đã có trong đơn thuốc.";
                        return RedirectToAction("CreatePrescription", new { medicalRecordId });
                    }

                    // Thêm chi tiết thuốc vào đơn thuốc
                    var prescriptionDetail = new PRESCRIPTIONDETAIL
                    {
                        MEDICAL_RECORD_ID = medicalRecordId,
                        MEDICINE_ID = medicineInput.MedicineId,
                        PRESCRIBED_QUANTITY = medicineInput.PrescribedQuantity,
                        DOSAGE = medicineInput.Dosage
                    };
                    _db.PRESCRIPTIONDETAILs.Add(prescriptionDetail);
                }

                // Lưu thay đổi
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đơn thuốc đã được thêm thành công!";
                return RedirectToAction("MedicalRecordDetails", new { id = medicalRecordId });
            }
            catch (DbUpdateException ex)
            {
                // In thông tin lỗi vào console hoặc ghi log
                Debug.WriteLine("DbUpdateException Thrown:");
                Debug.WriteLine($"Error Message: {ex.Message}");
                Debug.WriteLine("Inner Exception:");
                Debug.WriteLine(ex.InnerException?.Message);

                if (ex.InnerException != null)
                {
                    Debug.WriteLine("Inner Exception Details:");
                    Debug.WriteLine(ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                    {
                        Debug.WriteLine("Inner Exception Inner Details:");
                        Debug.WriteLine(ex.InnerException.InnerException.Message);
                    }
                }

                // Gửi thông báo lỗi chi tiết đến giao diện
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi lưu dữ liệu: " + ex.Message;
                return RedirectToAction("CreatePrescription", new { medicalRecordId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("CreatePrescription", new { medicalRecordId });
            }
        }


        // GET: Danh sách đơn thuốc
        public async Task<ActionResult> Prescriptions(int? medicalRecordId)
        {
            try
            {
                if (medicalRecordId == null)
                {
                    TempData["ErrorMessage"] = "ID hồ sơ bệnh án không hợp lệ.";
                    return RedirectToAction("Index");
                }

                var prescriptions = await _db.PRESCRIPTIONDETAILs
                            .Include(p => p.MEDICINE) // Thuốc
                            .Include(p => p.PRESCRIPTION) // Đơn thuốc
                            .Include(p => p.PRESCRIPTION.MEDICALRECORD.PATIENT) // Bệnh nhân
                            .Where(p => p.MEDICAL_RECORD_ID == medicalRecordId)
                            .Select(p => new PrescriptionViewModel
                            {
                                MedicalRecordId = p.MEDICAL_RECORD_ID,
                                PrescribedQuantity = p.PRESCRIBED_QUANTITY,
                                Dosage = p.DOSAGE,
                                MedicineName = p.MEDICINE.MEDICINE_NAME,
                                PatientName = (p.PRESCRIPTION.MEDICALRECORD.PATIENT.LAST_NAME + " " + p.PRESCRIPTION.MEDICALRECORD.PATIENT.FIRST_NAME).Trim(),
                                ExaminationDate = p.PRESCRIPTION.MEDICALRECORD.EXAMINATION_DATE
                            })
                            .OrderByDescending(p => p.ExaminationDate)
                            .ToListAsync();

                if (!prescriptions.Any())
                {
                    TempData["ErrorMessage"] = "Không tìm thấy đơn thuốc cho hồ sơ bệnh án này.";
                    return RedirectToAction("Index");
                }

                ViewBag.MedicalRecordId = medicalRecordId; // Để chuyển tiếp dữ liệu vào view
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