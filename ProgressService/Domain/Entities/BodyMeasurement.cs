using System;
namespace ProgressService.Domain.Entities
{
    public class BodyMeasurement : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public double? Neck { get; set; }
        public double? Chest { get; set; }
        public double? Biceps { get; set; }
        public double? Waist { get; set; }
        public double? Hips { get; set; }
        public double? Thighs { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
