using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iText.Layout.Borders;
using Org.BouncyCastle.Crypto.Digests;
using Pharmacy.Filters;
using Pharmacy.Models.Data;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Pharmacy.Controllers
{
    [Authenticate]
    public class MEDICINEsController : Controller
    {
        private PharmacyDBContext db = new PharmacyDBContext();

        // GET: MEDICINEs
        //public ActionResult Index()
        //{
        //    var mEDICINEs = db.MEDICINEs.Include(m => m.SUPPLIER);
        //    return View(mEDICINEs.ToList());
        //}

        //public ActionResult Index(string NameSort, string StockSort)
        //{
        //    ViewBag.CurrentSort = NameSort ?? StockSort;

        //    var medicines = db.MEDICINEs.Include(m => m.SUPPLIER);

        //    if (!string.IsNullOrEmpty(NameSort))
        //    {
        //        medicines = NameSort == "NameAsc"
        //            ? medicines.OrderBy(m => m.MEDICINE_NAME)
        //            : medicines.OrderByDescending(m => m.MEDICINE_NAME);
        //    }
        //    else if (!string.IsNullOrEmpty(StockSort))
        //    {
        //        medicines = StockSort == "StockAsc"
        //            ? medicines.OrderBy(m => m.STOCK_QUANTITY)
        //            : medicines.OrderByDescending(m => m.STOCK_QUANTITY);
        //    }

        //    return View(medicines.ToList());
        //}

        public ActionResult Index(string NameSort, string StockSort, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = NameSort ?? StockSort;
            ViewBag.NameSort = string.IsNullOrEmpty(NameSort) ? "NameDesc" : "";
            ViewBag.StockSort = StockSort == "StockAsc" ? "StockDesc" : "StockAsc";

            // Nếu tìm kiếm mới, bắt đầu từ trang đầu tiên
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            // Truy vấn danh sách thuốc
            var medicines = db.MEDICINEs.Include(m => m.SUPPLIER).AsQueryable();

            // Lọc theo tên thuốc (nếu có)
            if (!string.IsNullOrEmpty(searchString))
            {
                medicines = medicines.Where(m => m.MEDICINE_NAME.Contains(searchString));
            }

            // Sắp xếp theo các tiêu chí
            switch (NameSort)
            {
                case "NameDesc":
                    medicines = medicines.OrderByDescending(m => m.MEDICINE_NAME);
                    break;
                default:
                    medicines = medicines.OrderBy(m => m.MEDICINE_NAME);
                    break;
            }

            switch (StockSort)
            {
                case "StockAsc":
                    medicines = medicines.OrderBy(m => m.STOCK_QUANTITY);
                    break;
                case "StockDesc":
                    medicines = medicines.OrderByDescending(m => m.STOCK_QUANTITY);
                    break;
            }

            // Thiết lập phân trang
            int pageSize = 2; // Số lượng bản ghi mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại, mặc định là 1
            int totalRecords = medicines.Count(); // Tổng số bản ghi
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize); // Tổng số trang

            // Lấy dữ liệu cho trang hiện tại
            medicines = medicines.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Truyền thông tin phân trang qua ViewBag
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageNumber;

            return View(medicines.ToList());
        }


        // GET: MEDICINEs/Search
        //public ActionResult Search(string searchString)
        //{
        //    // Nếu không có từ khóa, trả về toàn bộ danh sách thuốc
        //    var medicines = string.IsNullOrEmpty(searchString)
        //        ? db.MEDICINEs.Include(m => m.SUPPLIER).ToList()
        //        : db.MEDICINEs
        //            .Include(m => m.SUPPLIER)
        //            .Where(m => m.MEDICINE_NAME.Contains(searchString))
        //            .ToList();

        //    // Truyền từ khóa tìm kiếm và kết quả vào ViewBag
        //    ViewBag.SearchString = searchString;
        //    return View("Search", medicines);
        //}

        public ActionResult Search(string searchString, int? minStock, int? maxStock)
        {
            // Lấy danh sách tất cả thuốc
            var medicines = db.MEDICINEs.Include(m => m.SUPPLIER).AsQueryable();

            // Lọc theo tên thuốc (nếu có)
            if (!string.IsNullOrEmpty(searchString))
            {
                medicines = medicines.Where(m => m.MEDICINE_NAME.Contains(searchString));
            }

            // Lọc theo số lượng tối thiểu (nếu có)
            if (minStock.HasValue)
            {
                medicines = medicines.Where(m => m.STOCK_QUANTITY >= minStock.Value);
            }

            // Lọc theo số lượng tối đa (nếu có)
            if (maxStock.HasValue)
            {
                medicines = medicines.Where(m => m.STOCK_QUANTITY <= maxStock.Value);
            }

            // Lưu lại giá trị tìm kiếm vào ViewBag để hiển thị trong View
            ViewBag.SearchString = searchString;
            ViewBag.MinStock = minStock;
            ViewBag.MaxStock = maxStock;

            // Trả về danh sách sau khi lọc
            return View(medicines.ToList());
        }



        // GET: MEDICINEs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEDICINE mEDICINE = db.MEDICINEs.Find(id);
            if (mEDICINE == null)
            {
                return HttpNotFound();
            }
            return View(mEDICINE);
        }

        // GET: MEDICINEs/Create
        //public ActionResult Create()
        //{
        //    ViewBag.SUPPLIER_ID = new SelectList(db.SUPPLIERs, "SUPPLIER_ID", "SUPPLIER_NAME");
        //    return View();
        //}

        //// POST: MEDICINEs/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "MEDICINE_ID,SUPPLIER_ID,MEDICINE_NAME,UNIT,STOCK_QUANTITY,EXPIRATION_DATE,PRICE")] MEDICINE mEDICINE)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.MEDICINEs.Add(mEDICINE);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.SUPPLIER_ID = new SelectList(db.SUPPLIERs, "SUPPLIER_ID", "SUPPLIER_NAME", mEDICINE.SUPPLIER_ID);
        //    return View(mEDICINE);
        //}

        public ActionResult Create()
        {
            ViewBag.SUPPLIER_ID = new SelectList(db.SUPPLIERs, "SUPPLIER_ID", "SUPPLIER_NAME");
            return View(new MEDICINE());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MEDICINE medicine)
        {
            if (ModelState.IsValid)
            {
                db.MEDICINEs.Add(medicine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SUPPLIER_ID = new SelectList(db.SUPPLIERs, "SUPPLIER_ID", "SUPPLIER_NAME", medicine.SUPPLIER_ID);
            return View(medicine);
        }

        public ActionResult OrderMedicine()
        {

            var query = from order in db.MEDICINEORDERs
                        join employee in db.EMPLOYEEs on order.EMPLOYEE_ID equals employee.EMPLOYEE_ID
                        join supplier in db.SUPPLIERs on order.SUPPLIER_ID equals supplier.SUPPLIER_ID
                        join detail in db.MEDICINEORDERDETAILs on order.MEDICINE_ORDER_ID equals detail.MEDICINE_ORDER_ID into orderDetails
                        from detail in orderDetails.DefaultIfEmpty()
                        join medicine in db.MEDICINEs on detail.MEDICINE_ID equals medicine.MEDICINE_ID into medicines
                        from medicine in medicines.DefaultIfEmpty()
                        select new OrderView
                        {
                            MEDICINE_ORDER_ID= order.MEDICINE_ORDER_ID,
                            EmployeeName = employee.FIRST_NAME + " " + employee.LAST_NAME,
                            MEDICINE_NAME = medicine != null ? medicine.MEDICINE_NAME : "Không có thuốc", // Kiểm tra nếu thuốc không có
                            Suplier_name = supplier.SUPPLIER_NAME,
                            REGISTRATION_DATE = order.ORDER_DATE
                        };

            // Lọc ra những đơn hàng có chi tiết thuốc
            var orderList = query.ToList();

            // Lấy danh sách mà không có bản ghi dư
            ViewBag.orderList = orderList.Count;

            return View(orderList);



        }

        public ActionResult Order()
        {
            var medicines = db.MEDICINEs.Select(m => m.MEDICINE_NAME).Distinct().ToList();
            var suppliers = db.SUPPLIERs.Select(s => s.SUPPLIER_NAME).Distinct().ToList();

            // Truyền dữ liệu qua ViewBag
            ViewBag.Medicines = medicines;
            ViewBag.Suppliers = suppliers;

            return View();

        }

        [HttpPost]

        public ActionResult SubmitOrder(FormCollection form)
        {
            try
            {
                Debug.WriteLine("Session EmployeeId: " + Session["EmployeeId"]);
                var EmployeeId = Convert.ToInt64(Session["EmployeeId"]);
                Debug.WriteLine("EmployeeId (converted): " + EmployeeId);

                string medicineName = form["medicine"];
                Debug.WriteLine("Medicine Name: " + medicineName);

                string newMedicineName = form["medicineNew"];
                Debug.WriteLine("New Medicine Name: " + newMedicineName);

                string supplierName = form["supplier"];
                Debug.WriteLine("Supplier Name: " + supplierName);

                string unit = form["unit"];
                Debug.WriteLine("Unit: " + unit);

                int quantity = int.Parse(form["quantity"]);
                Debug.WriteLine("Quantity: " + quantity);

                DateTime expiryDate = DateTime.Parse(form["expiryDate"]);
                Debug.WriteLine("Expiry Date: " + expiryDate);

                decimal price = decimal.Parse(form["price"]);
                Debug.WriteLine("Price: " + price);

                var supplier = db.SUPPLIERs.FirstOrDefault(s => s.SUPPLIER_NAME == supplierName);
                if (supplier == null)
                {
                    Debug.WriteLine("Supplier not found: " + supplierName);
                    return HttpNotFound("Nhà cung cấp không tồn tại.");
                }

                // Khai báo medicine ở ngoài để có thể sử dụng trong toàn bộ phương thức
                MEDICINE medicine;

                if (medicineName != "new")
                {
                    medicine = db.MEDICINEs.FirstOrDefault(m => m.MEDICINE_NAME == medicineName);
                }
                else
                {
                    // Thêm thuốc mới
                    medicine = new MEDICINE
                    {
                        SUPPLIER_ID = supplier.SUPPLIER_ID,
                        MEDICINE_NAME = newMedicineName,
                        UNIT = unit,
                        STOCK_QUANTITY = quantity,
                        EXPIRATION_DATE = expiryDate,
                        PRICE = price
                    };
                    db.MEDICINEs.Add(medicine);
                    db.SaveChanges();
                }

                if (medicine == null)
                {
                    Debug.WriteLine("Medicine not found: " + medicineName);
                    return HttpNotFound("Thuốc không tồn tại.");
                }

                // Tạo đối tượng đơn đặt thuốc
                var medicineOrder = new MEDICINEORDER
                {
                    EMPLOYEE_ID = EmployeeId,
                    SUPPLIER_ID = supplier.SUPPLIER_ID,
                    ORDER_DATE = DateTime.Now
                };

                db.MEDICINEORDERs.Add(medicineOrder);
                db.SaveChanges();

                // Thêm chi tiết đơn đặt thuốc
                var orderDetail = new MEDICINEORDERDETAIL
                {
                    MEDICINE_ORDER_ID = medicineOrder.MEDICINE_ORDER_ID,
                    MEDICINE_ID = medicine.MEDICINE_ID,
                    ORDERED_QUANTITY = quantity
                };

                // Cập nhật số lượng và giá thuốc
                if(medicineName != "new")
                {
                    var currentQuantity = medicine.STOCK_QUANTITY;
                    medicine.STOCK_QUANTITY = currentQuantity + quantity;
                    medicine.PRICE = price;
                }
               

                db.MEDICINEORDERDETAILs.Add(orderDetail);
                db.SaveChanges();

                long orderId = medicineOrder.MEDICINE_ORDER_ID;
                Debug.WriteLine("OrderID " + orderId);

                string urlSuccess = $"https://localhost:44394/MEDICINES/MedicineOrderDetail?medicineOrderConfirmID={orderId}";
                return Redirect(urlSuccess);
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException dbEx)
                {
                    Debug.WriteLine("DbUpdateException Message: " + dbEx.Message);

                    if (dbEx.InnerException != null)
                    {
                        Debug.WriteLine("Inner Exception: " + dbEx.InnerException.Message);
                        if (dbEx.InnerException.InnerException != null)
                        {
                            Debug.WriteLine("Inner Inner Exception: " + dbEx.InnerException.InnerException.Message);
                        }
                    }
                }

                return View("Error", new HandleErrorInfo(ex, "MedicineOrder", "SubmitOrder"));
            }
        }
        public class Temp
        {
            public string SuplierName { get; set; }
            public string Unit { get; set; }
            public int Stock { get; set; }
            public string medicineName { get; set; }
            public DateTime ExprirationDate { get; set; }
            public decimal Price { get; set; }
            public DateTime OrderDate{ get; set; }
        }
        public ActionResult MedicineOrderDetail(long medicineOrderConfirmID)
        {
            var temp = (from order in db.MEDICINEORDERs
                        join detail in db.MEDICINEORDERDETAILs on order.MEDICINE_ORDER_ID equals detail.MEDICINE_ORDER_ID
                        join medicine in db.MEDICINEs on detail.MEDICINE_ID equals medicine.MEDICINE_ID
                        join supplier in db.SUPPLIERs on order.SUPPLIER_ID equals supplier.SUPPLIER_ID
                        where medicineOrderConfirmID == order.MEDICINE_ORDER_ID
                        select new Temp
                        {
                            SuplierName = supplier.SUPPLIER_NAME,
                            Unit = medicine.UNIT,
                            Stock = detail.ORDERED_QUANTITY,
                            medicineName = medicine.MEDICINE_NAME,
                            ExprirationDate = medicine.EXPIRATION_DATE,
                            Price = medicine.PRICE,
                            OrderDate = order.ORDER_DATE
                        }).FirstOrDefault(); // Thực thi truy vấn và lấy đối tượng đầu tiên

            if (temp == null)
            {
                return HttpNotFound(); // Xử lý khi không tìm thấy kết quả
            }

            return View(temp);
        }
        public ActionResult MedicineOrderConfirm(long medicineOrderConfirmID)
        {
            var temp = (from order in db.MEDICINEORDERs
                        join detail in db.MEDICINEORDERDETAILs on order.MEDICINE_ORDER_ID equals detail.MEDICINE_ORDER_ID
                        join medicine in db.MEDICINEs on detail.MEDICINE_ID equals medicine.MEDICINE_ID
                        join supplier in db.SUPPLIERs on order.SUPPLIER_ID equals supplier.SUPPLIER_ID
                        where medicineOrderConfirmID == order.MEDICINE_ORDER_ID
                        select new Temp
                        {
                            SuplierName = supplier.SUPPLIER_NAME,
                            Unit = medicine.UNIT,
                            Stock = detail.ORDERED_QUANTITY,
                            medicineName = medicine.MEDICINE_NAME,
                            ExprirationDate = medicine.EXPIRATION_DATE,
                            Price = medicine.PRICE,
                            OrderDate = order.ORDER_DATE
                        }).FirstOrDefault(); // Thực thi truy vấn và lấy đối tượng đầu tiên

            if (temp == null)
            {
                return HttpNotFound(); // Xử lý khi không tìm thấy kết quả
            }

            return View(temp);
        }



        // GET: MEDICINEs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEDICINE mEDICINE = db.MEDICINEs.Find(id);
            if (mEDICINE == null)
            {
                return HttpNotFound();
            }
            ViewBag.SUPPLIER_ID = new SelectList(db.SUPPLIERs, "SUPPLIER_ID", "SUPPLIER_NAME", mEDICINE.SUPPLIER_ID);
            return View(mEDICINE);
        }

        // POST: MEDICINEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MEDICINE_ID,SUPPLIER_ID,MEDICINE_NAME,UNIT,STOCK_QUANTITY,EXPIRATION_DATE,PRICE")] MEDICINE mEDICINE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mEDICINE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SUPPLIER_ID = new SelectList(db.SUPPLIERs, "SUPPLIER_ID", "SUPPLIER_NAME", mEDICINE.SUPPLIER_ID);
            return View(mEDICINE);
        }

        // GET: MEDICINEs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEDICINE mEDICINE = db.MEDICINEs.Find(id);
            if (mEDICINE == null)
            {
                return HttpNotFound();
            }
            return View(mEDICINE);
        }

        // POST: MEDICINEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            MEDICINE mEDICINE = db.MEDICINEs.Find(id);
            db.MEDICINEs.Remove(mEDICINE);
            db.SaveChanges();
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
