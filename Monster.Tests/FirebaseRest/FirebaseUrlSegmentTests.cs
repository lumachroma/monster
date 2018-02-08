using FirebaseRest;
using FirebaseRest.Extensions;
using FirebaseRest.Models;
using Xunit;

namespace Monster.Tests.FirebaseRest
{
    public class FirebaseUrlSegmentTests
    {
        public FirebaseUrlSegmentTests()
        {
            _firebaseClient = new FirebaseClient(FirebaseUrl, FirebaseAuth);
            _firebaseQuery = new FirebaseQuery(_firebaseClient);
        }

        private const string FirebaseUrl = "https://lumachroma.firebaseio.com";
        private const string FirebaseAuth = "x2yPyLti57aYEcKFJMHA4tMd97R7ML3jP6ZHiSs5";
        private readonly FirebaseClient _firebaseClient;
        private readonly FirebaseQuery _firebaseQuery;

        [Fact]
        public void FirebaseUrlSegmentTest()
        {
            Assert.NotNull(_firebaseClient.GetBaseUrl());
            Assert.NotNull(_firebaseClient.GetDatabaseSecret());
        }

        [Fact]
        public void BuildUrlSegmentTest()
        {
            var url = _firebaseQuery.BuildUrlSegment();
            Assert.Contains(".json", url);
            Assert.Contains(FirebaseAuth, url);
        }

        [Theory]
        [InlineData("child", "JungleBook", "/JungleBook")]
        [InlineData("shallow", "true", "shallow=true")]
        [InlineData("print", "pretty", "print=pretty")]
        [InlineData("limit-to-first", "5", "limitToFirst=5")]
        [InlineData("limit-to-last", "3", "limitToLast=3")]
        [InlineData("start-at", "1", "startAt=1")]
        [InlineData("end-at", "9", "endAt=9")]
        [InlineData("equal-to", "\"John Doe\"", "equalTo=\"John Doe\"")]
        [InlineData("order-by", "\"Jane Doe\"", "orderBy=\"Jane Doe\"")]
        [InlineData("order-by-key", "", "orderBy=\"$key\"")]
        [InlineData("order-by-value", "", "orderBy=\"$value\"")]
        public void BuildUrlSegmentsTest(string type, string param, string path)
        {
            var url = string.Empty;
            switch (type)
            {
                case "child":
                    url = _firebaseQuery.Child(param).BuildUrlSegment();
                    break;
                case "shallow":
                    url = _firebaseQuery.Shallow(param).BuildUrlSegment();
                    break;
                case "print":
                    url = _firebaseQuery.Print(param).BuildUrlSegment();
                    break;
                case "limit-to-first":
                    url = _firebaseQuery.LimitToFirst(param).BuildUrlSegment();
                    break;
                case "limit-to-last":
                    url = _firebaseQuery.LimitToLast(param).BuildUrlSegment();
                    break;
                case "start-at":
                    url = _firebaseQuery.StartAt(param).BuildUrlSegment();
                    break;
                case "end-at":
                    url = _firebaseQuery.EndAt(param).BuildUrlSegment();
                    break;
                case "equal-to":
                    url = _firebaseQuery.EqualTo(param).BuildUrlSegment();
                    break;
                case "order-by":
                    url = _firebaseQuery.OrderBy(param).BuildUrlSegment();
                    break;
                case "order-by-key":
                    url = _firebaseQuery.OrderByKey().BuildUrlSegment();
                    break;
                case "order-by-value":
                    url = _firebaseQuery.OrderByValue().BuildUrlSegment();
                    break;
                default:
                    Assert.True(false, "Not Called");
                    break;
            }

            Assert.Contains(path, url);
        }

        [Theory]
        [InlineData("multiple-child", "JungleBook", "Animal", "", "/JungleBook/Animal")]
        [InlineData("shallow-print", "true", "pretty", "", "shallow=true&print=pretty")]
        [InlineData("paging1", "\"Animal\"", "150", "200", "orderBy=\"Animal\"&startAt=150&limitToLast=200")]
        [InlineData("paging2", "50", "\"-KuF8zKqt6Sr0moA-4ov\"", "",
            "orderBy=\"$key\"&limitToLast=50&endAt=\"-KuF8zKqt6Sr0moA-4ov\"")]
        [InlineData("paging3", "50", "\"-KsMRjFvaTZpnQz-a5IE\"", "",
            "orderBy=\"$key\"&limitToFirst=50&startAt=\"-KsMRjFvaTZpnQz-a5IE\"")]
        [InlineData("search", "\"-KsMRjFvaTZpnQz-a5IE\"", "", "", "orderBy=\"$key\"&equalTo=\"-KsMRjFvaTZpnQz-a5IE\"")]
        public void BuildUrlSegmentsComplexTest(string type, string param1, string param2, string param3, string path)
        {
            var url = string.Empty;
            switch (type)
            {
                case "multiple-child":
                    url = _firebaseQuery.Child(param1).Child(param2).BuildUrlSegment();
                    break;
                case "shallow-print":
                    url = _firebaseQuery.Shallow(param1).Print(param2).BuildUrlSegment();
                    break;
                case "paging1":
                    url = _firebaseQuery.OrderBy(param1).StartAt(param2).LimitToLast(param3).BuildUrlSegment();
                    break;
                case "paging2":
                    url = _firebaseQuery.OrderByKey().LimitToLast(param1).EndAt(param2).BuildUrlSegment();
                    break;
                case "paging3":
                    url = _firebaseQuery.OrderByKey().LimitToFirst(param1).StartAt(param2).BuildUrlSegment();
                    break;
                case "search":
                    url = _firebaseQuery.OrderByKey().EqualTo(param1).BuildUrlSegment();
                    break;
                default:
                    Assert.True(false, "Not Called");
                    break;
            }

            Assert.Contains(path, url);
        }
    }
}