using System;
using System.Collections.Generic;

namespace StackOverFlow
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<DateTime> ClosedDate { get; set; }
        public Nullable<int> AcceptAnswer { get; set; }
        
        public Post AcceptAnswerPost { get; set; }
        public Post Post { get; set;}
        public List<Post> Answers { get; set;}
        
    }
}