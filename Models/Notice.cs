namespace bshbbackend.Models
{
    public class Notice
    {
        public int Id { get; set; }
        public string? Text { get; set; } 
        public string? Url { get; set; }
        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set; }
    }
}
