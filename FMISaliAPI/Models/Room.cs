using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FMISaliAPI.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public required string Name { get; set; }

        public int Capacity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter<RoomType>))]
        public RoomType Type { get; set; }
        
        public ICollection<RoomFacility> RoomFacilities { get; set; } = new List<RoomFacility>();

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }

    public enum RoomType
    {
        [EnumMember(Value = "Sala de seminar")]
        Seminar,
        [EnumMember(Value = "Amfiteatru")]
        Amfiteatru,
        [EnumMember(Value = "Laborator")]
        Laborator
    }
}
