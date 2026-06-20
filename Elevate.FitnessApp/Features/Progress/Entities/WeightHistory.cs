using System;
using Elevate.FitnessApp.Features.Auth.Entities;
namespace Elevate.FitnessApp.Features.Progress.Entities
{
    public class WeightHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
        // Navigation property
        public User User { get; set; } = null!;
    }
}
