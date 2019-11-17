using System;
using System.Collections.Generic;

namespace Assignment4
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public Nullable<DateTime> ClosedDate { get; set; }
        public Nullable<int> AcceptAnswer { get; set;}
        public Post QuestionPost { get; set;}
        public List<Answer> Answers { get; set;}
        
    }
}