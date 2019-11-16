using System;

namespace Assignment4
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string TextContain { get; set; }
        public int Score { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
}