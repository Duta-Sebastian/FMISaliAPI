namespace FMISaliAPI.DTO
{
    public class CalendarEvent
    {
        public int RoomId { get; set; }
        public string? Title { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}
