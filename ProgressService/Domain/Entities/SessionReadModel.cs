namespace ProgressService.Domain.Entities
{
    public class SessionReadModel : BaseEntity
    {
        public string SessionId { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public bool IsActive { get; set; } = true;

    }
}
