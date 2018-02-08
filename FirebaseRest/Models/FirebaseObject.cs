namespace FirebaseRest.Models
{
    public class FirebaseObject<T>
    {
        public FirebaseObject()
        {
        }

        public FirebaseObject(string key, T obj)
        {
            Key = key;
            Object = obj;
        }

        public string Key { get; }
        public T Object { get; }
    }
}