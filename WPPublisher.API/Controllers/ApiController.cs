using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPPublisher.Model;

namespace WPPublisher.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public int GetUserId(string username)
        {
            int result = 0;
            try
            {
                var client = new RestClient("http://localhost:8080");
                //client.Authenticator = new HttpBasicAuthenticator("test", "test");

                var request = new RestRequest("?rest_route=/wp/v2/users?username=" + username, Method.GET);
                IRestResponse response = client.Execute(request);
                if (response != null)
                {
                    JObject obj = (JObject)JToken.Parse(response.Content);
                    result = int.Parse(obj["id"].ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = 0;
            }
            return result;
        }

        [HttpGet]
        public WPPost GetPost(string idPost)
        {
            // author, status
            var client = new RestClient("http://localhost:8080");
            //client.Authenticator = new HttpBasicAuthenticator("test", "test");

            var request = new RestRequest("?rest_route=/wp/v2/posts" + idPost, Method.GET);
            IRestResponse response = client.Execute(request);

            return null;
        }

        [HttpGet]
        public IEnumerable<WPPost> GetPosts(int author, string status)
        {
            var client = new RestClient("http://localhost:8080");
            //client.Authenticator = new HttpBasicAuthenticator("test", "test");

            var request = new RestRequest("?rest_route=/wp/v2/posts", Method.GET);
            request.AddParameter("author", author);
            request.AddParameter("status", status);
            IRestResponse response = client.Execute(request);

            return null;
        }
    }
}
