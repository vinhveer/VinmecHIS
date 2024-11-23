namespace Authentication.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APPOINTMENT")]
    public partial class APPOINTMENT
    {
        [Key]
        public long APPOINTMENT_ID { get; set; }

        public long PATIENT_ID { get; set; }

        public long EMPLOYEE_ID { get; set; }

        public DateTime APPOINTMENT_TIME { get; set; }

        public virtual EMPLOYEE EMPLOYEE { get; set; }

        public virtual PATIENT PATIENT { get; set; }
    }
}
