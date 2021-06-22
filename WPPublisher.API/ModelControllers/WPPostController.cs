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
            string title = obj["title"]["rendered"].ToString();
            string content = obj["content"]["rendered"].ToString();
            int idPost = int.Parse(obj["id"].ToString());
            string status = obj["status"].ToString();

            return new WPPost(title, content, idPost, status);
        }
    }
}
