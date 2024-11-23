using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Receptionist.Models.Data
{
    public partial class ReceiptionistDbContext : DbContext
    {
        public ReceiptionistDbContext()
            : base("name=ReceiptionistDbContext")
        {
        }

        public virtual DbSet<APPOINTMENT> APPOINTMENTs { get; set; }
        public virtual DbSet<EMPLOYEE> EMPLOYEEs { get; set; }
        public virtual DbSet<EMPLOYEEACCOUNT> EMPLOYEEACCOUNTs { get; set; }
        public virtual DbSet<INVOICE> INVOICEs { get; set; }
        public virtual DbSet<MEDICALRECORD> MEDICALRECORDs { get; set; }
        public virtual DbSet<MEDICINE> MEDICINEs { get; set; }
        public virtual DbSet<MEDICINEORDER> MEDICINEORDERs { get; set; }
        public virtual DbSet<MEDICINEORDERDETAIL> MEDICINEORDERDETAILs { get; set; }
        public virtual DbSet<PATIENT> PATIENTs { get; set; }
        public virtual DbSet<PATIENTACCOUNT> PATIENTACCOUNTs { get; set; }
        public virtual DbSet<PRESCRIPTION> PRESCRIPTIONs { get; set; }
        public virtual DbSet<PRESCRIPTIONDETAIL> PRESCRIPTIONDETAILs { get; set; }
        public virtual DbSet<SUPPLIER> SUPPLIERs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EMPLOYEE>()
                .Property(e => e.FIRST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE>()
                .Property(e => e.LAST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE>()
                .Property(e => e.GENDER)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE>()
                .Property(e => e.EMPLOYEE_EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE>()
                .Property(e => e.EMPLOYEE_PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE>()
                .Property(e => e.EMPLOYEE_ADDRESS)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE>()
                .Property(e => e.ROLE_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE>()
                .Property(e => e.EMPLOYEE_ROOM)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE>()
                .Property(e => e.DEPARTMENT)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE>()
                .HasMany(e => e.APPOINTMENTs)
                .WithRequired(e => e.EMPLOYEE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EMPLOYEE>()
                .HasOptional(e => e.EMPLOYEEACCOUNT)
                .WithRequired(e => e.EMPLOYEE);

            modelBuilder.Entity<EMPLOYEE>()
                .HasMany(e => e.INVOICEs)
                .WithRequired(e => e.EMPLOYEE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EMPLOYEE>()
                .HasMany(e => e.MEDICALRECORDs)
                .WithRequired(e => e.EMPLOYEE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EMPLOYEE>()
                .HasMany(e => e.MEDICINEORDERs)
                .WithRequired(e => e.EMPLOYEE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EMPLOYEEACCOUNT>()
                .Property(e => e.EMPLOYEE_USERNAME)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEEACCOUNT>()
                .Property(e => e.EMPLOYEE_PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<INVOICE>()
                .Property(e => e.PAYMENT_METHOD)
                .IsUnicode(false);

            modelBuilder.Entity<MEDICALRECORD>()
                .Property(e => e.DIAGNOSIS)
                .IsUnicode(false);

            modelBuilder.Entity<MEDICALRECORD>()
                .Property(e => e.TREATMENT)
                .IsUnicode(false);

            modelBuilder.Entity<MEDICALRECORD>()
                .Property(e => e.ADDITIONAL_NOTES)
                .IsUnicode(false);

            modelBuilder.Entity<MEDICALRECORD>()
                .Property(e => e.HOSPITAL_FEES)
                .HasPrecision(10, 2);

            modelBuilder.Entity<MEDICALRECORD>()
                .HasOptional(e => e.INVOICE)
                .WithRequired(e => e.MEDICALRECORD);

            modelBuilder.Entity<MEDICALRECORD>()
                .HasOptional(e => e.PRESCRIPTION)
                .WithRequired(e => e.MEDICALRECORD);

            modelBuilder.Entity<MEDICINE>()
                .Property(e => e.MEDICINE_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<MEDICINE>()
                .Property(e => e.UNIT)
                .IsUnicode(false);

            modelBuilder.Entity<MEDICINE>()
                .Property(e => e.PRICE)
                .HasPrecision(10, 2);

            modelBuilder.Entity<MEDICINE>()
                .HasMany(e => e.MEDICINEORDERDETAILs)
                .WithRequired(e => e.MEDICINE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MEDICINE>()
                .HasMany(e => e.PRESCRIPTIONDETAILs)
                .WithRequired(e => e.MEDICINE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MEDICINEORDER>()
                .HasMany(e => e.MEDICINEORDERDETAILs)
                .WithRequired(e => e.MEDICINEORDER)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PATIENT>()
                .Property(e => e.FIRST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<PATIENT>()
                .Property(e => e.LAST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<PATIENT>()
                .Property(e => e.GENDER)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PATIENT>()
                .Property(e => e.PATIENT_EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<PATIENT>()
                .Property(e => e.PATIENT_PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<PATIENT>()
                .Property(e => e.PATIENT_ADDRESS)
                .IsUnicode(false);

            modelBuilder.Entity<PATIENT>()
                .Property(e => e.EMERGENCY_CONTACT)
                .IsUnicode(false);

            modelBuilder.Entity<PATIENT>()
                .HasMany(e => e.APPOINTMENTs)
                .WithRequired(e => e.PATIENT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PATIENT>()
                .HasMany(e => e.MEDICALRECORDs)
                .WithRequired(e => e.PATIENT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PATIENT>()
                .HasOptional(e => e.PATIENTACCOUNT)
                .WithRequired(e => e.PATIENT);

            modelBuilder.Entity<PATIENTACCOUNT>()
                .Property(e => e.PATIENT_USERNAME)
                .IsUnicode(false);

            modelBuilder.Entity<PATIENTACCOUNT>()
                .Property(e => e.PATIENT_PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<PRESCRIPTION>()
                .Property(e => e.NOTES)
                .IsUnicode(false);

            modelBuilder.Entity<PRESCRIPTION>()
                .HasMany(e => e.PRESCRIPTIONDETAILs)
                .WithRequired(e => e.PRESCRIPTION)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRESCRIPTIONDETAIL>()
                .Property(e => e.DOSAGE)
                .IsUnicode(false);

            modelBuilder.Entity<SUPPLIER>()
                .Property(e => e.SUPPLIER_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<SUPPLIER>()
                .Property(e => e.SUPPLIER_ADDRESS)
                .IsUnicode(false);

            modelBuilder.Entity<SUPPLIER>()
                .Property(e => e.CONTACT_EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<SUPPLIER>()
                .Property(e => e.CONTACT_PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<SUPPLIER>()
                .HasMany(e => e.MEDICINEs)
                .WithRequired(e => e.SUPPLIER)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SUPPLIER>()
                .HasMany(e => e.MEDICINEORDERs)
                .WithRequired(e => e.SUPPLIER)
                .WillCascadeOnDelete(false);
        }
    }
}