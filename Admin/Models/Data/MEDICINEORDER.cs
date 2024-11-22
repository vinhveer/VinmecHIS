namespace Admin.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MEDICINEORDER")]
    public partial class MEDICINEORDER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MEDICINEORDER()
        {
            MEDICINEORDERDETAIL = new HashSet<MEDICINEORDERDETAIL>();
        }

        [Key]
        public long MEDICINE_ORDER_ID { get; set; }

        public long EMPLOYEE_ID { get; set; }

        public long SUPPLIER_ID { get; set; }

        public DateTime ORDER_DATE { get; set; }

        public virtual EMPLOYEE EMPLOYEE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEDICINEORDERDETAIL> MEDICINEORDERDETAIL { get; set; }

        public virtual SUPPLIER SUPPLIER { get; set; }
    }
}
