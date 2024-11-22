using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Receptionist.Models.Data
{
    public class AppointmentViewModel
    {
        public long AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string PatientGender { get; set; }
        public string EmployeeName { get; set; }
        public string Speciality { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Room { get; set; }
    }
    public class SpecialtyViewModel
    {
        public string Department { get; set; }
    }
}