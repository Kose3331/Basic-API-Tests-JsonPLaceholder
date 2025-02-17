using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;

namespace API_tests_JsonPlaceholder
{
    [TestFixture]
    public class Tests
    {
        public const string BaseURL = "https://jsonplaceholder.typicode.com/";
        private RestClient client;
        [SetUp]
        public void Setup()
        {
           client = new RestClient(BaseURL);
        }

      

        [Test]
        public void GetAllPosts()
        {
            var request = new RestRequest("/posts",Method.Get);
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code is not 200!");
            Assert.That(response.Content, Is.Not.Empty, "Content is empty!");

            var posts = JArray.Parse(response.Content);

            Assert.That(posts.Count, Is.GreaterThan(1));

            foreach ( var post in posts )
            {
                Assert.That(post["userId"]?.ToString(), Is.Not.Null.And.Not.Empty, "Property userId is null or empty");
                Assert.That(post["id"]?.ToString(), Is.Not.Null.And.Not.Empty, "Property id is null or empty");
                Assert.That(post["title"]?.ToString(), Is.Not.Null.And.Not.Empty, "Property title is null or empty");
                Assert.That(post["body"]?.ToString(), Is.Not.Null.And.Not.Empty, "Property body is null or empty");
            }
        }

        [Test]

        public void GetSpecificPost()
        {
            var postNumber = 2;
            var request = new RestRequest($"/posts/{postNumber}", Method.Get);
            var response =client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code is not 200!");
            Assert.That(response.Content, Is.Not.Empty, "Content is empty!");

            var post = JObject.Parse(response.Content);

            Assert.That(post["id"]?.ToString(), Is.EqualTo("2"), "Property id is not correct!");

        }

        [Test]

        public void CreateAPost()
        {
            var postUserID = 379;
            var postID = 5;
            var postTitle = "testTitle";
            var postBody = "Trying out JsonPlaceholder";

            var createRequest = new RestRequest("/posts", Method.Post);
            createRequest.AddUrlSegment("id", postID);
            createRequest.AddJsonBody(new
            {
                userId = postUserID,
                title = postTitle,
                body = postBody
            }

            );
            var createResponse = client.Execute(createRequest);

            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Status code should be ok!");
            Assert.That(createResponse.Content, Is.Not.Null.Or.Empty, "Content is empty!");

            var post = JObject.Parse(createResponse.Content);
            Assert.Multiple(() =>
            {
              
                Assert.That(post["userId"]?.Value<int>(), Is.EqualTo(postUserID), "Post userId is not correct!");
                Assert.That(post["title"]?.ToString(), Is.EqualTo(postTitle), "Post title is not correct!");
                Assert.That(post["body"]?.ToString(), Is.EqualTo(postBody), "Post body is not correct!");
            });
          
        }

        [Test]

        public void UpdateAPost()
        {
            var updatedTitle = "Edited Title";
            var postToUpdate = 1;

            var updateRequest = new RestRequest($"posts/{postToUpdate}", Method.Patch);
            updateRequest.AddJsonBody(new
            {
                title = updatedTitle
            });

            var updateResponse = client.Execute(updateRequest);

            Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response in not successfull!");

            var post = JObject.Parse(updateResponse.Content);

            Assert.That(post["title"]?.ToString(), Is.EqualTo("Edited Title"), "Title doesnt match!");
        }


        [Test]

        public void DeleteAPost()
        {
            var postToDelete = 55;

            var deleteRequest = new RestRequest($"posts/{postToDelete}",Method.Delete);
            var deleteResponse = client.Execute(deleteRequest);

            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response is not successfull!");

        }



    }
}