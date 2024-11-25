using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Receptionist.Models.Data;
using Receptionist.Filters;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Net;
using Receptionist.Models.Views;

namespace Receptionist.Controllers
{
    [Authenticate]
    public class InvoiceReceptionistController : Controller
    {
        // GET: InvoiceReceptionist
        private readonly ReceiptionistDbContext db = new ReceiptionistDbContext();

        public ActionResult Index(string keyword)
        {
            // Bắt đầu truy vấn cơ bản
            var query = from invoice in db.INVOICEs
                        join medicalRecord in db.MEDICALRECORDs on invoice.MEDICAL_RECORD_ID equals medicalRecord.MEDICAL_RECORD_ID
                        join employee in db.EMPLOYEEs on medicalRecord.EMPLOYEE_ID equals employee.EMPLOYEE_ID
                        join patients in db.PATIENTs on medicalRecord.PATIENT_ID equals patients.PATIENT_ID
                        select new InvoiceViewModel
                        {
                            InvoiceId = invoice.MEDICAL_RECORD_ID,
                            PaymentDate = invoice.PAYMENT_DATE,
                            PaymentMethod = invoice.PAYMENT_METHOD,
                            EmployeeName = employee.LAST_NAME + " " + employee.FIRST_NAME,
                            PatientName = patients.LAST_NAME + " " + patients.FIRST_NAME
                        };

            // Kiểm tra nếu có từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(keyword))
            {
                // Nếu có keyword, áp dụng điều kiện lọc
                query = query.Where(i => i.InvoiceId.ToString().Contains(keyword) ||
                                          i.EmployeeName.Contains(keyword) ||
                                          i.PatientName.Contains(keyword));
            }

            // Thực hiện truy vấn và trả về kết quả
            var result = query.ToList();

            // Trả về view với dữ liệu tìm được
            return View(result);
        }




        // GET: INVOICEs/Create
        public ActionResult Create(long medical_record_id)
        {
            INVOICE invoice = new INVOICE();

            invoice.MEDICAL_RECORD_ID = medical_record_id;
            invoice.PAYMENT_DATE = DateTime.Now;
            invoice.EMPLOYEE_ID = Convert.ToInt64(Session["EmployeeId"]);
            invoice.PAYMENT_METHOD = "THU_TRUC_TIEP";

            if (ModelState.IsValid)
            {
                db.INVOICEs.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public ActionResult MedicalRecord(string keyword, int page = 1, int pageSize = 10)
        {
            // Lọc hồ sơ bệnh án chưa tạo hóa đơn
            var medicalRecords = db.MEDICALRECORDs
                .Include(m => m.EMPLOYEE)
                .Include(m => m.PATIENT)
                .Where(m => m.INVOICE == null);

            // Tìm kiếm theo từ khóa (tên bệnh nhân hoặc bác sĩ)
            if (!string.IsNullOrEmpty(keyword))
            {
                medicalRecords = medicalRecords.Where(m =>
                    m.PATIENT.FIRST_NAME.Contains(keyword) ||
                    m.PATIENT.LAST_NAME.Contains(keyword) ||
                    m.EMPLOYEE.FIRST_NAME.Contains(keyword) ||
                    m.EMPLOYEE.LAST_NAME.Contains(keyword));
            }

            // Tổng số bản ghi
            int totalRecords = medicalRecords.Count();

            // Lấy danh sách đã phân trang
            var pagedRecords = medicalRecords
                .OrderBy(m => m.MEDICAL_RECORD_ID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Truyền thông tin phân trang vào ViewBag
            ViewBag.CurrentKeyword = keyword;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(pagedRecords);
        }

        public ActionResult Delete(int id)
        {
            var invoice = db.INVOICEs.FirstOrDefault(i => i.MEDICAL_RECORD_ID == id);
            if (invoice != null)
            {
                db.INVOICEs.Remove(invoice);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(long id)
        {
            var medicalrecord = db.MEDICALRECORDs.FirstOrDefault(p => p.MEDICAL_RECORD_ID == id);
            var prescription = db.PRESCRIPTIONs.FirstOrDefault(p => p.MEDICAL_RECORD_ID == id);
            var prescriptiondetails = db.PRESCRIPTIONDETAILs
                                .Where(p => p.MEDICAL_RECORD_ID == id)
                                .Include(p => p.MEDICINE)
                                .ToList();
            ViewBag.medicalrecord = medicalrecord;
            ViewBag.prescription = prescription;
            var employee = db.EMPLOYEEs.FirstOrDefault(e => e.EMPLOYEE_ID == medicalrecord.EMPLOYEE_ID);
            if (employee != null)
            {
                ViewBag.EmployeeFirstName = employee.FIRST_NAME;
                ViewBag.EmployeeLastName = employee.LAST_NAME;
            }
            var patient = db.PATIENTs.FirstOrDefault(p => p.PATIENT_ID == medicalrecord.PATIENT_ID);
            if (patient != null)
            {
                ViewBag.patientFirstName = patient.FIRST_NAME;
                ViewBag.patientLastName = patient.LAST_NAME;
            }
            return View(prescriptiondetails);
        }

        public ActionResult PrintPDF(int id)
        {
            // Lấy dữ liệu từ database giống như action Details
            var prescription = db.PRESCRIPTIONs.Find(id);
            var prescriptionDetails = db.PRESCRIPTIONDETAILs.Where(p => p.MEDICAL_RECORD_ID == id).Include(p => p.MEDICINE);
            var medicalRecord = db.MEDICALRECORDs.Find(prescription.MEDICAL_RECORD_ID);
            var employee = db.EMPLOYEEs.Find(medicalRecord.EMPLOYEE_ID);

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Font cho tiếng Việt
                BaseFont bf = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf",
                    BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font font = new Font(bf, 12);
                Font boldFont = new Font(bf, 12, Font.BOLD);
                Font titleFont = new Font(bf, 18, Font.BOLD);

                // Thêm logo
                string logoPath = Server.MapPath("~/Image/logo.png"); // Đường dẫn đến file logo
                if (System.IO.File.Exists(logoPath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                    // Điều chỉnh kích thước logo
                    logo.ScaleToFit(50f, 50f); // Điều chỉnh con số này theo kích thước mong muốn
                                               // Căn giữa logo
                    logo.Alignment = Element.ALIGN_CENTER;
                    document.Add(logo);
                }

                // Thêm thông tin bệnh viện
                Paragraph hospitalInfo = new Paragraph();
                hospitalInfo.Alignment = Element.ALIGN_CENTER;
                hospitalInfo.Add(new Chunk("BỆNH VIỆN Vinmec\n", boldFont));
                hospitalInfo.Add(new Chunk("Địa chỉ: Đường Trần Phú 42A Nha Trang\n", font));
                hospitalInfo.Add(new Chunk("Điện thoại: (025) 8390 0168\n", font));
                document.Add(hospitalInfo);
                document.Add(new Paragraph("\n"));

                // Tiêu đề
                Paragraph title = new Paragraph("HÓA ĐƠN KHÁM BỆNH", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                document.Add(new Paragraph("\n"));

                // Thông tin bác sĩ và ngày kê đơn
                document.Add(new Paragraph($"Bác sĩ: {employee.FIRST_NAME} {employee.LAST_NAME}", font));
                document.Add(new Paragraph($"Ngày kê đơn: {prescription.PRESCRIPTION_DATE:dd/MM/yyyy}", font));
                document.Add(new Paragraph($"Ghi chú: {prescription.NOTES}", font));
                document.Add(new Paragraph($"Viện phí: {medicalRecord.HOSPITAL_FEES:N0} VNĐ", font));
                document.Add(new Paragraph("\n"));

                // Tạo bảng chi tiết đơn thuốc
                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                float[] columnWidths = new float[] { 10f, 25f, 15f, 20f, 15f, 15f };
                table.SetWidths(columnWidths);

                // Header của bảng
                string[] headers = { "STT", "Tên thuốc", "Số lượng", "Liều dùng", "Đơn giá", "Tổng tiền" };
                foreach (string header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, boldFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.PaddingBottom = 5f;
                    table.AddCell(cell);
                }

                // Thêm dữ liệu vào bảng
                int stt = 1;
                decimal totalAmount = 0;
                foreach (var item in prescriptionDetails)
                {
                    // STT
                    PdfPCell cell1 = new PdfPCell(new Phrase(stt++.ToString(), font));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell1);

                    // Tên thuốc
                    table.AddCell(new PdfPCell(new Phrase(item.MEDICINE.MEDICINE_NAME, font)));

                    // Số lượng
                    PdfPCell cell3 = new PdfPCell(new Phrase(item.PRESCRIBED_QUANTITY.ToString(), font));
                    cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell3);

                    // Liều dùng
                    table.AddCell(new PdfPCell(new Phrase(item.DOSAGE, font)));

                    // Đơn giá
                    PdfPCell cell5 = new PdfPCell(new Phrase(item.MEDICINE.PRICE.ToString("N0"), font));
                    cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell5);

                    // Tổng tiền
                    decimal lineTotal = item.PRESCRIBED_QUANTITY * item.MEDICINE.PRICE;
                    PdfPCell cell6 = new PdfPCell(new Phrase(lineTotal.ToString("N0"), font));
                    cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell6);
                    totalAmount += lineTotal;
                }

                document.Add(table);
                document.Add(new Paragraph("\n"));

                // Tổng tiền
                decimal grandTotal = totalAmount + medicalRecord.HOSPITAL_FEES;
                Paragraph total = new Paragraph($"Tổng số tiền: {grandTotal:N0} VNĐ", boldFont);
                total.Alignment = Element.ALIGN_RIGHT;
                document.Add(total);

                // Chữ ký
                document.Add(new Paragraph("\n\n"));
                Paragraph signature = new Paragraph();
                signature.Alignment = Element.ALIGN_RIGHT;
                signature.Add(new Chunk($"Nha Trang, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}\n", font));
                signature.Add(new Chunk("Bác sĩ ký tên\n\n\n\n", font));
                signature.Add(new Chunk($"{employee.FIRST_NAME} {employee.LAST_NAME}", font));
                document.Add(signature);

                document.Close();
                writer.Close();

                return File(ms.ToArray(), "application/pdf", $"HoaDon_{id}.pdf");
            }
        }
    }
}