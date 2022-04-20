using System.ComponentModel.DataAnnotations;

namespace ProbneKolokwium.DTO;

public class Patient
{
    public int IdPatient { get; set; }
    
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [MaxLength(100)]
    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }
}