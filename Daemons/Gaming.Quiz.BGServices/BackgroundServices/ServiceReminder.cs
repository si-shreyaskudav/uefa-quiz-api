
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Library.Utility;
using System.Text;
using System.Data;
using Gaming.Quiz.Interfaces.Storage;
using System.Collections.Generic;
using Gaming.Quiz.Interfaces.ServiceReminder;

namespace Gaming.Quiz.BGServices.BackgroundServices
{
    public class ServiceReminder : BaseService<ServiceReminder>, IHostedService, IDisposable
    {
        private Timer _Timer;
        private Int32 _Interval;
        private readonly IServiceReminderBGService _serviceReminderBGService;


        public ServiceReminder(ILogger<ServiceReminder> logger, IOptions<Application> appSettings, IOptions<Services> serviceSettings,
            IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            IServiceReminderBGService serviceReminderBGService) : base(logger, appSettings, serviceSettings, aws, postgre, redis, asset)
        {
            _Interval = _ServiceSettings.Value.ServiceReminder.IntervalMinutes;
            this._serviceReminderBGService = serviceReminderBGService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Catcher("ServiceReminder: Started.");

            //Timer runs immediately. Periodic intervals is disabled.
            _Timer = new Timer(Process, null, 0, Timeout.Infinite);

            return Task.CompletedTask;
        }

        private void Process(object state)
        {
            Run(state);

            //Timer runs after the interval period. Periodic intervals is disabled.
            _Timer?.Change(Convert.ToInt32(TimeSpan.FromHours(_Interval).TotalMilliseconds), Timeout.Infinite);
        }

        private async void Run(object state)
        {
            StringBuilder rp = new StringBuilder();
            string error = string.Empty;
            try
            {
                Catcher("ServiceReminder: Initiated.");

                Int64 DayNo = 15;
                Int32 retVal = -20;
                List<string> PendingDays = _serviceReminderBGService.GetPendingDays(DayNo, ref retVal, ref error);

                if (PendingDays.Count > 0 && retVal ==1)
                {
                    PendingNotify(retVal, GenericFunctions.Serialize(PendingDays));
                }
                else
                    Catcher("No pending Question Bank for next 2 Weeks");


            }
            catch (Exception ex)
            {
                Catcher("Service Question Reminder", LogLevel.Error, ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _Timer?.Change(Timeout.Infinite, 0);

            Catcher("ServiceReminder: Stopped.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _Timer?.Dispose();
        }

        #region " Notification body "

        private void PendingNotify(Int64 result, string reports)
        {
            try
            {
                string caption = $"{_Service} [Quiz Pending Question Reminder]";
                string remark = ((result == 1) ? "SUCCESS" : "FAILED");

                string content = "Remark: " + caption + " - " + remark + "<br/>";
                content += $"Daily Quiz - [ {_Service} RetVal: {result} ]<br/><br/><br/>";
                content += reports.ToString();

                String body = GenericFunctions.EmailBody(_Service, content);
                Notify(caption, body);
            }
            catch { }
        }

        #endregion

    }
}
