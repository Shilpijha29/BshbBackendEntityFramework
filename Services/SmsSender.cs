using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace bshbbackend.Services
{
    public class SmsSender
    {
        private readonly IConfiguration _configuration;

        public SmsSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Result SendSms(string senderName, string recipient, string content)
        {
            var result = new Result();

            try
            {
                var apiKey = _configuration["TextlocalApi:ApiKey"];
                var sender = _configuration["TextlocalApi:SenderName"];
                var message = Uri.EscapeDataString(content);
                var url = $"https://api.textlocal.in/send/?apikey={apiKey}&sender={sender}&numbers={recipient}&message={message}";

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";

                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseContent = streamReader.ReadToEnd();
                    result.Status = Result.ResultStatus.Success;
                    result.Message = "SMS sent successfully.";
                    result.Data = responseContent;
                }
            }
            catch (Exception e)
            {
                result.Status = Result.ResultStatus.Danger;
                result.Message = $"An exception occurred: {e.Message}";
            }

            return result;
        }
    }

}
