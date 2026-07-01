using System.Text.Json.Serialization;

namespace FCEService.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum PlanStatus
    {
        None = 0,
        Weak = 1,
        Normal = 2,
        Hard = 3
    }
}
