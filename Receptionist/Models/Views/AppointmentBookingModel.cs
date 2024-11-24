using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Receptionist.Models.Views
{
    public class AppointmentBookingModel
    {
        public string Specialty { get; set; }
        public string AppointmentDate { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public DateTime DATE_OF_BIRTH { get; set; }

        public string GENDER { get; set; }

        public string PATIENT_EMAIL { get; set; }

        public string PATIENT_PHONE { get; set; }

        public string PATIENT_ADDRESS { get; set; }

        public string EMERGENCY_CONTACT { get; set; }

        public DateTime REGISTRATION_DATE { get; set; }
    }
}