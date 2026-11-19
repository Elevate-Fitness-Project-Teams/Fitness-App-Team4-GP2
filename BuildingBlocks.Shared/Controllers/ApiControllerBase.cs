using BuildingBlocks.Shared.Exceptions;
using BuildingBlocks.Shared.Responses;
using BuildingBlocks.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        protected bool TryGetCurrentUserId(out Guid userId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst("sub")?.Value;

            return Guid.TryParse(claim, out userId);
        }

        protected Guid GetCurrentUserId()
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                throw new AppException(Error.Unauthorized("AUTH_INVALID_TOKEN", "Unable to resolve the authenticated user."));
            }
            return userId;
        }

        protected bool IsOwnerOrAdmin(Guid targetUserId)
        {
            if (!TryGetCurrentUserId(out var currentUserId))
                return false;

            return currentUserId == targetUserId || User.IsInRole("Admin");
        }
    }
}
