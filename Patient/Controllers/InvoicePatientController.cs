using Patient.Models.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patient.Controllers
{
    public class InvoicePatientController : Controller
    {
        // GET: InvoicePatient
        private PatientDbContext db = new PatientDbContext();
        public ActionResult Index()
        {
            if (Session["PatientId"] != null)
            {
                var patientId = Convert.ToInt64(Session["PatientId"]);
                var medicalrecord = db.MEDICALRECORDs.Where(p => p.PATIENT_ID == patientId).Include(p => p.EMPLOYEE).ToList();
                return View(medicalrecord);
            }
            else
            {
                return View();
            }
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
