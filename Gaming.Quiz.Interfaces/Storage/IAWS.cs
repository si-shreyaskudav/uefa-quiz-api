using System;
using System.IO;
using System.Threading.Tasks;
using  Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Enums;

namespace  Gaming.Quiz.Interfaces.Storage
{
    public interface IAWS
    {
        Task<String> Get(String fileName);
        Task<byte[]> GetImage(String fileName);

        Task<bool> Set(String fileName, Object content, bool serialize);
        Task<bool> Set(String bucket, String fileName, Object content, bool serialize);
        Task<bool> SetImage(String fileName, Stream image, bool downloadable);
        Task Log(HTTPLog logMessage);
        Task Debug(String message);

        Task<bool> Has(String fileName);
        Task<bool> Remove(String fileName);

        Task<String> Environment();

        //Task<object> TextEmail(string from, string to, string cc, string bcc, string subject, string msg, bool isHtml);
        //Task<object> AttachmentEmail(string from, string to, string cc, string bcc, string subject, string msg, bool isHtml, byte[] attachment = null);
        Task<object> SMTPEmail(string from, string to, string cc, string bcc, string subject, string msg, bool isHtml);

        //Task<bool> SendSESMail(string from, string to, string cc, string bcc, string subject, string msg, bool isHtml, byte[] attachment = null);

        Task<bool> WriteImageOnS3(Stream imageStream, String extension, String keyValue);

        bool WriteS3Asset(String fileName, MimeType type, Object content, byte[] imageBytes = null);

        Task<bool> SendSNSAlert(String subject, String message);
    }
}
