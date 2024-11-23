using Pharmacy.Models.Data;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Drawing;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;



// Để tùy chỉnh thuộc tính, ví dụ căn chỉnh văn bản


namespace Pharmacy.Controllers
{
    public class ConfirmedPrescriptionsController : Controller
    {
        private readonly PharmacyDBContext _db = new PharmacyDBContext();

        private PrecsriptionViewModel GetPrescriptionById(int id)
        {
            return (from p in _db.PRESCRIPTIONs
                    join mr in _db.MEDICALRECORDs on p.MEDICAL_RECORD_ID equals mr.MEDICAL_RECORD_ID
                    join pat in _db.PATIENTs on mr.PATIENT_ID equals pat.PATIENT_ID
                    where p.MEDICAL_RECORD_ID == id
                    select new PrecsriptionViewModel
                    {
                        PatientName = pat.FIRST_NAME + " " + pat.LAST_NAME,
                        PrescriptionDate = p.PRESCRIPTION_DATE,
                        Medicines = (from pd in _db.PRESCRIPTIONDETAILs
                                     join m in _db.MEDICINEs on pd.MEDICINE_ID equals m.MEDICINE_ID
                                     where pd.MEDICAL_RECORD_ID == id
                                     select new MedicineDetail
                                     {
                                         MedicineName = m.MEDICINE_NAME,
                                         Dosage = pd.DOSAGE,
                                         Quantity = pd.PRESCRIBED_QUANTITY
                                     }).ToList()

                    }).FirstOrDefault();
        }
        private IEnumerable<PrecsriptionViewModel> GetPrescription()
        {
            return (from p in _db.PRESCRIPTIONs
                    join mr in _db.MEDICALRECORDs on p.MEDICAL_RECORD_ID equals mr.MEDICAL_RECORD_ID
                    join pat in _db.PATIENTs on mr.PATIENT_ID equals pat.PATIENT_ID
                    join pd in _db.PRESCRIPTIONDETAILs on p.MEDICAL_RECORD_ID equals pd.MEDICAL_RECORD_ID
                    join m in _db.MEDICINEs on pd.MEDICINE_ID equals m.MEDICINE_ID
                    select new PrecsriptionViewModel
                    {
                        MEDICAL_RECORD_ID = p.MEDICAL_RECORD_ID,
                        PatientName = pat.FIRST_NAME + " " + pat.LAST_NAME,
                        PrescriptionDate = p.PRESCRIPTION_DATE
                    }).ToList();
                    
        }
        // GET: ConfirmedPrescriptions
        public ActionResult Index()
        {

            var list_prescription = GetPrescription();
            return View(list_prescription);
            
        }


        // GET: ConfirmedPrescriptions/Details/5
        public ActionResult Details(int id)
        {

            var prescription = GetPrescriptionById(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }

            Debug.WriteLine("Danh sách thuốc", prescription.Medicines);
            ViewBag.MedicalRecordId = id;

            return View(prescription);
        }
        [HttpGet]
        public ActionResult SearchPrescription(string prescrID, string patientName, int page = 1)
        {
            // Lấy danh sách đơn thuốc từ cơ sở dữ liệu
            var prescriptions = GetPrescription();
           
            
            // Kiểm tra nếu prescriptionId được nhập vào
            if (!string.IsNullOrEmpty(prescrID))
            {
                var prescriptionID = Int64.Parse(prescrID);
                prescriptions = prescriptions.Where(p => p.MEDICAL_RECORD_ID == prescriptionID);
            }

            // Kiểm tra nếu patientName (họ tên bệnh nhân) được nhập vào
            if (!string.IsNullOrEmpty(patientName))
            {
                prescriptions = prescriptions.Where(p => p.PatientName.ToLower().Contains(patientName.ToLower()));
            }


            prescriptions = prescriptions.OrderBy(p => p.MEDICAL_RECORD_ID);
            // Phân trang (nếu cần)
            int pageSize = 10; // Số lượng đơn thuốc mỗi trang
            int skip = (page - 1) * pageSize; // Số bản ghi bỏ qua
            var prescriptionsPage = prescriptions.Skip(skip).Take(pageSize).ToList();

            // Trả về View với danh sách các đơn thuốc
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)prescriptions.Count() / pageSize);

            return View(prescriptionsPage);
        }

        
        public class ValidateStockQuantity
        {
            public string MedicineName { get; set; }
            public int CurrentStock { get; set; }
            public int NeededStock { get; set; }
        }
        [HttpPost]
        public ActionResult ConfirmAndRedirectToPdf(int id)
        {
            System.Diagnostics.Debug.WriteLine("Action ConfirmAndRedirectToPdf đã được gọi với id: " + id);
            var prescription = GetPrescriptionById(id);

            if (prescription == null)
            {
                Debug.WriteLine("Không có đơn thuốc");
                return HttpNotFound();
            }
            else
            {
                Debug.WriteLine("Đơn thuốc: ", prescription);
            }
            // Cập nhật trạng thái xác nhận
            prescription.IsConfirmed = true;
           
            if (prescription == null)
            {
                Debug.WriteLine("Không tìm thấy đơn thuốc");
                return HttpNotFound();
            }

            // Lấy danh sách thuốc cần cập nhật và số lượng tương ứng
            var prescriptionDetails = _db.PRESCRIPTIONDETAILs
                .Where(pd => pd.MEDICAL_RECORD_ID == id)
                .Select(pd => new
                {
                    pd.MEDICINE_ID,
                    pd.PRESCRIBED_QUANTITY,
                    
                })
                .ToList();
                var invalidStock = (from pd in prescriptionDetails
                                join m in _db.MEDICINEs on pd.MEDICINE_ID equals m.MEDICINE_ID
                                where m.STOCK_QUANTITY < pd.PRESCRIBED_QUANTITY
                                select new ValidateStockQuantity
                                {
                                    MedicineName = m.MEDICINE_NAME,
                                    CurrentStock = m.STOCK_QUANTITY,
                                    NeededStock = pd.PRESCRIBED_QUANTITY
                                }).ToList();


            if (invalidStock.Any())
                {
                    Debug.WriteLine("Không đủ số lượng thuốc trong kho:");
                    foreach (var item in invalidStock)
                    {
                        Debug.WriteLine($"Thuốc {item.MedicineName}: " +
                            $"Tồn kho: {item.CurrentStock}, Cần: {item.NeededStock}");
                    }
                }
            else
            {
                foreach (var detail in prescriptionDetails)
                {
                    var medicine = _db.MEDICINEs
                        .First(m => m.MEDICINE_ID == detail.MEDICINE_ID);

                    medicine.STOCK_QUANTITY -= detail.PRESCRIBED_QUANTITY;

                    // Log thay đổi
                    Debug.WriteLine($"Cập nhật thuốc {medicine.MEDICINE_NAME}: " +
                        $"Số lượng kê: {detail.PRESCRIBED_QUANTITY}, " +
                        $"Số lượng tồn mới: {medicine.STOCK_QUANTITY}");
                }
                _db.SaveChanges();
            }
            // Chuyển hướng sang hành động hiển thị PDF
            using (var memoryStream = new MemoryStream())
            {
                // Tạo tài liệu PDF
                var document = new PdfDocument();
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);

                // Định dạng font
                var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
                var fontText = new XFont("Arial", 12, XFontStyle.Regular);

                // Thêm tiêu đề
                gfx.DrawString("Chi tiết đơn thuốc", fontTitle, XBrushes.Black, new XRect(0, 20, page.Width, page.Height),
                    XStringFormats.TopCenter);

                // Thêm thông tin bệnh nhân
                gfx.DrawString($"Bệnh nhân: {prescription.PatientName}", fontText, XBrushes.Black, new XPoint(50, 80));
                gfx.DrawString($"Ngày kê đơn: {prescription.PrescriptionDate:dd/MM/yyyy}", fontText, XBrushes.Black, new XPoint(50, 100));

                // Thêm danh sách thuốc
                gfx.DrawString("Danh sách thuốc:", fontText, XBrushes.Black, new XPoint(50, 130));
                int yOffset = 150;
                foreach (var medicine in prescription.Medicines)
                {
                    gfx.DrawString($"- Tên thuốc: {medicine.MedicineName}, Liều dùng: {medicine.Dosage}, Số lượng: {medicine.Quantity}",
                        fontText, XBrushes.Black, new XPoint(50, yOffset));
                    yOffset += 20;
                }

                // Thêm trạng thái xác nhận
                gfx.DrawString($"Trạng thái: {(prescription.IsConfirmed ? "Đã xác nhận cấp thuốc" : "Chưa xác nhận cấp thuốc")}",
                    fontText, XBrushes.Black, new XPoint(50, yOffset + 20));

                gfx.DrawString("Bệnh nhân", fontText, XBrushes.Black, new XPoint(50, yOffset + 50));

                // Ký nhận của người cấp thuốc, đặt ở bên phải
                gfx.DrawString("Người cấp thuốc", fontText, XBrushes.Black, new XPoint(page.Width - 250, yOffset + 50));


                // Lưu tài liệu PDF vào MemoryStream
                document.Save(memoryStream);
                document.Close();

                TempData["PdfGenerated"] = "Đơn thuốc đã được xác nhận và file PDF đã được tạo!";

                string fileName = $"donthuoc_{prescription.PrescriptionDate:yyyyMMdd}_{prescription.PatientName.Replace(" ", "_")}.pdf";

                // Trả về file PDF
                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }
        }
            
            // GET: ConfirmedPrescriptions/Create
            public ActionResult Create()
        {
            return View();
        }

        // POST: ConfirmedPrescriptions/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ConfirmedPrescriptions/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ConfirmedPrescriptions/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ConfirmedPrescriptions/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ConfirmedPrescriptions/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
