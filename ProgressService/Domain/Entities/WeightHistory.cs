using System;
namespace ProgressService.Domain.Entities
{
    public class WeightHistory : BaseEntity
    {
        public Guid UserId { get; set; } 
        public double Weight { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
    }
}
