namespace Tutorial5.DTOs;

public class PrescriptionDto
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public string DoctorFirstName { get; set; }
    public string DoctorLastName { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
}