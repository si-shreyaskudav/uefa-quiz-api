using Amazon.SimpleNotificationService;
using System;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService.Model;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Interfaces.Storage;

namespace Gaming.Quiz.Library.AWS.SNS
{
    public class Alerts : EC2.Instance
    {
        private AmazonSimpleNotificationServiceClient _Client;

        public Alerts(IOptions<Application> appSettings) : base(appSettings)
        {
        }

        public async Task<bool> SendSNSAlert(String subject, String message)
        {
            bool success = false;

            using (_Client = SNSClient())
            {
                PublishRequest publishRequest = new PublishRequest("arn:aws:sns:us-east-1:572143828798:SI-Gaming-Fantasy-Alerts", message, subject);

                PublishResponse publishResponse = await _Client.PublishAsync(publishRequest);

                success = !String.IsNullOrEmpty(publishResponse.MessageId);
            }

            return success;
        }
    }
}
