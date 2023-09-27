using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.Enums;
using Gaming.Quiz.DataInitializer.Common;
using Gaming.Quiz.Interfaces.Analytics;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Blanket.Analytics
{
    public class Analytics : Common.BaseBlanket, IAnalyticsBlanket
    {
        protected readonly Gaming.Quiz.DataAccess.Analytics.Analytics _AnalyticsContext;

        public Analytics(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
           : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _AnalyticsContext = new DataAccess.Analytics.Analytics(appSettings, postgre, cookies);
        }


        public Tuple<int, string> GetAnalytics(string fromDate, string toDate, ref Contracts.Analytics.Analytics analytics)
        {
            int retval = -40;
            string error = string.Empty;

            try
            {
                 analytics = _AnalyticsContext.GetAnalytics(fromDate, toDate, ref retval);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retval, error);
        }

        public Tuple<int, string> GetAnalyticsNew(string fromDate, string toDate, ref Contracts.Analytics.AnalyticsNew analytics)
        {
            int retval = -40;
            string error = string.Empty;

            try
            {
                analytics = _AnalyticsContext.GetAnalyticsNew(fromDate, toDate, ref retval);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retval, error);
        }

        public Tuple<int, string> GetQPLAnalytics(string fromDate, string toDate, ref Contracts.Analytics.QPLAnalytics analytics)
        {
            int retval = -40;
            string error = string.Empty;

            try
            {
                analytics = _AnalyticsContext.GetQPLAnalytics(fromDate, toDate, ref retval);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retval, error);
        }

        public string GetServiceAnalytics(string fromDate, string toDate)
        {
            int retval = -40;
            string vHtml = string.Empty;
            string reportShare = string.Empty;
            string shareLink = string.Empty;

            Contracts.Analytics.Analytics analytics = new Contracts.Analytics.Analytics();
            int counter = 1;
            try
            {
                analytics = _AnalyticsContext.GetAnalytics(fromDate, toDate, ref retval);

                #region "Analytics Meta"

                vHtml = "<!DOCTYPE html><html> ";
                vHtml += "<head></head><body>";
                vHtml += "<div>";

                if (analytics != null)
                {
                    #region " Overall Stats Information "

                    vHtml += "<table style='border-collapse:collapse;text-align:center;line-height:25px;' border='1' width='100%'>";
                    vHtml += "<tr><th colspan='3' class='text-center'>Overall Stats</th></tr>";
                    vHtml += " <tr  bgcolor='#0B6DB1' color='#FFFFFF' style='color:#FFFFFF;'><th align='center'  class='text-center'> Sr. no </th><th align='center'  class='text-center'> Stat Name </th><th align='center'  class='text-center'> Stat Count </th></tr>";

                    foreach (var overall in analytics.OverallStats)
                    {
                        vHtml += "<tr>";
                        vHtml += "<td align='center'> " + counter + " </td>";
                        vHtml += "<td align='center'>" + overall.Stat_Name + " </td>";
                        vHtml += "<td align='center'>" + overall.Stat_Count + " </td>";
                        vHtml += "</tr>";
                        counter++;
                    }
                    vHtml += "</table></div></br>";

                    #endregion

                    #region " Day Stats Information "

                    counter = 1;
                    vHtml += "<div>";
                    vHtml += "<table style='border-collapse:collapse;text-align:center;line-height:25px;' border='1' width='100%'>";
                    vHtml += "<tr><th colspan='9' class='text-center'>Day Stats</th></tr>";
                    vHtml += " <tr  bgcolor='#0B6DB1' color='#FFFFFF' style='color:#FFFFFF;'>" +
                        "<th align='center'  class='text-center'> Sr. no </th>" +
                        "<th align='center'  class='text-center'> Game Date </th>" +
                        "<th align='center'  class='text-center'> Attempt (1) </th>" +
                        "<th align='center'  class='text-center'> Attempt (2) </th>" +
                        "<th align='center'  class='text-center'> Attempt (3) </th>" +
                        "<th align='center'  class='text-center'> Active User </th>" +
                        "<th align='center'  class='text-center'> Total Attempts</th>" +
                        "<th align='center'  class='text-center'> Unique User </th>" +
                        "<th align='center'  class='text-center'> Recurring User </th>" +
                        "</tr>";

                    foreach (var day in analytics.DayStats)
                    {
                        vHtml += "<tr>";
                        vHtml += "<td align='center'> " + counter + " </td>";
                        vHtml += "<td align='center'>" + day.GameDate + " </td>";
                        vHtml += "<td align='center'>" + day.Attempt_1 + " </td>";
                        vHtml += "<td align='center'>" + day.Attempt_2 + " </td>";
                        vHtml += "<td align='center'>" + day.Attempt_3 + " </td>";
                        vHtml += "<td align='center'>" + day.ActiveUsers + " </td>";
                        vHtml += "<td align='center'>" + day.Total_Attempts + " </td>";
                        vHtml += "<td align='center'>" + day.Unique_User + " </td>";
                        vHtml += "<td align='center'>" + day.Recurring_User + " </td>";
                        vHtml += "</tr>";
                        counter++;
                    }
                    vHtml += "</table></br></br>";

                    #endregion

                }
                vHtml += "</div> ";

                vHtml += "</body></html> ";
                #endregion

                string fileName = "analytics_report_" + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmm");
                
                bool status = _AWS.WriteS3Asset(_Asset.AnalyticsReport(fileName), MimeType.Text, vHtml);

                if (status)
                {
                    reportShare += "Analytics Report Generation Status " + (status ? "Success\n" : "Failed\n");
                    shareLink = _Asset.AnalyticsReport(fileName);
                    shareLink = shareLink.Replace("static", "static-assets");
                    reportShare += "Analytics Report Available Link : " + "https://"+_AppSettings.Value.API.Domain +"/"+_AppSettings.Value.Properties.StaticAssetBasePath+ shareLink;
                }
                else
                {
                    reportShare += "Analytics Report Generation Status " + (status ? "Success\n" : "Failed\n");
                }

            }
            catch (Exception ex)
            {
                reportShare += "Analytics Report Generation Status Failed\n";
                reportShare += "Analytics Report Generation Error " + ex.Message;
            }

            return reportShare;
        }


        public string GetServiceAnalyticsNew(string fromDate, string toDate)
        {
            int retval = -40;
            string vHtml = string.Empty;
            string reportShare = string.Empty;
            string shareLink = string.Empty;

            Contracts.Analytics.AnalyticsNew analytics = new Contracts.Analytics.AnalyticsNew();
            int counter = 1;
            try
            {
                analytics = _AnalyticsContext.GetAnalyticsNew(fromDate,toDate, ref retval);

                #region "Analytics Meta"

                vHtml = "<!DOCTYPE html><html> ";
                vHtml += "<head></head><body>";
                vHtml += "<div>";

                if (analytics != null)
                {
                    #region " Overall Stats Information "

                    vHtml += "<table style='border-collapse:collapse;text-align:center;line-height:25px;' border='1' width='100%'>";
                    vHtml += "<tr><th colspan='3' class='text-center'>Overall Stats</th></tr>";
                    vHtml += " <tr  bgcolor='#0B6DB1' color='#FFFFFF' style='color:#FFFFFF;'><th align='center'  class='text-center'> Sr. no </th><th align='center'  class='text-center'> From Date </th><th align='center'  class='text-center'> To Date </th><th align='center'  class='text-center'> Total Registrants </th><th align='center'  class='text-center'> Total Attempts </th><th align='center'  class='text-center'> Incomplete Attempts Registrants </th></tr>";

                    foreach (var overall in analytics.OverallStats)
                    {
                        vHtml += "<tr>";
                        vHtml += "<td align='center'> " + counter + " </td>";
                        vHtml += "<td align='center'>" + overall.FromDate + " </td>";
                        vHtml += "<td align='center'>" + overall.ToDate + " </td>";
                        vHtml += "<td align='center'>" + overall.Total_Registrants + " </td>";
                        vHtml += "<td align='center'>" + overall.Attempts_Played + " </td>";
                        vHtml += "<td align='center'>" + overall.Incomplete_Attempts + " </td>";

                        vHtml += "</tr>";
                        counter++;
                    }
                    vHtml += "</table></div></br>";

                    #endregion

                    #region " User Stats Information "

                    counter = 1;
                    vHtml += "<div>";
                    vHtml += "<table style='border-collapse:collapse;text-align:center;line-height:25px;' border='1' width='100%'>";
                    vHtml += "<tr><th colspan='9' class='text-center'>Day Stats</th></tr>";
                    vHtml += " <tr  bgcolor='#0B6DB1' color='#FFFFFF' style='color:#FFFFFF;'>" +
                        "<th align='center'  class='text-center'> Sr. no </th>" +
                        "<th align='center'  class='text-center'> UserId </th>" +
                        "<th align='center'  class='text-center'> Name </th>" +
                        "<th align='center'  class='text-center'> EmailId </th>" +
                        "<th align='center'  class='text-center'> Reg. Date </th>" +
                        "<th align='center'  class='text-center'> Tota Points </th>" +
                        "<th align='center'  class='text-center'> Total Attempts</th>" +
                        "<th align='center'  class='text-center'> Correct Answers </th>" +
                        "<th align='center'  class='text-center'> Longest Streak </th>" +
                        "<th align='center'  class='text-center'> Streak Points </th>" +
                        "<th align='center'  class='text-center'> Time Bonus Points </th>" +
                        "<th align='center'  class='text-center'> Fastest Time Complete </th>" +
                        "<th align='center'  class='text-center'> Montly Highest Rank </th>" +
                        "<th align='center'  class='text-center'> Daily Highest Rank </th>" +
                        "<th align='center'  class='text-center'> Month Rank </th>" +
                        "<th align='center'  class='text-center'> Month </th>" +
                        "</tr>";

                    foreach (var day in analytics.UserStats)
                    {
                        vHtml += "<tr>";
                        vHtml += "<td align='center'> " + counter + " </td>";
                        vHtml += "<td align='center'>" + day.UserId + " </td>";
                        vHtml += "<td align='center'>" + day.Name + " </td>";
                        vHtml += "<td align='center'>" + day.EmailId + " </td>";
                        vHtml += "<td align='center'>" + day.RegisterDate + " </td>";
                        vHtml += "<td align='center'>" + day.Total_Points + " </td>";
                        vHtml += "<td align='center'>" + day.Total_Qes_Attempted + " </td>";
                        vHtml += "<td align='center'>" + day.Correct_Answers + " </td>";
                        vHtml += "<td align='center'>" + day.Longest_Streak + " </td>";
                        vHtml += "<td align='center'>" + day.Streak_Points + " </td>";
                        vHtml += "<td align='center'>" + day.Time_Bonus_Points + " </td>";
                        vHtml += "<td align='center'>" + day.Fastest_Time_Complete + " </td>";
                        vHtml += "<td align='center'>" + day.Monthly_Highest_Rank + " </td>";
                        vHtml += "<td align='center'>" + day.Daily_Highest_Rank + " </td>";
                        vHtml += "<td align='center'>" + day.Month_Rank + " </td>";
                        vHtml += "<td align='center'>" + day.Month + " </td>";
                        vHtml += "</tr>";
                        counter++;
                    }
                    vHtml += "</table></br></br>";

                    #endregion

                }
                vHtml += "</div> ";

                vHtml += "</body></html> ";
                #endregion

                string fileName = "analytics_report_" + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmm");

                bool status = _AWS.WriteS3Asset(_Asset.AnalyticsReport(fileName), MimeType.Text, vHtml);

                if (status)
                {
                    reportShare += "Analytics Report Generation Status " + (status ? "Success\n" : "Failed\n");
                    shareLink = _Asset.AnalyticsReport(fileName);
                    shareLink = shareLink.Replace("static", "static-assets");
                    reportShare += "Analytics Report Available Link : " + "https://" + _AppSettings.Value.API.Domain + "/" + _AppSettings.Value.Properties.StaticAssetBasePath + shareLink;
                }
                else
                {
                    reportShare += "Analytics Report Generation Status " + (status ? "Success\n" : "Failed\n");
                }

            }
            catch (Exception ex)
            {
                reportShare += "Analytics Report Generation Status Failed\n";
                reportShare += "Analytics Report Generation Error " + ex.Message;
            }

            return reportShare;
        }

        public string GetServiceQPLAnalytics(string fromDate, string toDate)
        {
            int retval = -40;
            string vHtml = string.Empty;
            string reportShare = string.Empty;
            string shareLink = string.Empty;

            Contracts.Analytics.QPLAnalytics analytics = new Contracts.Analytics.QPLAnalytics();
            int counter = 1;
            try
            {
                analytics = _AnalyticsContext.GetQPLAnalytics(fromDate, toDate, ref retval);

                #region "Analytics Meta"

                vHtml = "<!DOCTYPE html><html> ";
                vHtml += "<head></head><body>";
                vHtml += "<div>";

                if (analytics != null)
                {
                    #region " Overall QPL Stats Information "

                    vHtml += "<table style='border-collapse:collapse;text-align:center;line-height:25px;' border='1' width='100%'>";
                    vHtml += "<tr><th colspan='3' class='text-center'>QPL Overall Stats</th></tr>";
                    vHtml += " <tr  bgcolor='#0B6DB1' color='#FFFFFF' style='color:#FFFFFF;'><th align='center'  class='text-center'> Sr. no </th><th align='center'  class='text-center'> Date </th><th align='center'  class='text-center'> GamedayId </th><th align='center'  class='text-center'> Total Registrants </th><th align='center'  class='text-center'> Total Attempts </th><th align='center'  class='text-center'> Attempts Per Day </th><th align='center'  class='text-center'> Lifeline Count </th></tr>";

                    foreach (var overall in analytics.OverallQPLStats)
                    {
                        vHtml += "<tr>";
                        vHtml += "<td align='center'> " + counter + " </td>";
                        vHtml += "<td align='center'>" + overall.Date + " </td>";
                        vHtml += "<td align='center'>" + overall.GamedayId + " </td>";
                        vHtml += "<td align='center'>" + overall.Total_Registrants + " </td>";
                        vHtml += "<td align='center'>" + overall.Attempts_Played + " </td>";
                        vHtml += "<td align='center'>" + overall.Attempts_Played_Perday + " </td>";
                        vHtml += "<td align='center'>" + overall.Lifeline_Used_Count + " </td>";

                        vHtml += "</tr>";
                        counter++;
                    }
                    vHtml += "</table></div></br>";

                    #endregion

                    

                }
                vHtml += "</div> ";

                vHtml += "</body></html> ";
                #endregion

                string fileName = "analytics_qpl_report_" + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmm");

                bool status = _AWS.WriteS3Asset(_Asset.AnalyticsReport(fileName), MimeType.Text, vHtml);

                if (status)
                {
                    reportShare += "QPL Analytics Report Generation Status " + (status ? "Success\n" : "Failed\n");
                    shareLink = _Asset.AnalyticsReport(fileName);
                    shareLink = shareLink.Replace("static", "static-assets");
                    reportShare += "QPL Analytics Report Available Link : " + "https://" + _AppSettings.Value.API.Domain + "/" + _AppSettings.Value.Properties.StaticAssetBasePath + shareLink;
                }
                else
                {
                    reportShare += "QPL Analytics Report Generation Status " + (status ? "Success\n" : "Failed\n");
                }

            }
            catch (Exception ex)
            {
                reportShare += "QPL Analytics Report Generation Status Failed\n";
                reportShare += "QPL Analytics Report Generation Error " + ex.Message;
            }

            return reportShare;
        }

    }
}
