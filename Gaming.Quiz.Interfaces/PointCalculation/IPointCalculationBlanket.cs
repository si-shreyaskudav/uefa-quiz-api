using Gaming.Quiz.Contracts.PointCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.PointCalculation
{
    public interface IPointCalculationBlanket
    {
        Task<Tuple<int, string>> EODSettlement();
        Tuple<int, Exception> ServiceEODSettlement(ref SettlmentGet settlmentGet);
        Task<Tuple<int, Exception>> ServiceUserPointProcess(int monthId, int gamedayId, int weekId);
        Task<Tuple<int, string>> UserBestScoreUpdate(long value1, int value2, int value3, int value4);
        List<PntPrcGet> UserEODSettlementGet(ref string error);
        Task<Tuple<int, string>> UserPointProcess(int value1, int value2, int value3);
        List<PntPrcGet> UserPointProcessGet(ref string error);
    }
}
