using System.ComponentModel;

namespace FMISaliAPI.Models
{
    public class File
    {
        public int Id { get; set; }

        public required string FileName { get; set; }

        public required string ContentType { get; set; }

        public required byte[] FileData { get; set; }

        public DateTime UploadedOn { get; set; }
    }
}
