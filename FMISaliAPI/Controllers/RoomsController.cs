using FMISaliAPI.Data;
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
                    .Include(r => r.RoomFacilities)!
                    .ThenInclude(rf => rf.Facility)
                    .Select(r => new
                    {
                        r.Name,
                        Type = r.Type.ToString(),
                        RoomFacilities = (r.RoomFacilities ?? new List<RoomFacility>())
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

        [HttpGet("getAvailableRoomsByDate")]
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
        }

        [HttpGet("getRoomScheduleWeek")]
        public async Task<IActionResult> GetRoomScheduleWeek([FromQuery] DateTime startDate, int roomId)
        {
            try
            {
                var roomSchedule = await context.Schedules
                    .Where(s => s.RoomId == roomId &&
                                s.Start >= startDate &&
                                s.Start < startDate.AddDays(7))
                    .OrderBy(s => s.Start)
                    .Select(s => new
                    {
                        s.Id,
                        s.RoomId,
                        s.Start,
                        s.End,
                        s.Type,
                        s.Status,
                        s.Description,
                        s.Reason
                    })
                    .AsNoTracking()
                    .ToListAsync();
                return Ok(roomSchedule);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
