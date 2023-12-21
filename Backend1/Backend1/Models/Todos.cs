using System.ComponentModel.DataAnnotations;

namespace Backend1.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public bool IsDone { get; set; }
    }
}
