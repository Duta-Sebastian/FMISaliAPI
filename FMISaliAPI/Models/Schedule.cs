using System.ComponentModel.DataAnnotations;

namespace FMISaliAPI.Models
{
    public class Schedule
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public int? UserId { get; set; }

        public User? User { get; set; }

        public required Room Room { get; set; }

        public required ScheduleType Type { get; set; }

        public required DateTime Start { get; set; }

        public required DateTime End { get; set; }

        public Status? Status { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [MaxLength(255)]
        public string? Reason { get; set; }
    }

    public enum ScheduleType
    {
        Orar,
        Rezervare
    }

    public enum Status
    {
        Da,
        Nu
    }
}
