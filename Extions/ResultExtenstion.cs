using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Abstractions;

namespace SurveyBasket.Extions
{
    public static class ResultExtenstion 
    {
        public static ObjectResult ToProblem(this Result result)
        {
            if(result.IsSucess) {
                throw new InvalidOperationException("Can not convert Succes result to Problem");
            }
            var problem = Results.Problem(statusCode: result.erorr.statusCode);
            var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;
            problemDetails!.Extensions = new Dictionary<string, object?> {
                {
                    "errors" , new [] {
                        result.erorr.code,
                        result.erorr.description
                    }
                }
            };
            return new ObjectResult(problemDetails);
        }
    }
}
