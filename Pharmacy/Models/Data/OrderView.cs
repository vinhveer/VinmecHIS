using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharmacy.Models.Data
{
    public class OrderView
    {
        public long MEDICINE_ORDER_ID { get; set; }
        public string MEDICINE_NAME { get; set; }
        public string Suplier_name { get; set; }

        public string EmployeeName { get; set; }
        

        public DateTime REGISTRATION_DATE { get; set; }
    }
}