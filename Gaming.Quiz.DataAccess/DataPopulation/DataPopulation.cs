using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.DataInitializer.Common;
using Gaming.Quiz.DataInitializer.Datapopulation;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;


namespace Gaming.Quiz.DataAccess.DataPopulation
{
    public class DataPopulation : Common.BaseDataAccess
    {
        IPostgre _Postgre;
        public DataPopulation(IOptions<Application> appSettings, IPostgre postgre, ICookies cookies) : base(appSettings, postgre, cookies)
        {
            _Postgre = postgre;
        }


        public Int32 IngestQuestion(Int64 OptType, Int64 QMId, Int64[] QCtgId, Int64[] SprtId, string[] QDesc, string[] QOptA, string[] QOptB, string[] QOptC, string[] QOptD, string[] QOptE, string[] QOptF, string[] QCrrtAns, string[] QCrtAnsOpt,
            string[] QHint1, string[] QHint2, string[] QHint3, string[] QHint4, string[] QHint5, string[] QHint6, Int32[] Cmplx, Int32[] SubCmplx, Int64[] QzQstTypId, string[] URL, string[] AstObjId, Int64[] MaxTime, Int64[] PosPoint,
            Int64[] NegPoint, Int64[] OptCnt, Int64[] IsActive, DateTime[] GameDate, Int64[] QzQusNo, string[] QLngCode, Int64[] HintCnt, int[] isIplQuestions)
        {
            Int32 retVal = -50;
            String spName = String.Empty;

            spName = "qz_admin_ins_question";

      

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_question_no", NpgsqlDbType.Array | NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzQusNo;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Array | NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QCtgId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Array | NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SprtId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_question_description", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QDesc;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_option_a", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QOptA;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_option_b", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QOptB;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_option_c", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QOptC;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_option_d", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QOptD;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_option_e", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QOptE.StringNullCheck();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_option_f", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QOptF.StringNullCheck();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_correct_answer", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QCrrtAns;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_correct_answer_option", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QCrtAnsOpt;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_hint_1", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QHint1.StringNullCheck();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_hint_2", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QHint2.StringNullCheck();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_hint_3", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QHint3.StringNullCheck();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_hint_4", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QHint4.StringNullCheck();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_hint_5", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QHint5.StringNullCheck();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_hint_6", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QHint6.StringNullCheck();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_complex", NpgsqlDbType.Array | NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = Cmplx;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_sub_complex", NpgsqlDbType.Array | NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = SubCmplx;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_question_typeid", NpgsqlDbType.Array | NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzQstTypId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_url", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = URL.StringNullCheck();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_asset_object_id", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = AstObjId.StringNullCheck();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_max_time", NpgsqlDbType.Array | NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MaxTime;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_positive_pt", NpgsqlDbType.Array | NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = PosPoint;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_negative_pt", NpgsqlDbType.Array | NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = NegPoint;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_option_count", NpgsqlDbType.Array | NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptCnt;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_is_active", NpgsqlDbType.Array | NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = IsActive;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_game_date", NpgsqlDbType.Array | NpgsqlDbType.Date) { Direction = ParameterDirection.Input }).Value = GameDate;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_language_code", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = QLngCode.StringNullCheck();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_hint_count", NpgsqlDbType.Array | NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = HintCnt;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_is_ipl_questions", NpgsqlDbType.Array | NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = isIplQuestions;


                        NpgsqlParameter value = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(value);

                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = value.Value != null && value.Value.ToString().Trim() != "" ? Int32.Parse(value.Value.ToString()) : retVal;
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

        public Int32 VerifyQuestion(Int64 OptType, Int64 QMId, Int64 QCtgId, Int64 SprtId, DateTime date, out DataSet ds)
        {
            Int32 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;
            ResponseObject QuestionSet = new ResponseObject();
           
            spName = "qz_admin_quiz_ques_day_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<String>() { "p_curr_question" };
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_date", NpgsqlDbType.Date) { Direction = ParameterDirection.Input }).Value = date;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid",  NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id",  NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SprtId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id",  NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QCtgId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_curr_question", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);
                        ds = Datapopulation.InitializeQuestionSet(ds, out retVal);
                        if (ds.Tables.Count > 0)
                        {
                            retVal = 1;
                        }
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
