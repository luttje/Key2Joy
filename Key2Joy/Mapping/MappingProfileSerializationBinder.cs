using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Plugins;

namespace Key2Joy.Mapping
{
    public class MappingProfileSerializationBinder : ISerializationBinder
    {
        private List<MappingTypeFactory> allowedTypes;

        public MappingProfileSerializationBinder()
        {
            allowedTypes = new List<MappingTypeFactory>();
            
            allowedTypes.AddRange(
                ActionsRepository.GetAllActions().Select(x => x.Value).ToList()
            );
            
            allowedTypes.AddRange(
                TriggersRepository.GetAllTriggers().Select(x => x.Value).ToList()
            );
        }

        public Type BindToType(string assemblyName, string fullTypeName)
        {
            //return allowedTypes.SingleOrDefault(t => t.FullTypeName == fullTypeName);
            throw new NotImplementedException("TODO: Somehow create the instance elsewhere (using the above typeFactory)");
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.FullName;
        }
    }
}