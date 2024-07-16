using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using bshbbackend.ModelDto;
using bshbbackend.Models;
using bshbbackend.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace bshbbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BshbDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly EmailSender _emailSender;
        private readonly SmsSender _smsSender;

        public UserController(BshbDbContext context, IMemoryCache memoryCache, IConfiguration configuration,
            EmailSender emailSender, SmsSender smsSender)
        {
            _context = context;
            _memoryCache = memoryCache;
            _configuration = configuration;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        [HttpPost("sign-up/request-otp-for-login")]
        public async Task<IActionResult> RequestOtpforlogin([FromBody] EmailDto emailDto)
        {
            var existingUser = _context.Users.FirstOrDefault(o => o.Email == emailDto.Email);
            if (existingUser == null)
            {
                return BadRequest("Account doesn't exists with this email.");
            }

            if (!IsValidEmail(emailDto.Email))
            {
                return BadRequest("Invalid email address.");
            }

            if (!_memoryCache.TryGetValue(emailDto.Email, out string existingOtp))
            {
                var otp = GenerateOtp();
                _memoryCache.Set(emailDto.Email, otp, TimeSpan.FromMinutes(5));

                var senderName = _configuration["BrevoApi:SenderName"];
                var senderEmail = _configuration["BrevoApi:SenderEmail"];
                var subject = "Your OTP for verification";
                var message = $"Your OTP for verification is: {otp}";
                _emailSender.SendEmail(senderName, senderEmail, emailDto.Email, emailDto.Email, subject, message);

                return Ok("OTP sent successfully.");
            }
            else
            {
                var senderName = _configuration["BrevoApi:SenderName"];
                var senderEmail = _configuration["BrevoApi:SenderEmail"];
                var subject = "Resending OTP for verification";
                var message = $"Your OTP for verification is: {existingOtp}";

                _emailSender.SendEmail(senderName, senderEmail, emailDto.Email, emailDto.Email, subject, message);

                return Ok("OTP resent successfully.");
            }
        }

        [HttpPost("sign-up/request-otp-for-signup")]
        public async Task<IActionResult> RequestOtpforsignup([FromBody] EmailDto emailDto)
        {
            var existingUser = _context.Users.FirstOrDefault(o => o.Email == emailDto.Email);
            if (existingUser != null)
            {
                return BadRequest("Account already exists with this email.");
            }

            if (!IsValidEmail(emailDto.Email))
            {
                return BadRequest("Invalid email address.");
            }

            if (!_memoryCache.TryGetValue(emailDto.Email, out string existingOtp))
            {
                var otp = GenerateOtp();
                _memoryCache.Set(emailDto.Email, otp, TimeSpan.FromMinutes(5));

                var senderName = _configuration["BrevoApi:SenderName"];
                var senderEmail = _configuration["BrevoApi:SenderEmail"];
                var subject = "Your OTP for verification";
                var message = $"Your OTP for verification is: {otp}";
                _emailSender.SendEmail(senderName, senderEmail, emailDto.Email, emailDto.Email, subject, message);

                return Ok("OTP sent successfully.");
            }
            else
            {
                var senderName = _configuration["BrevoApi:SenderName"];
                var senderEmail = _configuration["BrevoApi:SenderEmail"];
                var subject = "Resending OTP for verification";
                var message = $"Your OTP for verification is: {existingOtp}";

                _emailSender.SendEmail(senderName, senderEmail, emailDto.Email, emailDto.Email, subject, message);

                return Ok("OTP resent successfully.");
            }
        }

        [HttpPost("sign-up/verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyEmailOtpDto verifyOtpDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_memoryCache.TryGetValue(verifyOtpDto.Email, out string storedOtp) && storedOtp == verifyOtpDto.Otp)
            {
                _memoryCache.Remove(verifyOtpDto.Email);
                return Ok("OTP verified successfully.");
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

        [HttpPost("sign-up/request-otp-phone")]
        public async Task<IActionResult> RequestOtpPhone([FromBody] PhoneNumberDto phoneNumberDto)
        {
            var existingUser = _context.Users.FirstOrDefault(o => o.Phone == phoneNumberDto.PhoneNumber);
            if (existingUser != null)
            {
                return BadRequest("Account already exists with this phone number.");
            }

            if (!IsValidPhoneNumber(phoneNumberDto.PhoneNumber))
            {
                return BadRequest("Invalid phone number.");
            }

            if (!_memoryCache.TryGetValue(phoneNumberDto.PhoneNumber, out string existingOtp))
            {
                var otp = GenerateOtp();
                _memoryCache.Set(phoneNumberDto.PhoneNumber, otp, TimeSpan.FromMinutes(5));

                var senderName = _configuration["BrevoApi:SenderName"];
                var message = $"Your OTP for verification is: {otp}";

                _smsSender.SendSms(senderName, phoneNumberDto.PhoneNumber, message);

                return Ok("OTP sent successfully.");
            }
            else
            {
                var senderName = _configuration["BrevoApi:SenderName"];
                var message = $"Your OTP for verification is: {existingOtp}";

                _smsSender.SendSms(senderName, phoneNumberDto.PhoneNumber, message);

                return Ok("OTP resent successfully.");
            }
        }

        [HttpPost("sign-up/verify-otp-phone")]
        public async Task<IActionResult> VerifyOtpPhone([FromBody] VerifyPhoneOtpDto verifyOtpDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_memoryCache.TryGetValue(verifyOtpDto.PhoneNumber, out string storedOtp) && storedOtp == verifyOtpDto.Otp)
            {
                _memoryCache.Remove(verifyOtpDto.PhoneNumber);
                return Ok("OTP verified successfully.");
            }

            return BadRequest("Invalid OTP or OTP has expired.");
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            try
            {
                // Ensure the phone number is in the correct international format
                var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                var number = phoneNumberUtil.Parse(phoneNumber, null);
                return phoneNumberUtil.IsValidNumber(number);
            }
            catch
            {
                return false;
            }
        }
    }
}
