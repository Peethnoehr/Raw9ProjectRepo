using System;

namespace StackOverFlow
{
    public class Comment
    {
        public int Id { get; set; }
        public string TextContain { get; set; }
        public int Score { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserName { get; set; }
        public int PostId { get; set; }
    }
}