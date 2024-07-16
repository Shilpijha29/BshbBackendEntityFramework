using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace bshbbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        private static string currentCaptchaText;
        private static readonly Random random = new Random();

        [HttpGet("generate")]
        public IActionResult GenerateCaptcha()
        {
            currentCaptchaText = GenerateRandomCaptchaText();
            var captchaImage = GenerateCaptchaImage(currentCaptchaText);
            return File(ImageToByteArray(captchaImage), "image/png");
        }

        [HttpGet("audio")]
        public IActionResult GenerateCaptchaAudio()
        {
            // Implement audio CAPTCHA if needed
            return BadRequest("Audio CAPTCHA feature not implemented.");
        }

        [HttpPost("validate")]
        public IActionResult ValidateCaptcha([FromBody] CaptchaValidationModel model)
        {
            if (model.CaptchaText != currentCaptchaText)
            {
                return BadRequest("Invalid CAPTCHA");
            }

            return Ok("CAPTCHA is valid");
        }

        private string GenerateRandomCaptchaText(int length = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] captchaChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                captchaChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(captchaChars);
        }

        private Bitmap GenerateCaptchaImage(string text)
        {
            Bitmap bitmap = new Bitmap(200, 50);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (Font font = new Font("Arial", 20))
                {
                    graphics.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
                    graphics.DrawString(text, font, Brushes.Black, 10, 10);
                }
            }
            return bitmap;
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }

    public class CaptchaValidationModel
    {
        public string CaptchaText { get; set; }
    }
}
