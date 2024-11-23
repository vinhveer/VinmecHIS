namespace Patient.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MEDICALRECORD")]
    public partial class MEDICALRECORD
    {
        [Key]
        public long MEDICAL_RECORD_ID { get; set; }

        public long EMPLOYEE_ID { get; set; }

        public long PATIENT_ID { get; set; }

        public DateTime EXAMINATION_DATE { get; set; }

        [Required]
        [StringLength(255)]
        public string DIAGNOSIS { get; set; }

        [Required]
        [StringLength(255)]
        public string TREATMENT { get; set; }

        public DateTime? FOLLOW_UP_DATE { get; set; }

        [StringLength(255)]
        public string ADDITIONAL_NOTES { get; set; }

        public decimal HOSPITAL_FEES { get; set; }

        public virtual EMPLOYEE EMPLOYEE { get; set; }

        public virtual INVOICE INVOICE { get; set; }

        public virtual PATIENT PATIENT { get; set; }

        public virtual PRESCRIPTION PRESCRIPTION { get; set; }
    }
}
