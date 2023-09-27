using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Contracts.Analytics
{
    public class Analytics
    {
        public  List<OverallStats> OverallStats { get; set; }
        public List<DayStats> DayStats { get; set; }

    }

    public class OverallStats
    {
        public string Stat_Name { get; set; }
        public string Stat_Count { get; set; }
    }

    public class DayStats
    {
        public string GameDate { get; set; }
        public string Attempt_1 { get; set; }
        public string Attempt_2 { get; set; }
        public string Attempt_3 { get; set; }
        public string ActiveUsers { get; set; }
        public string Total_Attempts { get; set; }
        public string Unique_User { get; set; }
        public string Recurring_User { get; set; }
    }

    public class UserStats
    {
        public String UserId { get; set; }
        public String Name { get; set; }
        public String EmailId { get; set; }
        public String RegisterDate { get; set; }
        public String PlayedGames { get; set; }
        public String Total_Points{ get; set; }
        public String Total_Qes_Attempted { get; set; }
        public String Correct_Answers { get; set; }
        public String Longest_Streak { get; set; }
        public String Streak_Points { get; set; }
        public String Time_Bonus_Points { get; set; }
        public String Fastest_Time_Complete { get; set; }
        public String Monthly_Highest_Rank { get; set; }
        public String Daily_Highest_Rank { get; set; }
        public String Month_Rank { get; set; }
        public String Month { get; set; }

    }

    public class OverallStatsNew
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Total_Registrants { get; set; }
        public string Attempts_Played { get; set; }
        public string Incomplete_Attempts { get; set; }

    }

    public class AnalyticsNew
    {
        public List<OverallStatsNew> OverallStats { get; set; }
        public List<UserStats> UserStats { get; set; }

    }

    public class OverallQPLStats
    {
        public string Date { get; set; }
        public string GamedayId { get; set; }
        public string Total_Registrants { get; set; }
        public string Attempts_Played { get; set; }
        public string Attempts_Played_Perday { get; set; }
        public string Lifeline_Used_Count { get; set; }
    }

    public class QPLAnalytics
    {
        public List<OverallQPLStats> OverallQPLStats { get; set; }
       

    }
}
