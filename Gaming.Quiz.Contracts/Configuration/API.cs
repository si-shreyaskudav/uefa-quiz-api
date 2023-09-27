﻿using System;

namespace  Gaming.Quiz.Contracts.Configuration
{
    public class API
    {
        public Authentication Authentication { get; set; }
        public String Domain { get; set; }
    }

    public class Authentication
    {
        public String Header { get; set; }
        public String Backdoor { get; set; }
    }
}
