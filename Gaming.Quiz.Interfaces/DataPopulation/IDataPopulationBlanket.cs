using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.DataPopulation
{
    public interface IDataPopulationBlanket
    {
        Tuple<int, string> InsertQuestion(IFormFile questions);
        Tuple<int, string> VerifyQuestion(long value1, long value2, long value3, string date, out DataSet ds);
    }
}
