using Amazon.SQS;
using System;
using Microsoft.Extensions.Options;
using  Gaming.Quiz.Contracts.Configuration;
using  Gaming.Quiz.Library.Utility;
using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace  Gaming.Quiz.Library.AWS.SQS
{
    public class Queue: S3.Broker
    {
        private readonly AmazonSQSClient _Client;
        private readonly AmazonSQSConfig _Config;

        public Queue(IOptions<Application> appSettings) : base(appSettings)
        {
            //_Config = new AmazonSQSConfig();
            //_Config.ServiceURL = _SQSProperties.ServiceUrl;
            //_Client = SQSClient(_Config);
        }
    }
}
