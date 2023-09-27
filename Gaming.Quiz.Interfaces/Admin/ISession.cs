using System;
using System.Collections.Generic;

namespace  Gaming.Quiz.Interfaces.Admin
{
    public interface ISession
    {
        List<String> Pages(String name = "");

        bool _HasAdminCookie { get; }
        String _GetAdminCookie { get; }
        bool SetAdminCookie(String value);
        String SlideAdminCookie();
        void DeleteAdminCookie();
    }
}
