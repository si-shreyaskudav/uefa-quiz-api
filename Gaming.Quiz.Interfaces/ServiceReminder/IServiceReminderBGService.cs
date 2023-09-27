using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.ServiceReminder
{
    public interface IServiceReminderBGService
    {
        List<string> GetPendingDays(long dayNo, ref int retVal, ref string error);
    }
}
