using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProbneKolokwium.DTO;

namespace ProbneKolokwium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private IDbService _dbService;

        public PrescriptionController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPrescriptions(string? lastname)
        {
            
            if (lastname == null)
            {
                return Ok(await _dbService.GetPrescriptionsAsync());
            }

            var result = await _dbService.GetPrescriptionsByPatientAsync(lastname);
            if (result.Count() == 0)
            {
                return NotFound("No prescriptions found for patient with that name");
            }
            
            return Ok(result);
        }
    }
}
