namespace PLMS.API.Models
{
    public class RegisterModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
    }
}
