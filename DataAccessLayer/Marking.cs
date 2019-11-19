using System;

namespace StackOverFlow
{
    public class Marking
    {
        public int Id { get; set; }
        public string Annotation { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public Nullable<int> PostId { get; set; }
        public Nullable<int> CommentId { get; set; }
        
        public Post Post { get; set; }
        public Comment Comment { get; set; }
    }
}