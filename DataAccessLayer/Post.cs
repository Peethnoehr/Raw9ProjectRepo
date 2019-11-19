using System;
using System.Collections.Generic;

namespace StackOverFlow
{
    public class Post
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int Score { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        
        public List<Comment> Comments { get; set; }
        public List<Tag> Tags { get; set; }

    }
}