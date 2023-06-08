using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Plugins;

namespace Key2Joy.Mapping
{
    // TODO: ? Or will System.Text.Json figure out valid and plugin types itself?
    //public class MappingProfileSerializationBinder : ISerializationBinder
    //{
    //    private List<MappingTypeFactory> allowedTypes;

    //    public MappingProfileSerializationBinder()
    //    {
    //        allowedTypes = new List<MappingTypeFactory>();
            
    //        allowedTypes.AddRange(
    //            ActionsRepository.GetAllActions().Select(x => x.Value).ToList()
    //        );
            
    //        allowedTypes.AddRange(
    //            TriggersRepository.GetAllTriggers().Select(x => x.Value).ToList()
    //        );
    //    }

    //    public Type BindToType(string assemblyName, string fullTypeName)
    //    {
    //        return allowedTypes.SingleOrDefault(t => t.FullTypeName == fullTypeName)?.ToType() ?? null;
    //    }

    //    public void BindToName(Type serializedType, out string assemblyName, out string typeName)
    //    {
    //        assemblyName = null;
    //        typeName = serializedType.FullName;
    //    }
    //}
}