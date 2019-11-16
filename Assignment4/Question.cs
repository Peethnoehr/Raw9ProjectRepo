using System;

namespace Assignment4
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public DateTime ClosedDate { get; set; }
        public int AcceptAnswer { get; set; }
    }
}