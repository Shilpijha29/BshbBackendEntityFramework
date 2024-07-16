using System.ComponentModel.DataAnnotations;

namespace bshbbackend.ModelDto
{
    public class VerifyEmailOtpDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "OTP is required.")]
        [StringLength(6, ErrorMessage = "OTP must be 6 digits.", MinimumLength = 6)]
        public string Otp { get; set; }
    }
}
