using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;

namespace bshbbackend.Services
{
    public class EmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Result SendEmail(string senderName, string senderEmail, string receiverName, string receiverEmail, string subject, string message)
        {
            var result = new Result();

            // Configure API key authorization
            var apiKey = _configuration["BrevoApi:ApiKey"];
            Configuration.Default.AddApiKey("api-key", apiKey);

            var apiInstance = new TransactionalEmailsApi();
            SendSmtpEmailSender sender = new SendSmtpEmailSender(senderName, senderEmail);

            SendSmtpEmailTo receiver1 = new SendSmtpEmailTo(receiverEmail, receiverName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo> { receiver1 };

            string HtmlContent = null;
            string TextContent = message;

            try
            {
                var sendSmtpEmail = new SendSmtpEmail(sender, To, null, null, HtmlContent, TextContent, subject);
                CreateSmtpEmail apiResult = apiInstance.SendTransacEmail(sendSmtpEmail);

                if (string.IsNullOrWhiteSpace(apiResult.ToJson()))
                {
                    result.Status = Result.ResultStatus.Danger;
                    result.Message = "Empty response body from email service.";
                }
                else
                {
                    result.Status = Result.ResultStatus.Success;
                    result.Message = "Email sent successfully.";
                    result.Data = apiResult;
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

    public class Result
    {
        public enum ResultStatus
        {
            Success,
            Danger
        }

        public ResultStatus Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
