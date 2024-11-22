using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doctor.Models.Views
{
    public class MedicalRecordViewModel
    {
        public long MEDICAL_RECORD_ID { get; set; }
        public string PatientName { get; set; }
        public DateTime EXAMINATION_DATE { get; set; }
        public string DIAGNOSIS { get; set; }
        public long PATIENT_ID { get; set; }
        public string DoctorName { get; set; }
    }
}