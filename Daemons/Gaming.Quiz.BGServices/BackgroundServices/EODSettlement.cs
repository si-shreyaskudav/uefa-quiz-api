
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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Gaming.Quiz.Interfaces.PointCalculation;
using Gaming.Quiz.Interfaces.Leaderboard;

namespace Gaming.Quiz.BGServices.BackgroundServices
{
    public class EODSettlement : BaseService<EODSettlement>, IHostedService, IDisposable
    {
        private Timer _Timer;
        private Int32 _Interval;
        //Added for Timer
        private readonly bool _IsServer;
        //private string _Timestart;
        //private string _TimeEnd;
        private String _ScheduleTime;

        //private readonly Blanket.PointCalculation.PointCalculation _PointCalContext;
        //private readonly Blanket.Leaderboard.Leaderboard _LeaderboardContext;
        private readonly IPointCalculationBlanket _pointCalculationBlanket;
        private readonly ILeaderboardBlanket _leaderboardBlanket;

        public EODSettlement(ILogger<EODSettlement> logger, IOptions<Application> appSettings, IOptions<Services> serviceSettings,
            IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, 
            IPointCalculationBlanket pointCalculationBlanket,
            ILeaderboardBlanket leaderboardBlanket) : base(logger, appSettings, serviceSettings, aws, postgre, redis, asset)
        {
            _Interval = _ServiceSettings.Value.EODsettlement.IntervalMinutes;
            //added for Time functionality -- new code
            _IsServer = appSettings.Value.Properties.IsServer;
            //_Timestart = appSettings.Value.Properties.timestart;
            //_TimeEnd = appSettings.Value.Properties.timeend;
            _ScheduleTime = appSettings.Value.Properties.ScheduleTime;
            this._pointCalculationBlanket = pointCalculationBlanket;
            this._leaderboardBlanket = leaderboardBlanket;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Catcher("EOD Settlement: Started.");

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
            //int retval = -60;
            StringBuilder rp = new StringBuilder();
            Tuple<int, Exception> tuple = new Tuple<int, Exception>(-80, null);
            string error = string.Empty;
            try
            {
                 
                //String timestart = _Timestart;
                //String timeend = _TimeEnd;

                Catcher("EOD Settlement: Initiated.");
                string targettimeZone = _IsServer ? "Asia/Kolkata" : "India Standard Time";
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(targettimeZone);
                DateTime runTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZone);

                string currentDate = runTime.Date.ToShortDateString();
                DateTime scheduleTime = Convert.ToDateTime(currentDate + " " + _ScheduleTime);



                //DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZone));

                //if (today > DateTime.Parse(_Timestart) && today < DateTime.Parse(_TimeEnd))
                if (DateTime.Compare(runTime, scheduleTime) >= 0 && runTime <= scheduleTime.AddMinutes(1))
                {
                    List<PntPrcGet> processGet = _pointCalculationBlanket.UserEODSettlementGet(ref error);
                    Catcher("EOD Settlement: processGet." + GenericFunctions.Serialize(processGet));

                    if (processGet != null && processGet.Count > 0 && string.IsNullOrEmpty(error))
                    {

                        PntPrcGet pntPrcGet = processGet.FirstOrDefault();
                        SettlmentGet settlmentGet = new SettlmentGet();

                        Int32 monthId = (pntPrcGet != null ? pntPrcGet.MonthId : 0);

                        if (pntPrcGet?.SettleProcessFlag?.Trim() == "1")
                        {
                            Catcher("EOD Settlement: EOD Process Started.");

                            tuple = _pointCalculationBlanket.ServiceEODSettlement(ref settlmentGet);

                            if (tuple.Item1 == 1)
                            {
                                Catcher("EOD Settlement: EOD Process Successful.");

                                monthId = settlmentGet.QuMonthid == null ? 0 : settlmentGet.QuMonthid.Value;

                                Int64 mQuizId = settlmentGet.QuMasterid == null ? 0 : settlmentGet.QuMasterid;
                                Int64 mGamedayId = settlmentGet.QuGamedayid == null ? 0 : settlmentGet.QuGamedayid.Value;
                                Int64 mWeekId = settlmentGet.QuWeekid == null ? 0 : settlmentGet.QuWeekid.Value;

                                rp.Append("EOD Settlement Successful \n");
                                rp.Append("EOD Settlement Response : - \n ");
                                rp.Append(GenericFunctions.Serialize(settlmentGet));

                                await _leaderboardBlanket.GenMonthLeaderboard(mGamedayId, mWeekId, monthId, mQuizId);
                                Catcher("PointCalculation:  Month Leaderboard File Generated.");

                                await _leaderboardBlanket.GenOverallLeaderboard(mQuizId);
                                Catcher("PointCalculation:  Overall Leaderboard File Generated.");

                                await _leaderboardBlanket.GetMixApi(mQuizId);
                                Catcher("PointCalculation:  MixAPI File Generated.");

                                CalculationNotify(monthId, tuple.Item1, rp);
                            }
                            else
                            {
                                if (tuple.Item2 != null)
                                Catcher("EOD Settlement  ==>  Settlement Error ==>", LogLevel.Error, tuple.Item2);
                            }

                            //if (tuple.Item2 != null)
                            //    Catcher("Point Calculation  ==>  Settlement Error ==>", LogLevel.Error, tuple.Item2);
                            ////else
                            //    Catcher("Point Calculation ==>  Settlement RetVal ==> " + tuple.Item1, LogLevel.Information);
                        }
                        else
                        {
                            Catcher("EOD Settlement: SettleProcessFlag=" + pntPrcGet.SettleProcessFlag);
                        }



                    }
                }
                else
                {
                    Catcher("EOD Settlement: Service Scheduled run at " + scheduleTime + " IST. Current Time in IST " + runTime);
                }
            }
            catch (Exception ex)
            {
                Catcher("EOD Settlement", LogLevel.Error, ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _Timer?.Change(Timeout.Infinite, 0);

            Catcher("EOD Settlement: Stopped.");

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



        #endregion
    }
}

