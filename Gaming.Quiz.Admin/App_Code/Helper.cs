using System;
using  Gaming.Quiz.Interfaces.Asset;
using  Gaming.Quiz.Library.Utility;

namespace  Gaming.Quiz.Admin.App_Code
{
    public class Helper
    {
        protected readonly IAsset _Asset;

        public Helper(IAsset asset)
        {
            _Asset = asset;
        }

    }
}
