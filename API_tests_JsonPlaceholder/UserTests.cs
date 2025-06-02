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
    public class UserTests
    {
        public const string BaseURL = "https://jsonplaceholder.typicode.com/";
        private RestClient client;

        [SetUp]
        public void SetUp()
        {
            client = new RestClient(BaseURL);
        }

        [Test]
        public void GetAllUsers()
        {
            var request = new RestRequest("users/", Method.Get);
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response status code is not successfull");
            Assert.That(response.Content, Is.Not.Null.Or.Empty, "Content is null or empty");

            var users = JArray.Parse(response.Content);

            foreach (var user in users)
            {
                Assert.That(user["id"]?.Value<int>(), Is.GreaterThan(0), "Property id is bellow zero!");
                Assert.That(user["name"]?.ToString(), Is.Not.Null.Or.Empty, "Property name is null or empty!");
                Assert.That(user["username"]?.ToString(), Is.Not.Null.Or.Empty, "Property username is null or empty!");
                Assert.That(user["email"]?.ToString(), Is.Not.Null.Or.Empty, "Property email is null or empty!");

                var address = user["address"];
                Assert.That(address, Is.Not.Null.Or.Empty, "User adress is null or empty!");

                Assert.That(address["street"]?.ToString(), Is.Not.Null.Or.Empty, "Property street is bellow zero!");
                Assert.That(address["suite"]?.ToString(), Is.Not.Null.Or.Empty, "Property suite is null or empty!");
                Assert.That(address["city"]?.ToString(), Is.Not.Null.Or.Empty, "Property city is null or empty!");
                Assert.That(address["zipcode"]?.ToString(), Is.Not.Null.Or.Empty, "Property zipcode is null or empty!");

                var geo = address["geo"];
                Assert.That(geo, Is.Not.Null.Or.Empty, "User geo is null or empty!");
                Assert.That(geo["lat"]?.ToString(), Is.Not.Null.Or.Empty);

                Assert.That(user["phone"]?.ToString(), Is.Not.Null.Or.Empty, "Property phone is null or empty!");
                Assert.That(user["website"]?.ToString(), Is.Not.Null.Or.Empty, "Property website is null or empty!");

                var company = user["company"];
                Assert.That(company, Is.Not.Null.Or.Empty, "User company is null or empty!");

                Assert.That(company["name"]?.ToString(), Is.Not.Null.Or.Empty, "Property name is null or empty!");
                Assert.That(company["catchPhrase"]?.ToString(), Is.Not.Null.Or.Empty, "Property city is null or empty!");
                Assert.That(company["bs"]?.ToString(), Is.Not.Null.Or.Empty, "Property zipcode is null or empty!");
            }

        }

        [Test]

        public void GetSpecificUser()
        {
            var userToGet = 3;
            var userToGetUsername = "Samantha";
            var userToGetEmail = "Nathan@yesenia.net";
            var request = new RestRequest($"users/{userToGet}", Method.Get);
            var response = client.Execute(request);


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response status code is not successfull");
            Assert.That(response.Content, Is.Not.Null.Or.Empty, "Content is null or empty");

            var user = JObject.Parse(response.Content);

            Assert.That(user["username"]?.ToString(), Is.EqualTo(userToGetUsername), "Username doesnt match!");
            Assert.That(user["email"]?.ToString(), Is.EqualTo(userToGetEmail), "Email doesnt match!");
        }

        [Test]

        public void CreateUser()
        {
            var createdUser = @"
             {
               ""name"": ""Konstantin"",
               ""username"": ""Kosta"",
               ""email"": ""Kost@gmail.com"",
               ""address"": { 
                ""street"": ""Opulchenska"",
                ""suite"": ""ap.13"",
                ""city"": ""Varna"",
                ""zipcode"": ""9000"",
                ""geo"": {
                    ""lat"": ""29.4572"",
                    ""lng"": ""-47.0653""
                    }
                },
              ""phone"":""020-040"",
              ""website"":""github.com"",
              ""company"": {
                ""name"": ""Liverpool BG Fans"",
                ""catchprase"": ""YNWA"",
                ""bs"": ""Supporting a football team""
              }
               
              }";

            var request = new RestRequest($"/users", Method.Post);
            request.AddJsonBody(createdUser);
            var response = client.Execute(request);


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Response status code is not successfull");
            Assert.That(response.Content, Is.Not.Null.Or.Empty, "Content is null or empty");

            var newUser = JObject.Parse(response.Content);

            Assert.That(newUser["id"]?.Value<int>(), Is.EqualTo(11), "Incorrect Id for the new user!");
            Assert.That(newUser["name"]?.ToString(), Is.EqualTo("Konstantin"), "Incorrect name for the new user!");
            Assert.That(newUser["username"]?.ToString(), Is.EqualTo("Kosta"), "Incorrect username for the new user!");


        }


        [Test]

        public void UpdateUser()
        {
            var userToUpdateID = 2;
            var newUsername = "Erwin Merwin";

            var updatedUser = @"
             {
               ""name"": ""Ervin Howell"",
               ""username"": ""Erwin Merwin"",
               ""email"": ""Shanna@melissa.tv"",
               ""address"": { 
                ""street"": ""Victor Plains"",
                ""suite"": ""Suite 879"",
                ""city"": ""Wisokyburgh"",
                ""zipcode"": ""90566-7771"",
                ""geo"": {
                    ""lat"": ""-43.9509"",
                    ""lng"": ""-34.4618""
                    }
                },
              ""phone"":""010-692-6593 x09125"",
              ""website"":""anastasia.net"",
              ""company"": {
                ""name"": ""Deckow-Crist"",
                ""catchprase"": ""Proactive didactic contingency"",
                ""bs"": ""synergize scalable supply-chains""
              }
               
              }";

            var request = new RestRequest($"/users/{userToUpdateID}", Method.Patch);
            request.AddJsonBody(updatedUser);
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response status code is not succesfful");
            Assert.That(response.Content, Is.Not.Null.Or.Empty, "Content is null or empty");

            var editedUser = JObject.Parse(response.Content);

            Assert.That(editedUser["id"]?.Value<int>(), Is.EqualTo(2), "Incorrect Id for the edited user!");
            Assert.That(editedUser["username"]?.ToString(), Is.EqualTo(newUsername), "Usernames doesnt match");
        }

    }
}
