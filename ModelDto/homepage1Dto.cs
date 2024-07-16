namespace bshbbackend.ModelDto
{
    public class homepage1Dto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public IFormFile? chiefminister { get; set; }
        public string? chiefministerName { get; set; }
        public IFormFile? departmentminister { get; set; }

        public string? departmentministerName { get; set; }
    }
}
