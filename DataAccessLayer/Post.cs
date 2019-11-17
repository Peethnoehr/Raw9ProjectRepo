using System.Collections.Generic;

namespace StackOverFlow
{
    public class Post
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int Score { get; set; }
        List<Tag> Tag { get; set; }
        List<Comment> Comment { get; set; }
        Question question { get; set; }
    }
}