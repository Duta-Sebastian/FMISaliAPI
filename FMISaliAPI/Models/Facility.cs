using System.Text.Json.Serialization;

namespace FMISaliAPI.Models
{
    public class Facility
    {
        public int Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter<FacilityType>))]
        public FacilityType Type { get; set; }

        // ReSharper disable once CollectionNeverUpdated.Global
        public ICollection<RoomFacility> RoomFacilities { get; set; } = new List<RoomFacility>();
    }

    public enum FacilityType
    {
        Proiector,
        Tabla,
        TablaInteligenta,
    }
}
