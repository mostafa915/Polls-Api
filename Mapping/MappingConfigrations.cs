using Mapster;
using Microsoft.AspNetCore.Identity.Data;
using SurveyBasket.Constract.Poll;
using SurveyBasket.Constract.Qustions;
using SurveyBasket.Constract.User;
using SurveyBasket.Models;

namespace SurveyBasket.Mapping
{
    public class MappingConfigrations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Poll, PollResponse>();

            config.NewConfig<PollRequest, Poll>();

            config.NewConfig<QustionRequest, Question>()
                .Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answers { Content = answer }));

            config.NewConfig<(ApplicationUser User, IList<string> Roles), UserResponse>()
                .Map(dest => dest, src => src.User)
                .Map(dest => dest.Roles, src => src.Roles);
           
            config.NewConfig<CreateUserRequest, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.FirstName + src.LastName)
                .Map(dest => dest.EmailConfirmed, src => true);
            
            config.NewConfig<UpdateUserRequest,  ApplicationUser>()
                .Map(dest => dest.UserName , src => src.FirstName + src.LastName)
                .Map(dest => dest.NormalizedUserName, src => src.FirstName.ToUpper())
                .Map(dest => dest.NormalizedEmail, src => src.Email.ToUpper());
        }
    }
}
