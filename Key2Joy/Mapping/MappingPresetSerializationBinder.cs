using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Key2Joy.Mapping
{
    public class MappingPresetSerializationBinder : ISerializationBinder
    {
        private IList<Type> allowedTypes;

        public MappingPresetSerializationBinder()
        {
            allowedTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BaseTrigger))
                    || t.IsSubclassOf(typeof(BaseAction)))
                .ToList();
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            return allowedTypes.SingleOrDefault(t => t.Name == typeName);
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }
    }
}