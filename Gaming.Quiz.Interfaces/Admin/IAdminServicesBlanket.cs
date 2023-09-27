using Gaming.Quiz.Contracts.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.Admin
{
    public interface IAdminServicesBlanket
    {
        Tuple<int, string> AdminLLget(long value, ref List<vAdminLLget> vAdminLL);
        Tuple<int, string> GamedayDetailsGet(long value, ref List<vGamedayDetails> vGdDet);
        Tuple<int, string> MonthDetailsGet(long value, ref List<vMonthDetails> vMonthGet);
        Tuple<int, string> QuizMapping(long value, string qzMapDate);
    }
}
