namespace FMISaliAPI.DTO
{
    public class RoomFilterRequest
    {
        public int MinCapacity { get; set; }
        public int MaxCapacity { get; set; }
        public List<string> Facilities { get; set; } = [];
    }
}
