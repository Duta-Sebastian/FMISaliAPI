namespace FMISaliAPI.Models
{
    public class RoomFacility
    {
        public int FacilityId { get; set; }
        public required Facility Facility { get; set; }

        public int RoomId { get; set; }
        public required Room Room { get; set; }
    }
}
