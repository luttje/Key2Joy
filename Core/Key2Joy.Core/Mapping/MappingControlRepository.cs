using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Plugins;

namespace Key2Joy.Mapping;

public static class MappingControlRepository
{
    private static Dictionary<string, MappingControlFactory> mappingControls;

    /// <summary>
    /// Check all types for the MappingControl attribute and store them for later use. Optionally merging it with additional Mapping Controls.
    /// </summary>
    /// <param name="additionalMappingControlFactories"></param>
    public static void Buffer(IReadOnlyList<MappingControlFactory> additionalMappingControlFactories = null)
    {
        mappingControls = new Dictionary<string, MappingControlFactory>();

        foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()))
        {
            var attribute = type.GetCustomAttribute<MappingControlAttribute>();
            if (attribute == null)
            {
                continue;
            }

            if (attribute.ForType != null)
            {
                mappingControls.Add(attribute.ForType.FullName, new TypeMappingControlFactory(attribute.ForType.FullName, attribute.ImageResourceName, type));
            }

            if (attribute.ForTypes != null)
            {
                foreach (var forType in attribute.ForTypes)
                {
                    mappingControls.Add(forType.FullName, new TypeMappingControlFactory(forType.FullName, attribute.ImageResourceName, type));
                }
            }
        }

        if (additionalMappingControlFactories == null)
        {
            return;
        }

        foreach (var mappingControlFactory in additionalMappingControlFactories)
        {
            if (mappingControls.ContainsKey(mappingControlFactory.ForTypeFullName))
            {
                Console.WriteLine("Mapping Control {0} already exists in the action buffer. Overwriting.", mappingControlFactory.ForTypeFullName);
            }

            mappingControls.Add(mappingControlFactory.ForTypeFullName, mappingControlFactory);
        }
    }

    /// <summary>
    /// Gets all mapping controls and their targetted typename
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, MappingControlFactory> GetAllMappingControls() => mappingControls;

    /// <summary>
    /// Gets a specific mapping control factory by its type name
    /// </summary>
    /// <param name="typeFullName"></param>
    /// <returns></returns>
    public static MappingControlFactory GetMappingControlFactory(string typeFullName)
    {
        if (mappingControls.TryGetValue(typeFullName, out var mappingControlFactory))
        {
            return mappingControlFactory;
        }

        return null;
    }
}
