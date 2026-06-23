using System;
namespace ProgressService.Domain.Entities
{
    public class WeightHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
    }
}
