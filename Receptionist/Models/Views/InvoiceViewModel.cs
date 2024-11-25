using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Receptionist.Models.Views
{
    public class InvoiceViewModel
    {
        public long InvoiceId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string EmployeeName { get; set; }
        public string PatientName { get; set; }
    }
}