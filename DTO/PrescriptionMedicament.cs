using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ProbneKolokwium.DTO;

public class PrescriptionMedicament
{
    public int IdMedicament { get; set; }

    [AllowNull]
    public int IdPrescription { get; set; }

    public int Dose { get; set; }
    
    [MaxLength(100)]
    public string Details { get; set; }
}