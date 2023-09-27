using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gaming.Quiz.Admin.Models
{
    public class AnalyticsModel
    {
        public string fromdate { get; set; }
        public string todate { get; set; }

        public Contracts.Analytics.AnalyticsNew Analytics { get; set; }
        public Contracts.Analytics.QPLAnalytics QPLAnalytics { get; set; }
    }
}
