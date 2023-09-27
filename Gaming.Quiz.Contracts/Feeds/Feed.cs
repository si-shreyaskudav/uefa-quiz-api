using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Contracts.Feeds
{
    class Feed
    {
    }

    public class Tutoral
    {
        public List<SampleQuestion> SampleQuestion { get; set; }
    }
    public class SampleQuestion
    {
        public int QuId { get; set; }
        public string QuDes { get; set; }
        public List<OptionNode> OptionNode { get; set; }
        public int QuNo { get; set; }
        public string LastQuAnsOpt { get; set; }
    }
    public class OptionNode
    {
        public string Option { get; set; }
        public string Value { get; set; }
    }

}
