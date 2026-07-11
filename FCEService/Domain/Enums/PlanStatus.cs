using FCEService.Common;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace FCEService.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [TypeConverter(typeof(EnumMemberTypeConverter<PlanStatus>))]
    public enum PlanStatus
    {
        None = 0,
        Weak = 1,
        Normal = 2,
        Hard = 3
    }
}
