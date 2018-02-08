using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FirebaseRest.Extensions;
using FirebaseRest.Models;
using Newtonsoft.Json;

namespace FirebaseRest
{
    public class FirebaseQuery
    {
        private static string _firebaseDatabaseSecret;
        private readonly HttpClient _firebaseClient;
        internal readonly Dictionary<string, string> FilterParameters;
        internal readonly Dictionary<string, string> UrlParameters;
        internal readonly List<string> UrlSegments;

        public FirebaseQuery(FirebaseClient firebaseClient)
        {
            _firebaseClient = new HttpClient {BaseAddress = new Uri(firebaseClient.GetBaseUrl())};
            _firebaseDatabaseSecret = firebaseClient.GetDatabaseSecret();
            UrlSegments = new List<string>();
            UrlParameters = new Dictionary<string, string>();
            FilterParameters = new Dictionary<string, string>();
        }

        public async Task<IReadOnlyCollection<FirebaseObject<T>>> GetAsync<T>()
        {
            var urlSegment = BuildUrlSegment();
            return await _firebaseClient.GetObjectCollectionAsync<T>(urlSegment);
        }

        public async Task<T> GetSingleAsync<T>()
        {
            var urlSegment = BuildUrlSegment();
            return await _firebaseClient.GetObjectAsync<T>(urlSegment);
        }

        public async Task<FirebaseObject<string>> PostAsync(string data)
        {
            var path = BuildUrlSegment();
            var sendData = await _firebaseClient.SendAsync(path, data, HttpMethod.Post).ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<PostResult>(sendData);
            return new FirebaseObject<string>(result.Name, data);
        }

        public async Task<string> PutAsync(string data)
        {
            var urlSegment = BuildUrlSegment();
            return await _firebaseClient.SendAsync(urlSegment, data, HttpMethod.Put).ConfigureAwait(false);
        }

        public async Task<string> PatchAsync(string data)
        {
            var urlSegment = BuildUrlSegment();
            return await _firebaseClient.SendAsync(urlSegment, data, new HttpMethod("PATCH")).ConfigureAwait(false);
        }

        public async Task DeleteAsync()
        {
            var urlSegment = BuildUrlSegment();
            await _firebaseClient.RemoveAsync(urlSegment);
        }

        public string BuildUrlSegment()
        {
            var urlSegment = string.Empty;
            foreach (var s in UrlSegments) urlSegment += $"/{s}";
            urlSegment += $".json?auth={_firebaseDatabaseSecret}";
            foreach (var p in UrlParameters) urlSegment += $"&{p.Key}={p.Value}";
            foreach (var f in FilterParameters) urlSegment += $"&{f.Key}={f.Value}";

            UrlSegments.Clear();
            UrlParameters.Clear();
            FilterParameters.Clear();

            return urlSegment;
        }
    }
}