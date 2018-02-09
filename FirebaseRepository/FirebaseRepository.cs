using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseRest;
using FirebaseRest.Extensions;
using FirebaseRest.Models;

namespace FirebaseRepository
{
    public class FirebaseRepository<T> : IFirebaseRepository<T> where T : class, IFirebaseEntity, new()
    {
        private readonly FirebaseQuery _query;
        private readonly string _path;

        public FirebaseRepository(string path, FirebaseQuery query)
        {
            _path = path;
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

        public async Task<T> PatchAsync(T entity, string key = null)
        {
            T result;
            if (null != key) result = await _query.Child(_path).Child(key).PatchAsync(entity);
            else result = await _query.Child(_path).PutAsync(entity);
            return result;
        }

        public async Task DeleteAsync(string key)
        {
            await _query.Child(_path).Child(key).DeleteAsync();
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
    }
}