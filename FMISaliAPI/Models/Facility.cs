using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace FMISaliAPI.Models
{
    public class Facility
    {
        public int Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter<RoomType>))]
        public FacilityType Type { get; set; }

        public int NumarCalculatoare { get; set; }

    }

    public enum FacilityType
    {
        Proiector,
        Tabla,
        TablaInteligenta,
    }
}
