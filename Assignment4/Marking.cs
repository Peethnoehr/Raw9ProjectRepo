using System;

namespace Assignment4
{
    public class Marking
    {
        public int MarkingId { get; set; }
        public DateTime MarkingDate { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int CommentId { get; set; }
    }
}