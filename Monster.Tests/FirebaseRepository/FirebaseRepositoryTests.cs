using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseRepository;
using FirebaseRest;
using FirebaseRest.Models;
using Monster.Tests.Mocks;
using Newtonsoft.Json;
using Xunit;

namespace Monster.Tests.FirebaseRepository
{
    public class FirebaseRepositoryTests : IClassFixture<MockFirebaseFixture>, IDisposable
    {
        public FirebaseRepositoryTests(MockFirebaseFixture fixture)
        {
            var firebaseQuery = new FirebaseQuery(new FirebaseClient(FirebaseUrl, FirebaseAuth));
            _firebaseRepository = new FirebaseRepository<MockFirebasePerson>(FirebaseMockNode, firebaseQuery);
            _fixture = fixture;
        }

        public void Dispose()
        {
            RemoveMocks();
        }

        private const string FirebaseUrl = "https://lumachroma.firebaseio.com";
        private const string FirebaseAuth = "x2yPyLti57aYEcKFJMHA4tMd97R7ML3jP6ZHiSs5";
        private const string FirebaseMockNode = "FirebaseRestUnitTest";
        private readonly FirebaseRepository<MockFirebasePerson> _firebaseRepository;
        private readonly MockFirebaseFixture _fixture;

        private async Task<List<FirebaseObject<MockFirebasePerson>>> CreateMocks()
        {
            var people = _fixture.PersonList;
            var results = new List<FirebaseObject<MockFirebasePerson>>();
            foreach (var person in people)
            {
                var result = await _firebaseRepository.PostAsync(person);
                results.Add(result);
            }

            return results;
        }

        private async Task<MockFirebasePerson> CreateMock()
        {
            var person = _fixture.PersonList.First();
            var result = await _firebaseRepository.PutAsync(person);
            return result;
        }

        private void RemoveMocks()
        {
            var result = _firebaseRepository.DeleteAsync();
        }

        [Fact]
        public void FirebaseRepositoryTest()
        {
            Assert.NotNull(_firebaseRepository);
            Assert.Equal(FirebaseMockNode, _firebaseRepository.GetPath());
        }

        [Fact]
        public void GetPath()
        {
            var path = _firebaseRepository.GetPath();
            Assert.Equal(FirebaseMockNode, path);
        }

        [Theory]
        [InlineData("-KsMRk0EMcGH_DI-dMPw", FirebaseMockNode + "/-KsMRk0EMcGH_DI-dMPw")]
        [InlineData("ContactInformation/ContactNumber", FirebaseMockNode + "/ContactInformation/ContactNumber")]
        [InlineData(null, FirebaseMockNode)]
        public void SetPath(string input, string result)
        {
            var path = _firebaseRepository.SetPath(input);
            Assert.Equal(result, path);
            if (null != input)
            {
                path = _firebaseRepository.SetPath();
                Assert.Equal(FirebaseMockNode, path);
            }
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var person = _fixture.PersonList.ElementAt(3);
            var postResult = await _firebaseRepository.PostAsync(person);
            Assert.NotNull(postResult);

            var getResult = await _firebaseRepository.GetByKeyAsync(postResult.Key);
            Assert.NotNull(getResult);
            Assert.IsType<MockFirebasePerson>(getResult);
            Assert.Equal(person.Name, getResult.Name);

            await _firebaseRepository.DeleteAsync(postResult.Key);

            getResult = await _firebaseRepository.GetByKeyAsync(postResult.Key);
            Assert.Null(getResult);
        }

        [Fact]
        public async Task GetAllAsync()
        {
            var mocks = await CreateMocks();
            Assert.True(mocks.Any(), "Fail to create mocks");
            var results = await _firebaseRepository.GetAllAsync();

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
        public async Task GetByKeyAsync()
        {
            var mocks = await CreateMocks();
            Assert.True(mocks.Any(), "Fail to create mocks");
            var mock = mocks.First();
            var result = await _firebaseRepository.GetByKeyAsync(mock.Key);

            Assert.NotNull(result);
            Assert.IsType<MockFirebasePerson>(result);
            Assert.Equal(mock.Object.Name, result.Name);
            Assert.Equal(mock.Object.Gender, result.Gender);
        }

        [Fact]
        public async Task GetSingleAsync()
        {
            var mock = await CreateMock();
            Assert.True(null != mock, "Fail to create mock");
            var result = await _firebaseRepository.GetSingleAysnc();

            Assert.NotNull(result);
            Assert.IsType<MockFirebasePerson>(result);
            Assert.Equal(mock.Name, result.Name);
        }

        [Fact]
        public async Task PostAsync()
        {
            var person = _fixture.PersonList.ElementAt(3);
            var postResult = await _firebaseRepository.PostAsync(person);
            Assert.NotNull(postResult);
            Assert.IsType<FirebaseObject<MockFirebasePerson>>(postResult);
            Assert.Equal(person.Name, postResult.Object.Name);

            var getResult = await _firebaseRepository.GetByKeyAsync(postResult.Key);
            Assert.NotNull(getResult);
            Assert.IsType<MockFirebasePerson>(getResult);
            Assert.Equal(person.Name, getResult.Name);
        }

        [Fact]
        public async Task PutAsync()
        {
            var person = _fixture.PersonList.ElementAt(3);
            var postResult = await _firebaseRepository.PostAsync(person);
            Assert.NotNull(postResult);

            person.Name = "Johnathan Doe";
            person.Age = 25;
            var putResult = await _firebaseRepository.PutAsync(person, postResult.Key);
            Assert.NotNull(putResult);
            Assert.IsType<MockFirebasePerson>(putResult);
            Assert.Equal(person.Name, putResult.Name);

            var getResult = await _firebaseRepository.GetByKeyAsync(postResult.Key);
            Assert.NotNull(getResult);
            Assert.IsType<MockFirebasePerson>(getResult);
            Assert.Equal(person.Name, getResult.Name);
            Assert.Equal(person.Gender, getResult.Gender);
        }

        [Fact]
        public async Task PatchAsync()
        {
            var person = _fixture.PersonList.ElementAt(3);
            var postResult = await _firebaseRepository.PostAsync(person);
            Assert.NotNull(postResult);

            const string json = @"{
              'Name': 'Johnathan Doe',
              'Gender': 'Rather not say'
            }";
            var dynamicPerson = JsonConvert.DeserializeObject(json);
            var patchResult = await _firebaseRepository.PatchAsync(dynamicPerson, postResult.Key);
            Assert.NotNull(patchResult);

            var getResult = await _firebaseRepository.GetByKeyAsync(postResult.Key);
            Assert.NotNull(getResult);
            Assert.IsType<MockFirebasePerson>(getResult);
            Assert.Equal("Johnathan Doe", getResult.Name);
            Assert.Equal("Rather not say", getResult.Gender);
            Assert.Equal(person.Age, getResult.Age);
        }

        [Fact]
        public async Task SearchByKeyValueAsync()
        {
            var mocks = await CreateMocks();
            Assert.True(mocks.Any(), "Fail to create mocks");
            var results = await _firebaseRepository.SearchByKeyValueAsync("\"Gender\"", "\"Male\"");

            Assert.NotEmpty(results);
            Assert.Equal(2, results.Count);
            Assert.IsType<FirebaseObject<MockFirebasePerson>>(results.First());
            Assert.Collection(results, item => Assert.Contains("Johnathan Doe", item.Object.Name),
                item => Assert.DoesNotContain("Johanna Doe", item.Object.Name));
        }
    }
}