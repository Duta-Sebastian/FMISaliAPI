using FMISaliAPI.Data;
using FMISaliAPI.DTO;
using FMISaliAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FMISaliAPI.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomsController(ApplicationDbContext context) : ControllerBase
    {
        // POST: api/rooms
        [HttpPost("add")]
        public async Task<IActionResult> AddRoom([FromBody] Room? room)
        {
            if (room == null)
            {
                return BadRequest("Room data is invalid.");
            }

            context.Rooms.Add(room);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        [HttpGet("getAllRooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            try
            {
                var rooms = await context.Rooms
                    .Select(r => new
                    {
                        r.Id,
                        r.Name,
                        Type = r.Type.ToString()
                    })
                    .AsNoTracking()
                    .ToListAsync();
                return Ok(rooms);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("getAllRoomsWithFacilities")]
        public async Task<IActionResult> GetAllRoomsWithFacilities()
        {
            try
            {
                var rooms = await context.Rooms
                    .Include(r => r.RoomFacilities)
                    .ThenInclude(rf => rf.Facility)
                    .Select(r => new
                    {
                        r.Name,
                        Type = r.Type.ToString(),
                        RoomFacilities = (r.RoomFacilities)
                            .Select(rf => rf.Facility.Type.ToString())
                    })
                    .AsNoTracking()
                    .ToListAsync();
                return Ok(rooms);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("getFilteredRooms")]
        public async Task<IActionResult> GetFilteredRooms([FromBody] RoomFilterRequest request)
        {
            var minCapacity = request.MinCapacity;
            var maxCapacity = request.MaxCapacity;
            var facilities = request.Facilities;
            List<FacilityType> formattedFacilities = [];
            foreach (var facility in facilities)
            {
                Enum.TryParse<FacilityType>(facility, false, out var facilityType);
                formattedFacilities.Add(facilityType);
            }

            try
            {
                var rooms = await context.Rooms
                    .Include(r => r.RoomFacilities)
                    .ThenInclude(rf => rf.Facility)
                    .Where( r=>
                         r.Capacity <= maxCapacity &&
                         r.Capacity >= minCapacity &&
                         (formattedFacilities.Count == 0 ||
                          r.RoomFacilities.Any(rf => formattedFacilities.Contains(rf.Facility.Type)))
                        )
                    .Select(r => new
                    {
                        r.Id,
                        r.Name,
                        Type = r.Type.ToString()
                    })
                    .AsNoTracking()
                    .ToListAsync();
                return Ok(rooms);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await context.Rooms
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    r.Name, Type = r.Type.ToString()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        /*[HttpGet("getAvailableRoomsByDate")]
        public async Task<IActionResult> GetAvailableRoomsByDate([FromQuery] DateTime startDate, DateTime endDate)
        {
            try
            {
                var availableRooms = await context.Rooms
                    .Where(r => !context.Schedules
                        .Any(s => s.RoomId == r.Id && s.Start < endDate && s.End > startDate))
                    .Select(r => new
                    {
                        r.Name,
                        Type = r.Type.ToString()
                    })
                    .AsNoTracking()
                    .ToListAsync();
                return Ok(availableRooms);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }*/

        [HttpGet("getRoomSchedule")]
        public async Task<IActionResult> GetRoomScheduleWeek(string roomName)
        {
            try
            {
                var roomSchedule = await context.Schedules
                    .Include(s => s.Room)
                    .Where(s => s.Room.Name == roomName)
                    .AsNoTracking()
                    .ToListAsync();
                return Ok(roomSchedule);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("getMinMaxCapacity")]
        public async Task<IActionResult> GetMinMaxCapacity()
        {
            try
            {
                var capacities = await context.Rooms
                    .GroupBy(r => 1)
                    .OrderBy(r => 1)
                    .Select(g => new
                    {
                        MinCapacity = g.Min(r => r.Capacity),
                        MaxCapacity = g.Max(r => r.Capacity)
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                    
                if (capacities is null)
                    throw new Exception("Min and max capacities could not be retrieved!");
                return Ok(new
                {
                    maxCapacity = capacities.MaxCapacity,
                    minCapacity = capacities.MinCapacity
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
