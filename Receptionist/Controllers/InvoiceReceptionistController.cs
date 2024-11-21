
using Receptionist.Models.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Receptionist.Controllers
{
    public class InvoiceReceptionistController : Controller
    {
        // GET: InvoiceReceptionist
        private receptionistDbContext db = new receptionistDbContext();
        public ActionResult Index()
        {
            var patientId = Convert.ToInt64(Session["PatientId"]);
            var medicalrecord = db.MEDICALRECORDs.Include(p => p.PATIENT).ToList();
            return View(medicalrecord);
        }

        public ActionResult Details(long id)
        {
            var medicalrecord = db.MEDICALRECORDs.FirstOrDefault(p => p.MEDICAL_RECORD_ID == id);
            var prescriptiondetails = db.PRESCRIPTIONDETAILs
                                .Where(p => p.MEDICAL_RECORD_ID == id)
                                .Include(p => p.MEDICINE)
                                .ToList();
            ViewBag.medicalrecord = medicalrecord;
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
    }
}
