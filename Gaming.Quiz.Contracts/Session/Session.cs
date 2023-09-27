using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace  Gaming.Quiz.Contracts.Session
{
    
    public class Credentials
    {
        public Credentials()
        {
            this.FullName = ""; this.ClientId = 1; this.OptType = 1;
            this.PlatformId = 0; this.UserId = 0; this.SocialId = ""; this.ClientId = 1;
            this.EmailId = ""; this.PhoneNo = ""; this.CountryCode = ""; 
            
            this.DOB = "";
        }
        public Int32 OptType { get; set; }
        public Int32 PlatformId { get; set; }
        public Int32 UserId { get; set; }
        public String SocialId { get; set; }
        public Int32 ClientId { get; set; }
        public String FullName { get; set; }
        public String EmailId { get; set; }
        public String CountryCode { get; set; }
        public String PhoneNo { get; set; }
        public String ProfilePicture { get; set; }
        public String DOB { get; set; }
        public DateTime userCreatedDate { get; set; }
        //public String CountryOfResidence { get; set; }
    }

    public class Login
    {
        public Int64 userId { get; set; }
        public string Guid { get; set; }
        public string socialId { get; set; }
        public Int64 QuizMatserid { get; set; }
    }
}
