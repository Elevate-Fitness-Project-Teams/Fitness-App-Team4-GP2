using System;
using Elevate.FitnessApp.Features.Auth.Entities;
namespace Elevate.FitnessApp.Features.FitnessCalculation.Entities
{
    public class UserFitnessStat
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public string Goal { get; set; } = null!;
        public string ActivityLevel { get; set; } = null!;
        public DateTime RecordedAt { get; set; }
        // Navigation property to User
        public User User { get; set; } = null!;
    }
}
