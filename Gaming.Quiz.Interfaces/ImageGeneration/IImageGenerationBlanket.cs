using Gaming.Quiz.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.Sharing
{
    public interface IImageGenerationBlanket
    {
        Task<HTTPResponse> GenerateImage(string GUID, string UserName, Int32 RightAns, Int32 TotalQtn, string StreakPts, Int32 TotPoints, List<Int32> CorrectAns, List<Int32> QsnStreak, string message, int platformId, string Lang, Int32 Percentile, int gamedayId, Int32 AnsPoints, string TimePoints);
    }
}
