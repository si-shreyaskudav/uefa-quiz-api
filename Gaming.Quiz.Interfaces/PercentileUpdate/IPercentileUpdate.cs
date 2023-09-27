using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.PercentileUpdate
{
    public interface IPercentileUpdate
    {
        Tuple<int, Exception> UpdatePercentile();
    }
}
