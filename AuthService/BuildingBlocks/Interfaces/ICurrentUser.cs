namespace AuthService.BuildingBlocks.Interfaces
{
    /// <summary>
    /// Exposes the identity of the user making the current request,
    /// resolved from the authenticated <see cref="System.Security.Claims.ClaimsPrincipal"/>.
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// The authenticated user's id (Identity primary key), or null when the request is anonymous.
        /// </summary>
        string? UserId { get; }
    }
}
