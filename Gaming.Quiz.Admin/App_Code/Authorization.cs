using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using  Gaming.Quiz.Interfaces.Asset;
using Microsoft.Extensions.Options;
using Gaming.Quiz.Contracts.Configuration;

namespace  Gaming.Quiz.Admin.App_Code
{
    public class Authorization : Session, Interfaces.Admin.ISession
    {
        private readonly Helper _Helper;
        private readonly Contracts.Configuration.Admin _Admin;

        public Authorization(IOptions<Application> appSettings, IHttpContextAccessor httpContextAccessor, IAsset asset) : base(httpContextAccessor)
        {
            _Helper = new Helper(asset);
            _Admin = appSettings.Value.Admin;

        }

        public List<String> Pages(String name = "")
        {
            List<String> pages = new List<String>();

            try
            {
                String user = name;

                if (String.IsNullOrEmpty(user))
                    user = this.SlideAdminCookie();

                //Contracts.Configuration.Authorization authority = _Helper.Config().Authorization.Where(o => o.User.ToLower().Trim() == user.ToLower().Trim()).FirstOrDefault();
                //Contracts.Configuration.Authorization authority = _Admin.Authorization.Where(o => o.User.ToLower().Trim() == user.ToLower().Trim()).FirstOrDefault();


                Contracts.Configuration.Authorization authority = _Admin.Authorization.FirstOrDefault();

                pages = authority.Pages.ToList();
            }
            catch
            {

                throw new Exception("Please check if config exists on the path: \"config/admin_1.json\"");
            }

            return pages;
        }
    }
}
