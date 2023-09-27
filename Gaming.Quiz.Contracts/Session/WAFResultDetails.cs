using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Contracts.Session
{
    public class WAFResultDetails
    {
        public ResultData data { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class User
    {
        public string name { get; set; }
        public string edition { get; set; }
        public string favourite_club { get; set; }
        public string favourite_player_id { get; set; }
        //public string favourite_player_name { get; set; }
        public string social_user_image { get; set; }
        public string mobile_no { get; set; }
        public object profile_completion_percentage { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public object profile_page_enabled { get; set; }
        public object campaign_id { get; set; }
        public CampaignJson campaign_json { get; set; }
        public string membership_code { get; set; }
        public string social_user_name { get; set; }
        public bool subscribe_for_email { get; set; }
        public string dob { get; set; }
        public object sports_vertical { get; set; }
        public object sporting_event_inspired_you { get; set; }
        public object mail { get; set; }
        public string referral_code { get; set; }
        public string referral_code_change { get; set; }
    }

    public class ResultData
    {
        public string user_guid { get; set; }
        public string user_id { get; set; }
        public string status { get; set; }
        public string email_id { get; set; }
        public object failed_attempts { get; set; }
        public object unlock_date { get; set; }
        public string message { get; set; }
        public Error error { get; set; }
        public User user { get; set; }
        public string is_first_login { get; set; }
        public string is_custom_image { get; set; }
        public string old_profile_img_url { get; set; }
        public object gift_id { get; set; }
        public object gift_name { get; set; }
        public string client_id { get; set; }
        public object campaign_id { get; set; }
        public object id { get; set; }
        public string waf_user_guid { get; set; }
        public string created_date { get; set; }
    }

    public class CampaignJson
    {
    }

}
