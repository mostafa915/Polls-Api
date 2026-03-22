using SurveyBasket.Abstractions;
using SurveyBasket.Constract.Authentication;
using SurveyBasket.Models;

namespace SurveyBasket.Services
{
    public interface IAuthentication
    {
        Task<Result<AutheResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<Result<AutheResponse>> GetNewRefreshTokenAsync(string Token, string RefershToken, CancellationToken cancellationToken = default);
        Task<Result> RevokeRefreshTokenAsync(string token, string RefershToken, CancellationToken cancellationToken);
        Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task<Result> ConfirmEmailAsync(EmailConfirmRequest request, CancellationToken cancellationToken = default);
        Task<Result> ResendEmailConfirAsync(ResendConfirmEmailRequest request, CancellationToken cancellationToken = default);
        Task<Result> ResetPasswordEmailCodeAsync(string email);
        Task<Result> ResetPasswordConfirmAsync(ResetPasswordRequest request);
    }
}
