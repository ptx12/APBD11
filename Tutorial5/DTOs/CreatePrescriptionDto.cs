namespace Tutorial5.DTOs;

public class CreatePrescriptionDto
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }

    public PatientDto Patient { get; set; }
    public int DoctorId { get; set; }

    public List<PrescriptionMedicamentDto> Medicaments { get; set; }
}