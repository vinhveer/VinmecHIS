using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doctor.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            var employeeId = Session["EmployeeId"];
            ViewBag.EmployeeId = employeeId;
            return View();
        }

    }
}
