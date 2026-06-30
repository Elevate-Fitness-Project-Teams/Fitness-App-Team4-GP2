using System.Text.Json.Serialization;

namespace FCEService.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ActivityLevel
    {
        None = 0,
        Rookie = 1,
        Beginner = 2,
        Intermediate = 3,
        Advance = 4,
        TrueBeast = 5
    }
}
