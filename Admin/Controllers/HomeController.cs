using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models.Data;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly AdminDbContext _db = new AdminDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            // Add List Employee
            var employees = _db.EMPLOYEEs.ToList();

            return View(employees);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}