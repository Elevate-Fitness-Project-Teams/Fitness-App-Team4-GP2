namespace AuthService.BuildingBlocks.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("AUTH_UNAUTHORIZED") { }

        public UnauthorizedException(string message) : base(message) { }
    }
}
