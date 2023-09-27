using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Contracts.Session
{
    public class WAFLoginData
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public string user_guid { get; set; }
        public string is_app { get; set; }
    }
}
