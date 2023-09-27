using Gaming.Quiz.Interfaces.Session;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Gaming.Quiz.Library.Utility
{
    public class LocalErrorLog
    {
        public static void LogException(System.Reflection.MethodBase methodBase, ICookies UsrCookie , Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;

            try
            {
                var fullFilePath = CreateFile();
                var ExceptionAPI = "Exception API :- " + methodBase.DeclaringType.FullName;
                var UserCookie = "UserCookie :- " + GenericFunctions.Serialize(UsrCookie);
                var ErrorLog = "nException Details: " + GenericFunctions.Serialize(ex);
                using (StreamWriter sw = File.AppendText(fullFilePath))
                {
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(ExceptionAPI);
                    sw.WriteLine(UserCookie);
                    sw.WriteLine(ErrorLog);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        public static void Log(string message)
        {
            var line = Environment.NewLine + Environment.NewLine;

            try
            {
                var fullFilePath = CreateFile();
                using (StreamWriter sw = File.AppendText(fullFilePath))
                {
                    sw.WriteLine("-----------Checkpoint Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine("Info: " + message);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        private static string CreateFile()
        {
            var baseLogPath = Path.Combine("/var/log/dream11/");
            var dailyFolderPath = Path.Combine(baseLogPath, DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
            var fullFilePath = Path.Combine(dailyFolderPath, "LogFile_" + DateTime.Now.ToString("HH", CultureInfo.InvariantCulture) + ".txt");

            if (!Directory.Exists(baseLogPath))
            {
                Directory.CreateDirectory(baseLogPath);
            }
            if (!Directory.Exists(dailyFolderPath))
            {
                Directory.CreateDirectory(dailyFolderPath);
            }
            if (!File.Exists(fullFilePath))
            {
                File.Create(fullFilePath).Dispose();
            }

            return fullFilePath;
        }
    }
}
