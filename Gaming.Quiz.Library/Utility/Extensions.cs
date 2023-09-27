using Gaming.Quiz.Contracts.Common;
using System;
using System.Linq;
using System.Text;

namespace  Gaming.Quiz.Library.Utility
{
    public static class Extensions
    {
        /*public static String USFormatString(this String date)
        {
            try
            {
                return DateTime.Parse(date).ToString(new System.Globalization.CultureInfo("en-US"));
            }
            catch
            {
                try
                {
                    return DateTime.Parse(date).ToString("MM/dd/yyyy hh:mm tt");
                }
                catch
                {
                    return date;
                }
            }
        }*/

        public static T ConvertToClassType<T>(String data)
        {
            T obj = default(T);

            try
            {
                if (!String.IsNullOrEmpty(data))
                {
                    obj = GenericFunctions.Deserialize<T>(GenericFunctions.Serialize(
                        ((GenericFunctions.Deserialize<ResponseObject>(GenericFunctions.Serialize((GenericFunctions.Deserialize<HTTPResponse>(data)).Data))).Value)));
                }

                return obj;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static T ConvertToClassTypeResponseObject<T>(String data)
        {
            T obj = default(T);

            try
            {
                if (!String.IsNullOrEmpty(data))
                {
                    obj = GenericFunctions.Deserialize<T>(data);
                }

                return obj;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static T ConvertToClassType<T>(HTTPResponse data)
        {
            T obj = default(T);

            try
            {
                if (data != null)
                {
                    obj = GenericFunctions.Deserialize<T>(GenericFunctions.Serialize(
                        ((GenericFunctions.Deserialize<ResponseObject>(GenericFunctions.Serialize(data.Data))).Value)));
                }

                return obj;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static String ToUniversalFormat(this String vDate)
        {
            String mDateStr = String.Empty;
            DateTime mDate;
            try
            {
                mDate = DateTime.ParseExact(vDate, "M/d/yy h:m:ss tt", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                //for local debuggning
                mDate = DateTime.ParseExact(vDate, "M/d/yyyy h:m:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                // = DateTime.ParseExact(vDate, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }

            mDateStr = mDate.ToString("u").Replace(" ", "T");

            return mDateStr;
        }

        public static String UniversalFormatToUTCTime(this String vDate)
        {
            String mDateStr = String.Empty;
            DateTime mDate;
            try
            {
                mDate = DateTimeOffset.Parse(vDate).DateTime.AddHours(-5).AddMinutes(-30);
            }
            catch (Exception ex)
            {
                //for local debuggning
                mDate = DateTimeOffset.Parse(vDate).DateTime.AddHours(-5).AddMinutes(-30);
            }

            mDateStr = mDate.ToString("u").Replace(" ", "T");

            return mDateStr;
        }

        public static String EditedName(this String vShortName)
        {
            String mShortName = String.Empty;

            if (vShortName.IndexOf(" ") > -1)
            {
                String[] arrays = vShortName.Split(' ');
                String initial = arrays[0].Substring(0, 1);
                String lastname = "";
                for (int i = 0; i < arrays.Count(); i++)
                {
                    if (arrays.Length == 2)
                    {
                        lastname = arrays[i].Substring(0, 1);
                    }
                    else if (arrays.Length == 3)
                    {
                        lastname = arrays[i].Substring(0, 1);
                    }
                    else if (arrays.Length > 3)
                        lastname = arrays[arrays.Count() - 1].Substring(0, 1);
                }
                mShortName += initial + lastname;

            }
            else
                mShortName = vShortName;

            return mShortName.ToUpper().Trim();
        }

        public static String ShortName(this String vShortName)
        {
            String name = "";

            if (vShortName.IndexOf(" ") > -1)
            {
                String[] arrays = vShortName.Split(' ');
                String lastName = "";

                for (int i = 0; i < arrays.Count(); i++)
                {
                    if (arrays[i] == arrays[i].ToUpper())
                        lastName += arrays[i] + " ";
                    else
                    {
                        if (arrays.Length == 3)
                        {
                            if (i == (arrays.Count() - 2))
                                lastName = arrays[i];
                            else if (i == (arrays.Count() - 1))
                                lastName += " " + arrays[i];
                        }
                        else
                        {
                            if (i == (arrays.Count() - 1))
                                lastName = arrays[i];
                        }
                    }
                }

                String initial = arrays[0].Substring(0, 1);

                name = initial + ". " + lastName;
            }
            else
                name = vShortName;

            return name.Trim();
        }

        public static String USFormatString(this String date)
        {
            try
            {
                return Convert.ToDateTime(date).ToString("MM/dd/yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
            }
            catch
            {
                return date;
            }
        }

        public static DateTime USFormatDateTime(this String dateTime)
        {
            return Convert.ToDateTime(dateTime, new System.Globalization.CultureInfo("en-US"));
        }

        public static String UniversalFormat(this String date)
        {
            DateTime d = DateTime.Parse(date, System.Globalization.CultureInfo.InvariantCulture);
            return d.ToString("u").Replace(" ", "T");
        }

        public static Double ToDouble(this String value)
        {
            Double d = 0d;
            try
            {
                if (!String.IsNullOrEmpty(value))
                {
                    double.TryParse(value.Trim(), out d);
                }
            }
            catch
            {
                d = 0d;
            }

            return d;
        }

        public static StringBuilder Log(this StringBuilder sb, String message)
        {
            sb.Append(message);
            sb.Append("<br/>");
            return sb;
        }

        public static bool HasError(this Tuple<int, string> tuple)
        {
            bool hasError = false;

            if (tuple.Item1 != 1 || !String.IsNullOrEmpty(tuple.Item2))
                hasError = true;

            return hasError;
        }

        public static double RunMilliseconds(this DateTime schedule, DateTime currentTime, Int32 value, String unit)
        {
            double ms = default(double);
            DateTime dt = default(DateTime);

            if (unit == "hours")
                dt = schedule.AddHours(value);
            else if (unit == "minutes")
                dt = schedule.AddMinutes(value);
            else if (unit == "seconds")
                dt = schedule.AddSeconds(value);

            ms = dt.Subtract(currentTime).TotalMilliseconds;

            return ms;
        }

        public static Int32 RunMilliseconds(Int32 value, String unit)
        {
            double ms = default(double);

            if (unit == "hours")
                ms = TimeSpan.FromHours(value).TotalMilliseconds;
            else if (unit == "minutes")
                ms = TimeSpan.FromMinutes(value).TotalMilliseconds;
            else if (unit == "seconds")
                ms = TimeSpan.FromSeconds(value).TotalMilliseconds;

            return Convert.ToInt32(ms);
        }

        public static Int32 SmartIntParse(this String value)
        {

            return Int32.Parse(String.IsNullOrEmpty(value) ? "0" : value);
        }

        public static Double SmartDoubleParse(this String value)
        {

            return Double.Parse(String.IsNullOrEmpty(value) ? "0" : value);
        }
    }
}
