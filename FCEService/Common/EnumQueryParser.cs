using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace FCEService.Common
{
    public sealed class EnumMemberTypeConverter<TEnum> : EnumConverter
        where TEnum : struct, Enum
    {
        public EnumMemberTypeConverter() : base(typeof(TEnum)) { }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string stringValue)
            {
                foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    var enumMemberValue = field.GetCustomAttribute<EnumMemberAttribute>()?.Value;

                    if (string.Equals(enumMemberValue, stringValue, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(field.Name, stringValue, StringComparison.OrdinalIgnoreCase))
                    {
                        return field.GetValue(null);
                    }
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
