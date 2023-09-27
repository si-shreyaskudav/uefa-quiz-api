using System;
using System.Data;
using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.DataInitializer.Common;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Library.Utility;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using Gaming.Quiz.Contracts.Session;
using Microsoft.Extensions.Options;
using Gaming.Quiz.Contracts.Configuration;

namespace Gaming.Quiz.DataAccess.Session
{
    public class Session : Common.BaseDataAccess
    {
        IPostgre _Postgre;

        public Session(IOptions<Application> appSettings, IPostgre postgre, ICookies cookies) : base(appSettings, postgre, cookies)
        {
            _Postgre = postgre;
        }

        public UserDetails Login(Int32 optType, Int32 platformId, Int32 tourId, String TnCVersion, Int64 userId, String socialId, Int32 clientId, String fullName,
         String emailId, Int64? phoneno, String countryCode, ref HTTPMeta httpMeta)
        {
            UserDetails details = new UserDetails();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            spName = "qz_quiz_user_get";

            Object mPhoneNo = DBNull.Value;
            if (phoneno != null && phoneno != 0)
                mPhoneNo = phoneno;

            Object mEmail = DBNull.Value;
            if (emailId != null && emailId != "string")
                mEmail = emailId;

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<String>() { "p_out_user_cur" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {
                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = optType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_platformid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = platformId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = tourId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = userId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_social_id", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = socialId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_clientid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = clientId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_full_name", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = fullName;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_email_id", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = mEmail;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cf_phone", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = mPhoneNo;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_terms", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = TnCVersion;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_country_of_residence", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = (String.IsNullOrEmpty(countryCode) ? "IN" : countryCode);
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_out_user_cur", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);
                        details = DataInitializer.Session.Session.InitializeLogin(ds, ref retVal);

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                        transaction.Rollback();

                    throw ex;
                }
                finally
                {
                    if (transaction != null && transaction.IsCompleted == false)
                        transaction.Commit();

                    connection.Close();
                    connection.Dispose();
                }
            }

            return details;
        }

        public UserDetails Login(string SocialId, ref HTTPMeta httpMeta)
        {
            UserDetails login = new UserDetails();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            spName = "qz_admin_user_ins";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {
                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_social_id", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = SocialId.ToString();

                        NpgsqlParameter retJson = new NpgsqlParameter("p_ret_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retJson);

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);

                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int64.Parse(retvalue.Value.ToString()) : retVal;

                        if (retJson != null && retVal == 1)
                        {
                           var logindata  = GenericFunctions.Deserialize<dynamic>(retJson.Value.ToString());
                            login = DataInitializer.Session.Session.InitializeLogin(logindata);

                        }

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                        transaction.Rollback();

                    throw ex;
                }
                finally
                {
                    if (transaction != null && transaction.IsCompleted == false)
                        transaction.Commit();

                    connection.Close();
                    connection.Dispose();
                }
            }

            return login;
        }


        public UserDetails AnonymousLogin(Int32 OptType,Int64 MasterId, Int64 UserId, String SocialId,String FullName,String EmailId, Int64 PhoneNo, String CountryCode, Int32 ClientId, Int32 PlatformId, String TnC, Int32 IsAnonymous, ref HTTPMeta httpMeta)
        {
            UserDetails details = new UserDetails();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            object mFullName = new object();

            if (!string.IsNullOrEmpty(FullName))
                mFullName = FullName;
            else
                mFullName = DBNull.Value;

            spName = "qz_quiz_user_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<String>() { "p_out_user_cur" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {
                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MasterId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_social_id", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = SocialId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_full_name", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = mFullName;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_email_id", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = EmailId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cf_phone", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = PhoneNo;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_country_of_residence", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = CountryCode;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_clientid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = ClientId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_platformid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = PlatformId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_terms", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = TnC;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_is_anonymous", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value =IsAnonymous;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_out_user_cur", NpgsqlDbType.Refcursor)).Value = cursors[0];


                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);
                        details = DataInitializer.Session.Session.InitializeLogin(ds, ref retVal);

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                        transaction.Rollback();

                    throw ex;
                }
                finally
                {
                    if (transaction != null && transaction.IsCompleted == false)
                        transaction.Commit();

                    connection.Close();
                    connection.Dispose();
                }
            }

            return details;
        }

        public UserLoginDBResp WafLogin(Int32 optType, Int32 platformId, Int32 tourId, Int64 userId, String socialId, Int32 clientId, String fullName,
         String emailId, String PhoneNo, String countryCode, String ProfilePicture, String DOB, DateTime userCreatedDateTime,
         Int32 tandcVersion, Int32 privacyPolicyVersion, ref HTTPMeta httpMeta)
        {
            UserLoginDBResp detail = new UserLoginDBResp();

            Int32 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            object _countryCode = DBNull.Value;
            if (!String.IsNullOrEmpty(countryCode))
                _countryCode = countryCode;

            object _ProfilePicture = DBNull.Value;
            if (!String.IsNullOrEmpty(ProfilePicture))
                _ProfilePicture = ProfilePicture;

            spName = "cf_user_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {
                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = optType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cf_platformid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = platformId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cf_tourid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = tourId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cf_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = userId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_social_id", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = socialId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cf_clientid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = clientId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_full_name", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = fullName;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_email_id", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = emailId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cf_phone", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = String.IsNullOrEmpty(PhoneNo)
                                                                                                                                                            ? "" : PhoneNo.ToString();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_country_of_residence", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = _countryCode;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_user_profile_pic", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = _ProfilePicture;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_dob", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = DOB;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_registered_dt", NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input }).Value = userCreatedDateTime;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_tnc_version", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = tandcVersion;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_privacy_policy", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = privacyPolicyVersion;

                        if (connection.State != ConnectionState.Open) connection.Open();
                        transaction = connection.BeginTransaction();

                        var data = mNpgsqlCmd.ExecuteScalar();

                        detail = GenericFunctions.Deserialize<UserLoginDBResp>(data.ToString());

                        retVal = detail.Retval;

                        transaction.Commit();

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                        transaction.Rollback();

                    throw ex;
                }
                finally
                {
                    if (transaction != null && transaction.IsCompleted == false)
                        transaction.Commit();

                    connection.Close();
                    connection.Dispose();
                }
            }

            return detail;
        }

    }
}
