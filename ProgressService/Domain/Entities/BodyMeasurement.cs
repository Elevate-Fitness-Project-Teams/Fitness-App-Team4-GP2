using System;
namespace ProgressService.Domain.Entities
{
    public class BodyMeasurement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double? Neck { get; set; }
        public double? Chest { get; set; }
        public double? Biceps { get; set; }
        public double? Waist { get; set; }
        public double? Hips { get; set; }
        public double? Thighs { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
