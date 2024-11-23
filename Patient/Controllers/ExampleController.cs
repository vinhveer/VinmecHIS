using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patient.Controllers
{
    public class ExampleController : Controller
    {
        // GET: Example
        public ActionResult Index()
        {
            var patientId = Session["PatientId"];
            ViewBag.PatientId = patientId;
            return View();
        }

        
    }
}
