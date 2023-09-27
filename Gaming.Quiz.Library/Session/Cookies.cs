using System;
using Microsoft.AspNetCore.Http;
using Gaming.Quiz.Contracts.Session;
using Gaming.Quiz.Library.Utility;
using Microsoft.Extensions.Options;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Interfaces.Session;
using System.Linq;

namespace Gaming.Quiz.Library.Session
{
    public class Cookies : Logs, ICookies
    {
        #region " PROPERTIES "

        public readonly string _UserCookey;
        public readonly string _GameCookey ;
        public readonly string _WAFCookey;
        public readonly string _WAFUSCCookey;
        private readonly int _ExpiryDays;
        private readonly string _Domain;

        public Cookies(IHttpContextAccessor httpContextAccessor, IOptions<Application> appSettings) : base(appSettings, httpContextAccessor)
        {
            _UserCookey = appSettings.Value.Properties.ClientName + "_008";
            _GameCookey = appSettings.Value.Properties.ClientName + "_SRAW";

            _WAFCookey = appSettings.Value.Cookies.WAFCookie;
            _WAFUSCCookey = appSettings.Value.Cookies.WAFUSCCookie;

            _ExpiryDays = appSettings.Value.Cookies.ExpiryDays;
            _Domain = appSettings.Value.Cookies.Domain;
        }

        public bool _HasWAFCookies
        {
            get
            {
                return (_HttpContextAccessor.HttpContext.Request.Cookies[_WAFCookey] != null);
            }
        }

        public bool _HasUserCookies
        {
            get
            {
                return (_HttpContextAccessor.HttpContext.Request.Cookies[_UserCookey] != null);
            }
        }

        public UserCookie _GetUserCookies
        {
            get
            {
                return GetUserCookies();
            }
        }

        public Int64 _GetUserId
        {
            get
            {
              return Convert.ToInt64(_GetUserCookies.UserId);
            }
                 
        }

        public Int64 _GetMasterId
        {
            get
            {
                return Convert.ToInt64(_GetUserCookies.QzMasterID);
            }

        }

        public bool _HasGameCookies
        {
            get
            {
                return (_HttpContextAccessor.HttpContext.Request.Cookies[_GameCookey] != null);
            }
        }

        public GameCookie _GetGameCookies
        {
            get
            {
                return GetGameCookies();
            }
        }

        public bool _HasWAFUSCCookies
        {
            get
            {
                return (_HttpContextAccessor.HttpContext.Request.Cookies[_WAFUSCCookey] != null);
            }
        }

        public bool _HasTeam
        {
            get
            {
                if (_HasGameCookies)
                    return true;
                else
                    return false;
            }
        }

        #region " WAF Cookie "

        public WAFCookie _GetWAFCookies
        {
            get
            {
                return GetWAFCookies();
            }
        }

        public String _GetWAFUSCCookies
        {
            get
            {
                return GetWAFUSCCookies();
            }
        }


        #endregion


        #endregion " PROPERTIES "

        #region " FUNCTIONS "

        #region " User Cookie "

        private UserCookie GetUserCookies()
        {
            String cookie = _HttpContextAccessor.HttpContext.Request.Cookies[_UserCookey];

            if (!String.IsNullOrEmpty(cookie))
            {
                UserCookie uc = new UserCookie();

                try
                {
                    String DecryptedCookie = Utility.Encryption.BaseDecrypt(cookie);
                    uc = GenericFunctions.Deserialize<UserCookie>(DecryptedCookie);
                }
                catch { }

                return uc;
            }
            else
                return null;
        }

        public bool SetUserCookies(UserCookie uc)
        {
            bool set = false;

            try
            {
                String serializedCookie = GenericFunctions.Serialize(uc);
                String value = Utility.Encryption.BaseEncrypt(serializedCookie);

                SETUser(_UserCookey, value);

                set = true;
            }
            catch { }

            return set;
        }


        /*public bool UpdateUserCookies(UserCookie values)
        {
            bool set = false;
            try
            {
                UserCookie uc = new UserCookie();
                uc = GetUserCookies();

                if (uc != null)
                {
                    if (values != null && !String.IsNullOrEmpty(values.UserId))
                        uc.UserId = values.UserId;

                    set = SetUserCookies(uc);
                }
            }
            catch { }

            return set;
        }*/

        #endregion " User Cookie "

        #region " Game Cookie "

        private GameCookie GetGameCookies()
        {
            String cookie = _HttpContextAccessor.HttpContext.Request.Cookies[_GameCookey];

            if (!String.IsNullOrEmpty(cookie))
            {
                GameCookie gc = new GameCookie();

                try
                {
                    gc = GenericFunctions.Deserialize<GameCookie>(cookie);
                }
                catch { }

                return gc;
            }
            else
                return null;
        }

        public bool SetGameCookies(GameCookie gc)
        {
            bool set = false;

            try
            {
                string value = GenericFunctions.Serialize(gc);

                SET(_GameCookey, value);

                set = true;
            }
            catch { }

            return set;
        }

        public bool UpdateGameCookies(GameCookie values)
        {
            bool set = false;
            try
            {
                GameCookie gc = new GameCookie();
                gc = GetGameCookies();

                UserCookie uc = new UserCookie();
                uc = GetUserCookies();
                if (uc != null)
                {
                    set = SetUserCookies(uc);
                }

                if (gc != null)
                {
                    if (values != null && !String.IsNullOrEmpty(values.FavPlayer))
                        gc.FavPlayer = values.FavPlayer;

                    set = SetGameCookies(gc);
                }

            }
            catch { }

            return set;
        }

        #endregion " Game Cookie "

        #region " WAF Cookie "

        private WAFCookie GetWAFCookies()
        {
            String cookie = _HttpContextAccessor.HttpContext.Request.Cookies[_WAFCookey];

            if (!String.IsNullOrEmpty(cookie))
            {
                WAFCookie gc = null;

                try
                {
                    gc = GenericFunctions.Deserialize<WAFCookie>(cookie);
                }
                catch { }

                return gc;
            }
            else
                return null;
        }

        private String GetWAFUSCCookies()
        {
            String cookie = _HttpContextAccessor.HttpContext.Request.Cookies[_WAFUSCCookey];

            if (!String.IsNullOrEmpty(cookie))
            {
                try
                {
                    return cookie;
                }
                catch { }

                return null;
            }
            else
                return null;
        }

        public bool SetWAFUSCCookies(String userGuid)
        {
            bool set = false;

            try
            {
                SETWAF(_WAFUSCCookey, userGuid);

                set = true;
            }
            catch { }

            return set;
        }

        public bool SetWAFCookies(WAFCookie wc)
        {
            bool set = false;

            try
            {
                string value = (GenericFunctions.Serialize(wc));

                SET(_WAFCookey, value);

                set = true;
            }
            catch { }

            return set;
        }

        #endregion

        private void SET(String key, String value)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(_ExpiryDays);
            option.Secure = true;
            option.Domain = _Domain;
            //option.HttpOnly = true;

            _HttpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }
        private void SETUser(String key, String value)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(_ExpiryDays);
            option.Secure = true;
            option.Domain = _Domain;
            //option.HttpOnly = true;

            _HttpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }
        private void SETWAF(String key, String value)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(_ExpiryDays);
            option.Secure = true;
            option.HttpOnly = true;
            _HttpContextAccessor.HttpContext.Response.Cookies.Append(key, Uri.UnescapeDataString(value), option);
        }

        public void DeleteGameCookies()
        {
            _HttpContextAccessor.HttpContext.Response.Cookies.Delete(_UserCookey);
            _HttpContextAccessor.HttpContext.Response.Cookies.Delete(_GameCookey);
            _HttpContextAccessor.HttpContext.Response.Cookies.Delete(_WAFUSCCookey);
            _HttpContextAccessor.HttpContext.Response.Cookies.Delete(_WAFCookey);
        }


        private void SETDelete(String key, String value)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(-1);
            _HttpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }

        public void DeleteOnlyGameCookies()
        {
            SETDelete(_GameCookey, "");
            _HttpContextAccessor.HttpContext.Response.Cookies.Delete(_GameCookey);
        }

        #endregion " FUNCTIONS "

        public bool DELETE()
        {
            bool set = false;
            try
            {
                _HttpContextAccessor.HttpContext.Response.Cookies.Delete(_UserCookey);
                _HttpContextAccessor.HttpContext.Response.Cookies.Delete(_GameCookey);
                set = true;
            }
            catch (Exception ex)
            {
                set = false;
            }
            return set;
        }
    }
}