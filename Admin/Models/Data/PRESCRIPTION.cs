namespace Admin.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRESCRIPTION")]
    public partial class PRESCRIPTION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRESCRIPTION()
        {
            PRESCRIPTIONDETAIL = new HashSet<PRESCRIPTIONDETAIL>();
        }

        [Key]
        public long MEDICAL_RECORD_ID { get; set; }

        public DateTime PRESCRIPTION_DATE { get; set; }

        [StringLength(255)]
        public string NOTES { get; set; }

        public virtual MEDICALRECORD MEDICALRECORD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRESCRIPTIONDETAIL> PRESCRIPTIONDETAIL { get; set; }
    }
}
