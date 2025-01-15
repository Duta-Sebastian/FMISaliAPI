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
        private readonly ApplicationDbContext _context = context;

        // POST: api/rooms
        [HttpPost("add")]
        public async Task<IActionResult> AddRoom([FromBody] Room room)
        {
            if (room == null)
            {
                return BadRequest("Room data is invalid.");
            }

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        // GET: api/rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();

            if (rooms == null || !rooms.Any())
            {
                return NotFound("No rooms found.");
            }

            return Ok(rooms);
        }
    }
}
