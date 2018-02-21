using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using FirebaseRepository;
using FirebaseRest.Extensions;
using FirebaseRest.Models;
using Newtonsoft.Json;

namespace Monster.Firebase
{
    public class UserControllerQueryEntity<T> : ControllerQueryEntityStrategy<T> where T : IFirebaseEntity
    {
        private readonly FirebaseDataContext<T> _context;
        private readonly T _dummyNullEntity;

        public UserControllerQueryEntity(FirebaseDataContext<T> context)
        {
            _context = context;
            _dummyNullEntity = JsonConvert.DeserializeObject<T>("null");
        }

        public override async Task<IReadOnlyCollection<FirebaseObject<T>>> All()
        {
            return await _context.Query.Child(_context.GetPath())
                .OrderBy("\"CreatedBy\"").EqualTo($"\"{GetCurrentUser()}\"")
                .GetAsync<T>();
        }

        public override async Task<IReadOnlyCollection<FirebaseObject<T>>> Some(string limit)
        {
            return await _context.Query.Child(_context.GetPath())
                .OrderBy("\"CreatedBy\"").EqualTo($"\"{GetCurrentUser()}\"").
                LimitToLast(limit).GetAsync<T>();
        }

        public override async Task Delete(string key)
        {
            var existing = await _context.GetByKeyAsync(key);
            if (existing == null) return;
            if (existing.CreatedBy != GetCurrentUser()) return;
            await _context.DeleteAsync(key);
        }

        public override async Task<T> Get(string key)
        {
            var result = await _context.GetByKeyAsync(key);
            if (result == null) return _dummyNullEntity;
            return result.CreatedBy == GetCurrentUser() ? result : _dummyNullEntity;
        }

        public override async Task<FirebaseObject<T>> Post(T entity)
        {
            return await _context.PostAsync(entity);
        }

        public override async Task<T> Put(string key, T entity)
        {
            var existing = await _context.GetByKeyAsync(key);
            if (existing == null) return _dummyNullEntity;
            if (existing.CreatedBy != GetCurrentUser()) return _dummyNullEntity;
            return await _context.PutAsync(entity, key);
        }

        public override string GetCurrentUser()
        {
            return HttpContext.Current.User.Identity.Name;
        }
    }
}