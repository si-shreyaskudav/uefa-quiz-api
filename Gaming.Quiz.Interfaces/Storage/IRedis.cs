using System;
using System.Threading.Tasks;

namespace  Gaming.Quiz.Interfaces.Storage
{
    public interface IRedis
    {
        void Multiplexer();
        void Disposer();

        Task<String> Get(String key);
        //bool Set(String key, Object content, bool serialize);
        Task<bool> Set(String key, Object content, bool serialize);
        bool Has(String key);
        bool Remove(String key);
        bool RemoveAllLiveLBKeys();
        bool RemoveKeys(String pattern);


    }
}
