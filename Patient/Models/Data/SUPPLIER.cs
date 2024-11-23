namespace Patient.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SUPPLIER")]
    public partial class SUPPLIER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUPPLIER()
        {
            MEDICINEs = new HashSet<MEDICINE>();
            MEDICINEORDERs = new HashSet<MEDICINEORDER>();
        }

        [Key]
        public long SUPPLIER_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string SUPPLIER_NAME { get; set; }

        [Required]
        [StringLength(255)]
        public string SUPPLIER_ADDRESS { get; set; }

        [Required]
        [StringLength(50)]
        public string CONTACT_EMAIL { get; set; }

        [Required]
        [StringLength(15)]
        public string CONTACT_PHONE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEDICINE> MEDICINEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEDICINEORDER> MEDICINEORDERs { get; set; }
    }
}
