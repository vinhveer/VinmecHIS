using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Doctor.Filters;
using Doctor.Models.Data;

namespace Doctor.Controllers
{
    [Authenticate]
    public class AppointMentController : Controller
    {
        // GET: AppointMent
        private readonly DoctorDbContext _db = new DoctorDbContext();
        public ActionResult Index(int page = 1)
        {
            if (Session["EmployeeId"] != null)
            {
                var EmployeeId = Convert.ToInt64(Session["EmployeeId"]);
                var appointments = (from patient in _db.PATIENTs
                                    join appointment in _db.APPOINTMENTs on patient.PATIENT_ID equals appointment.PATIENT_ID
                                    join employee in _db.EMPLOYEEs on appointment.EMPLOYEE_ID equals employee.EMPLOYEE_ID

                                    where employee.EMPLOYEE_ID == EmployeeId
                                    orderby appointment.APPOINTMENT_TIME.Year,
                                    appointment.APPOINTMENT_TIME.Month,
                                    appointment.APPOINTMENT_TIME.Day,
                                    appointment.APPOINTMENT_TIME.Hour,
                                    appointment.APPOINTMENT_TIME.Minute,
                                    appointment.APPOINTMENT_TIME.Second
                                    select new AppointmentViewModel
                                    {
                                        AppointmentId = appointment.APPOINTMENT_ID,
                                        PatientId = patient.PATIENT_ID,
                                        PatientName = patient.FIRST_NAME + " " + patient.LAST_NAME,
                                        PatientGender = patient.GENDER,
                                        AppointmentDate = appointment.APPOINTMENT_TIME,
                                        Room = employee.EMPLOYEE_ROOM,
                                    }).ToList();
                int pageSize = 10;
                var currentAppointments = appointments.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Cập nhật các giá trị phân trang cho ViewBag


                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)appointments.Count() / pageSize);
                ViewBag.STT = ((page - 1) * pageSize) + 1;
                return View(currentAppointments);

            }
            else
            {
                Debug.WriteLine("Session[\"EmployeeId\"] is null.");
                string redirectUrl = $"https://localhost:44371/Authentication/EmployeeSignIn";
                return Redirect(redirectUrl);
            }
        }
    }
}