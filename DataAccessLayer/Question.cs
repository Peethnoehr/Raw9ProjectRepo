using System;
using System.Collections.Generic;

namespace StackOverFlow
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<DateTime> ClosedDate { get; set; }
        public Nullable<int> AcceptAnswer { get; set;}
        public Post QuestionPost { get; set;}
        public List<Answer> Answers { get; set;}
        
    }
}