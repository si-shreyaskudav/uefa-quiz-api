using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Contracts.Leaderboard
{
    public class UserRank
    {
        public string UserId { get; set; }

        public string UserName { get; set; }
        public string Points { get; set; }
        public string Rank { get; set; }
        public string Cur_Rno { get; set; }
        public string GUID { get; set; }
        public Int32 TotalAttempts { get; set; }
        public Int64 Trend { get; set; }

    }

    public class GenLeaderbaord
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Points { get; set; }
        public string Rank { get; set; }
        public string  Cur_Rno { get; set; }
        public Int64 TotalMember { get; set; }
        public string GUID { get; set; }
        public Int32 TotalAttempts { get; set; }
        public Int64 Trend { get; set; }
    }

    public class GenPlayerLeaderboard
    {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string Points { get; set; }
        public string Rank { get; set; }
        public string Cur_Rno { get; set; }
        public Int64 TotalMember { get; set; }
        public string GUID { get; set; }
        public Int64 TotalAttempts { get; set; }
        public Int64 Trend { get; set; }
    }

    public class UserRankInput
    {
        public Int64 OptType { get; set; }
        public Int64 MonthId { get; set; }
        public Int64 GamedayId { get; set; }
        public Int64 WeekId { get; set; }
        public Int64 QuizId { get; set; }
    }

    public class UserPlayerRankInput
    {
        public Int64 OptType { get; set; }
        public Int64 PlayerId { get; set; }
        public Int64 QuizId { get; set; }
    }

    public class Month
    {
        public string MonthId { get; set; }
        public string Month_Desc { get; set; }
    }

    public class Week
    {
        public string WeekId { get; set; }
        public string Week_Desc { get; set; }
    }

    public class Gameday
    {
        public string GamedayId { get; set; }
        public string Gameday_Desc { get; set; }
        public string Gameday_Date { get; set; }
    }

    public class FavPlayer
    {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string IsActive { get; set; }
    }

}
