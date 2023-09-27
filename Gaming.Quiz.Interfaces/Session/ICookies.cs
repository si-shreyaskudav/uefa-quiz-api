using System;
using Gaming.Quiz.Contracts.Common;
using  Gaming.Quiz.Contracts.Session;

namespace  Gaming.Quiz.Interfaces.Session
{
    public interface ICookies
    {
        #region " WAF "

        /************* URC, USC *************/

        bool _HasWAFUSCCookies { get; }
        bool _HasWAFCookies { get; }

        String _GetWAFUSCCookies { get; }
        WAFCookie _GetWAFCookies { get; }

        bool SetWAFCookies(WAFCookie wc);
        bool SetWAFUSCCookies(String userGuid);
        
        bool DELETE();

        #endregion

        #region " User "

        bool _HasUserCookies { get; }

        UserCookie _GetUserCookies { get; }

        Int64 _GetUserId { get; }
        Int64 _GetMasterId { get; }

        bool SetUserCookies(UserCookie uc);

        #endregion

        #region " Game "

        bool _HasGameCookies { get; }
        GameCookie _GetGameCookies { get; }
        bool SetGameCookies(GameCookie gc);
        bool UpdateGameCookies(GameCookie values);
        bool _HasTeam { get; }
        void DeleteGameCookies();
        void DeleteOnlyGameCookies();


        #endregion

        //bool UpdateUserCookies(UserCookie values);

        HTTPLog PopulateLog(String FunctionName, String Message);
    }
}
