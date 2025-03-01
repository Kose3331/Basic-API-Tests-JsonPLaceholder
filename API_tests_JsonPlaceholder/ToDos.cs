using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace API_tests_JsonPlaceholder
{
    [TestFixture]
    public class ToDos
    {
        public const string BaseURL = "https://jsonplaceholder.typicode.com/";
        private RestClient client;

        [SetUp]

        public void SetUp()
        {
            client = new RestClient(BaseURL);
        }

        [Test]
        public void GetAllToDos()
        {
            var request = new RestRequest("/todos", Method.Get);
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code is not successfull!");
            Assert.That(response.Content, Is.Not.Null.Or.Empty, "Response is either null or empty!");

            var todos = JArray.Parse(response.Content);

            foreach ( var todo in todos)
            {
                Assert.That(todo["userId"]?.Value<int>(), Is.GreaterThan(0), "Property userId is null or empty!");
                Assert.That(todo["id"]?.Value<int>(), Is.GreaterThan(0), "Property id is null or empty!");
                Assert.That(todo["title"]?.ToString(), Is.Not.Null.And.Not.Empty, "Property title is null or empty!");
                Assert.That(todo["completed"]?.Value<bool?>(), Is.Not.Null, "Property title is null or empty!");
            }
        }

        [Test]
        public void GetSpecificToDo()
        {
            var numberToDo = 2;
            var titleOfToDo = "quis ut nam facilis et officia qui";
            var statusOfToDo = false;
            var request = new RestRequest($"/todos/{numberToDo}", Method.Get);
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code is not successfull!");
            Assert.That(response.Content, Is.Not.Null.Or.Empty, "Response is either null or empty!");

            var toDo = JToken.Parse(response.Content);

            Assert.That(toDo["title"]?.ToString(), Is.EqualTo(titleOfToDo), "Title of ToDo doesnt match!");
            Assert.That(toDo["completed"]?.Value<bool?>(), Is.EqualTo(statusOfToDo), "Boolean value doesnt match!");


        }

        [Test]

        public void CreateToDo()
        {
            var crUserId = 3;
            var crId = 201;
            var crTitle = "I wish to try out this this code";
            var crCompleted = false;

            var request = new RestRequest("/todos", Method.Post);
            request.AddJsonBody( new
            {
                userId = crUserId,
                id = crId,
                title = crTitle,
                completed = crCompleted

            });
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Status code is not successfull!");
            Assert.That(response.Content, Is.Not.Null.Or.Empty, "Response is either null or empty!");

            var createdToDo = JToken.Parse(response.Content);

            Assert.That(createdToDo["userId"]?.Value<int>(), Is.EqualTo(crUserId), "Property userId doesnt match!");
            Assert.That(createdToDo["id"]?.Value<int>(), Is.EqualTo(crId), "Property id doesnt match!");
            Assert.That(createdToDo["title"]?.ToString(), Is.EqualTo(crTitle), "Property title doesnt match!");
            Assert.That(createdToDo["completed"]?.Value<bool?>(), Is.EqualTo(crCompleted), "Property completed doesnt match!");

        }

        [Test]

        public void UpdateToDo()
        {
            var updatedTitle = "My name is edited title now!";
            var updatedNumber = 2;

            var request = new RestRequest($"/todos/{updatedNumber}", Method.Patch);
            request.AddJsonBody(new
            {
                title = updatedTitle
            });
            var response = client.Execute(request);


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code is not successfull!");
            Assert.That(response.Content, Is.Not.Null.Or.Empty, "Response is either null or empty!");

            var updatedToDo = JToken.Parse(response.Content);

            Assert.That(updatedToDo["title"]?.ToString(), Is.EqualTo(updatedTitle), "Title of ToDo doesnt match!");
            

        }

        [Test]
        public void DeleteTodo()
        {
            var toDoToBeDeleted = 2;

            var request = new RestRequest($"/todos/{toDoToBeDeleted}", Method.Delete);
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code is not successfull!");
            
        }
    }
}
