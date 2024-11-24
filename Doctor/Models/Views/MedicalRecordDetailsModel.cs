using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doctor.Models.Views
{
    public class MedicalRecordDetailsModel
    {
        public long MEDICAL_RECORD_ID { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime EXAMINATION_DATE { get; set; }
        public string DIAGNOSIS { get; set; }
        public string TREATMENT { get; set; }
        public string ADDITIONAL_NOTES { get; set; }
        public decimal? HOSPITAL_FEES { get; set; }
    }
}