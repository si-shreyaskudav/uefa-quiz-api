using Gaming.Quiz.Contracts.Configuration;
using Microsoft.Extensions.Options;

namespace Gaming.Quiz.Library.Social
{
    public class BaseOAuth
    {
        protected readonly Settings _Settings;

        public BaseOAuth(IOptions<Application> application)
        {
            _Settings = application.Value.Settings;
        }
    }
}
