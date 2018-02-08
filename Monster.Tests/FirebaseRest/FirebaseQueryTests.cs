using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseRest;
using FirebaseRest.Extensions;
using FirebaseRest.Models;
using Newtonsoft.Json;
using Xunit;

namespace Monster.Tests.FirebaseRest
{
    public class FirebaseQueryTests : IClassFixture<MockFirebaseFixture>, IDisposable
    {
        public FirebaseQueryTests(MockFirebaseFixture fixture)
        {
            _firebaseClient = new FirebaseClient(FirebaseUrl, FirebaseAuth);
            _firebaseQuery = new FirebaseQuery(_firebaseClient);
            _fixture = fixture;
        }

        public void Dispose()
        {
            RemoveMocks();
        }

        private const string FirebaseUrl = "https://lumachroma.firebaseio.com";
        private const string FirebaseAuth = "x2yPyLti57aYEcKFJMHA4tMd97R7ML3jP6ZHiSs5";
        private const string FirebaseMockNode = "FirebaseRestUnitTest";
        private readonly FirebaseClient _firebaseClient;
        private readonly FirebaseQuery _firebaseQuery;
        private readonly MockFirebaseFixture _fixture;

        private async Task<List<FirebaseObject<MockFirebasePerson>>> CreateMocks()
        {
            var people = _fixture.PersonList;
            var results = new List<FirebaseObject<MockFirebasePerson>>();
            foreach (var person in people)
            {
                var result = await _firebaseQuery.Child(FirebaseMockNode).PostAsync(person);
                results.Add(result);
            }

            return results;
        }

        private async Task<MockFirebasePerson> CreateMock()
        {
            var person = _fixture.PersonList.First();
            var result = await _firebaseQuery.Child(FirebaseMockNode).Child(person.Name).PutAsync(person);
            return result;
        }

        private void RemoveMocks()
        {
            var result = _firebaseQuery.Child(FirebaseMockNode).DeleteAsync();
        }

        [Fact]
        public void FirebaseQueryTest()
        {
            Assert.NotNull(_firebaseClient.GetBaseUrl());
            Assert.NotNull(_firebaseClient.GetDatabaseSecret());
        }

        [Fact]
        public async Task GetAsyncTest()
        {
            var mocks = await CreateMocks();
            Assert.True(mocks.Any(), "Fail to create mocks");
            var results = await _firebaseQuery.Child(FirebaseMockNode).GetAsync<MockFirebasePerson>();

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
        public async Task GetSingleAsyncTest()
        {
            var mock = await CreateMock();
            Assert.True(null != mock, "Fail to create mock");
            var result = await _firebaseQuery.Child(FirebaseMockNode).Child(mock.Name)
                .GetSingleAsync<MockFirebasePerson>();

            Assert.NotNull(result);
            Assert.IsType<MockFirebasePerson>(result);
            Assert.Equal(mock.Name, result.Name);
        }

        [Fact]
        public async Task PostAsyncTest()
        {
            var person = _fixture.PersonList.ElementAt(3);
            var postResult = await _firebaseQuery.Child(FirebaseMockNode).PostAsync(person);
            Assert.NotNull(postResult);
            Assert.IsType<FirebaseObject<MockFirebasePerson>>(postResult);
            Assert.Equal(person.Name, postResult.Object.Name);

            var getResult = await _firebaseQuery.Child(FirebaseMockNode).Child(postResult.Key)
                .GetSingleAsync<MockFirebasePerson>();
            Assert.NotNull(getResult);
            Assert.IsType<MockFirebasePerson>(getResult);
            Assert.Equal(person.Name, getResult.Name);
        }

        [Fact]
        public async Task PutAsyncTest()
        {
            var person = _fixture.PersonList.ElementAt(3);
            var postResult = await _firebaseQuery.Child(FirebaseMockNode).PostAsync(person);
            Assert.NotNull(postResult);

            person.Name = "Johnathan Doe";
            person.Age = 25;
            var putResult = await _firebaseQuery.Child(FirebaseMockNode).Child(postResult.Key).PutAsync(person);
            Assert.NotNull(putResult);
            Assert.IsType<MockFirebasePerson>(putResult);
            Assert.Equal(person.Name, putResult.Name);

            var getResult = await _firebaseQuery.Child(FirebaseMockNode).Child(postResult.Key)
                .GetSingleAsync<MockFirebasePerson>();
            Assert.NotNull(getResult);
            Assert.IsType<MockFirebasePerson>(getResult);
            Assert.Equal(person.Name, getResult.Name);
            Assert.Equal(person.Gender, getResult.Gender);
        }

        [Fact]
        public async Task PatchAsyncTest()
        {
            var person = _fixture.PersonList.ElementAt(3);
            var postResult = await _firebaseQuery.Child(FirebaseMockNode).PostAsync(person);
            Assert.NotNull(postResult);

            const string json = @"{
              'Name': 'Johnathan Doe',
              'Gender': 'Rather not say'
            }";
            var dynamicPerson = JsonConvert.DeserializeObject(json);
            var patchResult = await _firebaseQuery.Child(FirebaseMockNode).Child(postResult.Key)
                .PatchAsync<dynamic>(dynamicPerson);
            Assert.NotNull(patchResult);

            var getResult = await _firebaseQuery.Child(FirebaseMockNode).Child(postResult.Key)
                .GetSingleAsync<MockFirebasePerson>();
            Assert.NotNull(getResult);
            Assert.IsType<MockFirebasePerson>(getResult);
            Assert.Equal("Johnathan Doe", getResult.Name);
            Assert.Equal("Rather not say", getResult.Gender);
            Assert.Equal(person.Age, getResult.Age);
        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            var person = _fixture.PersonList.ElementAt(3);
            var postResult = await _firebaseQuery.Child(FirebaseMockNode).PostAsync(person);
            Assert.NotNull(postResult);

            var getResult = await _firebaseQuery.Child(FirebaseMockNode).Child(postResult.Key)
                .GetSingleAsync<MockFirebasePerson>();
            Assert.NotNull(getResult);
            Assert.IsType<MockFirebasePerson>(getResult);
            Assert.Equal(person.Name, getResult.Name);

            await _firebaseQuery.Child(FirebaseMockNode).Child(postResult.Key).DeleteAsync();

            getResult = await _firebaseQuery.Child(FirebaseMockNode).Child(postResult.Key)
                .GetSingleAsync<MockFirebasePerson>();
            Assert.Null(getResult);
        }
    }
}