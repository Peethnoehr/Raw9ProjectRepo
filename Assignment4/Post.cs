using System;
using System.Collections.Generic;

namespace Assignment4
{
    public class Post
    {
        public int PostId { get; set; }
        public string Body { get; set; }
        public int Score { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        
        public List<Comment> Comments { get; set; }

    }
}