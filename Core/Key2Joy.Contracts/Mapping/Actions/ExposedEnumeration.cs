using System;
using System.Collections.Generic;

namespace Key2Joy.Contracts.Mapping
{
    public struct ExposedEnumeration
    {
        public string Name { get; set; }
        public IDictionary<string, object> KeyValues { get; set; }

        public static ExposedEnumeration FromType(Type enumType)
        {
            var result = new ExposedEnumeration
            {
                Name = enumType.Name,
                KeyValues = new Dictionary<string, object>()
            };
            var names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType);

            for (int i = 0; i < names.Length; i++)
            {
                var enumValue = values.GetValue(i);
                var intValue = Convert.ChangeType(enumValue, Enum.GetUnderlyingType(enumType));
                result.KeyValues.Add(names[i], intValue);
            }

            return result;
        }
    }
}
