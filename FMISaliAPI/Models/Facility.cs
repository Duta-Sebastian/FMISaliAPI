using System.Text.Json.Serialization;

namespace FMISaliAPI.Models
{
    public class Facility
    {
        public int Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter<RoomType>))]
        public FacilityType Type { get; set; }

        public ICollection<RoomFacility> RoomFacilities { get; set; } = new List<RoomFacility>();
    }

    public enum FacilityType
    {
        Proiector,
        Tabla,
        TablaInteligenta,
    }
}
