using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharmacy.Models.Data
{
    public class PrecsriptionViewModel
    {
        public long MEDICAL_RECORD_ID { get; set; }
        public string PatientName { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public List<MedicineDetail> Medicines { get; set; } // Danh sách thuốc kê
        public bool IsConfirmed { get; set; } = false;
    }

    public class MedicineDetail 
    {
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public int Quantity { get; set; }
    }

}