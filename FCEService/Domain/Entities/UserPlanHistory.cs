using System;
namespace FCEService.Domain.Entities
{
    public class UserPlanHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PlanId { get; set; } = null!;
        public DateTime AssignedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public string ReasonForChange { get; set; } = null!;
    }
}
