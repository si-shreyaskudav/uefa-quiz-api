using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.Enums;
using Gaming.Quiz.Contracts.Session;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Library.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Library.Session
{
    public class Session
    {
       
        
        public static HTTPResponse ValidateUser(String ProfileUrl, Dictionary<String, String> vAuthHeaders, String vWafGUID)
        {

            HTTPResponse mHTTPResponse = new HTTPResponse();
            HTTPMeta mHTTPMeta = new HTTPMeta();
            String mMessage = String.Empty;
            try
            {

                string result = GenericFunctions.GetWAFWebData(ProfileUrl, vAuthHeaders);

                mMessage += " result:" + result;
                WAFResultDetails userData = GenericFunctions.Deserialize<WAFResultDetails>(result);

                if (!String.IsNullOrEmpty(userData.data.status) &&
                        userData.data.status == "1")
                // || userData.data.status == "2")
                {
                    Credentials credentials = new Credentials();

                    credentials.SocialId = userData.data.user_id; //AesCryptography.AesDecrypt(vWafGUID).Split('|')[0];
                                                                  //credentials.FullName = BareEncryption.BaseEncrypt(userData.data.user.name.ToString());
                    if (!String.IsNullOrEmpty(userData.data.user.last_name))
                        credentials.FullName = userData.data.user.first_name.Trim() + " " + userData.data.user.last_name.Trim();
                    else
                        credentials.FullName = userData.data.user.first_name.Trim();

                    credentials.EmailId = String.IsNullOrEmpty(userData.data.email_id) == false
                      ? BareEncryption.BaseEncrypt(userData.data.email_id.Trim().ToLower())
                      : "";
                    credentials.DOB = BareEncryption.BaseEncrypt(userData.data.user.dob);

                    //if (!String.IsNullOrEmpty(userData.data.user.mobile_no))
                    //    credentials.PhoneNo = userData.data.user.mobile_no;
                    //else
                    //    credentials.PhoneNo = "";

                    credentials.PhoneNo = String.IsNullOrEmpty(userData.data.user.mobile_no) == false
                      ? BareEncryption.BaseEncrypt(userData.data.user.mobile_no.Trim())
                      : "";

                    try
                    {
                        if (String.IsNullOrEmpty(userData.data.created_date) == false)
                        {
                            DateTime userCreateDateTime = new DateTime();
                            userCreateDateTime = DateTime.Parse(userData.data.created_date);

                            userCreateDateTime = DateTime.SpecifyKind(userCreateDateTime, DateTimeKind.Unspecified);

                            credentials.userCreatedDate = userCreateDateTime;
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    mHTTPResponse.Data = credentials;
                    GenericFunctions.AssetMeta(1, ref mHTTPMeta, userData.data.user_guid + mMessage);
                }
                else
                {
                    if (!String.IsNullOrEmpty(userData.data.status) &&
                        userData.data.status == "2")
                    {
                        GenericFunctions.AssetMeta(2, ref mHTTPMeta, mMessage);
                    }
                    else
                    {
                        GenericFunctions.AssetMeta(-999, ref mHTTPMeta, mMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                GenericFunctions.AssetMeta(-40, ref mHTTPMeta, "Problem in Stumpped API." + ex.Message+ mMessage);
            }

            mHTTPResponse.Meta = mHTTPMeta;

            return mHTTPResponse;
        }
    }
}
    

