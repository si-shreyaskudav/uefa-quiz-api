using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.Services;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gaming.Quiz.BGServices.BackgroundServices
{
    public class BaseService<T>
    {
        protected readonly ILogger<T> _Logger;
        protected readonly IOptions<Application> _AppSettings;
        protected readonly IOptions<Contracts.Configuration.Services> _ServiceSettings;
        protected readonly IAWS _AWS;
        protected readonly IPostgre _Postgre;
        protected readonly IRedis _Redis;
        protected readonly IAsset _Asset;
        protected readonly Int32 _TourId;
        protected readonly String _Service;
        protected readonly String _Environment;

        public BaseService(ILogger<T> logger, IOptions<Application> appSettings, IOptions<Contracts.Configuration.Services> serviceSettings, IAWS aws,
            IPostgre postgre, IRedis redis, IAsset asset)
        {
            _Logger = logger;
            _AppSettings = appSettings;
            _ServiceSettings = serviceSettings;
            _AWS = aws;
            _Postgre = postgre;
            _Redis = redis;
            _Asset = asset;
            _TourId = appSettings.Value.Properties.TourId;
            _Environment = appSettings.Value.Connection.Environment;
            _Service = typeof(T).Name;
        }

        /// <summary>
        /// Catches error and information messages
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="level">Type of log</param>
        /// <param name="ex">Exception object</param>
        public void Catcher(String message, LogLevel level = LogLevel.None, Exception ex = null)
        {
            try
            {
                if (level == LogLevel.Error)
                {
                    _Logger.LogError(ex, message);
                    Notify($"{nameof(T)} Error", $"{message} \n\n Exception: {ex.Message} \n\n InnerException: {ex.InnerException}");
                }
                else if (level == LogLevel.Information)
                {
                    _Logger.LogInformation(message);
                    Notify($"{nameof(T)} ", $"{message}");
                }
                else
                    _Logger.LogInformation(message);
            }
            catch { }
        }

        /// <summary>
        /// Send email notification to the recipients address listed in appsettings
        /// </summary>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Body content</param>
        public void Notify(string subject, string body)
        {
            try
            {
                String sender = _ServiceSettings.Value.Notification.Sender;
                String recipient = _ServiceSettings.Value.Notification.Recipient;

                _AWS.SendSNSAlert($"RR Quiz 2023 [{_Environment.ToUpper()}] | {subject}", body);
            }
            catch { }
        }
    }
}
