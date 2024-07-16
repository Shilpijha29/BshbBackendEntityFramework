namespace bshbbackend.ModelDto
{
    public class ChairmenDto
    {
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IFormFile? photo { get; set; }
    }

}
