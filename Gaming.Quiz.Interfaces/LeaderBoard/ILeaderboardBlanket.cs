using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Leaderboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.Leaderboard
{
    public interface ILeaderboardBlanket
    {
        Task<HTTPResponse> BestScoreLeaderboard(int optType, long monthId, Int64 pageNo = 1, Int64 mTopNo = 2000, Int64 GamedayId = 0, Int64 WeekId = 0, bool offloadDB = true);
        Task<Tuple<int, string>> GenBadgesFeed(long value);
        Task<Tuple<int, string>> GenBestScoreLeaderboard(long value1, long value2, long value3);
        Task<Tuple<int, string>> GenFavPlayerFile(long value);
        Task<Tuple<int, string>> GenGamedayFile(long value);
        Task<Tuple<int, string>> GenMonthFile(long value);
        Task<Tuple<int, string>> GenMonthLeaderboard(long value1, long value2, long value3, long value4);
        Task<Tuple<int, string>> GenOverallLeaderboard(long value);
        Task<Tuple<int, string>> GenTeamLeaderboard(long value1, long value2, long value3);
        Task<Tuple<int, string>> GenWeekFile(long value);
        Task<HTTPResponse> GetBestScoreUserRank(UserRankInput userRank);
        Task<HTTPResponse> GetFavPlayers(bool v);
        Task<Tuple<Int32, string>> GetMixApi(long mQuizId);
        Task<HTTPResponse> GetMonth(bool offloadDB = true);
        Task<HTTPResponse> GetTeamPlayerRank(UserPlayerRankInput userPlayerRank);
        Task<HTTPResponse> GetUserRank(UserRankInput userRank);
        Task<HTTPResponse> MonthLeaderboard(int optType, long monthId, Int64 pageNo = 1, Int64 mTopNo = 2000, Int64 GamedayId = 0, Int64 WeekId = 0, bool offloadDB = true);
        Task<HTTPResponse> OverAllLeaderboard(int optType, Int64 pageNo = 1, Int64 mTopNo = 2000, bool offloadDB = true);
        Task<HTTPResponse> TeamPlayerLeaderboard(int optType, long playerId, Int64 pageNo = 1, Int64 mTopNo = 2000, Int64 GamedayId = 0, Int64 WeekId = 0, bool offloadDB = true);
    }
}
