﻿namespace DataAccess
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        
        public Post AnswerPost { get; set; }
    }
}