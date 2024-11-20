namespace Authentication.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MEDICINE")]
    public partial class MEDICINE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MEDICINE()
        {
            MEDICINEORDERDETAILs = new HashSet<MEDICINEORDERDETAIL>();
            PRESCRIPTIONDETAILs = new HashSet<PRESCRIPTIONDETAIL>();
        }

        [Key]
        public long MEDICINE_ID { get; set; }

        public long SUPPLIER_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string MEDICINE_NAME { get; set; }

        [Required]
        [StringLength(30)]
        public string UNIT { get; set; }

        public int STOCK_QUANTITY { get; set; }

        public DateTime EXPIRATION_DATE { get; set; }

        public decimal PRICE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEDICINEORDERDETAIL> MEDICINEORDERDETAILs { get; set; }

        public virtual SUPPLIER SUPPLIER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRESCRIPTIONDETAIL> PRESCRIPTIONDETAILs { get; set; }
    }
}
