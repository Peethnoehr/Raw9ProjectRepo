using System;
using System.Collections.Generic;

namespace StackOverFlow
{
    public class Post
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public Nullable<int> Score { get; set; }
        public Nullable<DateTime> CreationDate { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Title { get; set; }
        
        public List<Comment> Comments { get; set; }
        public List<Tag> Tags { get; set; }

    }
}