using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;
using Tutorial5.DTOs;
using Tutorial5.Models;

namespace Tutorial5.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddPrescriptionAsync(CreatePrescriptionDto dto)
    {
        if (dto.Medicaments.Count > 10)
            throw new Exception("too manuy medicaments (max 10)");

        if (dto.DueDate < dto.Date)
            throw new Exception("due date must be greater than or equal to date");

        var doctor = await _context.Doctors.FindAsync(dto.DoctorId);
        if (doctor == null)
            throw new Exception("doctor not found");

        var patient = await _context.Patients
            .FirstOrDefaultAsync(p =>
                p.FirstName == dto.Patient.FirstName &&
                p.LastName == dto.Patient.LastName &&
                p.Birthdate == dto.Patient.Birthdate);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                Birthdate = dto.Patient.Birthdate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        foreach (var med in dto.Medicaments)
        {
            var medicament = await _context.Medicaments.FindAsync(med.MedicamentId);
            if (medicament == null)
                throw new Exception($"medicament {med.MedicamentId} not found");
        }

        var prescription = new Prescription
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            IdDoctor = doctor.IdDoctor,
            IdPatient = patient.IdPatient
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        foreach (var med in dto.Medicaments)
        {
            _context.PrescriptionMedicaments.Add(new PrescriptionMedicament
            {
                IdPrescription = prescription.IdPrescription,
                IdMedicament = med.MedicamentId,
                Dose = med.Dose,
                Description = med.Description
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task<GetPatientDto> GetPatientAsync(int idPatient)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == idPatient);

        if (patient == null)
            throw new Exception("patient not found");

        return new GetPatientDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .OrderBy(pr => pr.DueDate)
                .Select(pr => new PrescriptionDto
                {
                    Date = pr.Date,
                    DueDate = pr.DueDate,
                    DoctorFirstName = pr.Doctor.FirstName,
                    DoctorLastName = pr.Doctor.LastName,
                    Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentDto
                    {
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
                        Type = pm.Medicament.Type,
                        Dose = pm.Dose
                    }).ToList()
                })
                .ToList()
        };
    }
}
