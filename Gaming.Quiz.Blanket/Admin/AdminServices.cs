using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.DataPopulation;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Library.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Gaming.Quiz.DataInitializer.Common;
using Gaming.Quiz.Contracts.Admin;
using Gaming.Quiz.Interfaces.Admin;

namespace Gaming.Quiz.Blanket.Admin
{
    public class AdminServices : Common.BaseBlanket, IAdminServicesBlanket
    {
        private readonly DataAccess.Admin.AdminServices _DBContext;

        public AdminServices(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
        : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _DBContext = new DataAccess.Admin.AdminServices(appSettings, postgre, cookies);
        }

        public Tuple<int, String> QuizMapping(Int64 QMId, string date)
        {
            String error = string.Empty;
            Int32 retVal = -40;
            Int64 OptType = 1;
            try
            {
                DateTime dt = date.ToDateTimeValue();

                retVal = _DBContext.QuizMappingProcess(OptType, dt,QMId);


            }
            catch (Exception ex)
            {
                LocalErrorLog.LogException(System.Reflection.MethodBase.GetCurrentMethod(), null, ex);

                error = ex.Message;
            }
            return new Tuple<int, String>(Convert.ToInt32(retVal), error);
        }

        public Tuple<int, String> AdminLLget(Int64 QzMId, ref List<vAdminLLget> cursorOutput)
        {
            String error = string.Empty;
            Int32 retVal = -40;
            Int32 OptType = 1;
            try
            {
                retVal = _DBContext.AdminLLGet(OptType, QzMId, ref cursorOutput);
            }
            catch (Exception ex)
            {
                LocalErrorLog.LogException(System.Reflection.MethodBase.GetCurrentMethod(), null, ex);

                error = ex.Message;
            }
            return new Tuple<int, String>(Convert.ToInt32(retVal), error);
        }

        public Tuple<int, String> GamedayDetailsGet(Int64 QzMId, ref List<vGamedayDetails> cursorOutput)
        {
            String error = string.Empty;
            Int32 retVal = -40;
            Int32 OptType = 1;
            try
            {
                retVal = _DBContext.GameDetailsGet(OptType, QzMId, ref cursorOutput);
            }
            catch (Exception ex)
            {
                LocalErrorLog.LogException(System.Reflection.MethodBase.GetCurrentMethod(), null, ex);

                error = ex.Message;
            }
            return new Tuple<int, String>(Convert.ToInt32(retVal), error);
        }

        public Tuple<int, String> MonthDetailsGet(Int64 QzMId, ref List<vMonthDetails> cursorOutput)
        {
            String error = string.Empty;
            Int32 retVal = -40;
            Int32 OptType = 1;
            try
            {
                retVal = _DBContext.MonthDetailsGet(OptType, QzMId, ref cursorOutput);
            }
            catch (Exception ex)
            {
                LocalErrorLog.LogException(System.Reflection.MethodBase.GetCurrentMethod(), null, ex);

                error = ex.Message;
            }
            return new Tuple<int, String>(Convert.ToInt32(retVal), error);
        }


    }
}
