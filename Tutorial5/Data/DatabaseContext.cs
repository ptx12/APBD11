using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Tutorial5.Models;

namespace Tutorial5.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }


        protected DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(p =>
            {
                p.ToTable("Patient");
                p.HasKey(e => e.IdPatient);
                p.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                p.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                p.Property(e => e.Birthdate).IsRequired();
            });

            modelBuilder.Entity<Doctor>(d =>
            {
                d.ToTable("Doctor");
                d.HasKey(e => e.IdDoctor);
                d.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                d.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                d.Property(e => e.Email).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Medicament>(m =>
            {
                m.ToTable("Medicament");
                m.HasKey(e => e.IdMedicament);
                m.Property(e => e.Name).HasMaxLength(100).IsRequired();
                m.Property(e => e.Description).HasMaxLength(100).IsRequired();
                m.Property(e => e.Type).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Prescription>(pr =>
            {
                pr.ToTable("Prescription");
                pr.HasKey(e => e.IdPrescription);
                pr.Property(e => e.Date).IsRequired();
                pr.Property(e => e.DueDate).IsRequired();

                pr.HasOne(e => e.Patient)
                  .WithMany(p => p.Prescriptions)
                  .HasForeignKey(e => e.IdPatient);

                pr.HasOne(e => e.Doctor)
                  .WithMany(d => d.Prescriptions)
                  .HasForeignKey(e => e.IdDoctor);
            });

            modelBuilder.Entity<PrescriptionMedicament>(pm =>
            {
                pm.ToTable("Prescription_Medicament");
                pm.HasKey(e => new { e.IdMedicament, e.IdPrescription });

                pm.Property(e => e.Dose).IsRequired(false);
                pm.Property(e => e.Description).HasMaxLength(100).IsRequired();

                pm.HasOne(e => e.Medicament)
                  .WithMany(m => m.PrescriptionMedicaments)
                  .HasForeignKey(e => e.IdMedicament);

                pm.HasOne(e => e.Prescription)
                  .WithMany(p => p.PrescriptionMedicaments)
                  .HasForeignKey(e => e.IdPrescription);
            });
        }
    }
}
