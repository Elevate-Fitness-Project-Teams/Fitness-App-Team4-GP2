namespace AuthService.BuildingBlocks.Exceptions
{
    public class UserLockedOutException : Exception
    {
        public UserLockedOutException(string message) : base(message)
        {
            
        }
    }
}
