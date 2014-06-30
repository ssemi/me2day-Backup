using System;
using System.Collections.Generic;
using System.Text;

namespace me2day.api.Model
{
    public class Person
    {

        public string id { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public string face { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public string me2dayHome { get; set; }
        public string rssDaily { get; set; }
        public string invitedBy { get; set; }
        public int friendsCount { get; set; }
        public DateTime updated { get; set; }

    }
}
