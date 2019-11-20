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
        private const string MarkApi = "http://localhost:5001/api/mark";
        private const string PostsApi = "http://localhost:5001/api/posts";
        private const string SearchApi = "http://localhost:5001/api/search";

        /* /api/auth/users */
        [Fact]
        public void ApiAuth_User_Created()
        {
            var NewUser = new 
            {
                UserName = "Paul",
                Password = "jtm",
                Email = "auba@ruc.dk"
            };
            
            PostData($"{AuthApi}/users", NewUser);
        }
        
        [Fact]
        public void ApiAuth_User_AlreadyCreated()
        {
            var NewUser = new 
            {
                UserName = "Baptiste",
            };
            
            var (user, statusCode) = PostData($"{AuthApi}/users", NewUser);
            
            Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        }
        
        [Fact]
        public void ApiAuth_Null_Username()
        {
            var (category, statusCode) = PostData($"{AuthApi}/users", null);

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        }
        
        /* /api/auth/tokens */
        [Fact]
        public void ApiAuth_User_Login()
        {
            var NewUser = new 
            {
                UserName = "Paul",
                Password = "jtm",
            };
            
            PostData($"{AuthApi}/tokens", NewUser);
        }
        
        /* /api/auth */
        [Fact]
        public void ApiAuth_User_Update()
        {
            var NewUser = new 
            {
                UserName = "Paul",
                Password = "jtm",
                Email = "update@ruc.dk"
            };
            
            PutData($"{AuthApi}", NewUser);
        }
        
        [Fact]
        public void ApiAuth_User_Delete()
        {
            DeleteData($"{AuthApi}/Paul");
        }
        
        /* /api/mark */
        [Fact]
        public void ApiMark_Marking_Created()
        {
            var NewMarking = new 
            {
                Annotation = "test", 
                Username = "Baptiste",
                PostId = 19
            };
            
            var (data, statusCode) = PostData($"{MarkApi}", NewMarking);
        }
        
        [Fact]
        public void ApiMark_Marking_Get()
        {
            var NewUser = new
            {
                UserName = "Baptiste"
            };
            
            var (markings,statusCode) = PostData($"{MarkApi}/markings", NewUser);
        }
        
        [Fact]
        public void ApiMark_Marking_Delete()
        {
            DeleteData($"{MarkApi}/1");
        }
        
        /* /api/search */
        [Fact]
        public void ApiSearch_SearchHistory_Get()
        {
            var NewUser = new 
            {
                UserName = "Baptiste"
            };
            
            var (markings,statusCode) = PostData($"{SearchApi}", NewUser);
        }

        /* /api/posts */
        [Fact]
        public void ApiPost_GetWithPostId_Ok()
        {
            var NewPost = new 
            {
                Id = 19
            };
            
            var (post, statusCode) = PostData($"{PostsApi}", NewPost);

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void ApiPost_GetWithSearchText_Ok()
        {
            var NewText = new 
            {
                SearchText = "java help me"
            };
            
            var (post, statusCode) = PostData($"{PostsApi}/search", NewText);

            Assert.Equal(HttpStatusCode.OK, statusCode);
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
