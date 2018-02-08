namespace FirebaseRest.Models
{
    public class FirebaseClient
    {
        public FirebaseClient(string baseUrl, string databaseSecret)
        {
            BaseUrl = baseUrl;
            DatabaseSecret = databaseSecret;
        }

        private string BaseUrl { get; }
        private string DatabaseSecret { get; }

        public string GetBaseUrl()
        {
            return BaseUrl;
        }

        public string GetDatabaseSecret()
        {
            return DatabaseSecret;
        }
    }
}