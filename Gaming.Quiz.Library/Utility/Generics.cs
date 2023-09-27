using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using  Gaming.Quiz.Contracts.Common;
using System.Threading.Tasks;
using System.Data;
using Syncfusion.XlsIO;

namespace  Gaming.Quiz.Library.Utility
{
    public class GenericFunctions
    {
        public static String Serialize(object data)
        {
            //Explicitly converting data to camelCase.
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            return JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                MaxDepth = Int32.MaxValue,
                //ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
        }

        public static T Deserialize<T>(String data)
        {
            return JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings() { MaxDepth = Int32.MaxValue });
        }

        public static T ChangeType<T>(Object data)
        {
            return (T)Convert.ChangeType(data, typeof(T));
        }

        public static bool HasWebData(String url)
        {
            bool exists = false;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    exists = response.StatusCode == HttpStatusCode.OK;
                };
            }
            catch (Exception ex)
            {
                exists = false;
                //throw new Exception($"HasWebData: {ex.Message}");
            }

            return exists;
        }

        public static String GetWebData(String url, String auth = null, bool compression = true)
        {
            string strRetVal = string.Empty;

            try
            {
                HttpWebRequest mHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                mHttpWebRequest.Method = "GET";
                mHttpWebRequest.Timeout = 30000; // 30 Seconds
                mHttpWebRequest.KeepAlive = false;
                mHttpWebRequest.ProtocolVersion = HttpVersion.Version10;
                mHttpWebRequest.Accept = "application/json";

                if (compression)
                    mHttpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                if (auth != null)
                    mHttpWebRequest.Headers.Add("Authorization", auth);

                using (WebResponse response = mHttpWebRequest.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        strRetVal = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"GetWebData: URL = {url} - {ex.Message}");
            }

            return strRetVal;
        }

        public static String GetWAFWebData(String url, Dictionary<String, String> headers = null)
        {
            string strRetVal = string.Empty;

            try
            {
                System.Net.HttpWebRequest mHttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);

                mHttpWebRequest.Method = "GET";
                mHttpWebRequest.Timeout = 30000; // 30 Seconds
                mHttpWebRequest.KeepAlive = false;
                mHttpWebRequest.ProtocolVersion = System.Net.HttpVersion.Version10;
                mHttpWebRequest.Accept = "application/json";
                mHttpWebRequest.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;

                if (headers != null)
                {
                    foreach (String mHeader in headers.Keys)
                        mHttpWebRequest.Headers.Add(mHeader, headers[mHeader].ToString());
                }

                using (System.Net.WebResponse response = mHttpWebRequest.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        strRetVal = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex) { }

            return strRetVal;
        }
        public static String GetSocialWebData(String url, String token = null)
        {
            string strRetVal = string.Empty;

            try
            {
                System.Net.HttpWebRequest mHttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);

                mHttpWebRequest.Method = "GET";
                mHttpWebRequest.Timeout = 30000; // 30 Seconds
                mHttpWebRequest.KeepAlive = false;
                mHttpWebRequest.ProtocolVersion = System.Net.HttpVersion.Version10;
                mHttpWebRequest.Accept = "application/json";

                if (token != null)
                    mHttpWebRequest.Headers.Add("Authorization", token);

                using (System.Net.WebResponse response = mHttpWebRequest.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        strRetVal = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetSocialWebData token: " + token + " Message: " + ex.Message);
            }

            return strRetVal;
        }

        public static String GetYahooTemplateData(String url, Dictionary<String, String> headers = null)
        {
            string strRetVal = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (String.IsNullOrWhiteSpace(response.CharacterSet))
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                    strRetVal = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception ex) { }

            return strRetVal;
        }

        public static String GetYahooWebData(String url, ref String usertoken, Dictionary<String, String> headers = null)
        {
            string strRetVal = string.Empty;

            try
            {
                System.Net.HttpWebRequest mHttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);

                mHttpWebRequest.Method = "GET";
                mHttpWebRequest.Timeout = 30000; // 30 Seconds
                mHttpWebRequest.KeepAlive = false;
                mHttpWebRequest.ProtocolVersion = System.Net.HttpVersion.Version10;
                mHttpWebRequest.Accept = "application/json";
                mHttpWebRequest.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;

                if (headers != null)
                {
                    foreach (String mHeader in headers.Keys)
                        mHttpWebRequest.Headers.Add(mHeader, headers[mHeader].ToString());
                }

                using (System.Net.WebResponse response = mHttpWebRequest.GetResponse())
                {
                    usertoken = response.Headers.Get("user_token");
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        strRetVal = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex) { }

            return strRetVal;
        }

        public static MemoryStream GetWebStream(String url)
        {
            MemoryStream ms = new MemoryStream();

            try
            {
                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData(url);
                ms = new MemoryStream(bytes);
            }
            catch (Exception ex)
            {
                throw new Exception($"GetWebStream: {ex.Message}");
            }

            return ms;
        }

        public static String PostWebData(String url, String param)
        {
            string strRetVal = string.Empty;

            try
            {
                WebRequest req = WebRequest.Create(url);
                req.ContentType = "application/x-www-form-urlencoded";
                req.Method = "POST";

                byte[] bytes = Encoding.UTF8.GetBytes(param);
                req.ContentLength = bytes.Length;
                Stream os = req.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);

                os.Close();

                using (WebResponse response = req.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        strRetVal = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"PostWebData: {ex.Message}");
            }

            return strRetVal;
        }

        public static String PostWebData(String url, String param, Dictionary<String, String> headers = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            var data = Encoding.ASCII.GetBytes(param);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Accept = "application/json";

            foreach (String mKey in headers.Keys.ToList())
            {
                request.Headers.Add(mKey, headers[mKey].ToString());
            }

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        public static String TimeDifference(DateTime startTime, DateTime endTime)
        {
            TimeSpan span = endTime.Subtract(startTime);
            return String.Format("{0} minute, {1} seconds", span.Minutes, span.Seconds);
        }
        public static FeedTime GetFeedTime()
        {
            return new FeedTime()
            {
                UTCTime = TimeFunction.CurrentUTCtime(),
                ISTTime = TimeFunction.CurrentISTtime(),
                CESTTime = TimeFunction.CurrentCESTtime()
            };
        }

        public static void AssetMeta(Int64 retVal, ref HTTPMeta httpMeta, String message = "")
        {
            httpMeta.Success = (retVal == 1);
            httpMeta.RetVal = retVal;
            httpMeta.Message = !String.IsNullOrEmpty(message) ? message : (retVal == 1 ? "Success" : "Failed");
            httpMeta.Timestamp = GetFeedTime();
        }

        public static String DecryptedValue(String encryptedValue)
        {
            string val = "";

            try
            {
                if (!String.IsNullOrEmpty(encryptedValue))
                    val = Library.Encryption.Aes192.Base16Decrypt(encryptedValue);
            }
            catch
            {
                try
                {
                    val = Library.Encryption.Aes192.Base64Decrypt(encryptedValue);
                }
                catch
                {
                    try
                    {
                        val = Library.Encryption.Aes192.Base16Decrypt(encryptedValue);
                    }
                    catch { }
                }
            }

            return val;
        }

        public static String DebugTable(System.Data.DataTable table)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--- cursor {" + table.TableName + "} ---");
            sb.Append(System.Environment.NewLine);
            int zeilen = table.Rows.Count;
            int spalten = table.Columns.Count;

            // Header
            for (int i = 0; i < table.Columns.Count; i++)
            {
                string s = table.Columns[i].ToString();
                sb.Append(String.Format("{0,-20} | ", s));
            }
            sb.Append(System.Environment.NewLine);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sb.Append("---------------------|-");
            }
            sb.Append(System.Environment.NewLine);

            // Data
            for (int i = 0; i < zeilen; i++)
            {
                System.Data.DataRow row = table.Rows[i];

                for (int j = 0; j < spalten; j++)
                {
                    string s = row[j].ToString();
                    if (s.Length > 20) s = s.Substring(0, 17) + "...";
                    sb.Append(String.Format("{0,-20} | ", s));
                }
                sb.Append(System.Environment.NewLine);
            }
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sb.Append("---------------------|-");
            }
            sb.Append(System.Environment.NewLine);

            Debug.WriteLine(sb);//writes to Output window
            return sb.ToString();
        }

        public static Int64[] GetPagePoints(Int64 pageOneChunk, Int64 pageChunk, Int64 pageNo)
        {
            Int64[] address = new Int64[2];

            Int64 mPageOneSize = pageOneChunk;
            Int64 mCurrPageSize = pageChunk;
            Int64 mPageNo = pageNo;

            Int64 mFrom = 0;
            Int64 mTo = 0;

            mTo = mPageOneSize + ((mPageNo - 1) * mCurrPageSize);
            if (mPageNo == 1)
                mFrom = mTo - mPageOneSize;
            else
                mFrom = mTo - mCurrPageSize;

            mFrom = mFrom + 1;

            address[0] = mFrom;
            address[1] = mTo;

            return address;
        }

        public static String EmailBody(String service, String contents)
        {
            String body = String.Empty;

            body = "This is a system generated mail from " + service + " daemon service running on " + Environment.MachineName.ToUpper() + ".\n\n";
            body += "The service invoked the " + service + " process.\n\n";
            body += "" + contents + "\n\n";
            body += "Thanks.";

            return body;
        }

        public static String Between(String input, String startText, String endText)
        {
            String between = default(String);

            int from = input.ToLower().IndexOf(startText) + startText.Length;
            int to = input.ToLower().LastIndexOf(endText);
            between = input.Substring(from, to - from);

            return between;
        }

        public static String UniqueString()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);

            string id = Convert.ToBase64String(bytes).Replace('+', '_').Replace('/', '-').TrimEnd('=');//11 characters
            //string id = DateTime.Now.ToString("yyyyMMddHHmmss");//14 characters
            //string id = DateTime.Now.ToString("yyMMddHHmmssff");//14 characters

            return id;
        }

        public static String GetFileName(String filePlaceholder)
        {
            if (String.IsNullOrEmpty(filePlaceholder))
                return "";

            Int32 indexOf = filePlaceholder.IndexOf("{");
            String placeholder = filePlaceholder.Substring(indexOf, ((filePlaceholder.IndexOf("}") + 1) - indexOf));
            String fileDate = TimeFunction.UTCtoCEST(DateTime.UtcNow).ToString(placeholder.Replace("{", "").Replace("}", ""));

            return filePlaceholder.Replace(placeholder, fileDate);
        }

        public static String DynamicToString(dynamic data)
        {
            if (data != null)
                return data.ToString();
            else
                return "";
        }

        public static Int64 DynamicToInt(dynamic data)
        {
            if (data != null && !String.IsNullOrEmpty(data.ToString().Trim()))
                return Convert.ToInt64(data.ToString());
            else
                return 0;
        }

        public static DateTime ToUSCulture(String dateTime)
        {
            return Convert.ToDateTime(dateTime, new System.Globalization.CultureInfo("en-US"));
        }

        public static async Task<String> ReadExcel(Stream stream, string password)
        {
            DataTable dataTable = new DataTable();
            //Create an instance of ExcelEngine
            using (Syncfusion.XlsIO.ExcelEngine excelEngine = new Syncfusion.XlsIO.ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = null;
                if (String.IsNullOrEmpty(password))
                {
                    workbook = application.Workbooks.Open(stream);
                }
                else
                {
                    workbook = application.Workbooks.Open(stream, ExcelParseOptions.Default, false, password, ExcelOpenType.Automatic);
                }
                IWorksheet sheet = workbook.Worksheets[0];
                dataTable = sheet.ExportDataTable(sheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
                stream.Close();
                DataRow[] filteredRows = dataTable.Rows.Cast<DataRow>()
                    .Where(row => row.ItemArray.All(field => field is DBNull)).ToArray();
                foreach (DataRow row in filteredRows)
                    dataTable.Rows.Remove(row);
                dataTable.AcceptChanges();
            }
            return JsonConvert.SerializeObject(dataTable);
        }


    }
}
