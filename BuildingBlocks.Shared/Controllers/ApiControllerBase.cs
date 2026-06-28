using BuildingBlocks.Shared.Responses;
using BuildingBlocks.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Shared.Controllers
{
    /// <summary>
    /// Base controller that maps an application-layer <see cref="Result{T}"/> onto the
    /// unified <see cref="ApiResponse{T}"/> envelope and sets the matching HTTP status code.
    /// Derived controllers may override the route by declaring their own <c>[Route]</c>.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult FromResult<T>(
            Result<T> result,
            string successMessage,
            int successStatusCode)
        {
            var response = result.ToApiResponse(successMessage, successStatusCode);
            return StatusCode(response.StatusCode, response);
        }
    }
}
