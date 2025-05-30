using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial5.Models;

[Table("Prescription_Medicament")]
public class PrescriptionMedicament
{
    [Key, Column(Order = 0)]
    public int IdMedicament { get; set; }

    [Key, Column(Order = 1)]
    public int IdPrescription { get; set; }

    [Required]
    public int? Dose { get; set; }

    [Required]
    [MaxLength(100)]
    public string Description { get; set; }

    public Medicament Medicament { get; set; }
    public Prescription Prescription { get; set; }
}