using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doctor.Models.Data
{
    public class AppointmentViewModel
    {
        public long AppointmentId { get; set; }
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientGender{ get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Room { get; set; }
    }
}