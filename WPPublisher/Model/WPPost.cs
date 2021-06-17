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
        private string Title { get; }
        private string Content { get; }
    
        public WPPost(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}
