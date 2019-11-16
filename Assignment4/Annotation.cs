using System;
using DatabaseService;

namespace Assignment4
{
    public class Annotation
    {
        public int AnnotationId { get; set; }
        public DateTime AnnotationDate { get; set; }
        public string AnnotationText { get; set; }
        public int MarkingId { get; set; }
    }
}