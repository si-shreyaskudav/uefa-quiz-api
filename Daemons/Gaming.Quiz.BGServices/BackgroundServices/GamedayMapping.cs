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
using Gaming.Quiz.Contracts.BGServices;
using Gaming.Quiz.Interfaces.GamedayMapping;

namespace Gaming.Quiz.BGServices.BackgroundServices
{
    public class GamedayMapping : BaseService<GamedayMapping>, IHostedService, IDisposable
    {
        private Timer _Timer;
        private Int32 _Interval;
        private readonly bool _IsServer;
        private readonly IGamedayMappingBGServiceBlanket _gamedayMappingBGServiceBlanket;

        public GamedayMapping(ILogger<GamedayMapping> logger, IOptions<Application> appSettings, IOptions<Services> serviceSettings,
            IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            IGamedayMappingBGServiceBlanket gamedayMappingBGServiceBlanket) : base(logger, appSettings, serviceSettings, aws, postgre, redis, asset)
        {
            _Interval = _ServiceSettings.Value.GamedayMapping.IntervalMinutes;
            _IsServer = appSettings.Value.Properties.IsServer;
            this._gamedayMappingBGServiceBlanket = gamedayMappingBGServiceBlanket;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Catcher("PointCalculation: Started.");

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
            int retVal = -60;
            try
            {
                TimeSpan start = new TimeSpan(8, 0, 0);
                TimeSpan end = new TimeSpan(8, 1, 0);
                List<GamedayProccesQuizes> quizes = new List<GamedayProccesQuizes>();


                string timeZone = _IsServer ? "Asia/Kolkata" : "India Standard Time";

                DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZone));

                if (today.TimeOfDay > start && today.TimeOfDay < end)
                {
                    quizes = _gamedayMappingBGServiceBlanket.GetQuizesForGamedayMapping();

                    if (quizes != null && quizes.Count > 0)
                    {
                        foreach (GamedayProccesQuizes quize in quizes)
                        {
                            retVal = _gamedayMappingBGServiceBlanket.ProcessGameday(quize.quizId);

                            if (retVal != 1)
                            {
                                throw new Exception($"Gameday Mapping failed for Quiz: {quize.quizId}, retVal: {retVal}");
                            }
                        }
                    }

                    if (retVal == 1)
                    {
                        GamedayMappingNotify(quizes.Select(a => a.quizId).ToList(), retVal);
                    }
                }

            }
            catch (Exception ex)
            {
                Catcher("Gameday Mapping", LogLevel.Error, ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _Timer?.Change(Timeout.Infinite, 0);

            Catcher("GamedayMapping: Stopped.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _Timer?.Dispose();
        }

        #region " Notification body "

        private void GamedayMappingNotify(List<long> quizId, Int64 result)
        {
            try
            {
                string caption = $"{_Service} [Quizes: {GenericFunctions.Serialize(quizId)}]";
                string remark = ((result == 1) ? "SUCCESS" : "FAILED");

                string content = "Remark: " + caption + " - " + remark + "<br/>";
                content += $"Quiz GamedayMapping - [ {_Service} RetVal: {result} ]<br/><br/><br/>";


                String body = GenericFunctions.EmailBody(_Service, content);
                Notify(caption, body);
            }
            catch { }
        }

        #endregion

    }
}
