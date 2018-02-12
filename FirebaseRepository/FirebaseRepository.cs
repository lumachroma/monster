using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseRest;
using FirebaseRest.Extensions;
using FirebaseRest.Models;
using Newtonsoft.Json;

namespace FirebaseRepository
{
    public class FirebaseRepository<T> : IFirebaseRepository<T> where T : IFirebaseEntity
    {
        private readonly string _node;
        private readonly FirebaseQuery _query;
        private string _path;

        public FirebaseRepository(string node, FirebaseQuery query)
        {
            _node = node;
            _path = node;
            _query = query;
        }

        public async Task<IReadOnlyCollection<FirebaseObject<T>>> GetAllAsync()
        {
            var results = await _query.Child(_path).GetAsync<T>();
            return results;
        }

        public async Task<T> GetSingleAysnc()
        {
            var result = await _query.Child(_path).GetSingleAsync<T>();
            return result;
        }

        public async Task<T> GetByKeyAsync(string key)
        {
            var result = await _query.Child(_path).Child(key).GetSingleAsync<T>();
            return result;
        }

        public async Task<FirebaseObject<T>> PostAsync(T entity)
        {
            var result = await _query.Child(_path).PostAsync(entity);
            return result;
        }

        public async Task<T> PutAsync(T entity, string key = null)
        {
            T result;
            if (null != key) result = await _query.Child(_path).Child(key).PutAsync(entity);
            else result = await _query.Child(_path).PutAsync(entity);
            return result;
        }

        public async Task<string> PatchAsync(dynamic entity, string key = null)
        {
            string result;
            if (null != key) result = await _query.Child(_path).Child(key).PatchAsync(JsonConvert.SerializeObject(entity));
            else result = await _query.Child(_path).PatchAsync(JsonConvert.SerializeObject(entity));
            return result;
        }

        public async Task DeleteAsync(string key = null)
        {
            if (null != key) await _query.Child(_path).Child(key).DeleteAsync();
            else await _query.Child(_path).DeleteAsync();
        }

        public async Task<IReadOnlyCollection<FirebaseObject<T>>> SearchByKeyValueAsync(string key, string value)
        {
            var results = await _query.Child(_path).OrderBy(key).EqualTo(value).GetAsync<T>();
            return results;
        }

        public string GetPath()
        {
            return _path;
        }

        public string SetPath(string path = null)
        {
            _path = null != path ? $"{_node}/{path}" : _node;
            return _path;
        }
    }
}