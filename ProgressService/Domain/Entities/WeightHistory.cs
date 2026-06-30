using System;
namespace ProgressService.Domain.Entities
{
    public class WeightHistory : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public double Weight { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
    }
}
