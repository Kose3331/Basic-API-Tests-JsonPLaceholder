using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API_tests_JsonPlaceholder
{
    [TestFixture]
    public class CommentsTests
    {
        public const string BaseURL = "https://jsonplaceholder.typicode.com/";
        private RestClient client;
        [SetUp]

        public void SetUp()
        {
            client = new RestClient(BaseURL);
        }

        [Test]

        public void GetAllComments()
        {
            var request = new RestRequest("/comments", Method.Get);
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code is not OK!");
            Assert.That(response.Content, Is.Not.Null.Or.Empty, "Content is null or empty");

            var comments = JArray.Parse(response.Content);

            Assert.Multiple(() =>
            {
                foreach (var comment in comments)
                {

                    Assert.That(comment["postId"]?.Value<int>(), Is.GreaterThan(0), "Property postID is null or empty!");
                    Assert.That(comment["id"]?.Value<int>(), Is.GreaterThan(0), "Property id is null or empty!");
                    Assert.That(comment["name"]?.ToString(), Is.Not.Null.Or.Empty, "Property name is null or empty!");
                    Assert.That(comment["email"]?.ToString(), Is.Not.Null.Or.Empty, "Property email is null or empty!");
                    Assert.That(comment["body"]?.ToString(), Is.Not.Null.Or.Empty, "Property body is null or empty!");
                }
            });

        }

        [Test]

        public void GetSpecificComment()
        {
            var specificNumber = 2;
            var request = new RestRequest($"/comments/{specificNumber}", Method.Get);
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response is not ok!");
            var comment = JToken.Parse(response.Content);

            Assert.That(comment["email"]?.ToString(), Is.EqualTo("Jayne_Kuhic@sydney.com"), "Property Email doesnt match!");
        }

        [Test]

        public void CreateAComment()
        {
            var newPostID = 24;
            var newID = 501;
            var newName = "Peacefull comment";
            var newEmail = "test@gmail.com";
            var newBody = "Adding some substance to the body - test";

            var request = new RestRequest("/comments", Method.Post);
            request.AddJsonBody(new 
            {
                postId = newPostID,
                id = newID,
                name = newName,
                email = newEmail,
                body = newBody
            });

            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Comment is not created!");

            var createdComment = JToken.Parse(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(createdComment["postId"]?.Value<int>(), Is.EqualTo(newPostID), "Property postID is null or empty!");
                Assert.That(createdComment["id"]?.Value<int>(), Is.EqualTo(newID), "Property id is null or empty!");
                Assert.That(createdComment["name"]?.ToString(), Is.EqualTo(newName), "Property name is null or empty!");
                Assert.That(createdComment["email"]?.ToString(), Is.EqualTo(newEmail), "Property email is null or empty!");
                Assert.That(createdComment["body"]?.ToString(), Is.EqualTo(newBody), "Property body is null or empty!");
            });

        }

        [Test]

        public void UpdateAComment()
        {
            var updatedBody = "Edited text for testing purpose";
            var specificComment = 2;

            var updateRequest =new RestRequest($"/comments/{specificComment}", Method.Patch);
            updateRequest.AddJsonBody(new
            {
                body = updatedBody
            });

            var response = client.Execute(updateRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Comment is not updated!");

            var updatedComment = JToken.Parse(response.Content);

            Assert.That(updatedComment["body"]?.ToString(), Is.EqualTo(updatedBody), "Comment is not updated properly with new text");
        }

        [Test]
        public void DeleteAComment()
        {
            var specificComment = 2;

            var request = new RestRequest($"/comments/{specificComment}", Method.Delete);
            var response = client.Execute(request); 

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Comment is not deleted successfully!");
        }
    }
}
