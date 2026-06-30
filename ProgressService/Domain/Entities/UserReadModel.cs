namespace ProgressService.Domain.Entities
{
    public class UserReadModel : BaseEntity
    {
        public Guid UserId { get; set; } = default!;
        public bool IsPremium { get; set; } = false;

    }
}
