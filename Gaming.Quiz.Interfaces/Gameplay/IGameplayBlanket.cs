using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.Gameplay
{
    public interface IGameplayBlanket
    {
        Task<HTTPResponse> GetHint(Lifeline lifeline);
        Task<HTTPResponse> GetUserBadges(long optType, long quizId);
        Task<HTTPResponse> GetUserDetails(long quizId);
        Task<HTTPResponse> GetUserStats(long optType, long quizId, long monthId);
        Task<HTTPResponse> LLFiftyFifty(Lifeline lifeline);
        Task<HTTPResponse> LLSneakPeak(Lifeline lifeline, long optType);
        Task<HTTPResponse> LLSwitch(Lifeline lifeline);
        Task<HTTPResponse> PlayAttempt(PlayAttempt playAttempt);
        Task<HTTPResponse> RegisterQuestion(Question question);
        Task<HTTPResponse> ScoreCard(ScoreCard card);
        Task<HTTPResponse> Settlement(long gamedayId, long attemptId, long platformId, long quizId);
        Task<HTTPResponse> UpdateBadgeNotify(long optType, long monthId, long quizId);
    }
}
