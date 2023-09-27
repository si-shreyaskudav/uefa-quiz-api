using System.Collections.Generic;
using System;

namespace Gaming.Quiz.Contracts.Feeds
{
    public class ProfanityWords
    {
        public List<string> words { get; set; }
    }

    public class BadgesList
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public Int64 IsActive { get; set; }
    }

    public class MixAPI
    {
        public string ldrbd { get; set; }
    }
}
