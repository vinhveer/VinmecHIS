namespace Patient.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PATIENTACCOUNT")]
    public partial class PATIENTACCOUNT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PATIENT_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string PATIENT_USERNAME { get; set; }

        [Required]
        [StringLength(255)]
        public string PATIENT_PASSWORD { get; set; }

        public virtual PATIENT PATIENT { get; set; }
    }
}
