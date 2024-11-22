﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Receptionist.Models.Data;
using Receptionist.Filters;

namespace Receptionist.Controllers
{
    [Authenticate]
    public class InvoiceReceptionistController : Controller
    {
        // GET: InvoiceReceptionist
        private readonly ReceiptionistDbContext db = new ReceiptionistDbContext();

        public ActionResult Index()
        {
            var patientId = Convert.ToInt64(Session["EmployeeId"]);
            var medicalrecord = db.MEDICALRECORDs.Where(p => p.PATIENT_ID == patientId).Include(p => p.INVOICE).Include(p => p.INVOICE.EMPLOYEE).ToList();
            return View(medicalrecord);
        }

        public ActionResult Details(long id)
        {
            var medicalrecord = db.MEDICALRECORDs.FirstOrDefault(p => p.MEDICAL_RECORD_ID == id);
            var prescription = db.PRESCRIPTIONs.FirstOrDefault(p => p.MEDICAL_RECORD_ID == id);
            var prescriptiondetails = db.PRESCRIPTIONDETAILs
                                .Where(p => p.MEDICAL_RECORD_ID == id)
                                .Include(p => p.MEDICINE)
                                .ToList();
            ViewBag.medicalrecord = medicalrecord;
            ViewBag.prescription = prescription;
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
