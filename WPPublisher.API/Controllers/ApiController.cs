using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPPublisher.API.ModelControllers;
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

        [HttpGet("GetUserId/{username}")]
        public int GetUserId(string username)
        {
            int result = 0;
            try
            {
                var client = new RestClient("http://localhost:8080");
                //client.Authenticator = new HttpBasicAuthenticator("test", "test");

                var request = new RestRequest("?rest_route=/wp/v2/users&username=" + username, Method.GET);
                IRestResponse response = client.Execute(request);
                if (response != null)
                {
                    JArray objArr = JArray.Parse(response.Content);
                    JObject obj = (JObject)objArr[0];
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

        [HttpGet("GetPost/{idPost}")]
        public WPPost GetPost(string idPost)
        {
            WPPost result = null;
            try
            {
                var client = new RestClient("http://localhost:8080");
                //client.Authenticator = new HttpBasicAuthenticator("test", "test");

                var request = new RestRequest("?rest_route=/wp/v2/posts" + idPost, Method.GET);
                IRestResponse response = client.Execute(request);

                // gestione errore
                if (response.Content != null)
                {
                    result = WPPostController.GetWPPostFromJObject((JObject)JArray.Parse(response.Content)[0]);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = null;
            }

            return result;
        }

        [HttpGet("GetPost/{author}/{status?}")]
        public IEnumerable<WPPost> GetPosts(int author, string status)
        {
            IEnumerable<WPPost> result;
            try
            {
                var client = new RestClient("http://localhost:8080");
                //client.Authenticator = new HttpBasicAuthenticator("test", "test");

                var request = new RestRequest("?rest_route=/wp/v2/posts", Method.GET);
                request.AddParameter("author", author);
                if (string.IsNullOrEmpty(status))
                    request.AddParameter("status", status);
                IRestResponse response = client.Execute(request);

                JArray objArr = JArray.Parse(response.Content);
                result = objArr.Select(o => WPPostController.GetWPPostFromJObject((JObject)o)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = null;
            }

            return result;
        }
    }
}
