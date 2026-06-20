using System;
using Elevate.FitnessApp.Features.Auth.Entities;
namespace Elevate.FitnessApp.Features.SmartCoach.Entities
{
    public class ChatSession
    {
        public string SessionId { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; } = "New Conversation";
        // Navigation property
        public User User { get; set; } = null!;
    }
}
