using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Contracts.Gameplay
{
    public class Gameplay
    {
    }

    public class PlayAttempt
    {
        public Int64 GamedayId { get; set; }
        public Int64 QuizId { get; set; }
        public String Lang { get; set; }
        public Int64 PlatformId { get; set; }
        public Int64 AttemptNo { get; set; }
    }

    public class Lifeline
    {
        public Int64 UsrAttemptId { get; set; }
        public Int64 QzQstMid { get; set; }
        public Int64 QuizId { get; set; }
        public Int64 GamedayId { get; set; }
        public string Lang { get; set; }
        public String SelectedAns { get; set; }
    }

    public class Question
    {
        public Int64 GamedayId { get; set; }
        public Int64 QzAtmpId { get; set; }
        public Int64 QstMId { get; set; }
        public String SltdAnsOptn { get; set; } 
        public Int64 PlatformId { get; set; }
        public Int64 TimeSpent { get; set; }
        public Int64 AtmptStatus { get; set; }
        public Int64 ResumeAtmp { get; set; }
        public Int64 HintCnt { get; set; }
        public Int64 QuizId { get; set; }
        public String Lang { get; set; }

    }

    public class ScoreCard
    {
        public Int64 GamedayId { get; set; }
        public Int64 QuizId { get; set; }
        public Int64 AttemptId { get; set; }
    }
}
