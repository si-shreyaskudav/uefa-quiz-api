using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Contracts.GamedayMapping
{
    public class GamedayMapping
    {
        public bool Checked { get; set; }
        public int QuizId { get; set; }
        public string Date { get; set; }
        public int GamedayId { get; set; }
        public string TagName { get; set; }
        public string IsMapped { get; set; }
        public int IsMatchday { get; set; }
        public int QuestionsLeft { get; set; }
    }
}
