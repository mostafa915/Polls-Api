namespace SurveyBasket.Services
{
    public interface INotification
    {
        Task SendNotficationByNewPollAsync(int? pollId = null);
    }
}
