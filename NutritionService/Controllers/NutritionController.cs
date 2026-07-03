using BuildingBlocks.Shared.Controllers;
using BuildingBlocks.Shared.Results.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionService.Features.GetMealRecommendations;
using System.Security.Claims;

namespace NutritionService.Controllers
{
    [Authorize]
    public class NutritionController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public NutritionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.Parse(userIdClaim!);
        }

        [HttpGet("recommendations")]
        public async Task<IActionResult> GetMealRecommendations([FromQuery] GetMealRecommendationsRequest request)
        {
            var userId = GetCurrentUserId();
            //var userId = new Guid("00000000-0000-0000-0000-000000000001");

            var query = new GetMealRecommendationsQuery
            (
                userId,
                request.MealType,
                request.MaxCalories,
                request.MinProtein,
                new PaginationRequest
                {
                    Page = request.Page,
                    PageSize = request.PageSize
                }
            );
            var result = await _mediator.Send(query);
            return FromResult(result, "Meal Recommendations Retrieved Successfully", StatusCodes.Status200OK);


        }

        [HttpGet("recommendations/{userId}")]
        public async Task<IActionResult> GetMealRecommendationsByUserId([FromRoute] Guid userId, [FromQuery] GetMealRecommendationsRequest request)
        {
            
            var query = new GetMealRecommendationsQuery
            (
                userId,
                request.MealType,
                request.MaxCalories,
                request.MinProtein,
                new PaginationRequest
                {
                    Page = request.Page,
                    PageSize = request.PageSize
                }
            );
            var result = await _mediator.Send(query);
            return FromResult(result, "Meal Recommendations Retrieved Successfully", StatusCodes.Status200OK);


        }

    }
}
