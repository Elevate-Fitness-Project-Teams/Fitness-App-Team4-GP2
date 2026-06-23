namespace FCEService.Domain.Entities
{
    public class CalculatedMetric
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Bmr { get; set; }
        public double Tdee { get; set; }
        public double CalorieTarget { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CalculatedAt { get; set; }
    }
}
