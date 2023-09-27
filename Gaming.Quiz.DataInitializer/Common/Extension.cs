using System;
using System.Collections.Generic;
using System.Globalization;

namespace  Gaming.Quiz.DataInitializer.Common
{
    public static class Extension
    {
        public static Int64 IntValue(this Object column)
        {
            //return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? 0 : Int64.Parse(column.ToString());
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? 0 : Convert.ToInt64(Convert.ToDecimal(column).ToString("0.#"));
        }
        public static Int32 Int32Value(this Object column)
        {
            //return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? 0 : Int64.Parse(column.ToString());
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? 0 : Convert.ToInt32(Convert.ToDecimal(column).ToString("0.#"));
        }


        public static object[] IntNullCheck(this Int64[] column)
        {
            List<object> mColum = new List<object>();
            foreach (var val in column)
            {
                if (val == 0)
                    mColum.Add(DBNull.Value);
                else
                    mColum.Add(val);
            }

            //return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? 0 : Int64.Parse(column.ToString());
            return mColum.ToArray();
        }

        public static object[] IntNullCheck(this Int32[] column)
        {
            List<object> mColum = new List<object>();
            foreach (var val in column)
            {
                if (val == 0)
                    mColum.Add(DBNull.Value);
                else
                    mColum.Add(val);
            }

            //return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? 0 : Int64.Parse(column.ToString());
            return mColum.ToArray();
        }

        public static object[] StringNullCheck(this string[] column)
        {
            List<object> mColum = new List<object>();
            foreach (var val in column)
            {
                if (string.IsNullOrEmpty(val) || val == "" || val ==null)
                    mColum.Add(DBNull.Value);
                else
                    mColum.Add(val);
            }

            //return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? 0 : Int64.Parse(column.ToString());
            return mColum.ToArray();
        }

        public static DateTime[] DateTimeArray(this String[] column)
        {
            List<DateTime> QDate = new List<DateTime>();
            foreach (var date in column)
            {
               QDate.Add(Convert.ToDateTime(date));
            }

            //return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? 0 : Int64.Parse(column.ToString());
            return QDate.ToArray();
        }

        public static Int64? NullableIntValue(this Object column)
        {
            //return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? 0 : Int64.Parse(column.ToString());
            if (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString()) || column.ToString().Trim() == "-1" || column.ToString().Trim() == "0")
                return null;
            else
                return Convert.ToInt64(column.ToString().Trim());
        }

        public static String StringValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? "" : column.ToString();
        }

        public static string DateValue(this Object column)
        {
            //return DateTime.ParseExact(column.ToString().Trim(), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            return DateTime.ParseExact(column.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
        }

        public static String OneDecimalValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? "" : Convert.ToDecimal(column).ToString("0.#");
        }

        public static String NoDecimalValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? "" : Convert.ToDecimal(column).ToString("0");
        }

        public static String AtleastOneDecimalValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? "" : Convert.ToDecimal(column).ToString("0.0");
        }

        public static String AtleastOneDecimalWithNullValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? null :
                (column.ToString() == "0" ? "0" : Convert.ToDecimal(column).ToString("0.0"));
        }

        public static String TwoDecimalValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? "" : Convert.ToDecimal(column).ToString("0.##");
        }

        public static String RankValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? null : column.ToString();
        }

        public static bool BoolValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? false : (column.ToString() == "1" ? true : false);
        }

        public static String HexValue(this Object column)
        {
            if (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString()))
                return "";
            else
            {
                return BitConverter.ToString(System.Text.Encoding.Default.GetBytes(column.ToString())).Replace("-", "");
            }
        }

        public static String USDateValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? ""
                : Library.Utility.Extensions.USFormatString(column.ToString());
        }

        public static String UniversalDateValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? ""
                : Library.Utility.Extensions.UniversalFormat(column.ToString());
        }

        public static Int64 ReturnValue(this Object column)
        {
            //return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? -50 : Int64.Parse(column.ToString());
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? -50 : Convert.ToInt64(Convert.ToDecimal(column).ToString("0.#"));
        }

        public static Object ToIntDBValue(this Int64? value)
        {
            if (value == null)
                return DBNull.Value;
            else
                return value;
        }

        public static String ToStringDBValue(this String value)
        {
            return (value == null) ? DBNull.Value.ToString() : value.ToString();
        }

        public static object[] ToStringDBValue(this String[] value)
        {
            List<object> val = new List<object>();

            foreach (var s in value)
            {
                if (string.IsNullOrEmpty(s) || s == "")
                    val.Add(DBNull.Value);
                else
                    val.Add(s);
            }
            
            return val.ToArray();
        }
        public static DateTime ToDateTimeValue(this string value)
        {
            return Convert.ToDateTime(DateTime.ParseExact(value.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
             //Convert.ToDateTime((DateTime.ParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")));
        }

        #region " Encrypt/Decrypt Mutator "

        public static String _192Base16EncryptValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? ""
                : Library.Encryption.Aes192.Base16Encrypt(column.ToString());
        }

        public static String _192Base16DecryptValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? ""
                : Library.Encryption.Aes192.Base16Decrypt(column.ToString());
        }

        public static String _128Base16EncryptValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? ""
                : Library.Encryption.Aes128.Base16Encrypt(column.ToString());
        }

        public static String _128Base16DecryptValue(this Object column)
        {
            return (Convert.IsDBNull(column) || column == null || String.IsNullOrEmpty(column.ToString())) ? ""
                : Library.Encryption.Aes128.Base16Decrypt(column.ToString());
        }

        #endregion
    }
}
