using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Constract.Authentication;
using SurveyBasket.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SurveyBasket.Jwt
{
    public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        private readonly JwtOptions Options  = options.Value;

        public (string token, int expireIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions)
        {
            Claim[] claims = [
                new(JwtRegisteredClaimNames.Sub , user.Id),
                new(JwtRegisteredClaimNames.Email , user.Email!),
                new(JwtRegisteredClaimNames.Name , user.FirstName),
                new(JwtRegisteredClaimNames.FamilyName , user.LastName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(nameof(roles), JsonSerializer.Serialize(roles), JsonClaimValueTypes.JsonArray),
                new(nameof(permissions), JsonSerializer.Serialize(permissions), JsonClaimValueTypes.JsonArray)
                ];

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var expireIn = Options.ExpireMinute;

            var token = new JwtSecurityToken(
                issuer: Options.Issuer,
                audience: Options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireIn),
                 signingCredentials: signingCredentials
                );
            return (token: new JwtSecurityTokenHandler().WriteToken(token), expire: expireIn * 30);
        }

        public string? ValidateToken(string token)
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.Key));

            try
            {
                TokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = symmetricSecurityKey,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken.Claims.First(j => j.Type == JwtRegisteredClaimNames.Sub).Value;
            }
            
            catch { 
            
                return null;
            }

        }
    }
}
