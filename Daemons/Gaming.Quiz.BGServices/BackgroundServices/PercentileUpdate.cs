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
using Gaming.Quiz.Interfaces.PercentileUpdate;

namespace Gaming.Quiz.BGServices.BackgroundServices
{
    public class PercentileUpdate : BaseService<PercentileUpdate>, IHostedService, IDisposable
    {
        private Timer _Timer;
        private Int32 _Interval;
        private readonly IPercentileUpdate _percentileUpdate;


        public PercentileUpdate(ILogger<PercentileUpdate> logger, IOptions<Application> appSettings, IOptions<Services> serviceSettings,
            IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            IPercentileUpdate percentileUpdate) : base(logger, appSettings, serviceSettings, aws, postgre, redis, asset)
        {
            _Interval = _ServiceSettings.Value.PercentileUpdate.IntervalMinutes;
            this._percentileUpdate = percentileUpdate;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Catcher("Percentile Update: Started.");

            //Timer runs immediately. Periodic intervals is disabled.
            _Timer = new Timer(Process, null, 0, Timeout.Infinite);

            return Task.CompletedTask;
        }

        private void Process(object state)
        {
            Run(state);

            //Timer runs after the interval period. Periodic intervals is disabled.
            _Timer?.Change(Convert.ToInt32(TimeSpan.FromMinutes(_Interval).TotalMilliseconds), Timeout.Infinite);
        }

        private async void Run(object state)
        {
            string error = string.Empty;
            Int32 RetVal = -40;
            Tuple<Int32, Exception> tuple = new Tuple<int, Exception>(RetVal, null);
            try
            {
                Catcher("Percentile Update: Initiated.");

                tuple = _percentileUpdate.UpdatePercentile();

                if(tuple.Item1 == 1)
                    Catcher("Percentile Update Process status : " + tuple.Item1);
                else if( tuple.Item1 != 1 && tuple.Item2 != null)
                    Catcher("Percentile Update Process status : " + tuple.Item1, LogLevel.Error, tuple.Item2);
            }
            catch (Exception ex)
            {
                Catcher("Percentile Update Run", LogLevel.Error, ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _Timer?.Change(Timeout.Infinite, 0);

            Catcher("Percentile Update: Stopped.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _Timer?.Dispose();
        }
    }
}
