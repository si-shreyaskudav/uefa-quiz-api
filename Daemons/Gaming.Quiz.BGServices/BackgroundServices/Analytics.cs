
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
using Gaming.Quiz.Contracts.PointCalculation;
using System.Collections.Generic;
using System.Linq;
using Gaming.Quiz.Interfaces.Analytics;

namespace Gaming.Quiz.BGServices.BackgroundServices
{
    public class Analytics : BaseService<Analytics>, IHostedService, IDisposable
    {
        private Timer _Timer;
        private Int32 _Interval;
        private readonly IAnalyticsBlanket _analyticsBlanket;

        public Analytics(ILogger<Analytics> logger, IOptions<Application> appSettings, IOptions<Services> serviceSettings,
            IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IAnalyticsBlanket analyticsBlanket) : base(logger, appSettings, serviceSettings, aws, postgre, redis, asset)
        {
            _Interval = _ServiceSettings.Value.Analytics.IntervalMinutes;
            this._analyticsBlanket = analyticsBlanket;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Catcher("Analytics: Started.");

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
            Int32 RetVal = 0;
            try
            {
                Catcher("Analytics: Initiated.");

                //string fromDate = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                //string toDate = DateTime.Now.ToString("yyyy-MM-dd");
                string fromDate = DateTime.Today.AddDays(-7).ToString("yyyyMMdd");
                string toDate = DateTime.Now.ToString("yyyyMMdd");

                //string analyicsMeta = _AnalyticsContext.GetServiceAnalytics(fromDate, toDate);
                string analyicsMeta = _analyticsBlanket.GetServiceAnalyticsNew(fromDate,toDate);

                if (!String.IsNullOrEmpty(analyicsMeta))
                    AnalyticsNotify(RetVal, analyicsMeta);
                else
                    Catcher("Analytics is null." + analyicsMeta);

                string qplanalyicsMeta = _analyticsBlanket.GetServiceQPLAnalytics(fromDate, toDate);

                if (!String.IsNullOrEmpty(qplanalyicsMeta))
                    AnalyticsNotify(RetVal, qplanalyicsMeta);
                else
                    Catcher("QPL Analytics is null." + qplanalyicsMeta);
            }
            catch (Exception ex)
            {
                Catcher("Analytics Run", LogLevel.Error, ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _Timer?.Change(Timeout.Infinite, 0);

            Catcher("Analytics: Stopped.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _Timer?.Dispose();
        }

        #region " Notification body "

        private void AnalyticsNotify(Int64 result, String reports)
        {
            try
            {
                string caption = $"{_Service} [Analytics]";

                string content = "RR Quiz Analytics \n\n";
                content += reports;

                String body = GenericFunctions.EmailBody(_Service, content);
                Notify(caption, body);
            }
            catch { }
        }

        #endregion

    }
}
