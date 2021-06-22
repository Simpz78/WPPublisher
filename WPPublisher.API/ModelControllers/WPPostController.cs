using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPPublisher.Model;

namespace WPPublisher.API.ModelControllers
{
    public static class WPPostController
    {
        public static WPPost GetWPPostFromJObject(JObject obj)
        {
            string title = obj["title"].ToString();
            string content = obj["content"].ToString();
            int idPost = int.Parse(obj["id"].ToString());
            string status = obj["status"].ToString();

            return new WPPost(title, content, idPost, status);
        }
    }
}
