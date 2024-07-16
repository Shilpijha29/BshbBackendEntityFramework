namespace bshbbackend.ModelDto
{
    public class OfficerListDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Designation { get; set; }
        public string? Details { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
