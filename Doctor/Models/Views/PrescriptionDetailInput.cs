using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doctor.Models.Views
{
    public class PrescriptionDetailInput
    {
        public long MedicineId { get; set; } // ID thuốc
        public int PrescribedQuantity { get; set; } // Số lượng thuốc được kê
        public string Note { get; set; } // Ghi chú
        public string Dosage { get; set; } // Liều lượng
    }
}