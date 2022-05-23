using System.ComponentModel.DataAnnotations;

namespace MvcProject.Entities
{
    public class ShortUrl
    {
        public int Id { get; set; }
        public int Counter { get; set;}
        public string? OriginalUrl { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public string? ShortendUrl { get; set; }
        public int UsedCount { get; set; }
        public bool IsDeleted {get; set;}
    }
}