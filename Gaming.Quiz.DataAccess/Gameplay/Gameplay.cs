using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.DataInitializer.Common;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Library.Utility;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;

namespace Gaming.Quiz.DataAccess.Gameplay
{
    public class Gameplay : Common.BaseDataAccess
    {
        IPostgre _Postgre;
        public Gameplay(IOptions<Application> appSettings, IPostgre postgre, ICookies cookies) : base(appSettings, postgre, cookies)
        {
            _Postgre = postgre;
        }


        public ResponseObject PlayAttempt(Int64 UserId, Int64 GamedayId, Int64 SportId, Int64 QzCtgId, Int64 QzMId, Int64 AttemptNo, String Lang, String UserIp, Int64 PlatformId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;

            spName = "qz_user_start_play_attempt";

            object mAttemptNo = new object();

            if (AttemptNo == 0)
                mAttemptNo = DBNull.Value;
            else
                mAttemptNo = AttemptNo;

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzCtgId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_language_code", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = Lang;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_user_ip", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = UserIp;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_platformid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = PlatformId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_attempt_no", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = mAttemptNo;

                        NpgsqlParameter retJson = new NpgsqlParameter("p_ret_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retJson);

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);

                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int64.Parse(retvalue.Value.ToString()) : retVal;


                        if (retJson != null && retVal == 1)
                        {
                            res.Value = GenericFunctions.Deserialize<dynamic>(retJson.Value.ToString());
                            res.FeedTime = GenericFunctions.GetFeedTime();
                        }

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject LLFiftyFifty(Int64 OptType, Int64 UserId, Int64 QzMId, Int64 SportId, Int64 QzCtgId, Int64 UsrAttemptId, Int64 QzQstMid, Int64 GamedayId, string Lang, Int64 quizId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;

            spName = "qz_user_reg_ll_ffifty";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzCtgId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_user_attemptid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UsrAttemptId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_question_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzQstMid;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_language_code", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = Lang;

                        NpgsqlParameter retJson = new NpgsqlParameter("p_ret_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retJson);

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);

                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int64.Parse(retvalue.Value.ToString()) : retVal;


                        if (retJson != null && retVal == 1)
                        {
                            res.Value = GenericFunctions.Deserialize<dynamic>(retJson.Value.ToString());
                            res.FeedTime = GenericFunctions.GetFeedTime();
                        }

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject LLSneakPeak(Int64 OptType, Int64 UserId, Int64 QzMId, Int64 SportId, Int64 QzCtgId, Int64 UsrAttemptId, Int64 QzQstMid, string SelectedAns, Int64 GamedayId, string lang, Int64 quizId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;


            object mSltdOptn = new object();
            object mQzQstMId = new object();

            if (string.IsNullOrEmpty(SelectedAns))
                mSltdOptn = DBNull.Value;
            else
                mSltdOptn = SelectedAns;

            spName = "qz_user_reg_ll_sneakpeak";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzCtgId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_user_attemptid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UsrAttemptId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_question_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzQstMid;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_selected_answer_option", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = mSltdOptn;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_language_code", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = lang;

                        NpgsqlParameter retJson = new NpgsqlParameter("p_ret_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retJson);

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);



                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int64.Parse(retvalue.Value.ToString()) : retVal;


                        if (retJson != null && retVal == 1)
                        {
                            res.Value = GenericFunctions.Deserialize<dynamic>(retJson.Value.ToString());
                            res.FeedTime = GenericFunctions.GetFeedTime();
                        }

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject LLSwitch(Int64 OptType, Int64 UserId, Int64 QzMId, Int64 SportId, Int64 QzCtgId, Int64 UsrAttemptId, Int64 QzQstMid, Int64 GamedayId, string Lang, Int64 quizId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;

            spName = "qz_user_reg_ll_switch";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzCtgId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_user_attemptid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UsrAttemptId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_question_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzQstMid;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_language_code", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = Lang;


                        NpgsqlParameter retJson = new NpgsqlParameter("p_ret_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retJson);

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);

                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int64.Parse(retvalue.Value.ToString()) : retVal;


                        if (retJson != null && retVal == 1)
                        {
                            res.Value = GenericFunctions.Deserialize<dynamic>(retJson.Value.ToString());
                            res.FeedTime = GenericFunctions.GetFeedTime();
                        }

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject GetHint(Int64 OptType, Int64 UserId, Int64 QzMId, Int64 SportId, Int64 QzCtgId, Int64 QzAttemptId, Int64 QzQstMid, Int64 GamedayId, Int64 quizId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;

            spName = "qz_user_hints_ins_upd";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzCtgId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_attemptid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzAttemptId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_question_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzQstMid;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;


                        NpgsqlParameter retJson = new NpgsqlParameter("p_ret_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retJson);

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);

                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int64.Parse(retvalue.Value.ToString()) : retVal;


                        if (retJson != null && retVal == 1)
                        {
                            res.Value = GenericFunctions.Deserialize<dynamic>(retJson.Value.ToString());
                            res.FeedTime = GenericFunctions.GetFeedTime();
                        }

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject RegisterQuestion(Int64 UserId, Int64 GamedayId, Int64 SportId, Int64 QzCtgId, Int64 QzMId, Int64 QzAtmptId, string Lang, Int64 QzQstMid, string UsrIp, string SltdAnsOptn, Int64 PlatformId, Int64 TimeSpent, Int64 AtmptStatus, Int64 ResumeAtmpt, Int64 HintCnt, Int64 quizId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;

            spName = "qz_user_register_question";

            object mAnsOption = new object();
            object mQzQstMId = new object();

            if (SltdAnsOptn.ToLower() == "resume")
            {
                mQzQstMId = DBNull.Value;
                mAnsOption = DBNull.Value;
            }
            else
            {
                mQzQstMId = QzQstMid;
                mAnsOption = SltdAnsOptn;
            }
            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzCtgId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_attemptid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzAtmptId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_language_code", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = Lang;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_question_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = mQzQstMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_user_ip", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = UsrIp;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_selected_answer_option", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = mAnsOption;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_platformid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = PlatformId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_time_spent", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = TimeSpent;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_attempt_status", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = AtmptStatus;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_resume_attempt", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = ResumeAtmpt;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_hint_count", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = HintCnt;

                        NpgsqlParameter retJson = new NpgsqlParameter("p_ret_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retJson);

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);


                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int64.Parse(retvalue.Value.ToString()) : retVal;


                        if (retJson != null && retVal == 1)
                        {
                            res.Value = GenericFunctions.Deserialize<dynamic>(retJson.Value.ToString());
                            res.FeedTime = GenericFunctions.GetFeedTime();
                        }
                        else if (retJson != null && retVal == 2)
                        {
                            res.Value = GenericFunctions.Deserialize<dynamic>(retJson.Value.ToString());
                            res.FeedTime = GenericFunctions.GetFeedTime();
                        }

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject GetUserDetails(Int64 OptType, Int64 UserId, Int64 QzMId, Int64 SportId, Int64 QzCtgId, ref String favPayer, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;

            spName = "qz_user_details_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzCtgId;

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);

                        NpgsqlParameter retJson = new NpgsqlParameter("p_ret_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retJson);



                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int64.Parse(retvalue.Value.ToString()) : retVal;


                        if (retJson != null && retVal == 1)
                        {
                            res.Value = GenericFunctions.Deserialize<dynamic>(retJson.Value.ToString());
                            res.FeedTime = GenericFunctions.GetFeedTime();
                        }
                        try
                        {
                            String favPlayerId = "0";
                            if (retJson != null && retVal == 1)
                            {
                                dynamic mplayer = GenericFunctions.Deserialize<dynamic>(retJson.Value.ToString());
                                favPlayerId = mplayer.fvtPlayer;
                            }
                            if (!String.IsNullOrEmpty(favPlayerId))
                            {
                                favPayer = favPlayerId;
                            }
                        }
                        catch { }

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject GetScoreCard(Int64 OptType, Int64 UserId, Int64 QzMId, Int64 GamedayId, Int64 SportsId, Int64 CategoryId, Int64 AttemptId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;

            spName = "qz_user_attempt_scorecard";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportsId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = CategoryId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_attemptid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = AttemptId;

                        NpgsqlParameter retJson = new NpgsqlParameter("p_ret_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retJson);

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);

                        if (connection.State != ConnectionState.Open) connection.Open();
                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int64.Parse(retvalue.Value.ToString()) : retVal;


                        if (retJson != null && retVal == 1)
                        {
                            res.Value = GenericFunctions.Deserialize<dynamic>(retJson.Value.ToString());
                            res.FeedTime = GenericFunctions.GetFeedTime();
                        }

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public Int32 Settlement(Int64 UserId, Int64 QzMId, Int64 SportsId, Int64 CategoryId, Int64 GamedayId, Int64 AttemptId, Int64 PlatformID, string IpAddress, ref HTTPMeta httpMeta)
        {
            Int32 retVal = -50;
            String spName = String.Empty;

            spName = "qz_user_settlement_process";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportsId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = CategoryId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_attemptid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = AttemptId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_user_ip", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = IpAddress;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_platformid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = PlatformID;

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);

                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int32.Parse(retvalue.Value.ToString()) : retVal;

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return retVal;
            }
        }


        public ResponseObject GetUserStats(Int64 OptType, Int64 SportsId, Int64 CategoryId, Int64 QzMId, Int64 UserId, Int64 MonthId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;

            spName = "qz_user_stats_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportsId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = CategoryId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_monthid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MonthId;

                        NpgsqlParameter retJson = new NpgsqlParameter("p_ret_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retJson);

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);

                        if (connection.State != ConnectionState.Open) connection.Open();
                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int64.Parse(retvalue.Value.ToString()) : retVal;


                        if (retJson != null && retVal == 1)
                        {
                            res.Value = GenericFunctions.Deserialize<dynamic>(retJson.Value.ToString());
                            res.FeedTime = GenericFunctions.GetFeedTime();
                        }

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }


        public ResponseObject GetUserBadges(Int64 OptType, Int64 UserId, Int64 QzMId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;
            dynamic retJson = null;

            spName = "qz_user_badge_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = UserId;
                        //mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_monthid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MonthId;

                        //NpgsqlParameter retJson = new NpgsqlParameter("p_ret_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        //mNpgsqlCmd.Parameters.Add(retJson);

                        //NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        //mNpgsqlCmd.Parameters.Add(retvalue);

                        if (connection.State != ConnectionState.Open) connection.Open();
                        retJson = mNpgsqlCmd.ExecuteScalar();

                        //retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int64.Parse(retvalue.Value.ToString()) : retVal;


                        if (retJson != null)
                        {
                            
                            res.Value = GenericFunctions.Deserialize<dynamic>(retJson.ToString());
                            res.FeedTime = GenericFunctions.GetFeedTime();
                            retVal = 1;
                        }

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public Int32 UpdateBadgeNotify(Int64 OptType, Int64 UserId, Int64 MonthId, Int64 QzMId, ref HTTPMeta httpMeta)
        {
            Int32 retVal = -50;
            String spName = String.Empty;

            spName = "qz_user_badge_notify_upd";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;
                        
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_monthid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MonthId;

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);

                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int32.Parse(retvalue.Value.ToString()) : retVal;

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return retVal;
            }
        }



    }
}
