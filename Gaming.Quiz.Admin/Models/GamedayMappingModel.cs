using Gaming.Quiz.Contracts.GamedayMapping;
using Gaming.Quiz.Interfaces.GamedayMapping;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gaming.Quiz.Admin.Models
{
    public class GamedayMappingModel
    {
        public List<GamedayMapping>  GamedayMapping { get; set; }
    }

    public class GamedayMappingWorker
    {
        public async Task<GamedayMappingModel> GetGamedayMappingModel(IGamedayMappingBlanket _GamedayMappingContext)
        {
            GamedayMappingModel data = new GamedayMappingModel();
            try
            {
                data.GamedayMapping = _GamedayMappingContext.GetGamedayMappings();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return data;
        }
    }
}
