using FluentValidation;
using Hangfire;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.AppliacationsConfingrations;
using SurveyBasket.Constract.Authentication;
using SurveyBasket.Erorrs;
using SurveyBasket.Jwt;
using SurveyBasket.Jwt.Autehntication.Filiters;
using SurveyBasket.MailSetting;
using SurveyBasket.Models;
using SurveyBasket.Services;
using System.Reflection;
using System.Text;

namespace SurveyBasket
{
    public static class DepenceyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwagerServices();
            services.AddHybridCache();
            services.AddServicesConfig();
            services.AddMapping();
            services.AddConnectionString(configuration);
            services.AddFluentValidation();
            services.AddAuthenConfig(configuration);
            services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
                    builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(configuration.GetSection("AllowedOrignes").Get<string[]>()!)
            )
            );
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.Configure<MailConfirmation>(configuration.GetSection(nameof(MailConfirmation)));
            services.AddExceptionHandler<GlobalExpectionHandler>();
            services.AddProblemDetails();
            services.AddBackgroundJobsConfig(configuration);
            return services;
        }
        private static IServiceCollection AddSwagerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            return services;
        }
        private static IServiceCollection AddServicesConfig(this IServiceCollection services)
        {
            services.AddScoped<IService, Service>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddScoped<IQuestionServices, QuestionServices>();
            services.AddScoped<IvoteServices, voteServices>();
            services.AddScoped<IResultServices, ResultServices>();
            services.AddScoped<INotification, Notification>();  
            services.AddScoped<IAuthentication, Authentication>();
            services.AddScoped<IEmailSender, EmailServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IRoleServices, RoleServices>();
            services.AddHttpContextAccessor();

            services.AddHealthChecks();
            return services;
        }
        private static IServiceCollection AddMapping(this IServiceCollection services)
        {
            var confing = TypeAdapterConfig.GlobalSettings;
            confing.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(confing));
            return services;
        }
        private static IServiceCollection AddConnectionString(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnectionString") ??
    throw new InvalidOperationException("The Database Not Found");

            services.AddDbContext<ApllicationDbContext>(options =>
                options.UseSqlServer(connectionString)
            );
            services.AddHttpContextAccessor();
            return services;
        }
        private static IServiceCollection AddAuthenConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var Settings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApllicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddTransient<IAuthorizationHandler, PermissionAuthHandler>();
            services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings?.Key!)),
                    ValidAudience = Settings?.Audience,
                    ValidIssuer = Settings?.Issuer,
                };
            });
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            });
            return services;
        }
        private static IServiceCollection AddBackgroundJobsConfig(this  IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            return services;
        }
    }
}
