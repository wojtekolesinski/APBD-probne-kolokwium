using Microsoft.AspNetCore.Mvc;
using ProbneKolokwium.DTO;

namespace ProbneKolokwium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionMedicamentController : ControllerBase
    {
        private IDbService _dbService;

        public PrescriptionMedicamentController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost("{prescriptionId:int}")]
        public async Task<IActionResult> AddMedicamentToPrescriptionAsync(int prescriptionId, IEnumerable<PrescriptionMedicament> medicaments)
        {
            var affected = await _dbService.AddMedicamentsToPrescriptionsAsync(prescriptionId, medicaments);
            if (affected == 0)
            {
                return BadRequest();
            }

            if (affected == medicaments.Count())
            {
                foreach (var prescriptionMedicament in medicaments)
                {
                    prescriptionMedicament.IdPrescription = prescriptionId;
                }

                return Created("", medicaments);
            }

            return Problem();
        }
    }
}
