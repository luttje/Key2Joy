using System;
using System.Collections.Generic;

namespace Key2Joy.Contracts.Mapping;

public class ExposedEnumeration
{
    public string Name { get; set; }
    public IDictionary<string, object> KeyValues { get; set; }

    public static ExposedEnumeration FromType(Type enumType)
    {
        ExposedEnumeration result = new()
        {
            Name = enumType.Name,
            KeyValues = new Dictionary<string, object>()
        };
        var names = Enum.GetNames(enumType);
        var values = Enum.GetValues(enumType);

        for (var i = 0; i < names.Length; i++)
        {
            var enumValue = values.GetValue(i);
            var intValue = Convert.ChangeType(enumValue, Enum.GetUnderlyingType(enumType));
            result.KeyValues.Add(names[i], intValue);
        }

        return result;
    }

    public static ExposedEnumeration FromDictionary(string enumType, Dictionary<string, object> enumerationValues)
        => new()
        {
            Name = enumType,
            KeyValues = enumerationValues
        };
}
