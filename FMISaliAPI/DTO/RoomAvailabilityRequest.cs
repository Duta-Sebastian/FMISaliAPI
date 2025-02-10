namespace FMISaliAPI.DTO
{
    public class RoomAvailabilityRequest
    {
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public RoomFilterRequest RoomFilter { get; set; } = new RoomFilterRequest();
    }
}