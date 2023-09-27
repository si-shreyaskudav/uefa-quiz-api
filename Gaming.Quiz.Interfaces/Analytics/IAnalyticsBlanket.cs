using Gaming.Quiz.Contracts.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.Analytics
{
    public interface IAnalyticsBlanket
    {
        Tuple<int, string> GetAnalyticsNew(string fromdate, string todate, ref AnalyticsNew analytics);
        Tuple<int, string> GetQPLAnalytics(string fromdate, string todate, ref QPLAnalytics qplanalytics);
        string GetServiceAnalyticsNew(string fromDate, string toDate);
        string GetServiceQPLAnalytics(string fromDate, string toDate);
    }
}
