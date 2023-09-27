using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using  Gaming.Quiz.Library.Utility;
using System.Threading.Tasks;
using System.Collections.Generic;
using  Gaming.Quiz.Contracts.Common;
using System.Text;
using Gaming.Quiz.Contracts.Enums;

namespace  Gaming.Quiz.Library.AWS.S3
{
    public partial class Broker
    {
        private String LogKey
        {
            get
            {
                String key = String.Empty;

                DateTime istDateTime = TimeFunction.UTCtoIST(DateTime.UtcNow);
                String date = istDateTime.Date.ToString("MM-dd-yyyy");
                key = "logs/" + date + "/" + "log-" + istDateTime.Hour + ".json";

                return key;
            }
        }

        private String DebugKey
        {
            get
            {
                String key = String.Empty;

                DateTime istDateTime = TimeFunction.UTCtoIST(DateTime.UtcNow);
                String date = istDateTime.Date.ToString("MM-dd-yyyy");
                key = "debug/" + date + "/" + "trace.txt";

                return key;
            }
        }

        #region " Asset "

        public async Task<bool> Set(String fileName, Object content, bool serialize)
        {
            bool success = false;
            String key = _AWSS3FolderPath + fileName;
            String extension = Path.GetExtension(fileName).Replace(".", "").ToLower().Trim();

            try
            {
                String contentType = "application/" + extension;

                if (extension == "txt")
                    contentType = "text/plain";
                if (extension == "html")
                    contentType = "text/html";
                if (extension == "css")
                    contentType = "text/css";
                if (extension == "js")
                    contentType = "application/javascript";

                //Adding charset for Russian characters
                contentType += "; charset=utf-8";

                using (_Client = S3Client())
                {
                    PutObjectRequest request = new PutObjectRequest();
                    request.BucketName = _AWSS3Bucket;
                    request.Key = key;
                    //request.CannedACL = S3CannedACL.PublicRead;
                    request.ContentType = contentType;
                    request.Headers.CacheControl = "private";
                    request.ContentBody = serialize ? GenericFunctions.Serialize(content) : content.ToString();

                    var response = await _Client.PutObjectAsync(request);

                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        success = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return success;
        }

        public async Task<bool> Set(String bucket, String fileName, Object content, bool serialize)
        {
            bool success = false;
            String key = fileName;
            String extension = Path.GetExtension(fileName).Replace(".", "").ToLower().Trim();

            try
            {
                String contentType = "application/" + extension;

                if (extension == "txt")
                    contentType = "text/plain";
                if (extension == "html")
                    contentType = "text/html";
                if (extension == "css")
                    contentType = "text/css";
                if (extension == "js")
                    contentType = "application/javascript";

                //Adding charset for Russian characters
                contentType += "; charset=utf-8";

                using (_Client = S3Client())
                {
                    PutObjectRequest request = new PutObjectRequest();
                    request.BucketName = bucket;
                    request.Key = key;
                    request.CannedACL = S3CannedACL.PublicRead;
                    request.ContentType = contentType;
                    request.Headers.CacheControl = "private";
                    request.ContentBody = serialize ? GenericFunctions.Serialize(content) : content.ToString();

                    //Making the write async by not awaiting it.
                    _Client.PutObjectAsync(request);

                    //var response = await _Client.PutObjectAsync(request);

                    //if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    //    success = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return success;
        }

        public async Task<bool> SetImage(String fileName, Stream image, bool downloadable)
        {
            bool success = false;
            String key = _AWSS3FolderPath + fileName;
            String extension = Path.GetExtension(fileName).Replace(".", "").ToLower().Trim();

            try
            {
                using (_Client = S3Client())
                {
                    PutObjectRequest request = new PutObjectRequest();
                    request.BucketName = _AWSS3Bucket;
                    request.Key = key;
                    request.CannedACL = S3CannedACL.PublicRead;
                    request.ContentType = "image/" + extension;
                    request.Headers.CacheControl = "private";
                    request.InputStream = image;

                    if (downloadable)
                        //to make the image auto-download on open
                        request.ContentType = "application/octet-stream";

                    var response = await _Client.PutObjectAsync(request);

                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        success = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return success;
        }

        #endregion

        #region " Log "

        public async Task Log(HTTPLog logMessage)
        {
            String key = _AWSS3FolderPath + LogKey;

            try
            {
                using (_Client = S3Client())
                {
                    GetObjectRequest request = new GetObjectRequest()
                    {
                        BucketName = _AWSS3Bucket,
                        Key = key
                    };

                    var response = await _Client.GetObjectAsync(request);

                    using (Stream amazonStream = response.ResponseStream)
                    {
                        StreamReader amazonStreamReader = new StreamReader(amazonStream);
                        string logs = amazonStreamReader.ReadToEnd();
                        List<HTTPLog> existing = GenericFunctions.Deserialize<List<HTTPLog>>(logs);

                        await Set(LogKey, ParseLogs(logMessage, existing), true);
                    }
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode.Equals("NoSuchKey"))
                    await Set(LogKey, ParseLogs(logMessage, null), true);
            }
        }

        private List<HTTPLog> ParseLogs(HTTPLog newLog, List<HTTPLog> existingLog)
        {
            List<HTTPLog> newRange = new List<HTTPLog>();

            if (existingLog != null)
                newRange.AddRange(existingLog);

            newRange.Add(newLog);

            return newRange;
        }

        #endregion

        #region " Debug "

        public async Task Debug(String message)
        {
            String key = _AWSS3FolderPath + DebugKey;

            try
            {
                using (_Client = S3Client())
                {
                    GetObjectRequest request = new GetObjectRequest()
                    {
                        BucketName = _AWSS3Bucket,
                        Key = key
                    };

                    var response = await _Client.GetObjectAsync(request);

                    using (Stream amazonStream = response.ResponseStream)
                    {
                        StreamReader amazonStreamReader = new StreamReader(amazonStream);
                        string logs = amazonStreamReader.ReadToEnd();
                        await Set(DebugKey, DumpLogs(message, logs), false);
                    }
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode.Equals("NoSuchKey"))
                    await Set(DebugKey, DumpLogs(message, ""), false);
            }
        }

        private String DumpLogs(string logMessage, string oldMessage)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(oldMessage);
            sb.Append(String.Format("Log Entry at : {0} - {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString()) + "\n");
            sb.Append(String.Format("{0}", logMessage) + "\n");
            sb.Append("-------------------------------" + "\n");

            return sb.ToString();
        }

        #endregion

        #region "Image Share"

        public async Task<bool> WriteImageOnS3(Stream imageStream, String bucketKey, String extension)
        {
            bool success = false;
            String key = bucketKey;
            try
            {
                using (_Client = S3Client())
                {
                    var request = new PutObjectRequest()
                    {
                        BucketName = _AWSS3Bucket,
                        Key = _AWSS3FolderPath + key,
                        InputStream = imageStream,
                        ContentType = "image/" + extension.ToString(),
                        //CannedACL = S3CannedACL.PublicRead// need to comment this line for prod push
                    };

                    PutObjectResponse response = await _Client.PutObjectAsync(request);
                    success = response != null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return success;
        }

        public bool WriteS3Asset(String fileName, MimeType type, Object content, byte[] imageBytes = null)
        {
            bool success = false;
            String key = _AWSS3FolderPath + fileName;
            String extension = Path.GetExtension(fileName).Replace(".", "").ToLower().Trim();

            try
            {
                using (_Client = S3Client())
                {
                    PutObjectRequest request = new PutObjectRequest();
                    request.BucketName = _AWSS3Bucket;
                    request.Key = key;
                    request.ContentType = String.Concat(type.ToString().ToLower(), "/", extension);

                    if (type == MimeType.Application)
                    {
                        request.ContentBody = Newtonsoft.Json.JsonConvert.SerializeObject(content);
                    }
                    else if (type == MimeType.Text)
                    {
                        if (extension == "txt")
                        {
                            request.ContentType = "text/plain";
                            //request.CannedACL = S3CannedACL.NoACL;
                        }
                        //else
                        //    request.CannedACL = S3CannedACL.PublicRead;

                        request.ContentBody = content.ToString();
                    }
                    else if (type == MimeType.Image)
                    {
                        //to make the image auto-download on open
                        request.ContentType = "application/octet-stream";
                        request.InputStream = new MemoryStream(imageBytes);
                        //request.CannedACL = S3CannedACL.PublicRead;
                    }

                    var response = _Client.PutObjectAsync(request);

                    if (response.Result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        success = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return success;
        }

        #endregion
    }
}
