using System;
using System.Data;
using System.Linq;
using Gaming.Quiz.Contracts.Session;
using Npgsql;
using System.Collections.Generic;
using Gaming.Quiz.Library.Utility;

namespace  Gaming.Quiz.DataInitializer.Session
{
    public class Session
    {
        public static UserDetails InitializeLogin(dynamic loginres)
        {
            UserDetails details = new UserDetails();
            UserCookie uc = new UserCookie();
            GameCookie gc = new GameCookie();

            Login login = GenericFunctions.Deserialize<Login>(GenericFunctions.Serialize(loginres));
            try
            {

                uc.UserId = login.userId;
                uc.QzMasterID = login.QuizMatserid;

                gc.GUID = login.Guid;
                gc.SocialId = login.socialId;
                //gc.IsRegister = "1";

                details.Game = gc;
                details.User = uc;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return details;
        }

        public static UserDetails InitializeLogin(DataSet ds, ref Int64 vRet)
        {
            UserDetails details = new UserDetails();
            UserCookie uc = new UserCookie();
            GameCookie gc = new GameCookie();
            vRet = -70;

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            vRet = Convert.ToInt64(ds.Tables[0].Rows[0]["ret_type"].ToString());

                            if (vRet == 1)
                            {
                                details.User = (from a in ds.Tables[0].AsEnumerable()
                                                select new UserCookie
                                                {
                                                    UserId = Convert.IsDBNull(a["qz_userid"]) ? 0 : Convert.ToInt64(a["qz_userid"]),
                                                    QzMasterID = Convert.IsDBNull(a["qz_quiz_masterid"]) ? 0 : Convert.ToInt64(a["qz_quiz_masterid"]),
                                                    //Email = Convert.IsDBNull(a["email_id"]) ? "" : a["email_id"].ToString(),
                                                    //Phoneno = Convert.IsDBNull(a["phone_no"]) ? 0 : Convert.ToInt64(a["phone_no"])
                                                }).FirstOrDefault();

                                details.Game = (from a in ds.Tables[0].AsEnumerable()
                                                select new GameCookie
                                                {
                                                    //FullName = Convert.IsDBNull(a["full_name"]) ? "" : a["full_name"].ToString(),
                                                    GUID = Convert.IsDBNull(a["user_guid"]) ? "" : a["user_guid"].ToString(),
                                                    SocialId = Convert.IsDBNull(a["social_id"]) ? "" : a["social_id"].ToString(),
                                                    //IsActiveTour = Convert.IsDBNull(a["is_tour_active"]) ? "0" : a["is_tour_active"].ToString(),
                                                    //IsRegister = Convert.IsDBNull(a["is_registered"]) ? "" : a["is_registered"].ToString(),
                                                    //Country = Convert.IsDBNull(a["cf_countryid"]) ? "" : a["cf_countryid"].ToString(),
                                                    //IsAnonymous = Convert.IsDBNull(a["is_anonymous"]) ? 0 : Convert.ToInt64(a["is_anonymous"]),
                                                    //ClientId = Convert.IsDBNull(a["client_id"]) ? 0 : Convert.ToInt32(a["client_id"])
                                                }).FirstOrDefault();
                            }

                        }
                    }
                }

            }
            catch( Exception ex)
            {

            }

            return details;
        }


    }
}
