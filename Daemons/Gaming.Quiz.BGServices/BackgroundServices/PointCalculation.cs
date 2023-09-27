
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
using Gaming.Quiz.Blanket.Feeds;
using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Leaderboard;
using System.Numerics;
using Gaming.Quiz.Interfaces.PointCalculation;
using Gaming.Quiz.Interfaces.Leaderboard;
using Gaming.Quiz.Interfaces.Feeds;

namespace Gaming.Quiz.BGServices.BackgroundServices
{
    public class PointsCalculation : BaseService<PointsCalculation>, IHostedService, IDisposable
    {
        private Timer _Timer;
        private Int32 _Interval;
        private readonly IPointCalculationBlanket _pointCalculationBlanket;
        private readonly ILeaderboardBlanket _leaderboardBlanket;

        public PointsCalculation(ILogger<PointsCalculation> logger, IOptions<Application> appSettings, IOptions<Services> serviceSettings,
            IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            IPointCalculationBlanket pointCalculationBlanket,
            ILeaderboardBlanket leaderboardBlanket,
            IFeedsBlanket feedsBlanket) : base(logger, appSettings, serviceSettings, aws, postgre, redis, asset)
        {
            _Interval = _ServiceSettings.Value.PointCal.IntervalMinutes;
            this._pointCalculationBlanket = pointCalculationBlanket;
            this._leaderboardBlanket = leaderboardBlanket;
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
            StringBuilder rp = new StringBuilder();
            Tuple<int, Exception> tuple = new Tuple<int, Exception>(-80, null);
            string error = string.Empty;
            try
            {
                Catcher("PointCalculation: Initiated.");

                List<PntPrcGet> processGet = _pointCalculationBlanket.UserPointProcessGet( ref error);

                Catcher("PointCalculation: processGet." + GenericFunctions.Serialize(processGet));

                if (processGet != null && processGet.Count > 0 && string.IsNullOrEmpty(error))
                {

                    PntPrcGet pntPrcGet = processGet.FirstOrDefault();
                    SettlmentGet settlmentGet = new SettlmentGet();

                    Int32 monthId = (pntPrcGet != null ? pntPrcGet.MonthId : 0);
                    Int32 gamedayId = (pntPrcGet != null ? pntPrcGet.GamedayId : 0);
                    Int32 quizId = (pntPrcGet != null ? pntPrcGet.QzMId : 0);
                    Int32 weekId = (pntPrcGet != null ? pntPrcGet.WeekId : 0);

                    //if (pntPrcGet?.SettleProcessFlag?.Trim() == "1")
                    //{
                    //    Catcher("PointCalculation: EOD Process Started.");

                    //    tuple = _PointCalContext.ServiceEODSettlement(ref settlmentGet);

                    //    if (tuple.Item1 == 1)
                    //    {
                    //        Catcher("PointCalculation: EOD Process Successful.");

                    //        monthId = settlmentGet.QuMonthid;

                    //        rp.Append("EOD Settlement Successful \n");
                    //        rp.Append("EOD Settlement Response : - \n ");
                    //        rp.Append(GenericFunctions.Serialize(settlmentGet));

                    //        CalculationNotify(settlmentGet.QuMonthid, tuple.Item1, rp);
                    //    }

                    //    //if (tuple.Item2 != null)
                    //    //    Catcher("Point Calculation  ==>  Settlement Error ==>", LogLevel.Error, tuple.Item2);
                    //    ////else
                    //    //    Catcher("Point Calculation ==>  Settlement RetVal ==> " + tuple.Item1, LogLevel.Information);
                    //}

                    tuple = await _pointCalculationBlanket.ServiceUserPointProcess(monthId, gamedayId, weekId);

                    Catcher("PointCalculation: Point Calcuation process satus ==> " + tuple.Item1);


                    if (tuple.Item1 == 1)
                    {
                        //Int64 weekId = 0;
                        //weekId = settlmentGet.QuWeekid == null ? 0 : settlmentGet.QuWeekid.Value;
                        await UpdateThis(monthId, weekId, gamedayId, quizId);
                    }
                    else
                    {
                        if (tuple.Item2 != null)
                            Catcher("Point Calculation", LogLevel.Error, tuple.Item2);
                        else
                            Catcher("Point Calculation ==> Different RetVal  ==> " + tuple.Item1, LogLevel.Information);
                    }
                }
                else if(!string.IsNullOrEmpty(error))
                    Catcher("PointCalculation: Point Process Get Error ==>" + error, LogLevel.Information);
                else
                    Catcher("PointCalculation: No Response Point Process Get.");
            }
            catch (Exception ex)
            {
                Catcher("Point Calculation", LogLevel.Error, ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _Timer?.Change(Timeout.Infinite, 0);

            Catcher("PointCalculation: Stopped.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _Timer?.Dispose();
        }

        #region " Notification body "

        private void CalculationNotify(Int32 MonthId, Int64 result, StringBuilder reports)
        {
            try
            {
                string caption = $"{_Service} [MonthId: {MonthId}]";
                string remark = ((result == 1) ? "SUCCESS" : "FAILED");

                string content = "Remark: " + caption + " - " + remark + "<br/>";
                content += $"Quiz Admin - [ {_Service} RetVal: {result} ]<br/><br/><br/>";
                content += reports.ToString();

                String body = GenericFunctions.EmailBody(_Service, content);
                Notify(caption, body);
            }
            catch { }
        }

        #endregion

        #region " Helper "

        protected async Task UpdateThis(Int32 MonthId, Int64 WeekId, Int64 GamedayId, Int64 QuizId)
        {
            int optType = 1;

            try
            {
                await _leaderboardBlanket.GenMonthFile(QuizId);
                Catcher("PointCalculation: Month File Generated.");

                await _leaderboardBlanket.GenGamedayFile(QuizId);
                Catcher("PointCalculation: Gameday File Generated.");

                await _leaderboardBlanket.GenWeekFile(QuizId);
                Catcher("PointCalculation: Week File Generated.");

                await _leaderboardBlanket.GenFavPlayerFile(QuizId);
                Catcher("PointCalculation: FavPlayer File Generated.");

                await _leaderboardBlanket.GenMonthLeaderboard(GamedayId, WeekId, Convert.ToInt64(MonthId), QuizId);
                Catcher("PointCalculation:  Month Leaderboard File Generated.");

                await _leaderboardBlanket.GenOverallLeaderboard(QuizId);
                Catcher("PointCalculation:  Overall Leaderboard File Generated.");

                await _leaderboardBlanket.GetMixApi(QuizId);
                Catcher("PointCalculation:  MixAPI File Generated.");

                //Int64 PlayerId = 0;
                List<FavPlayer> mFavPlayer = new List<FavPlayer>();

                try
                {
                    HTTPResponse httpResponse = _leaderboardBlanket.GetFavPlayers(false).Result;

                    if (httpResponse.Meta.RetVal == 1)
                    {
                        if (httpResponse.Data != null)
                            mFavPlayer = GenericFunctions.Deserialize<List<FavPlayer>>(GenericFunctions.Serialize(((ResponseObject)httpResponse.Data).Value));
                    }

                    foreach (FavPlayer player in mFavPlayer)
                    {
                        await _leaderboardBlanket.GenTeamLeaderboard(Convert.ToInt64(player.PlayerId), GamedayId, WeekId);
                        //Catcher("PointCalculation:  Player Leaderboard File Generated for Id=" + player.PlayerId);

                    }
                    Catcher("PointCalculation: All Players Leaderboard File Generated");

                }
                catch (Exception ex)
                {
                    Catcher("PointCalculation:  FavPlayer LB error:"+ex.Message);
                }
                

                //await _LeaderboardContext.GenBestScoreLeaderboard(GamedayId,WeekId, Convert.ToInt64(MonthId));
                //Catcher("PointCalculation: Best Score Leaderboard File Generated.");s

            }
            catch (Exception ex)
            {
                throw new Exception("Point Calculation ==> LeaderboardUpdate Error => ", ex);
            }
        }

        #endregion
    }
}
