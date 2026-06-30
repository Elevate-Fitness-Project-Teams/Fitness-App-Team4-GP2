namespace ProgressService.Features.ViewWeightHistory
{
    public class WeightHistoryResponse
    {
        public List<WeightEntry> WeightEntries { get; set; } = new List<WeightEntry>();
        public class WeightEntry
        {
            public double Weight { get; set; }
            public DateTime Date { get; set; }
            public string? Notes { get; set; }
        }
    }
}