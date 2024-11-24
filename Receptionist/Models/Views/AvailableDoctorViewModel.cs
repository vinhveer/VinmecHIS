using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Receptionist.Models.Views
{
    public class AvailableDoctorViewModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeRoom { get; set; }
        public List<DateTime> FreeHours { get; set; }
    }
}