namespace Authentication.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRESCRIPTIONDETAIL")]
    public partial class PRESCRIPTIONDETAIL
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MEDICAL_RECORD_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MEDICINE_ID { get; set; }

        public int PRESCRIBED_QUANTITY { get; set; }

        [Required]
        [StringLength(255)]
        public string DOSAGE { get; set; }

        public virtual MEDICINE MEDICINE { get; set; }

        public virtual PRESCRIPTION PRESCRIPTION { get; set; }
    }
}
