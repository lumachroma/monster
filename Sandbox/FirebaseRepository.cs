using System;
using System.Linq;
using System.Threading.Tasks;
using FirebaseRepository;
using FirebaseRest;
using FirebaseRest.Models;
using Newtonsoft.Json;
using Sandbox.Models.AddressBook;
using Sandbox.Models.LocalUser;

namespace Sandbox
{
    public static class FirebaseRepository
    {
        public static async Task RunAsync()
        {
            const string url = "https://lumachroma.firebaseio.com";
            const string auth = "x2yPyLti57aYEcKFJMHA4tMd97R7ML3jP6ZHiSs5";
            var firebaseQuery = new FirebaseQuery(new FirebaseClient(url, auth));
            var addressBookRepo = new FirebaseRepository<AddressBook>("AddressBook", firebaseQuery);
            var localUserRepo = new FirebaseRepository<LocalUser>("LocalUser", firebaseQuery);

            await GetAllAddresssBookAsync(addressBookRepo);
            await GetAllLocalUserAsync(localUserRepo);
            await GetAddressBookByKeyAsync(addressBookRepo);
            await GetLocalUserByKeyAsync(localUserRepo);
            await SearchAddressBookByKeyValueAsync(addressBookRepo);
            await SearchLocalUserByKeyValueAsync(localUserRepo);
        }

        private static async Task SearchLocalUserByKeyValueAsync(FirebaseRepository<LocalUser> localUserRepo)
        {
            if (localUserRepo == null) throw new ArgumentNullException(nameof(localUserRepo));
            Console.WriteLine("Search LocalUser");
            Console.WriteLine("Key:");
            var searchLocalUserKey = Console.ReadLine();
            Console.WriteLine("Value:");
            var searchLocalUserValue = Console.ReadLine();
            if (null != searchLocalUserKey && null != searchLocalUserValue)
            {
                var searchedUsers =
                    await localUserRepo.SearchByKeyValueAsync($"\"{searchLocalUserKey}\"",
                        $"\"{searchLocalUserValue}\"");
                if (searchedUsers.Any())
                    foreach (var user in searchedUsers)
                    {
                        Console.WriteLine($"Key: {user.Key}");
                        PrintLocalUser(user.Object);
                        Console.WriteLine();
                    }
                else Console.WriteLine("No Item Found!");
            }
            else
            {
                Console.WriteLine("Invalid Key!");
            }
        }

        private static async Task SearchAddressBookByKeyValueAsync(FirebaseRepository<AddressBook> addressBookRepo)
        {
            if (addressBookRepo == null) throw new ArgumentNullException(nameof(addressBookRepo));
            Console.WriteLine("Search AddressBook");
            Console.WriteLine("Key:");
            var searchAddressBookKey = Console.ReadLine();
            Console.WriteLine("Value:");
            var searchAddressBookValue = Console.ReadLine();
            if (null != searchAddressBookKey && null != searchAddressBookValue)
            {
                var searchedAddresses =
                    await addressBookRepo.SearchByKeyValueAsync($"\"{searchAddressBookKey}\"",
                        $"\"{searchAddressBookValue}\"");
                if (searchedAddresses.Any())
                    foreach (var address in searchedAddresses)
                    {
                        Console.WriteLine($"Key: {address.Key}");
                        PrintAddressBook(address.Object);
                        Console.WriteLine();
                    }
                else Console.WriteLine("No Item Found!");
            }
            else
            {
                Console.WriteLine("Invalid Key!");
            }
        }

        private static async Task GetLocalUserByKeyAsync(FirebaseRepository<LocalUser> localUserRepo)
        {
            if (localUserRepo == null) throw new ArgumentNullException(nameof(localUserRepo));
            Console.WriteLine("Get LocalUser");
            var localuserKey = Console.ReadLine();
            if (null != localuserKey)
            {
                var result = await localUserRepo.GetByKeyAsync(localuserKey);
                if (null != result) PrintLocalUser(result);
                else Console.WriteLine("Item Not Found!");
            }
            else
            {
                Console.WriteLine("Invalid Key!");
            }

            Console.WriteLine();
        }

        private static async Task GetAddressBookByKeyAsync(FirebaseRepository<AddressBook> addressBookRepo)
        {
            if (addressBookRepo == null) throw new ArgumentNullException(nameof(addressBookRepo));
            Console.WriteLine("Get AddressBook");
            var addressbookKey = Console.ReadLine();
            if (null != addressbookKey)
            {
                var result = await addressBookRepo.GetByKeyAsync(addressbookKey);
                if (null != result) PrintAddressBook(result);
                else Console.WriteLine("Item Not Found!");
            }
            else
            {
                Console.WriteLine("Invalid Key!");
            }

            Console.WriteLine();
        }

        private static async Task GetAllLocalUserAsync(FirebaseRepository<LocalUser> localUserRepo)
        {
            if (localUserRepo == null) throw new ArgumentNullException(nameof(localUserRepo));
            var users = await localUserRepo.GetAllAsync();
            Console.WriteLine("Users");
            foreach (var user in users.Take(5))
            {
                Console.WriteLine($"Key: {user.Key}");
                PrintLocalUser(user.Object);
                Console.WriteLine();
            }
        }

        private static async Task GetAllAddresssBookAsync(FirebaseRepository<AddressBook> addressBookRepo)
        {
            if (addressBookRepo == null) throw new ArgumentNullException(nameof(addressBookRepo));
            var addresses = await addressBookRepo.GetAllAsync();
            Console.WriteLine("Addresses");
            foreach (var address in addresses.Take(5))
            {
                Console.WriteLine($"Key: {address.Key}");
                PrintAddressBook(address.Object);
                Console.WriteLine();
            }
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

        private static void PrintLocalUser(LocalUser localuser)
        {
            Console.WriteLine($"Email: {localuser.Email}");
            Console.WriteLine($"Username: {localuser.Username}");
            Console.WriteLine($"Password: {localuser.Password}");
            Console.WriteLine($"FirstName: {localuser.FirstName}");
            Console.WriteLine($"LastName: {localuser.LastName}");
            Console.WriteLine($"DateOfBirth: {localuser.DateOfBirth}");
            Console.WriteLine($"Location: {localuser.Location}");
            Console.WriteLine($"Roles: {JsonConvert.SerializeObject(localuser.Roles)}");
        }
    }
}