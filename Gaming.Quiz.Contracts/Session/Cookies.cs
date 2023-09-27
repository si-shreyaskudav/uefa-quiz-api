using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace  Gaming.Quiz.Contracts.Session
{
    public class UserCookie
    {
        public UserCookie()
        {
            this.UserId = 0;
        }

        public Int64 UserId { get; set; }
        public Int64 TeamId { get; set; }
        public string FullName { get; set; }
        public string SocialId { get; set; }
        public Int64 QzMasterID { get; set; }
    }

    public class GameCookie
    {
        public String GUID { get; set; }
        public String WAF_GUID { get; set; }
        public String SocialId { get; set; }
        public String ClientId { get; set; }
        public Int32 CoinTotal { get; set; }
        public String FavPlayer { get; set; }
    }

    public class UserDetails
    {
        public UserCookie User { get; set; }
        public GameCookie Game { get; set; }
    }

    public class WAFCookie
    {

        public string name { get; set; }
        public string email_id { get; set; }
        public string is_first_login { get; set; }
        public string favourite_club { get; set; }
        public string edition { get; set; }
        public string status { get; set; }
        public string is_app { get; set; }
        public string is_custom_image { get; set; }
        public string social_user_image { get; set; }
        public string user_guid { get; set; }
    }

    public class WAFUSCCookie
    {
        public String user_guid { get; set; }
    }

    public class UserLoginDBResp
    {
        [JsonProperty("retval")]
        public int Retval { get; set; }
        [JsonProperty("usrid")]
        public int? Usrid { get; set; }
        [JsonProperty("teamid")]
        public int? Teamid { get; set; }
        [JsonProperty("usrguid")]
        public string Usrguid { get; set; }
        [JsonProperty("usrsocid")]
        public string Usrsocid { get; set; }
        [JsonProperty("usrclnid")]
        public int Usrclnid { get; set; }
        [JsonProperty("temname")]
        public string Temname { get; set; }
        [JsonProperty("usrname")]
        public string Usrname { get; set; }
        [JsonProperty("favteam")]
        public int? Favteam { get; set; }
        [JsonProperty("profpic")]
        public string Profpic { get; set; }
        [JsonProperty("cointotal")]
        public Int32? coinTotal { get; set; }
    }

}
