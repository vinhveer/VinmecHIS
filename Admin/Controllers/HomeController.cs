using Admin.Filters;
using Admin.Models.Data;
using System.Linq;
using System.Web.Mvc;

namespace Admin.Controllers
{
    [Authenticate]
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