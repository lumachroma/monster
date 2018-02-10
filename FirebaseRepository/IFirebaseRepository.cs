using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseRest.Models;

namespace FirebaseRepository
{
    public interface IFirebaseRepository<T>
    {
        Task<IReadOnlyCollection<FirebaseObject<T>>> GetAllAsync();
        Task<T> GetSingleAysnc();
        Task<T> GetByKeyAsync(string key);
        Task<FirebaseObject<T>> PostAsync(T entity);
        Task<T> PutAsync(T entity, string key = null);
        Task<string> PatchAsync(dynamic entity, string key = null);
        Task DeleteAsync(string key = null);
        Task<IReadOnlyCollection<FirebaseObject<T>>> SearchByKeyValueAsync(string key, string value);
        string GetPath();
        string SetPath(string path = null);
    }
}