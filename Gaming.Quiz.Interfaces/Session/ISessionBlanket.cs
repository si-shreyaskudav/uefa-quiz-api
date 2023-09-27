using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.Session
{
    public interface ISessionBlanket
    {
        Task<HTTPResponse> AnonymousLogin(Credentials credentials);
        HTTPResponse Login(string socialId);
        Task<HTTPResponse> WafLogin(Credentials credentials);
    }
}
