using System;
using System.Linq;
using System.Threading.Tasks;
using FirebaseRest;
using FirebaseRest.Extensions;
using FirebaseRest.Models;
using Newtonsoft.Json;
using Sandbox.Models.AddressBook;

namespace Sandbox
{
    public static class FirebaseRest
    {
        public static async Task RunAsync()
        {
            const string url = "https://lumachroma.firebaseio.com";
            const string auth = "x2yPyLti57aYEcKFJMHA4tMd97R7ML3jP6ZHiSs5";
            var fc = new FirebaseClient(url, auth);
            var fq = new FirebaseQuery(fc);

            await GetAsync(fq);
            await GetSingleAsync(fq);
            await GetShallowAsync(fq);
            var postResult = await PostAsync(fq);
            var itemKey = postResult.Key;
            Console.ReadLine();
            var putResult = await PutAsync(fq, itemKey);
            Console.ReadLine();
            var patchResult = await PatchAsync(fq, itemKey);
            Console.ReadLine();
            await DeleteAsync(fq, itemKey);
        }

        private static async Task GetAsync(FirebaseQuery fq)
        {
            var results = await fq.Child("AddressBook").GetAsync<AddressBook>();
            if (results.Any())
            {
                var addressbooks = results.Take(5);
                foreach (var addressbook in addressbooks)
                {
                    Console.WriteLine($"Key: {addressbook.Key}");
                    PrintAddressBook(addressbook.Object);
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Nothing");
            }
        }

        private static async Task GetShallowAsync(FirebaseQuery fq)
        {
            var results = await fq.Child("AddressBook").Shallow("true").Print("pretty").GetAsync<bool>();
            if (results.Any())
                foreach (var result in results)
                    Console.WriteLine($"Key: {result.Key} Value: {result.Object}");
            else
                Console.WriteLine("Nothing");
            Console.WriteLine();
        }

        private static async Task GetSingleAsync(FirebaseQuery fq)
        {
            const string itemKey = "-KsMRp__P7M3wGBHK6uz";
            var result = await fq.Child("AddressBook").Child(itemKey).GetSingleAsync<AddressBook>();
            if (null != result)
            {
                Console.WriteLine($"Key: {itemKey}");
                PrintAddressBook(result);
            }
            else
            {
                Console.WriteLine("Nothing");
            }

            Console.WriteLine();
        }

        private static async Task<FirebaseObject<Person>> PostAsync(FirebaseQuery fq)
        {
            var person = new Person
            {
                Name = "John",
                Age = 12,
                Gender = "Male"
            };
            var postResult = await fq.Child("MockData").PostAsync(person);
            Console.WriteLine($"Posted: {postResult.Key}");
            Console.WriteLine($"Name: {postResult.Object.Name}");
            Console.WriteLine($"Age: {postResult.Object.Age}");
            Console.WriteLine($"Gender: {postResult.Object.Gender}");
            Console.WriteLine();
            return postResult;
        }

        private static async Task<Person> PutAsync(FirebaseQuery fq, string key)
        {
            var person = new Person
            {
                Name = "John Doe",
                Age = 12,
                Gender = "Male"
            };
            var putResult = await fq.Child("MockData").Child(key).PutAsync(person);
            Console.WriteLine($"Put: {key}");
            Console.WriteLine($"Name: {putResult.Name}");
            Console.WriteLine($"Age: {putResult.Age}");
            Console.WriteLine($"Gender: {putResult.Gender}");
            Console.WriteLine();
            return putResult;
        }

        private static async Task<dynamic> PatchAsync(FirebaseQuery fq, string key)
        {
            const string json = @"{
              'Name': 'Jean Doe',
              'Gender': 'Rather not say'
            }";
            var dynamicPerson = JsonConvert.DeserializeObject(json);
            var patchResult = await fq.Child("MockData").Child(key).PatchAsync<dynamic>(dynamicPerson);
            Console.WriteLine($"Patched: {key}");
            Console.WriteLine($"Name: {patchResult.Name}");
            Console.WriteLine($"Gender: {patchResult.Gender}");
            Console.WriteLine();
            return patchResult;
        }

        private static async Task DeleteAsync(FirebaseQuery fq, string key)
        {
            await fq.Child("MockData").Child(key).DeleteAsync();
            Console.WriteLine($"Deleted: {key}");
        }

        private static void PrintAddressBook(AddressBook addressbook)
        {
            Console.WriteLine($"Contact Person: {addressbook.ContactPerson}");
            Console.WriteLine($"Company Name: {addressbook.CompanyName}");
            Console.WriteLine($"Groups: {JsonConvert.SerializeObject(addressbook.Groups)}");
            Console.WriteLine($"Email: {addressbook.ContactInformation.Email}");
            Console.WriteLine($"ContactNumber: {addressbook.ContactInformation.ContactNumber}");
            Console.WriteLine($"AlternativeContactNumber: {addressbook.ContactInformation.AlternativeContactNumber}");
            Console.WriteLine($"Address1: {addressbook.Address.Address1}");
            Console.WriteLine($"Address2: {addressbook.Address.Address2}");
            Console.WriteLine($"Address3: {addressbook.Address.Address3}");
            Console.WriteLine($"Address4: {addressbook.Address.Address4}");
            Console.WriteLine($"City: {addressbook.Address.City}");
            Console.WriteLine($"State: {addressbook.Address.State}");
            Console.WriteLine($"Country: {addressbook.Address.Country}");
            Console.WriteLine($"ProfilePictureUrl: {addressbook.ProfilePictureUrl}");
            Console.WriteLine($"Company UserId: {addressbook.UserId}");
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }
}