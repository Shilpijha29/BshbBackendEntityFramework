using bshbbackend;
using bshbbackend.ModelDto;
using bshbbackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly BshbDbContext _context;  // Your database context
    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration _configuration;
    private readonly EmailSender _emailSender;

    public AdminController(BshbDbContext context, IMemoryCache memoryCache, IConfiguration configuration, EmailSender emailSender)
    {
        _context = context;
        _memoryCache = memoryCache;
        _configuration = configuration;
        _emailSender = emailSender;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AdminLoginDto loginDto)
    {
        var admin = _context.Admins.FirstOrDefault(a => a.username == loginDto.Username && a.password == loginDto.Password);
        if (admin == null)
        {
            return BadRequest("Invalid username or password.");
        }

        if (!IsValidEmail(admin.Email))
        {
            return BadRequest("Invalid email address.");
        }

        var otp = GenerateOtp();
        _memoryCache.Set(admin.Email, otp, TimeSpan.FromMinutes(5));

        var senderName = _configuration["BrevoApi:SenderName"];
        var senderEmail = _configuration["BrevoApi:SenderEmail"];
        var subject = "Your OTP for verification";
        var message = $"Your OTP for verification is: {otp}";

        _emailSender.SendEmail(senderName, senderEmail, admin.Email, admin.Email, subject, message);

        // Return success message and email
        return Ok(new { message = "OTP sent successfully.", email = admin.Email });
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyEmailOtpDto verifyOtpDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (_memoryCache.TryGetValue(verifyOtpDto.Email, out string storedOtp) && storedOtp == verifyOtpDto.Otp)
        {
            _memoryCache.Remove(verifyOtpDto.Email);
            return Ok(new { message = "OTP verified successfully." });
        }

        return BadRequest("Invalid OTP or OTP has expired.");
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            var domain = addr.Host;
            var hostEntry = System.Net.Dns.GetHostEntry(domain);
            return addr.Address == email && hostEntry.AddressList.Length > 0;
        }
        catch
        {
            return false;
        }
    }

    private string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
