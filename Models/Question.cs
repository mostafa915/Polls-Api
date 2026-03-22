namespace SurveyBasket.Models
{
    public sealed class Question : EuidtableEntity
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int PollId { get; set; }
        public bool IsActive { get; set; } = true;
        public Poll Poll { get; set; } = default!;
        public ICollection<Answers> Answers { get; set; } = [];
        public ICollection<VoteAnswer> voteAnswers { get; set; } = [];

    }
}
