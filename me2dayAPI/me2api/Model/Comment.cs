using System;
using System.Collections.Generic;
using System.Text;

namespace me2day.api.Model
{
    public class Comment
    {
        public string commentId { get; set; }
        public string body { get; set; }
        public DateTime pubDate { get; set; }
        public Person Author { get; set; }
    }
}
