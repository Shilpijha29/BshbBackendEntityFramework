namespace bshbbackend.Models
{
    public class MDList
    {
        public int id {  get; set; }
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public byte[] Photo { get; set; }
    }
}
