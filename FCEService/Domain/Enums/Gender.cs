using System.Text.Json.Serialization;

namespace FCEService.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Gender
    {
        None =0,
        Male =1,
        Female=2
    }
}
