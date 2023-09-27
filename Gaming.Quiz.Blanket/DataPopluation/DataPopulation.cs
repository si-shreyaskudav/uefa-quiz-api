using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Library.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Gaming.Quiz.DataInitializer.Common;
using System.Reflection;
using System.IO;
using Gaming.Quiz.Contracts.DataPopulation;
using Gaming.Quiz.Interfaces.DataPopulation;

namespace Gaming.Quiz.Blanket.DataPopluation
{
    public class DataPopulation : Common.BaseBlanket, IDataPopulationBlanket
    {
        private readonly DataAccess.DataPopulation.DataPopulation _DBContext;

        public DataPopulation(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
        : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _DBContext = new DataAccess.DataPopulation.DataPopulation(appSettings, postgre, cookies);
        }

        public Tuple<int, String> IngestQuestion(string path)
        {
            String error = string.Empty;
            Int32 retVal = -40;
            Int32 OptType = 1;
            try
            {
                DataTable dt = Excel.ExcelPackageToDataTable(path);

                //excelCheck.checkexcel = 

                ExcelCheckModel checkModel = GetQuizList(dt);

                List<Contracts.DataPopulation.DataPopulation> QuizList = checkModel.QuizList;

                var QuestionCountCheck = (from p in QuizList
                                          group p by p.GameDate into g
                                          where g.Count() > 1
                                          select new { g.Key, QCount = g.Count() }).ToList();

                List<string> colname = new List<string>();
                if (!IsValidationPassed(checkModel.checkexcel, out colname))
                {
                    retVal = -45;
                    error = "Validation failed due to Following Columns : " + string.Join(",", colname.ToArray());
                }
                else
                {

                    Int64 QMId = QuizList.Select(o => o.QMId).FirstOrDefault();
                    Int64[] QCtgId = QuizList.Select(o => o.QCtgId).ToArray();
                    Int64[] SprtId = QuizList.Select(o => o.SprtId).ToArray();
                    string[] QDesc = QuizList.Select(o => o.QDesc).ToArray();
                    string[] QOptA = QuizList.Select(o => o.QOptA).ToArray();
                    string[] QOptB = QuizList.Select(o => o.QOptB).ToArray();
                    string[] QOptC = QuizList.Select(o => o.QOptC).ToArray();
                    string[] QOptD = QuizList.Select(o => o.QOptD).ToArray();
                    string[] QOptE = QuizList.Select(o => o.QOptE).ToArray();
                    string[] QOptF = QuizList.Select(o => o.QOptF).ToArray();
                    string[] QCrrtAns = QuizList.Select(o => o.QCrrtAns).ToArray();
                    string[] QCrtAnsOpt = QuizList.Select(o => o.QCrtAnsOpt).ToArray();
                    string[] QHint1 = QuizList.Select(o => o.QHint1).ToArray();
                    string[] QHint2 = QuizList.Select(o => o.QHint2).ToArray();
                    string[] QHint3 = QuizList.Select(o => o.QHint3).ToArray();
                    string[] QHint4 = QuizList.Select(o => o.QHint4).ToArray();
                    string[] QHint5 = QuizList.Select(o => o.QHint5).ToArray();
                    string[] QHint6 = QuizList.Select(o => o.QHint6).ToArray();
                    Int32[] Cmplx = QuizList.Select(o => o.Cmplx).ToArray();
                    Int32[] SubCmplx = QuizList.Select(o => o.SubCmplx).ToArray();
                    Int64[] QzQstTypId = QuizList.Select(o => o.QzQstTypId).ToArray();
                    string[] AstObjId = QuizList.Select(o => o.AstObjId).ToArray();
                    Int64[] MaxTime = QuizList.Select(o => o.MaxTime).ToArray();
                    Int64[] PosPoint = QuizList.Select(o => o.PosPoint).ToArray();
                    Int64[] NegPoint = QuizList.Select(o => o.NegPoint).ToArray();
                    Int64[] OptCnt = QuizList.Select(o => o.OptCnt).ToArray();
                    Int64[] IsActive = QuizList.Select(o => o.IsActive).ToArray();
                    string[] URL = QuizList.Select(o => o.URL).ToArray();

                    Int64[] QzQstNo = QuizList.Select(o => o.QzQstNo).ToArray();
                    string[] QLangCd = QuizList.Select(o => o.QLangCd).ToArray();
                    Int64[] HintCnt = QuizList.Select(o => o.HintCnt).ToArray();
                    Int32[] IsIPLQuestions = Enumerable.Repeat(0,QuizList.Count).ToArray();

                    DateTime[] GameDate = QuizList.Select(o => o.GameDate).ToArray().DateTimeArray();


                    retVal = _DBContext.IngestQuestion(OptType, QMId, QCtgId, SprtId, QDesc, QOptA, QOptB, QOptC, QOptD, QOptE, QOptF, QCrrtAns, QCrtAnsOpt,
                               QHint1, QHint2, QHint3, QHint4, QHint5, QHint6, Cmplx, SubCmplx, QzQstTypId, URL, AstObjId, MaxTime, PosPoint,
                                NegPoint, OptCnt, IsActive, GameDate, QzQstNo, QLangCd, HintCnt, IsIPLQuestions);

                    if (retVal == 2)
                    {
                        error = "RetVal ==> " + retVal + " Validate the following column in excel : QUESTION_DESCRIPTION, CATEGORY_ID, SPORT_ID, " +
                            "CORRECT_ANSWER, CORRECT_ANSWER_OPTION, MAXTIME,POSITIVE_PT, DATE, OPTION_COUNT,QUESTION_OPTION_A, " +
                            "QUESTION_OPTION_B,QUESTION_OPTION_C,QUESTION_OPTION_D, QUESTION_TYPEID, COMPLEXITY, SUB_COMPLEXITY";
                    }
                }
            }
            catch (Exception ex)
            {
                LocalErrorLog.LogException(System.Reflection.MethodBase.GetCurrentMethod(), null, ex);

                error = ex.Message;
            }
            return new Tuple<int, String>(Convert.ToInt32(retVal), error);
        }

        public Tuple<int, String> InsertQuestion(IFormFile questions)
        {
            String error = string.Empty;
            Int32 retVal = -40;
            Int32 OptType = 1;
            try
            {
                string fileExt = Path.GetExtension(questions.FileName);
                if (fileExt.ToLower().Equals(".xls") || fileExt.ToLower().Equals(".xlsx") || fileExt.ToLower().Equals(".csv"))
                {
                    var stream = questions.OpenReadStream();
                    String list = GenericFunctions.ReadExcel(stream, String.Empty).Result;
                    stream.Close();
                    List<ExcelData> QuizList = GenericFunctions.Deserialize<List<ExcelData>>(list);

                    Int64 QMId = QuizList.Select(o => o.QUIZ_MASTERID).FirstOrDefault();
                    Int64[] QCtgId = QuizList.Select(o => o.CATEGORY_ID).ToArray();
                    Int64[] SprtId = QuizList.Select(o => o.SPORT_ID).ToArray();
                    string[] QDesc = QuizList.Select(o => o.QUESTION_DESCRIPTION).ToArray();
                    string[] QOptA = QuizList.Select(o => o.QUESTION_OPTION_A).ToArray();
                    string[] QOptB = QuizList.Select(o => o.QUESTION_OPTION_B).ToArray();
                    string[] QOptC = QuizList.Select(o => o.QUESTION_OPTION_C).ToArray();
                    string[] QOptD = QuizList.Select(o => o.QUESTION_OPTION_D).ToArray();
                    string[] QOptE = QuizList.Select(o => o.QUESTION_OPTION_E).ToArray();
                    string[] QOptF = QuizList.Select(o => o.QUESTION_OPTION_F).ToArray();
                    string[] QCrrtAns = QuizList.Select(o => o.CORRECT_ANSWER).ToArray();
                    string[] QCrtAnsOpt = QuizList.Select(o => o.CORRECT_ANSWER_OPTION).ToArray();
                    string[] QHint1 = QuizList.Select(o => o.HINT_1).ToArray();
                    string[] QHint2 = QuizList.Select(o => o.HINT_2).ToArray();
                    string[] QHint3 = QuizList.Select(o => o.HINT_3).ToArray();
                    string[] QHint4 = QuizList.Select(o => o.HINT_4).ToArray();
                    string[] QHint5 = QuizList.Select(o => o.HINT_5).ToArray();
                    string[] QHint6 = QuizList.Select(o => o.HINT_6).ToArray();
                    Int32[] Cmplx = QuizList.Select(o => o.COMPLEXITY).ToArray();
                    Int32[] SubCmplx = QuizList.Select(o => o.SUB_COMPLEXITY).ToArray();
                    Int64[] QzQstTypId = QuizList.Select(o => o.QUESTION_TYPEID).ToArray();
                    string[] AstObjId = QuizList.Select(o => o.AstObjId).ToArray();
                    Int64[] MaxTime = QuizList.Select(o => o.MAXTIME).ToArray();
                    Int64[] PosPoint = QuizList.Select(o => o.POSITIVE_PT).ToArray();
                    Int64[] NegPoint = QuizList.Select(o => o.NEGATIVE_PT).ToArray();
                    Int64[] OptCnt = QuizList.Select(o => o.OPTION_COUNT).ToArray();
                    Int64[] IsActive = QuizList.Select(o => o.ISACTIVE).ToArray();
                    string[] URL = QuizList.Select(o => o.URL).ToArray();

                    Int64[] QzQstNo = QuizList.Select(o => o.QUESTION_NO).ToArray();
                    string[] QLangCd = QuizList.Select(o => o.LANG_CODE).ToArray();
                    Int64[] HintCnt = QuizList.Select(o => o.HINT_COUNT).ToArray();
                    Int32[] IsIPLQuestions = Enumerable.Repeat(0, QuizList.Count).ToArray();

                    DateTime[] GameDate = QuizList.Select(o => o.DATE.DateValue()).ToArray().DateTimeArray();


                    retVal = _DBContext.IngestQuestion(OptType, QMId, QCtgId, SprtId, QDesc, QOptA, QOptB, QOptC, QOptD, QOptE, QOptF, QCrrtAns, QCrtAnsOpt,
                               QHint1, QHint2, QHint3, QHint4, QHint5, QHint6, Cmplx, SubCmplx, QzQstTypId, URL, AstObjId, MaxTime, PosPoint,
                                NegPoint, OptCnt, IsActive, GameDate, QzQstNo, QLangCd, HintCnt, IsIPLQuestions);

                    if (retVal == 2)
                    {
                        error = "RetVal ==> " + retVal + " Validate the following column in excel : QUESTION_DESCRIPTION, CATEGORY_ID, SPORT_ID, " +
                            "CORRECT_ANSWER, CORRECT_ANSWER_OPTION, MAXTIME,POSITIVE_PT, DATE, OPTION_COUNT,QUESTION_OPTION_A, " +
                            "QUESTION_OPTION_B,QUESTION_OPTION_C,QUESTION_OPTION_D, QUESTION_TYPEID, COMPLEXITY, SUB_COMPLEXITY";
                    }
                }
                else
                {
                    retVal = -41;
                    error = "Wrong extensions.";
                }
            }
            catch (Exception ex)
            {
                LocalErrorLog.LogException(System.Reflection.MethodBase.GetCurrentMethod(), null, ex);

                error = ex.Message;
            }
            return new Tuple<int, String>(Convert.ToInt32(retVal), error);
        }

        public Tuple<int, String> VerifyQuestion(Int64 QMId, Int64 QCtgId, Int64 SprtId, string date, out DataSet ds)
        {
            String error = string.Empty;
            Int32 retVal = -40;
            Int32 OptType = 1;
            try
            {
                DateTime dt = date.ToDateTimeValue();
                retVal = _DBContext.VerifyQuestion(OptType, QMId, QCtgId, SprtId, dt, out ds);
            }
            catch (Exception ex)
            {
                ds = null;
                LocalErrorLog.LogException(System.Reflection.MethodBase.GetCurrentMethod(), null, ex);

                error = ex.Message;
            }
            return new Tuple<int, String>(Convert.ToInt32(retVal), error);
        }


        #region "Helper"

        private ExcelCheckModel GetQuizList(DataTable dt)
        {
            ExcelCheckModel excelCheck = new ExcelCheckModel();

            List<Contracts.DataPopulation.DataPopulation> QuestionList = new List<Contracts.DataPopulation.DataPopulation>();
            try
            {
                if (dt != null)
                {
                    QuestionList = dt.AsEnumerable().
                    Select(s => new Contracts.DataPopulation.DataPopulation
                    {
                        QMId = s[("QUIZ_MASTERID")].IntValue(),
                        SprtId = s[("SPORT_ID")].IntValue(),
                        QCtgId = s[("CATEGORY_ID")].IntValue(),
                        QDesc = s[("QUESTION_DESCRIPTION")].StringValue(),
                        QOptA = s[("QUESTION_OPTION_A")].StringValue(),
                        QOptB = s[("QUESTION_OPTION_B")].StringValue(),
                        QOptC = s[("QUESTION_OPTION_C")].StringValue(),
                        QOptD = s[("QUESTION_OPTION_D")].StringValue(),
                        QOptE = s[("QUESTION_OPTION_E")].StringValue(),
                        QOptF = s[("QUESTION_OPTION_F")].StringValue(),
                        QCrrtAns = s[("CORRECT_ANSWER")].StringValue(),
                        QCrtAnsOpt = s[("CORRECT_ANSWER_OPTION")].StringValue(),
                        QHint1 = s[("HINT_1")].StringValue(),
                        QHint2 = s[("HINT_2")].StringValue(),
                        QHint3 = s[("HINT_3")].StringValue(),
                        QHint4 = s[("HINT_4")].StringValue(),
                        QHint5 = s[("HINT_5")].StringValue(),
                        QHint6 = s[("HINT_6")].StringValue(),
                        Cmplx = s[("COMPLEXITY")].Int32Value(),
                        SubCmplx = s[("SUB_COMPLEXITY")].Int32Value(),
                        QzQstTypId = s[("QUESTION_TYPEID")].IntValue(),
                        MaxTime = s[("MAXTIME")].IntValue(),
                        PosPoint = s[("POSITIVE_PT")].IntValue(),
                        NegPoint = s[("NEGATIVE_PT")].IntValue(),
                        IsActive = s[("ISACTIVE")].IntValue(),
                        QLangCd = s[("LANG_CODE")].StringValue(),
                        QzQstNo = s[("QUESTION_NO")].IntValue(),
                        HintCnt = s[("HINT_COUNT")].IntValue(),
                        GameDate = s[("DATE")].DateValue(),
                        OptCnt = s[("OPTION_COUNT")].IntValue()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Checkpoint 1 Error in Excel Data Extarction ==>" + ex.Message);
            }

            excelCheck.checkexcel = ExcelValidation(QuestionList);
            excelCheck.QuizList = QuestionList;

            return excelCheck;
        }

        private ValidationCheckParams ExcelValidation(List<Contracts.DataPopulation.DataPopulation> excelList)
        {
            ValidationCheckParams validationCheck = new ValidationCheckParams();

            validationCheck.QOptA = excelList.Select(o => o.QOptA).ToList().Any(s => string.IsNullOrEmpty(s));
            validationCheck.QOptB = excelList.Select(o => o.QOptB).ToList().Any(s => string.IsNullOrEmpty(s));
            validationCheck.QOptC = excelList.Select(o => o.QOptC).ToList().Any(s => string.IsNullOrEmpty(s));
            validationCheck.QOptD = excelList.Select(o => o.QOptD).ToList().Any(s => string.IsNullOrEmpty(s));
            validationCheck.Cmplx = excelList.Select(o => o.Cmplx).ToList().Any(s => string.IsNullOrEmpty(s.ToString()));
            validationCheck.SubCmplx = excelList.Select(o => o.SubCmplx).ToList().Any(s => string.IsNullOrEmpty(s.ToString()));
            validationCheck.QzQstTypId = excelList.Select(o => o.QzQstTypId).ToList().Any(s => string.IsNullOrEmpty(s.ToString()));
            validationCheck.OptCnt = excelList.Select(o => o.OptCnt).ToList().Any(s => string.IsNullOrEmpty(s.ToString()));
            validationCheck.GameDate = excelList.Select(o => o.GameDate).ToList().Any(s => string.IsNullOrEmpty(s));
            validationCheck.PosPoint = excelList.Select(o => o.PosPoint).ToList().Any(s => string.IsNullOrEmpty(s.ToString()));
            validationCheck.QCrtAnsOpt = excelList.Select(o => o.QCrtAnsOpt).ToList().Any(s => string.IsNullOrEmpty(s));
            validationCheck.QCrrtAns = excelList.Select(o => o.QCrrtAns).ToList().Any(s => string.IsNullOrEmpty(s));
            validationCheck.SprtId = excelList.Select(o => o.SprtId).ToList().Any(s => string.IsNullOrEmpty(s.ToString()));
            validationCheck.QCtgId = excelList.Select(o => o.QCtgId).ToList().Any(s => string.IsNullOrEmpty(s.ToString()));
            validationCheck.QDesc = excelList.Select(o => o.QDesc).ToList().Any(s => string.IsNullOrEmpty(s));

            return validationCheck;
        }

        private class ValidationCheckParams
        {
            public bool QOptA { get; set; }
            public bool QOptB { get; set; }
            public bool QOptC { get; set; }
            public bool QOptD { get; set; }
            public bool Cmplx { get; set; }
            public bool SubCmplx { get; set; }
            public bool QzQstTypId { get; set; }
            public bool OptCnt { get; set; }
            public bool GameDate { get; set; }
            public bool PosPoint { get; set; }
            public bool QCrtAnsOpt { get; set; }
            public bool QCrrtAns { get; set; }
            public bool SprtId { get; set; }
            public bool QCtgId { get; set; }
            public bool QDesc { get; set; }
        }

        private class ExcelCheckModel
        {
            public ValidationCheckParams checkexcel { get; set; }

            public List<Contracts.DataPopulation.DataPopulation> QuizList { get; set; }
        }

        bool IsValidationPassed(object myObject, out List<string> colName)
        {

            colName = new List<string>();

            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(bool))
                {
                    bool value = (bool)pi.GetValue(myObject);
                    if (value)
                    {
                        colName.Add(pi.Name);
                    }

                }
            }

            if (colName != null)
            {
                if (colName.Count > 0)
                    return false;
                else
                    return true;
            }
            else
                return true;
        }
        #endregion

    }
}
