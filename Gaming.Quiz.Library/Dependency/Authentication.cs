using  Gaming.Quiz.Contracts.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;

namespace  Gaming.Quiz.Library.Dependency
{
    public class Authentication
    {
        private readonly String _Header;
        private readonly String _Backdoor;
        private readonly String _Domain;
        private readonly IHttpContextAccessor _HttpContext;

        public Authentication(IOptions<Application> appSettings, IHttpContextAccessor context)
        {
            _Header = appSettings.Value.API.Authentication.Header;
            _Backdoor = appSettings.Value.API.Authentication.Backdoor;
            _Domain = appSettings.Value.API.Domain;
            _HttpContext = context;
        }

        public bool Validate(String backdoor = null)
        {
            bool valid = false;

            if (backdoor == _Backdoor)
                return true;

            String referer = _HttpContext.HttpContext.Request.Headers["Referer"];
            String header = _HttpContext.HttpContext.Request.Headers["entity"];

            if (String.IsNullOrEmpty(referer))
                return false;

            if ((referer.ToLower().IndexOf(_Domain.ToLower()) > -1 && _Header == header)
                || referer.ToLower().Trim().IndexOf("index.html") > -1)
                valid = true;

            return valid;

        }
    }
}
