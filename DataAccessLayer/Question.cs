using System;
using System.Collections.Generic;

namespace StackOverFlow
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ClosedDate { get; set; }
        List<Answer> Answer { get; set; }
        Answer AcceptAnswer { get; set; }
    }
}