using FMISaliAPI.Data;
using FMISaliAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FMISaliAPI.Controllers
{
    [Route("api/facilities")]
    [ApiController]
    public class FacilitiesController: ControllerBase
    {
        [HttpGet("getAllFacilityTypes")]
        public async Task<IActionResult> GetFacilityTypes()
        {
            try
            {
                var facilityTypes = await FacilityService.GetFacilityTypesTask();
                if(facilityTypes.Count != 0)
                    return Ok(facilityTypes);
                return NotFound("No facility types found.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
