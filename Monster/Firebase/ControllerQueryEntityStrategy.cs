using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseRest.Models;

namespace Monster.Firebase
{
    public abstract class ControllerQueryEntityStrategy<T>
    {
        public abstract Task<IReadOnlyCollection<FirebaseObject<T>>> All();
        public abstract Task<IReadOnlyCollection<FirebaseObject<T>>> Some(string limit);
        public abstract Task<T> Get(string key);
        public abstract Task<FirebaseObject<T>> Post(T entity);
        public abstract Task<T> Put(string key, T entity);
        public abstract Task Delete(string key);
        public abstract string GetCurrentUser();
    }
}