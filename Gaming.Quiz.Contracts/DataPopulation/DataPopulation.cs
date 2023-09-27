using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Contracts.DataPopulation
{
    public class DataPopulation
    {
        public Int64 QMId { get; set; }
        public Int64 QCtgId { get; set; }
        public Int64 SprtId { get; set; }
        public string QDesc { get; set; }
        public string QOptA { get; set; }
        public string QOptB { get; set; }
        public string QOptC { get; set; }
        public string QOptD { get; set; }
        public string QOptE { get; set; }
        public string QOptF { get; set; }
        public string QCrrtAns { get; set; }
        public string QCrtAnsOpt { get; set; }
        public string QHint1 { get; set; }
        public string QHint2 { get; set; }
        public string QHint3 { get; set; }
        public string QHint4 { get; set; }
        public string QHint5 { get; set; }
        public string QHint6 { get; set; }
        public Int32 Cmplx { get; set; }
        public Int32 SubCmplx { get; set; }
        public Int64 QzQstTypId { get; set; }
        public string URL { get; set; }
        public string AstObjId { get; set; }
        public Int64 MaxTime { get; set; }
        public Int64 PosPoint { get; set; }
        public Int64 NegPoint { get; set; }
        public Int64 OptCnt { get; set; }
        public Int64 IsActive { get; set; }
        public Int64 IsDeleted { get; set; }

        public string GameDate { get; set; }
        public Int64 QzQstNo { get; set; }
        public string QLangCd { get; set; }
        public Int64 HintCnt { get; set; }
    }

    public class ExcelData
    {
        public Int64 QUIZ_MASTERID { get; set; }
        public Int64 SPORT_ID { get; set; }
        public Int64 CATEGORY_ID { get; set; }
        public string QUESTION_DESCRIPTION { get; set; }
        public string QUESTION_OPTION_A { get; set; }
        public string QUESTION_OPTION_B { get; set; }
        public string QUESTION_OPTION_C { get; set; }
        public string QUESTION_OPTION_D { get; set; }
        public string QUESTION_OPTION_E { get; set; }
        public string QUESTION_OPTION_F { get; set; }
        public string CORRECT_ANSWER { get; set; }
        public string CORRECT_ANSWER_OPTION { get; set; }
        public string HINT_1 { get; set; }
        public string HINT_2 { get; set; }
        public string HINT_3 { get; set; }
        public string HINT_4 { get; set; }
        public string HINT_5 { get; set; }
        public string HINT_6 { get; set; }
        public Int32 COMPLEXITY { get; set; }
        public Int32 SUB_COMPLEXITY { get; set; }
        public Int64 QUESTION_TYPEID { get; set; }
        public string URL { get; set; }
        public string AstObjId { get; set; }
        public Int64 MAXTIME { get; set; }
        public Int64 POSITIVE_PT { get; set; }
        public Int64 NEGATIVE_PT { get; set; }
        public Int64 OPTION_COUNT { get; set; }
        public Int64 ISACTIVE { get; set; }
        public Int64 IsDeleted { get; set; }

        public string DATE { get; set; }
        public Int64 QUESTION_NO { get; set; }
        public string LANG_CODE { get; set; }
        public Int64 HINT_COUNT { get; set; }
    }
}
