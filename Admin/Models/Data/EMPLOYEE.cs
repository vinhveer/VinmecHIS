namespace Admin.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EMPLOYEE")]
    public partial class EMPLOYEE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EMPLOYEE()
        {
            APPOINTMENT = new HashSet<APPOINTMENT>();
            INVOICE = new HashSet<INVOICE>();
            MEDICALRECORD = new HashSet<MEDICALRECORD>();
            MEDICINEORDER = new HashSet<MEDICINEORDER>();
        }

        [Key]
        public long EMPLOYEE_ID { get; set; }

        [Required]
        [StringLength(30)]
        public string FIRST_NAME { get; set; }

        [Required]
        [StringLength(50)]
        public string LAST_NAME { get; set; }

        public DateTime DATE_OF_BIRTH { get; set; }

        [Required]
        [StringLength(1)]
        public string GENDER { get; set; }

        [Required]
        [StringLength(50)]
        public string EMPLOYEE_EMAIL { get; set; }

        [Required]
        [StringLength(15)]
        public string EMPLOYEE_PHONE { get; set; }

        [Required]
        [StringLength(255)]
        public string EMPLOYEE_ADDRESS { get; set; }

        [Required]
        [StringLength(100)]
        public string ROLE_NAME { get; set; }

        public DateTime HIRE_DATE { get; set; }

        [Required]
        [StringLength(100)]
        public string EMPLOYEE_ROOM { get; set; }

        [Required]
        [StringLength(100)]
        public string DEPARTMENT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APPOINTMENT> APPOINTMENT { get; set; }

        public virtual EMPLOYEEACCOUNT EMPLOYEEACCOUNT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INVOICE> INVOICE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEDICALRECORD> MEDICALRECORD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEDICINEORDER> MEDICINEORDER { get; set; }
    }
}
