namespace Receptionist.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("INVOICE")]
    public partial class INVOICE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MEDICAL_RECORD_ID { get; set; }

        public long EMPLOYEE_ID { get; set; }

        public DateTime PAYMENT_DATE { get; set; }

        [Required]
        [StringLength(100)]
        public string PAYMENT_METHOD { get; set; }

        public virtual EMPLOYEE EMPLOYEE { get; set; }

        public virtual MEDICALRECORD MEDICALRECORD { get; set; }
    }
}
