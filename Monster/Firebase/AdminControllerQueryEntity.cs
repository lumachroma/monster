using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FirebaseRepository;
using FirebaseRest.Extensions;
using FirebaseRest.Models;
using WebGrease.Css.Extensions;

namespace Monster.Firebase
{
    public class AdminControllerQueryEntity<T> : ControllerQueryEntityStrategy<T> where T : IFirebaseEntity
    {
        private readonly FirebaseDataContext<T> _context;

        public AdminControllerQueryEntity(FirebaseDataContext<T> context)
        {
            _context = context;
        }

        public override async Task<IReadOnlyCollection<FirebaseObject<T>>> All()
        {
            return await _context.GetAllAsync();
        }

        public override async Task<IReadOnlyCollection<FirebaseObject<T>>> Newest(string limit)
        {
            var results = await _context.Query.Child(_context.GetPath()).OrderByKey().LimitToLast(limit).GetAsync<T>();
            return results.Reverse().ToSafeReadOnlyCollection();
        }

        public override async Task<IReadOnlyCollection<FirebaseObject<T>>> Oldest(string limit)
        {
            return await _context.Query.Child(_context.GetPath()).OrderByKey().LimitToFirst(limit).GetAsync<T>();
        }

        public override async Task Delete(string key)
        {
            await _context.DeleteAsync(key);
        }

        public override async Task<T> Get(string key)
        {
            return await _context.GetByKeyAsync(key);
        }

        public override async Task<FirebaseObject<T>> Post(T entity)
        {
            return await _context.PostAsync(entity);
        }

        public override async Task<T> Put(string key, T entity)
        {
            return await _context.PutAsync(entity, key);
        }

        public override string GetCurrentUser()
        {
            try
            {
                return HttpContext.Current.User.Identity.Name;
            }
            catch (NullReferenceException)
            {
                return "Anonymous";
            }
        }
    }
}