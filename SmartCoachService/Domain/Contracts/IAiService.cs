namespace SmartCoachService.Domain.Contracts
{
    public interface IAiService 
    {
        Task<string> GenerateAsync(string prompt);
    }
}
