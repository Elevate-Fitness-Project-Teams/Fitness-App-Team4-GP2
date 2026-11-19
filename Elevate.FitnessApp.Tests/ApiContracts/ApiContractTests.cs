using System.Reflection;
using BuildingBlocks.Shared.Responses;
using BuildingBlocks.Shared.Results;
using FCEService.Controllers;
using Microsoft.AspNetCore.Mvc;
using WorkoutService.Controllers;
using Xunit;

namespace Elevate.FitnessApp.Tests.ApiContracts
{
    public class ApiContractTests
    {
        [Fact]
        public void FUNC_003_All_Nine_Required_REST_Endpoints_Are_Mapped_On_WorkOutController()
        {
            var methods = typeof(WorkOutController).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            var routes = new List<(string HttpMethod, string Template)>();
            foreach (var m in methods)
            {
                var httpGets = m.GetCustomAttributes<HttpGetAttribute>();
                foreach (var g in httpGets)
                {
                    routes.Add(("GET", g.Template ?? ""));
                }

                var httpPosts = m.GetCustomAttributes<HttpPostAttribute>();
                foreach (var p in httpPosts)
                {
                    routes.Add(("POST", p.Template ?? ""));
                }
            }

            // 1. Browse workouts: GET /api/v1/workouts
            Assert.Contains(routes, r => r.HttpMethod == "GET" && r.Template.Contains("workouts"));

            // 2. Workout details: GET /api/v1/workouts/{id}
            Assert.Contains(routes, r => r.HttpMethod == "GET" && r.Template.Contains("workouts/{id"));

            // 3. Workouts by plan: GET /api/v1/workouts/by-plan/{planId}
            Assert.Contains(routes, r => r.HttpMethod == "GET" && r.Template.Contains("workouts/by-plan/{planId}"));

            // 4. Workouts by category: GET /api/v1/workouts/category/{categoryName}
            Assert.Contains(routes, r => r.HttpMethod == "GET" && r.Template.Contains("workouts/category/{categoryName}"));

            // 5. Start workout: POST /api/v1/workouts/{id}/start
            Assert.Contains(routes, r => r.HttpMethod == "POST" && r.Template.Contains("workouts/{id") && r.Template.Contains("/start"));

            // 6. Browse exercises: GET /api/v1/exercises
            Assert.Contains(routes, r => r.HttpMethod == "GET" && r.Template.Contains("exercises"));

            // 7. Exercise details: GET /api/v1/exercises/{id}
            Assert.Contains(routes, r => r.HttpMethod == "GET" && r.Template.Contains("exercises/{id"));

            // 8. Browse workout plans: GET /api/v1/workout-plans
            Assert.Contains(routes, r => r.HttpMethod == "GET" && r.Template.Contains("workout-plans"));

            // 9. Workout plan details: GET /api/v1/workout-plans/{planId}
            Assert.Contains(routes, r => r.HttpMethod == "GET" && r.Template.Contains("workout-plans/{planId}"));
        }

        [Fact]
        public void FUNC_002_FitnessController_Contains_Required_WeightGoalActivity_Route()
        {
            var method = typeof(FitnessController).GetMethod(nameof(FitnessController.SubmitFitnessStats));
            Assert.NotNull(method);

            var postAttributes = method.GetCustomAttributes<HttpPostAttribute>().ToList();
            Assert.Contains(postAttributes, attr => attr.Template == "weight-goal-activity");
        }

        [Fact]
        public void FUNC_004_AuthTokenInvalid_Error_Code_Has_No_Leading_Or_Trailing_Whitespace()
        {
            const string expectedCode = "AUTH_TOKEN_INVALID";

            var error = Error.InvalidCredentials(expectedCode);

            Assert.Equal("AUTH_TOKEN_INVALID", error.Code);
            Assert.False(error.Code.StartsWith(" "), "Error code must not start with whitespace.");
            Assert.False(error.Code.EndsWith(" "), "Error code must not end with whitespace.");
            Assert.False(error.Code.Contains("."), "Error code must not contain punctuation dots.");
        }

        [Fact]
        public void ApiResponse_Envelope_Formats_Success_And_Failure_Accurately()
        {
            var successEnvelope = ResponseFactory.Success(new List<string> { "item1" }, "Fetched successfully", 200);

            Assert.True(successEnvelope.IsSuccess);
            Assert.Equal(200, successEnvelope.StatusCode);
            Assert.Equal("Fetched successfully", successEnvelope.Message);
            Assert.Single(successEnvelope.Data);

            var failureEnvelope = ResponseFactory.Failure("Not found", 404, "RES_NOT_FOUND");
            Assert.False(failureEnvelope.IsSuccess);
            Assert.Equal(404, failureEnvelope.StatusCode);
            Assert.True(failureEnvelope.Errors.ContainsKey("RES_NOT_FOUND"));
        }

        [Fact]
        public void ResultExtensions_Maps_Empty_Success_Result_To_200_OK_With_Envelope()
        {
            var emptyList = new List<int>();
            var result = Result<List<int>>.OK(emptyList);

            var apiResponse = result.ToApiResponse("No items", 200);

            Assert.True(apiResponse.IsSuccess);
            Assert.Equal(200, apiResponse.StatusCode);
            Assert.NotNull(apiResponse.Data);
            Assert.Empty(apiResponse.Data);
        }
    }
}

