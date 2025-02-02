using FMISaliAPI.Data;
using FMISaliAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FMISaliAPI.Services;

namespace FMISaliAPI.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet("events")]
        public async Task<IActionResult> GetEvents(string roomName, DateTime? start, DateTime? end)
        {
            // Fetch schedules for the specified room
            var schedules = await context.Schedules
                .Include(s => s.Room)
                .Where(s => s.Room.Name == roomName)
                .Select(s => s)
                .ToListAsync();

            // Generate calendar events
            var events = new List<CalendarEvent>();
            foreach (var schedule in schedules)
            {
                events.AddRange(ScheduleService.GenerateCalendarEvents(schedule));
            }

            // Filter events by the provided date range (if any)
            if (start.HasValue && end.HasValue)
            {
                events = events
                    .Where(e => e.Start >= start.Value && e.End <= end.Value)
                    .ToList();
            }

            return Ok(events);
        }
    }
}
