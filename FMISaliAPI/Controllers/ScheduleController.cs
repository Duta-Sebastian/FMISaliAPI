using FMISaliAPI.Data;
using FMISaliAPI.DTO;
using FMISaliAPI.Models;
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
            var schedules = await context.Schedules
                .Include(s => s.Room)
                .Where(s => s.Room.Name == roomName)
                .Select(s => s)
                .ToListAsync();

            var eventTasks = schedules.Select(schedule => Task.Run(() => 
                ScheduleService.GenerateCalendarEvents(schedule)));
            var eventList = await Task.WhenAll(eventTasks);

            /*if (start.HasValue && end.HasValue)
            {
                events = events
                    .Where(e => e.Start >= start.Value && e.End <= end.Value)
                    .ToList();
            }*/

            return Ok(eventList.SelectMany(events => events).ToList());
        }

        [HttpPost("availableRoomsOnDate")]
        public async Task<IActionResult> GetAvailableRoomsOnDate([FromBody] RoomAvailabilityRequest roomAvailabilityRequest)
        {
            var startDate = roomAvailabilityRequest.StartDate;
            var endDate = roomAvailabilityRequest.EndDate;
            var roomFilter = roomAvailabilityRequest.RoomFilter;
            
            var facilityTypes = roomFilter.Facilities
                .Select(facility => Enum.Parse<FacilityType>(facility))
                .ToList();
            
            var schedules = await context.Schedules
                .AsNoTracking()
                .ToListAsync();

            var eventTasks = schedules.Select(schedule => Task.Run(() =>
                ScheduleService.GenerateCalendarEvents(schedule)));

            var eventList = await Task.WhenAll(eventTasks);

            var occupiedRooms = eventList
                .SelectMany(events => events)
                .GroupBy(e => e.RoomId)
                .Where(group => group.Any(e =>
                    e.Start <= endDate && e.End >= startDate))
                .Select(group => group.Key)
                .ToList();
            try
            {
                var availableRooms = await context.Rooms
                    .Where(r => !occupiedRooms.Contains(r.Id))
                    .Include(r => r.RoomFacilities)
                    .ThenInclude(rf => rf.Facility)
                    .Where(r =>
                        r.Capacity >= roomFilter.MinCapacity &&
                        r.Capacity <= roomFilter.MaxCapacity &&
                        facilityTypes.All(facilityType =>
                            r.RoomFacilities
                                .Select(rf => rf.Facility.Type)
                                .Contains(facilityType)))
                    .Select(r => new
                    {
                        r.Name,
                        Type = r.Type.ToString(),
                        r.Capacity,
                        Facilities = r.RoomFacilities.Select(rf => rf.Facility.Type.ToString()).ToList()
                    })
                    .ToListAsync();

                return Ok(availableRooms);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }
    }
}
