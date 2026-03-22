namespace SurveyBasket.Models
{
    public class VoteAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int AnswersId { get; set; }
        public int VoteId { get; set; }
        public Vote vote { get; set; } = default!;
        public Question Question { get; set; } = default!;
        public Answers Answers { get; set; } = default!;
    }
}
