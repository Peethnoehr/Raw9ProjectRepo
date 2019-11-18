using System;

namespace DataAccess
{
    public class MarkingPost
    {
        public int MarkingId { get; set; }
        public DateTime MarkingDate { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Annotation { get; set; }
    }
}