namespace FMISaliAPI.DTO
{
    public class CalendarEvent
    {
        public int RoomId { get; set; }
        public string? Title { get; set; }
        public DateTimeOffset? Start { get; set; }
        public DateTimeOffset? End { get; set; }
    }
}
