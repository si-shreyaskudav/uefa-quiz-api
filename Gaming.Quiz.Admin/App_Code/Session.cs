using Microsoft.AspNetCore.Http;
using System;

namespace  Gaming.Quiz.Admin.App_Code
{
    public class Session
    {
        IHttpContextAccessor _HttpContextAccessor;
        public const string _AdminCookey = "UCL_DEVIL_FRUIT";
        private readonly int _ExpiryDays = 1;

        public Session(IHttpContextAccessor httpContextAccessor)
        {
            _HttpContextAccessor = httpContextAccessor;
        }

        public bool _HasAdminCookie
        {
            get
            {
                return (_HttpContextAccessor.HttpContext.Request.Cookies[_AdminCookey] != null);
            }
        }

        public String _GetAdminCookie
        {
            get
            {
                String cookie = _HttpContextAccessor.HttpContext.Request.Cookies[_AdminCookey];

                return !String.IsNullOrEmpty(cookie) ? cookie : "";
            }
        }

        public bool SetAdminCookie(String value)
        {
            bool status = false;
            try
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(_ExpiryDays);
                _HttpContextAccessor.HttpContext.Response.Cookies.Append(_AdminCookey, value.ToLower(), option);

                status= true;
            }
            catch (Exception ex)
            {
                status = false;
                throw ex;
            }

            return status;
        }

        public String SlideAdminCookie()
        {
            String value = _HttpContextAccessor.HttpContext.Request.Cookies[_AdminCookey];
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(_ExpiryDays);

            _HttpContextAccessor.HttpContext.Response.Cookies.Append(_AdminCookey, value, option);

            return !String.IsNullOrEmpty(value) ? value : "";
        }

        public void DeleteAdminCookie()
        {
            _HttpContextAccessor.HttpContext.Response.Cookies.Delete(_AdminCookey);
        }
    }
}
