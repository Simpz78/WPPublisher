using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPPublisher.Model
{
    /// <summary>
    /// Classe per un post di WordPress
    /// </summary>
    public class WPPost
    {
        public string Title { get; }
        public string Content { get; }
        public int Id { get; }
        public string Status { get; }

        public WPPost(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public WPPost(string title, string content, int id, string status)
        {
            Title = title;
            Content = content;
            Id = id;
            Status = status;
        }
    }
}
