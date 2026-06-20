using System;
namespace Elevate.FitnessApp.Features.SmartCoach.Entities
{
    public class ChatMessage
    {
        public int MessageId { get; set; }
        public string SessionId { get; set; } = null!;
        public string Sender { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        // Navigation property
        public ChatSession ChatSession { get; set; } = null!;
    }
}
