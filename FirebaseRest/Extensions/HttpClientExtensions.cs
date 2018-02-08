using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FirebaseRest.Models;
using Newtonsoft.Json;

namespace FirebaseRest.Extensions
{
    internal static class HttpClientExtensions
    {
        public static async Task<IReadOnlyCollection<FirebaseObject<T>>> GetObjectCollectionAsync<T>(
            this HttpClient client, string requestUri)
        {
            var responseData = string.Empty;
            var statusCode = HttpStatusCode.OK;

            try
            {
                var response = await client.GetAsync(requestUri).ConfigureAwait(false);
                statusCode = response.StatusCode;
                responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, T>>(responseData);

                if (dictionary == null) return new FirebaseObject<T>[0];

                return dictionary.Select(item => new FirebaseObject<T>(item.Key, item.Value)).ToList();
            }
            catch (Exception ex)
            {
                throw new FirebaseException(requestUri, string.Empty, responseData, statusCode, ex);
            }
        }

        public static async Task<T> GetObjectAsync<T>(this HttpClient client, string requestUri)
        {
            var responseData = string.Empty;
            var statusCode = HttpStatusCode.OK;

            try
            {
                var response = await client.GetAsync(requestUri).ConfigureAwait(false);
                statusCode = response.StatusCode;
                responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<T>(responseData);
            }
            catch (Exception ex)
            {
                throw new FirebaseException(requestUri, string.Empty, responseData, statusCode, ex);
            }
        }

        public static async Task<string> SendAsync(this HttpClient client, string requestUri, string requestData,
            HttpMethod requestMethod)
        {
            var responseData = string.Empty;
            var statusCode = HttpStatusCode.OK;
            var message = new HttpRequestMessage(requestMethod, requestUri)
            {
                Content = new StringContent(requestData)
            };

            try
            {
                var result = await client.SendAsync(message).ConfigureAwait(false);
                statusCode = result.StatusCode;
                responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                result.EnsureSuccessStatusCode();

                return responseData;
            }
            catch (Exception ex)
            {
                throw new FirebaseException(requestUri, requestData, responseData, statusCode, ex);
            }
        }

        public static async Task RemoveAsync(this HttpClient client, string requestUri)
        {
            var responseData = string.Empty;
            var statusCode = HttpStatusCode.OK;

            try
            {
                var result = await client.DeleteAsync(requestUri).ConfigureAwait(false);
                statusCode = result.StatusCode;
                responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new FirebaseException(requestUri, string.Empty, responseData, statusCode, ex);
            }
        }
    }
}