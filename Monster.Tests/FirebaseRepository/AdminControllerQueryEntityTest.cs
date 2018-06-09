using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseRest;
using FirebaseRest.Extensions;
using FirebaseRest.Models;
using Monster.Firebase;
using Monster.Tests.Mocks;
using Xunit;

namespace Monster.Tests.FirebaseRepository
{
    public class AdminControllerQueryEntityTest : IClassFixture<MockFirebaseFixture>, IDisposable
    {
        public AdminControllerQueryEntityTest(MockFirebaseFixture fixture)
        {
            _firebaseQuery = new FirebaseQuery(new FirebaseClient(FirebaseUrl, FirebaseAuth));
            var firebaseDataContext = new FirebaseDataContext<MockFirebasePerson>(FirebaseMockNode, _firebaseQuery);
            _adminControllerQueryEntity = new AdminControllerQueryEntity<MockFirebasePerson>(firebaseDataContext);
            _fixture = fixture;
        }

        public void Dispose()
        {
            RemoveMocks();
        }

        private const string FirebaseUrl = "https://swag-monster.firebaseio.com";
        private const string FirebaseAuth = "MfX7DaAWOUjr0zJ2invYbaX6UceHZ3vrif0VGeL4";
        private const string FirebaseMockNode = "MockFirebasePeople";
        private readonly FirebaseQuery _firebaseQuery;
        private readonly AdminControllerQueryEntity<MockFirebasePerson> _adminControllerQueryEntity;
        private readonly MockFirebaseFixture _fixture;

        private async Task<List<FirebaseObject<MockFirebasePerson>>> CreateMocks()
        {
            var people = _fixture.PersonList;
            var results = new List<FirebaseObject<MockFirebasePerson>>();
            foreach (var person in people)
            {
                var result = await _adminControllerQueryEntity.Post(person);
                results.Add(result);
            }

            return results;
        }

        private async Task<FirebaseObject<MockFirebasePerson>> CreateMock(int index)
        {
            var person = _fixture.PersonList.ElementAt(index);
            var result = await _adminControllerQueryEntity.Post(person);
            return result;
        }

        private void RemoveMocks()
        {
            var result = _firebaseQuery.Child(FirebaseMockNode).DeleteAsync();
        }

        [Fact]
        public async void All()
        {
            var mocks = await CreateMocks();
            Assert.True(mocks.Any(), "Fail to create mocks");

            var results = await _adminControllerQueryEntity.All();
            Assert.NotEmpty(results);
            Assert.Equal(mocks.Count, results.Count);
            Assert.IsType<FirebaseObject<MockFirebasePerson>>(results.First());
            Assert.Equal(mocks.First().Object.Name, results.First().Object.Name);
            Assert.Equal(mocks.Last().Object.Name, results.Last().Object.Name);
            Assert.Collection(results, item => Assert.Contains("Johnathan Doe", item.Object.Name),
                item => Assert.Contains("Johanna Doe", item.Object.Name),
                item => Assert.Contains("John Clayton", item.Object.Name),
                item => Assert.Contains("Jane Porter", item.Object.Name));
        }

        [Fact]
        public async void NewestOldest()
        {
            var mocks = await CreateMocks();
            Assert.True(mocks.Any(), "Fail to create mocks");

            var newestResults = await _adminControllerQueryEntity.Newest("2");
            Assert.NotEmpty(newestResults);
            Assert.Equal(2, newestResults.Count);
            Assert.IsType<FirebaseObject<MockFirebasePerson>>(newestResults.First());
            Assert.Equal("Jane Porter", newestResults.ToList().ElementAt(0).Object.Name);
            Assert.Equal("John Clayton", newestResults.ToList().ElementAt(1).Object.Name);

            var oldestResults = await _adminControllerQueryEntity.Oldest("2");
            Assert.NotEmpty(oldestResults);
            Assert.Equal(2, oldestResults.Count);
            Assert.IsType<FirebaseObject<MockFirebasePerson>>(oldestResults.First());
            Assert.Equal("Johnathan Doe", oldestResults.ToList().ElementAt(0).Object.Name);
            Assert.Equal("Johanna Doe", oldestResults.ToList().ElementAt(1).Object.Name);
        }

        [Fact]
        public async void CreateReadUpdateDelete()
        {
            //Post
            var person = _fixture.PersonList.ElementAt(3);
            var postResult = await _adminControllerQueryEntity.Post(person);
            Assert.NotNull(postResult);
            Assert.IsType<FirebaseObject<MockFirebasePerson>>(postResult);
            Assert.Equal(person.Name, postResult.Object.Name);

            //Get
            var getResult = await _adminControllerQueryEntity.Get(postResult.Key);
            Assert.NotNull(getResult);
            Assert.IsType<MockFirebasePerson>(getResult);
            Assert.Equal(person.Name, getResult.Name);

            //Put
            person.Name = "Johnathan Doe";
            person.Age = 25;
            var putResult = await _adminControllerQueryEntity.Put(postResult.Key, person);
            Assert.NotNull(putResult);
            Assert.IsType<MockFirebasePerson>(putResult);

            getResult = await _adminControllerQueryEntity.Get(postResult.Key);
            Assert.NotNull(getResult);
            Assert.IsType<MockFirebasePerson>(getResult);
            Assert.Equal(person.Name, getResult.Name);
            Assert.Equal(person.Age, getResult.Age);
            Assert.Equal(person.Gender, getResult.Gender);

            //Delete
            await _adminControllerQueryEntity.Delete(postResult.Key);
            getResult = await _adminControllerQueryEntity.Get(postResult.Key);
            Assert.Null(getResult);
        }

        [Fact]
        public void GetCurrentUserWhithoutAuthentication()
        {
            var user = _adminControllerQueryEntity.GetCurrentUser();
            Assert.True(user == "Anonymous");
        }

        [Fact]
        public void GetCurrentUserWhithAuthentication()
        {
            Assert.False(false, "Test CreatedBy current user with auhtentication not implemented!");
        }

        [Fact]
        public void CheckCreator()
        {
            Assert.False(false, "Test CreatedBy not implemented!");
        }

        [Fact]
        public void CheckEditor()
        {
            Assert.False(false, "Test ChangedBy not implemented!");
        }
    }
}