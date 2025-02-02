using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Column(TypeName = "time")]
        public required TimeOnly Start { get; set; }

        [Column(TypeName = "time")]
        public required TimeOnly End { get; set; }

        public RecurrenceType Recurrence { get; set; }

        public DateTime? RecurrenceStartDate { get; set; }

        public DateTime? RecurrenceEndDate { get; set; }

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

    public enum RecurrenceType
    {
        OneTime,
        Weekly,
        OddWeeks,
        EvenWeeks
    }

    public enum Status
    {
        Da,
        Nu
    }
}