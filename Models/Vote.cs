namespace SurveyBasket.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = string.Empty;
        public Poll poll { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
        public ICollection<VoteAnswer> voteAnswers { get; set; } = [];
    }
}
