namespace StackOverFlow
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Post AnswerPost { get; set; }
    }
}