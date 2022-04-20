using Microsoft.AspNetCore.Mvc;
using ProbneKolokwium.DTO;

namespace ProbneKolokwium;

public interface IDbService
{
    Task<IEnumerable<Prescription>> GetPrescriptionsAsync();
    Task<IEnumerable<Prescription>> GetPrescriptionsByPatientAsync(string lastname);
    Task<int> AddMedicamentsToPrescriptionsAsync(int prescriptionId, IEnumerable<PrescriptionMedicament> prescriptions);
}