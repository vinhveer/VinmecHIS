using Receptionist.Models.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Receptionist.Models.Views;
using Receptionist.Filters;

namespace Receptionist.Controllers
{
    [Authenticate]
    public class AppointMentController : Controller
    {
        // GET: AppointMent
        private readonly ReceiptionistDbContext _db = new ReceiptionistDbContext();
        public ActionResult Index(int page = 1)
        {
            if (Session["EmployeeId"] != null)
            {
                var appointments = (from patient in _db.PATIENTs
                                    join appointment in _db.APPOINTMENTs on patient.PATIENT_ID equals appointment.PATIENT_ID
                                    join employee in _db.EMPLOYEEs on appointment.EMPLOYEE_ID equals employee.EMPLOYEE_ID
                                    orderby appointment.APPOINTMENT_TIME.Year,
                                    appointment.APPOINTMENT_TIME.Month,
                                    appointment.APPOINTMENT_TIME.Day,
                                    appointment.APPOINTMENT_TIME.Hour,
                                    appointment.APPOINTMENT_TIME.Minute,
                                    appointment.APPOINTMENT_TIME.Second
                                    select new AppointmentViewModel
                                    {
                                        AppointmentId = appointment.APPOINTMENT_ID,
                                        PatientName = patient.LAST_NAME + " " + patient.FIRST_NAME,
                                        PatientGender = patient.GENDER,
                                        EmployeeName = employee.LAST_NAME + " " + employee.FIRST_NAME,
                                        Speciality = employee.DEPARTMENT,
                                        AppointmentDate = appointment.APPOINTMENT_TIME,
                                        Room = employee.EMPLOYEE_ROOM,
                                    }).ToList();
                int pageSize = 10;
                var currentAppointments = appointments.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Cập nhật các giá trị phân trang cho ViewBag
                foreach (var appointment in appointments)
                {
                    Debug.WriteLine($"AppointmentId: {appointment.AppointmentId},PatientName: {appointment.PatientName}, PatientGender: {appointment.PatientGender}, EmployeeName: {appointment.EmployeeName}, Speciality: {appointment.Speciality}, AppointmentDate: {appointment.AppointmentDate}, Room: {appointment.Room}");
                }

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)appointments.Count() / pageSize);
                ViewBag.STT = ((page - 1) * pageSize) + 1;
                return View(currentAppointments);
            }
            else
            {
                Debug.WriteLine("Session[\"PatientId\"] is null.");
                string redirectUrl = $"https://localhost:44371/Authentication/EmployeeSignIn";
                return Redirect(redirectUrl);
            }

        }
        public ActionResult BookingApointmnet()
        {
            if (Session["EmployeeId"] != null)
            {
                var specialitys = _db.EMPLOYEEs
               .Where(e => e.ROLE_NAME == "DOCTOR")
               .Select(e => e.DEPARTMENT)
               .Distinct()
               .ToList();
                DateTime currentDate = DateTime.Now;

                // Số ngày bạn muốn lấy, ví dụ: 7 ngày (1 tuần)
                int numberOfDays = 7;

                // Danh sách chứa các ngày từ thứ Hai đến thứ Sáu
                List<DateTime> weekDays = new List<DateTime>();

                // Duyệt qua các ngày trong tuần
                for (int i = 0; i < numberOfDays; i++)
                {
                    DateTime nextDate = currentDate.AddDays(i);

                    // Kiểm tra xem ngày đó có phải từ thứ Hai đến thứ Sáu không
                    if (nextDate.DayOfWeek >= DayOfWeek.Monday && nextDate.DayOfWeek <= DayOfWeek.Friday)
                    {
                        weekDays.Add(nextDate);
                    }
                }
                ViewBag.WeekDays = weekDays;
                return View(specialitys);
            }
            else
            {
                Debug.WriteLine("Session[\"EmployeeId\"] is null.");
                string redirectUrl = $"https://localhost:44371/Authentication/EmployeeSignIn";
                return Redirect(redirectUrl);
            }
        }
        [HttpPost]
        public ActionResult BookingApointMent(string lastname, string phone, string address, string firstname, string dob, string email, string emergency_contact, string specialty, bool? genderMale, bool? genderFemale, string appointmentDate)
        {


            string gender = "";
            if (genderFemale == true)
            {
                gender = "F";
            }
            else
            {
                gender = "M";
            }
            Debug.WriteLine("DOB:" + DateTime.Parse(dob));
            Debug.WriteLine("RD:" + DateTime.Now);
            var bookingModel = new AppointmentBookingModel
            {
                Specialty = specialty,
                AppointmentDate = appointmentDate,
                FIRST_NAME = firstname,
                LAST_NAME = lastname,
                DATE_OF_BIRTH = DateTime.Parse(dob),
                PATIENT_EMAIL = email,
                PATIENT_PHONE = phone,
                PATIENT_ADDRESS = address,
                EMERGENCY_CONTACT = emergency_contact,
                REGISTRATION_DATE = DateTime.Now,
                GENDER = gender,
            };

            // Chuyển hướng đến phương thức GET khác để hiển thị thông tin
            return RedirectToAction("AvailableAppointments", bookingModel);
        }

        public ActionResult AvailableAppointments(AppointmentBookingModel bookingModel)
        {
            if (string.IsNullOrEmpty(bookingModel.AppointmentDate) ||
                string.IsNullOrEmpty(bookingModel.Specialty))
            {

                // Chuyển hướng về trang trước đó hoặc một trang khác
                return RedirectToAction("BookingApointmnet", "Appointment");
            }
            // Lọc các bác sĩ thuộc chuyên khoa Cardiology
            var employees = _db.EMPLOYEEs
                              .Where(e => e.DEPARTMENT == bookingModel.Specialty)
                              .ToList();

            // Xác định các giờ có sẵn trong ngày từ 7h00 đến 18h00
            var availableHours = new List<DateTime>
            {
                DateTime.Parse("07:00"),
                DateTime.Parse("08:00"),
                DateTime.Parse("09:00"),
                DateTime.Parse("10:00"),
                DateTime.Parse("11:00"),
                DateTime.Parse("13:00"),
                DateTime.Parse("14:00"),
                DateTime.Parse("15:00"),
                DateTime.Parse("16:00"),
                DateTime.Parse("17:00")
            };

            // Tạo một danh sách chứa thông tin bác sĩ và các giờ trống
            var availableAppointments = new List<AvailableDoctorViewModel>();
            DateTime selectedDate = DateTime.Parse(bookingModel.AppointmentDate);
            foreach (var employee in employees)
            {
                // Lọc các cuộc hẹn của bác sĩ trong ngày đã chọn
                var appointments = _db.APPOINTMENTs
                .Where(a => a.EMPLOYEE_ID == employee.EMPLOYEE_ID &&
                            a.APPOINTMENT_TIME.Year == selectedDate.Year &&
                            a.APPOINTMENT_TIME.Month == selectedDate.Month &&
                            a.APPOINTMENT_TIME.Day == selectedDate.Day)
                .ToList();
                var bookedHours = appointments
                .Select(a => a.APPOINTMENT_TIME.TimeOfDay)
                .ToList();
                // Lấy các giờ đã có cuộc hẹn trong ngày của bác sĩ 

                // Tìm các giờ trống bằng cách loại bỏ các giờ đã có cuộc hẹn

                var freeHours = availableHours
                .Where(hour => !bookedHours.Contains(hour.TimeOfDay))
                .ToList();
                // Thêm bác sĩ và các giờ trống vào kết quả
                if (!appointments.Any())
                {
                    freeHours = availableHours.ToList();
                }

                // Thêm thông tin bác sĩ vào danh sách kết quả
                availableAppointments.Add(new AvailableDoctorViewModel
                {
                    EmployeeId = employee.EMPLOYEE_ID,
                    EmployeeName = $"{employee.FIRST_NAME} {employee.LAST_NAME}",
                    FreeHours = freeHours,
                    EmployeeRoom = employee.EMPLOYEE_ROOM,
                });
            }
            // Trả về view với danh sách bác sĩ và giờ trống
            ViewBag.firstName = bookingModel.FIRST_NAME;
            ViewBag.lastName = bookingModel.LAST_NAME;
            ViewBag.DateOfBirth = bookingModel.DATE_OF_BIRTH;
            ViewBag.Phone = bookingModel.PATIENT_PHONE;
            ViewBag.Email = bookingModel.PATIENT_EMAIL;
            ViewBag.Address = bookingModel.PATIENT_ADDRESS;
            ViewBag.EmergencyContact = bookingModel.EMERGENCY_CONTACT;
            ViewBag.RegisTrationDate = bookingModel.REGISTRATION_DATE;
            ViewBag.Gender = bookingModel.GENDER;
            ViewBag.AppointmentDate = DateTime.Parse(bookingModel.AppointmentDate);
            return View(availableAppointments);
        }
        public ActionResult ConfirmBooking()
        {
            // Lấy thông tin từ TempData
            ViewBag.Lastname = TempData["Lastname"];
            ViewBag.Firstname = TempData["Firstname"];
            ViewBag.Phone = TempData["Phone"];
            ViewBag.Address = TempData["Address"];
            ViewBag.Dob = TempData["Dob"];
            ViewBag.Email = TempData["Email"];
            ViewBag.EmergencyContact = TempData["EmergencyContact"];
            ViewBag.Specialty = TempData["Specialty"];
            ViewBag.Gender = TempData["Gender"];
            ViewBag.AppointmentDate = TempData["AppointmentDate"];

            // Trả về view để hiển thị thông tin
            return View();
        }
        // GET: AppointMent/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: AppointMent/Create
        [HttpPost]
        public ActionResult AvailableAppointments(FormCollection form)
        {
            try
            {
                foreach (var key in form.AllKeys)
                {
                    Debug.WriteLine($"{key}: {form[key]}");
                }
                // Get the form data
                string firstname = form["PatientFirstName"];
                string lastname = form["PatientLastName"];
                DateTime dob = DateTime.Parse(form["PatientDateOfBirth"]);
                string phone = form["PatientPhone"];
                DateTime RD = DateTime.Parse(form["PatientRegisTrationDate"]);
                string address = form["PatientAddress"];
                string email = form["PatientEmail"];
                string emergency_contact = form["PatientEmergencyContact"];
                string gender = form["PatientGender"];
                long employeeId = Convert.ToInt64(form["selectedDoctor"]); // Convert employee_id to bigint
                DateTime AT = DateTime.Parse(form["selectedHour"]);
                DateTime appointmentTime = DateTime.Parse(form["PatientAppointmentDate"]) + AT.TimeOfDay;
                Debug.WriteLine($"Dob: {dob}");
                Debug.WriteLine($"Dob Type: {dob.GetType()}");
                Debug.WriteLine($"RD: {RD}");
                Debug.WriteLine($"RD Type: {RD.GetType()}");
                Debug.WriteLine($"AppointmentTime: {appointmentTime}");
                Debug.WriteLine($"AppointmentTime Type: {appointmentTime.GetType()}");



                // Create a new instance of the model
                var patient = new PATIENT
                {
                    FIRST_NAME = firstname,
                    LAST_NAME = lastname,
                    PATIENT_PHONE = phone,
                    PATIENT_ADDRESS = address,
                    DATE_OF_BIRTH = dob,
                    PATIENT_EMAIL = email,
                    EMERGENCY_CONTACT = emergency_contact,
                    REGISTRATION_DATE = RD,
                    GENDER = gender
                };

                // Add the patient to the database
                _db.PATIENTs.Add(patient);
                _db.SaveChanges();

                // Get the patient ID
                long patientId = patient.PATIENT_ID;
                Debug.WriteLine("patientID:", patientId);
                // Create a new instance of the appointment model
                var appointment = new APPOINTMENT
                {
                    EMPLOYEE_ID = employeeId,
                    PATIENT_ID = patientId,
                    APPOINTMENT_TIME = appointmentTime
                };

                // Add the appointment to the database
                _db.APPOINTMENTs.Add(appointment);
                _db.SaveChanges();

                // Redirect to the index page
                return RedirectToAction("Index");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CreateFailed: " + ex.Message);
                // Handle any errors and return the create view
                return View();
            }
        }

        // POST: AppointMent/Create
        [HttpPost]
        public ActionResult BookingAppointment(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AppointMent/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AppointMent/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AppointMent/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AppointMent/Delete/5
        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {

            try
            {
                if (form["AppointmentId"] != null)
                {
                    var appointmentId = Convert.ToInt64(form["AppointmentId"]);
                    var appointment = _db.APPOINTMENTs.Find(appointmentId);
                    if (appointment != null)
                    {
                        _db.APPOINTMENTs.Remove(appointment);
                        _db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                {
                    // Handle the case where AppointmentId is null
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
