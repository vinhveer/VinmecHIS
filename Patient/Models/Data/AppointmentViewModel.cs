using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patient.Models.Data
{
    public class AppointmentViewModel
    {
        public long AppointmentId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePhone { get; set; }
        public string Speciality { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Room { get; set; }
    }
}