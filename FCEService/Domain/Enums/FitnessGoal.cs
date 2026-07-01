using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FCEService.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum FitnessGoal
    {
        None = 0,
        [EnumMember(Value = "Lose Weight")]
        LoseWeight = 1,
        [EnumMember(Value = "Get Fitter")]
        GetFitter = 2,
        [EnumMember(Value = "Gain Weight")]
        GainWeight = 3,
        [EnumMember(Value = "Gain More Flexible")]
        GainMoreFlexible = 4,
        [EnumMember(Value = "Learn the Basic")]
        LearnTheBasic = 5
    }
}
