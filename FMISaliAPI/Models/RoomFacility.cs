namespace FMISaliAPI.Models
{
    public class RoomFacility
    {
        public int FacilityId { get; set; }
        public Facility Facility { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
