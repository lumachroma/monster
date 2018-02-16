using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using FirebaseRepository;
using FirebaseRest;
using FirebaseRest.Extensions;
using FirebaseRest.Models;
using Newtonsoft.Json;

namespace Monster.Firebase
{
    public class FirebaseDataContext<T> : IFirebaseRepository<T> where T : IFirebaseEntity
    {
        private readonly string _node;
        private readonly FirebaseQuery _query;
        private string _path;

        public FirebaseDataContext(string node, FirebaseQuery query)
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
            InitNewEntity(entity);
            var result = await _query.Child(_path).PostAsync(entity);
            return result;
        }

        public async Task<T> PutAsync(T entity, string key = null)
        {
            await InitExistingEntity(entity, key);
            T result;
            if (null != key) result = await _query.Child(_path).Child(key).PutAsync(entity);
            else result = await _query.Child(_path).PutAsync(entity);
            return result;
        }

        public async Task<string> PatchAsync(dynamic entity, string key = null)
        {
            await InitExistingEntity(entity, key);
            string result;
            if (null != key)
                result = await _query.Child(_path).Child(key).PatchAsync(JsonConvert.SerializeObject(entity));
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

        private static void InitNewEntity(T entity)
        {
            var now = DateTime.Now;
            var who = HttpContext.Current.User.Identity.Name;
            entity.Id = Guid.NewGuid().ToString();
            entity.CreatedDate = now;
            entity.CreatedBy = who;
            entity.ChangedDate = now;
            entity.ChangedBy = who;
        }

        private async Task InitExistingEntity(T entity, string key = null)
        {
            T existing;
            if (null != key) existing = await GetByKeyAsync(key);
            else existing = await GetSingleAysnc();
            InitNewEntity(entity);
            if (null != existing)
            {
                entity.Id = existing.Id;
                entity.CreatedDate = existing.CreatedDate;
                entity.CreatedBy = existing.CreatedBy;
            }
        }

        private static void InitNewEntity(dynamic entity)
        {
            var now = DateTime.Now;
            var who = HttpContext.Current.User.Identity.Name;
            entity.Id = Guid.NewGuid().ToString();
            entity.CreatedDate = now;
            entity.CreatedBy = who;
            entity.ChangedDate = now;
            entity.ChangedBy = who;
        }

        private async Task InitExistingEntity(dynamic entity, string key = null)
        {
            T existing;
            if (null != key) existing = await GetByKeyAsync(key);
            else existing = await GetSingleAysnc();
            InitNewEntity(entity);
            if (null != existing)
            {
                entity.Id = existing.Id;
                entity.CreatedDate = existing.CreatedDate;
                entity.CreatedBy = existing.CreatedBy;
            }
        }
    }
}