using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authentication.Controllers
{
    public class AuthenticationController : Controller
    {

        public ActionResult PatientSignIn()
        {
            return View();
        }

        public ActionResult PatientSignUp()
        {
            return View();
        }

        public ActionResult EmployeeSignIn()
        {
            return View();
        }

        public ActionResult EmployeeSignUp()
        {
            return View();
        }
    }
}
