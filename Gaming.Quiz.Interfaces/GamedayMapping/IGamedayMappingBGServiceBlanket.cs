using Gaming.Quiz.Contracts.BGServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.GamedayMapping
{
    public interface IGamedayMappingBGServiceBlanket
    {
        List<GamedayProccesQuizes> GetQuizesForGamedayMapping();
        int ProcessGameday(long quizId);
    }
}
