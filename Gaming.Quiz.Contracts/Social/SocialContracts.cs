using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Contracts.Social
{
    #region " facebook "

    public class SocialUser
    {
        public String id { get; set; }
        public String name { get; set; }
        public String email { get; set; }
        public Int32 phoneno { get; set; }
    }

    #endregion

    #region " Google "

    public class Email
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class Name
    {
        public string familyName { get; set; }
        public string givenName { get; set; }
    }

    public class Image
    {
        public string url { get; set; }
        public bool isDefault { get; set; }
    }

    public class GoogleUser
    {
        public string email { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string locale { get; set; }
        public Int32 phoneno { get; set; }
    }

    #endregion
}
