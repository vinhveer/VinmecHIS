namespace Doctor.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EMPLOYEEACCOUNT")]
    public partial class EMPLOYEEACCOUNT
    {
        [Key]
        public long EMPLOYEE_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string EMPLOYEE_USERNAME { get; set; }

        [Required]
        [StringLength(255)]
        public string EMPLOYEE_PASSWORD { get; set; }

        public virtual EMPLOYEE EMPLOYEE { get; set; }
    }
}
