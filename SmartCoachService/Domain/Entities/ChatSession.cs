using System;
namespace SmartCoachService.Domain.Entities
{
    public class ChatSession
    {
        public string SessionId { get; set; } = null!;
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; } = "New Conversation";
    }
}
