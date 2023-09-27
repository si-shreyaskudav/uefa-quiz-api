using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Gaming.Quiz.Library.Utility;

namespace Gaming.Quiz.Library.Daemon
{
    public class BaseService<T, T1>
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
        //public void Catcher(String message, LogLevel level = LogLevel.Information, Exception ex = null)
        //{
        //    try
        //    {
        //        if (level == LogLevel.Error)
        //        {

        //            _Logger.LogError(ex, message);
        //            Notify($"{nameof(T)} Error", $"{message}<br/>Exception: {ex.Message}<br/>InnerException: {ex.InnerException}");
        //        }
        //        else
        //            _Logger.LogInformation(message);
        //    }
        //    catch { }
        //}

        /// <summary>
        /// Send email notification to the recipients address listed in appsettings
        /// </summary>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Body content</param>
        //public void Notify(string subject, string body)
        //{
        //    try
        //    {
        //        String sender = _ServiceSettings.Value.Notification.Sender;
        //        String recipient = _ServiceSettings.Value.Notification.Recipient;

        //        _AWS.SendSESMail(sender, recipient, "", "", $"Yahoo [{_Environment.ToUpper()}] " + subject, body, true);
        //    }
        //    catch { }
        //}

        
    }
}
