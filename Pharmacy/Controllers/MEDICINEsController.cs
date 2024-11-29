using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pharmacy.Models.Data;

namespace Pharmacy.Controllers
{
    public class MEDICINEsController : Controller
    {
        private PharmacyDbContext db = new PharmacyDbContext();

        // GET: MEDICINEs
        //public ActionResult Index()
        //{
        //    var mEDICINEs = db.MEDICINEs.Include(m => m.SUPPLIER);
        //    return View(mEDICINEs.ToList());
        //}

        public ActionResult Index(string NameSort, string StockSort)
        {
            ViewBag.CurrentSort = NameSort ?? StockSort;

            var medicines = db.MEDICINEs.Include(m => m.SUPPLIER);

            if (!string.IsNullOrEmpty(NameSort))
            {
                medicines = NameSort == "NameAsc"
                    ? medicines.OrderBy(m => m.MEDICINE_NAME)
                    : medicines.OrderByDescending(m => m.MEDICINE_NAME);
            }
            else if (!string.IsNullOrEmpty(StockSort))
            {
                medicines = StockSort == "StockAsc"
                    ? medicines.OrderBy(m => m.STOCK_QUANTITY)
                    : medicines.OrderByDescending(m => m.STOCK_QUANTITY);
            }

            return View(medicines.ToList());
        }

        // GET: MEDICINEs/Search
        public ActionResult Search(string searchString)
        {
            // Nếu không có từ khóa, trả về toàn bộ danh sách thuốc
            var medicines = string.IsNullOrEmpty(searchString)
                ? db.MEDICINEs.Include(m => m.SUPPLIER).ToList()
                : db.MEDICINEs
                    .Include(m => m.SUPPLIER)
                    .Where(m => m.MEDICINE_NAME.Contains(searchString))
                    .ToList();

            // Truyền từ khóa tìm kiếm và kết quả vào ViewBag
            ViewBag.SearchString = searchString;
            return View("Search", medicines);
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
