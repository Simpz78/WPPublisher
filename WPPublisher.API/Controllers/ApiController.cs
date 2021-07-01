using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using WPPublisher.API.Configuration;

namespace WPPublisher.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;
        private readonly IOptions<Auth> _auth;
        private readonly IOptions<WordpressRestApi> _wordpressRestApi;

        public ApiController(ILogger<ApiController> logger, IOptions<Auth> auth, IOptions<WordpressRestApi> wordpressRestApi)
        {
            _logger = logger;
            _auth = auth;
            _wordpressRestApi = wordpressRestApi;
        }

        [HttpPost("PublishPost/{idPost}")]
        public int PublishPost(int idPost)
        {
            int result = 0;
            try
            {
                var client = new RestClient("http://localhost:8080");
                client.Authenticator = new HttpBasicAuthenticator(_auth.Value.Username, _auth.Value.Password);
                string idPostQuery = "/" + idPost;
                var request = new RestRequest(_wordpressRestApi.Value.PostsRestRoute + idPostQuery, Method.POST);
                request.AddParameter("status", "publish");
                IRestResponse response = client.Execute(request);
                if (response != null)
                {
                    JArray objArr = JArray.Parse(response.Content);
                    JObject obj = (JObject)objArr[0];
                    // se ritorna un id allora non ci sono stati errori
                    if (int.TryParse(obj["id"].ToString(), out int id))
                    {
                        string status = obj["status"].ToString();
                        if (id == idPost && status.ToLower() == "publish")
                            result = 1;
                    }
                    else
                        result = 0; 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = 0;
            }
            return result;

        }

        [HttpGet("GetUserId/{username}")]
        public int GetUserId(string username)
        {
            int result = 0;
            try
            {
                var client = new RestClient("http://localhost:8080");
                string usernameQuery = "&username=" + username;
                var request = new RestRequest(_wordpressRestApi.Value.UserRestRoute + usernameQuery, Method.GET);
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
        public string GetPost(string idPost)
        {
            string result = null;
            try
            {
                var client = new RestClient("http://localhost:8080");
                string idPostQuery = "/" + idPost;
                var request = new RestRequest(_wordpressRestApi.Value.PostsRestRoute + idPostQuery, Method.GET);
                IRestResponse response = client.Execute(request);
                result = response.Content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = null;
            }

            return result;
        }

        [HttpGet("GetPosts/{author}")]
        public string GetPosts(int author, string status = null)
        {
            string result;
            try
            {
                var client = new RestClient("http://localhost:8080");
                client.Authenticator = new HttpBasicAuthenticator(_auth.Value.Username, _auth.Value.Password);
                string postQuery = "&author=" + author;
                if (!string.IsNullOrEmpty(status))
                    postQuery += "&status=" + status;

                var request = new RestRequest(_wordpressRestApi.Value.PostsRestRoute + postQuery, Method.GET);
                IRestResponse response = client.Execute(request);
                result = response.Content;
                //JArray objArr = JArray.Parse(response.Content);
                //result = objArr.Select(o => WPPostController.GetWPPostFromJObject((JObject)o)).ToList();
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
