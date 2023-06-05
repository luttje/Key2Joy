using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    public struct ExposedEnumeration
    {
        public string Name { get; set; }
        public IDictionary<string, object> KeyValues { get; set; }

        public static ExposedEnumeration FromType(Type enumType)
        {
            var result = new ExposedEnumeration();
            result.Name = enumType.Name;
            result.KeyValues = new Dictionary<string, object>();
            var names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType);

            for (int i = 0; i < names.Length; i++)
            {
                result.KeyValues.Add(names[i], values.GetValue(i));
            }
            
            return result;
        }
    }
}
