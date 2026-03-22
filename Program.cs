using FluentValidation;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket;
using SurveyBasket.AppliacationsConfingrations;
using SurveyBasket.Models;
using SurveyBasket.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDependencies(builder.Configuration);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseHangfireDashboard("/jobs", new DashboardOptions { 
    Authorization = 
    [
        new HangfireCustomBasicAuthenticationFilter{
            Pass = app.Configuration.GetValue<string>("HangFireSettings:Password"),
            User = app.Configuration.GetValue<string>("HangFireSettings:UserName")
        }
    ],
    DashboardTitle = "SurveyBasket DashBoard"

});
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();
var notfication = scope.ServiceProvider.GetRequiredService<INotification>();
RecurringJob.AddOrUpdate("SendPollNoTfication", () => notfication.SendNotficationByNewPollAsync(null), Cron.Daily);
app.UseCors();

app.UseAuthorization();
//app.UseMiddleware<ExceptionHanddlerMiddleware>();
app.MapControllers();
app.UseExceptionHandler();
app.MapHealthChecks("health");
app.Run();
