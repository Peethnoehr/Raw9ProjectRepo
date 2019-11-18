using System;

namespace DataAccess
{
    public class MarkingComment
    {
        public int MarkingId { get; set; }
        public DateTime MarkingDate { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public string Annotation { get; set; }
    }
}