namespace bshbbackend.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string username { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string password { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
