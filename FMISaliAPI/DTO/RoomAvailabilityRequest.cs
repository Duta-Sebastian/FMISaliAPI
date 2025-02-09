namespace FMISaliAPI.DTO
{
    public class RoomAvailabilityRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RoomFilterRequest RoomFilter { get; set; } = new RoomFilterRequest();
    }
}