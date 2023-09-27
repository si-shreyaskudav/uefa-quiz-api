using System;
using System.IO;
using System.Threading.Tasks;

namespace  Gaming.Quiz.Interfaces.Asset
{
    public interface IAction
    {
        Task<bool> RedisSET(String key, Object content, bool serialize = true);
        Task<String> GET(String key);
        Task<bool> SET(String key, Object content, bool serialize = true);
        Task<bool> SET(String bucket, String key, Object content, bool serialize = true);
        Task<bool> SETimage(String key, Stream content, bool downloadable = false);
        Task LOG(System.Reflection.MethodBase methodBase, String message);
        Task LOG(String message, Exception exception = null);
        Task DEBUG(String message);
        Task<bool> HAS(String key);
        Task<bool> REMOVE(String key);
    }
}
