using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Key2Joy.Mapping
{
    public class MappingProfileSerializationBinder : ISerializationBinder
    {
        private List<Type> allowedTypes;

        public MappingProfileSerializationBinder()
        {
            allowedTypes = new List<Type>();
            allowedTypes.AddRange(ActionAttribute.GetAllActions().Select(x => x.Key));
            allowedTypes.AddRange(TriggerAttribute.GetAllTriggers().Select(x => x.Key));
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