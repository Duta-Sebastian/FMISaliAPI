using FMISaliAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace FMISaliAPI.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController(ApplicationDbContext context) : ControllerBase
    {
    }
}
