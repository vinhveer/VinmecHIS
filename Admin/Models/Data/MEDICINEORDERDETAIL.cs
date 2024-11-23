namespace Admin.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MEDICINEORDERDETAIL")]
    public partial class MEDICINEORDERDETAIL
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MEDICINE_ORDER_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MEDICINE_ID { get; set; }

        public int ORDERED_QUANTITY { get; set; }

        public virtual MEDICINE MEDICINE { get; set; }

        public virtual MEDICINEORDER MEDICINEORDER { get; set; }
    }
}
