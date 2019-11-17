using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebServiceToken.Models;
using Xunit;

namespace StackOverFlow.Tests
{
    
    public class WebServiceTests
    {
        private const string AuthApi = "http://localhost:5001/api/auth";
        private const string PostsApi = "http://localhost:5001/api/posts";

        /* /api/auth */
        [Fact]
        public void ApiAuth_User_Created()
        {
            var NewUser = new 
            {
                UserName = "Aurélien",
                Password = "test",
                Email = "auba@ruc.dk"
            };
            
            var (user, statusCode) = PostData($"{AuthApi}/users", NewUser);
            
            Assert.Equal("Aurélien", user["Username"]);
        }
        
        [Fact]
        public void ApiAuth_Null_Username()
        {
            var (category, statusCode) = PostData($"{AuthApi}/users", null);

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        }
        
        /* /api/posts */
        [Fact]
        public void ApiPost_GetWithPostId_Ok()
        {
            var (post, statusCode) = GetArray($"{PostsApi}/19");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(164, post["score"]);
        }

        
        
        
        
        // Helpers

        (JArray, HttpStatusCode) GetArray(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JArray)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) GetObject(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) PostData(string url, object content)
        {
            var client = new HttpClient();
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        HttpStatusCode PutData(string url, object content)
        {
            var client = new HttpClient();
            var response = client.PutAsync(
                url,
                new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.UTF8,
                    "application/json")).Result;
            return response.StatusCode;
        }

        HttpStatusCode DeleteData(string url)
        {
            var client = new HttpClient();
            var response = client.DeleteAsync(url).Result;
            return response.StatusCode;
        }
    }
}
