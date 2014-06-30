using System;
using System.Collections.Generic;
using System.Text;
using me2day.api.Model;

namespace me2day.api.Model
{
    public class Post
    {
        public string post_id { get; set; }
        public string permalink { get; set; }
        public string body { get; set; }
        public string kind { get; set; }
        public string icon { get; set; }
        public string me2dayPage { get; set; }
        public string pubDate { get; set; }
        public Int32 commentsCount { get; set; }
        public Int32 metooCount { get; set; }
        public Person Author { get; set; }
        public List<Tag> Tags { get; set; }

        public Post()
        {
            Tags = new List<Tag>();
        }
    }
}
