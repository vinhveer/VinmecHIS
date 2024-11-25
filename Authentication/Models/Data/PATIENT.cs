namespace Authentication.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PATIENT")]
    public partial class PATIENT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PATIENT()
        {
            APPOINTMENTs = new HashSet<APPOINTMENT>();
            MEDICALRECORDs = new HashSet<MEDICALRECORD>();
        }

        [Key]
        public long PATIENT_ID { get; set; }

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
        public string PATIENT_EMAIL { get; set; }

        [Required]
        [StringLength(15)]
        public string PATIENT_PHONE { get; set; }

        [Required]
        [StringLength(255)]
        public string PATIENT_ADDRESS { get; set; }

        [StringLength(15)]
        public string EMERGENCY_CONTACT { get; set; }

        public DateTime REGISTRATION_DATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APPOINTMENT> APPOINTMENTs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEDICALRECORD> MEDICALRECORDs { get; set; }

        public virtual PATIENTACCOUNT PATIENTACCOUNT { get; set; }
    }
}
