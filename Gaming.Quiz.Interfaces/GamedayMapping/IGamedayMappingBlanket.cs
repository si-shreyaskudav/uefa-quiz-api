using Gaming.Quiz.Contracts.BGServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.GamedayMapping
{
    public interface IGamedayMappingBlanket
    {
        List<Contracts.GamedayMapping.GamedayMapping> GetGamedayMappings();
        int UpdateGamedayMapping(List<int> gamedayId, List<string> tag);
    }
}
