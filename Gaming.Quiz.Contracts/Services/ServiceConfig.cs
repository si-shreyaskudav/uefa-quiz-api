using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Contracts.Services
{
    public class ServiceConfig
    {
        public string ServiceName { get; set; }
        public int IsActive { get; set; }
        public int SendEmail { get; set; }
    }
}
