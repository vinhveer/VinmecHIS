using Patient.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Patient.Filters;

namespace Patient.Controllers
{
    [Authenticate]
    public class HomeController : Controller
    {
        private readonly PatientDbContext _db = new PatientDbContext();
        public ActionResult Index()
        {
            

            return View();
        }
    }
}