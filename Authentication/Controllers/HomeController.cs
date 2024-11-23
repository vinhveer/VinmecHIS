using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authentication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MD5Hashing()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MD5Hashing(string input)
        {
            string hashedInput = Helper.MD5Helper.Hash(input);
            ViewBag.HashedInput = hashedInput;
            return View();
        }
    }
}