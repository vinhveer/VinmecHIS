using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doctor.Models.Views
{
    public class PrescriptionViewModel
    {
        public long MedicalRecordId { get; set; }
        public int PrescribedQuantity { get; set; }
        public string Dosage { get; set; }
        public string MedicineName { get; set; }
        public string PatientName { get; set; }
        public DateTime ExaminationDate { get; set; }
    }
}