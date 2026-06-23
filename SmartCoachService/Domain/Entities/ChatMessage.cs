using System;
namespace SmartCoachService.Domain.Entities
{
    public class ChatMessage
    {
        public int MessageId { get; set; }
        public string SessionId { get; set; } = null!;
        public string Sender { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public ChatSession ChatSession { get; set; } = null!;
    }
}
