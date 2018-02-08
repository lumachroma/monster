using System.Threading.Tasks;
using FirebaseRest.Models;
using Newtonsoft.Json;

namespace FirebaseRest.Extensions
{
    public static class FirebaseQueryExtensions
    {
        public static FirebaseQuery Child(this FirebaseQuery query, string path)
        {
            query.UrlSegments.Add(path);
            return query;
        }

        public static FirebaseQuery Shallow(this FirebaseQuery query, string value)
        {
            query.UrlParameters.Add("shallow", value);
            return query;
        }

        public static FirebaseQuery Print(this FirebaseQuery query, string value)
        {
            query.UrlParameters.Add("print", value);
            return query;
        }

        public static FirebaseQuery OrderBy(this FirebaseQuery query, string childKey)
        {
            query.FilterParameters.Add("orderBy", childKey);
            return query;
        }

        public static FirebaseQuery OrderByKey(this FirebaseQuery query)
        {
            query.FilterParameters.Add("orderBy", "\"$key\"");
            return query;
        }

        public static FirebaseQuery OrderByValue(this FirebaseQuery query)
        {
            query.FilterParameters.Add("orderBy", "\"$value\"");
            return query;
        }

        public static FirebaseQuery LimitToFirst(this FirebaseQuery query, string limit)
        {
            query.FilterParameters.Add("limitToFirst", limit);
            return query;
        }

        public static FirebaseQuery LimitToLast(this FirebaseQuery query, string limit)
        {
            query.FilterParameters.Add("limitToLast", limit);
            return query;
        }

        public static FirebaseQuery StartAt(this FirebaseQuery query, string range)
        {
            query.FilterParameters.Add("startAt", range);
            return query;
        }

        public static FirebaseQuery EndAt(this FirebaseQuery query, string range)
        {
            query.FilterParameters.Add("endAt", range);
            return query;
        }

        public static FirebaseQuery EqualTo(this FirebaseQuery query, string range)
        {
            query.FilterParameters.Add("equalTo", range);
            return query;
        }

        public static async Task<FirebaseObject<T>> PostAsync<T>(this FirebaseQuery query, T obj)
        {
            var result = await query.PostAsync(JsonConvert.SerializeObject(obj));
            return new FirebaseObject<T>(result.Key, obj);
        }

        public static async Task<T> PutAsync<T>(this FirebaseQuery query, T obj)
        {
            var result = await query.PutAsync(JsonConvert.SerializeObject(obj));
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<T> PatchAsync<T>(this FirebaseQuery query, T obj)
        {
            var result = await query.PatchAsync(JsonConvert.SerializeObject(obj));
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}