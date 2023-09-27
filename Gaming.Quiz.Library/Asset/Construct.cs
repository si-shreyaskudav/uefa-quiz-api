using System;
using  Gaming.Quiz.Interfaces.Session;
using Microsoft.AspNetCore.Http;
using  Gaming.Quiz.Contracts.Common;
using  Gaming.Quiz.Library.Utility;
using System.IO;
using System.Collections.Generic;

namespace  Gaming.Quiz.Library.Asset
{
    public class Construct
    {
        public static HTTPLog Log(String message, Exception exception)
        {
            HTTPLog httpLog = new HTTPLog();

            try
            {
                String msg = message, func = null;

                if (exception != null)
                {
                    msg = $"{message}. {exception.Message}";
                    func = exception.StackTrace;
                }

                httpLog.Function = func;
                httpLog.Message = msg;
                httpLog.Timestamp = GenericFunctions.GetFeedTime();
            }
            catch (Exception ex) { }

            return httpLog;
        }

        public static HTTPLog Log(System.Reflection.MethodBase methodBase, String message, ICookies cookies, IHttpContextAccessor httpContextAccessor)
        {
            HTTPLog httpLog = new HTTPLog();

            try
            {
                if (methodBase == null)
                    methodBase = System.Reflection.MethodBase.GetCurrentMethod();

                httpLog.Function = methodBase.DeclaringType.FullName + "." + methodBase.Name;
                httpLog.Message = message;
                httpLog.RequestType = httpContextAccessor.HttpContext.Request.Method;
                httpLog.RequestUri = httpContextAccessor.HttpContext.Request.Path + Query(httpContextAccessor.HttpContext.Request.Query, httpContextAccessor);
                httpLog.UserAgent = httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
                httpLog.Timestamp = GenericFunctions.GetFeedTime();
                httpLog.Cookies = new {GameCookie = cookies._GetGameCookies };

                if (httpLog.RequestType.ToUpper() == "GET")
                    httpLog.Payload = Query(httpContextAccessor.HttpContext.Request.Query, httpContextAccessor);
                else if (httpLog.RequestType.ToUpper().ToUpper() == "POST")
                {
                    //ASP.NET Core not let you read request body several times, hence the below two lines.
                    httpContextAccessor.HttpContext.Request.EnableBuffering();
                    httpContextAccessor.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                    //---- end ----

                    StreamReader reader = new StreamReader(httpContextAccessor.HttpContext.Request.Body, System.Text.Encoding.UTF8);
                    httpLog.Payload = reader.ReadToEnd();
                }
            }
            catch (Exception ex) { }

            return httpLog;
        }

        private static String Query(IQueryCollection query, IHttpContextAccessor httpContextAccessor)
        {
            string value = "?";

            foreach (KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues> dict in httpContextAccessor.HttpContext.Request.Query)
            {
                value += dict.Key + "=" + dict.Value + "&";
            }

            if (value == "?")
                value = "";

            return value;
        }
    }
}
