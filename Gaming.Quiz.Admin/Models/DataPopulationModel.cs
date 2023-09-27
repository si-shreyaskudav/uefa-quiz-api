using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gaming.Quiz.Admin.Models
{
    public class DataPopulationModel
    {
        public IFormFile file { get; set; }
        public String ExcelPath { get; set; }

        public string Date { get; set; }
        public Int64? QMId { get; set; }
        public Int64? SportId { get; set; }
        public Int64? CatgId { get; set; }
    }
}
