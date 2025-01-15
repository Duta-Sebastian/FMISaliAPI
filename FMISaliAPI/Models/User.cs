using System.ComponentModel.DataAnnotations;

namespace FMISaliAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public required string Username { get; set; }
    }
}
