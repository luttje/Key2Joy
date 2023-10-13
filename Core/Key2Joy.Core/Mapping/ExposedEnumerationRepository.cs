using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Key2Joy.Contracts.Mapping;

namespace Key2Joy.Mapping;

public static class ExposedEnumerationRepository
{
    private static List<ExposedEnumeration> exposedEnumerations;

    /// <summary>
    /// Check all types for the ExposedEnumeration attribute and store them for later use. Optionally merging it with additional enumerations.
    /// </summary>
    /// <param name="additionalExposedEnumerations"></param>
    public static void Buffer(IReadOnlyList<ExposedEnumeration> additionalExposedEnumerations = null)
    {
        exposedEnumerations = new();

        foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()))
        {
            var attributes = type.GetCustomAttributes<ExposesEnumerationAttribute>();

            foreach (var attribute in attributes)
            {
                exposedEnumerations.Add(ExposedEnumeration.FromType(attribute.EnumerationType));
            }
        }

        if (additionalExposedEnumerations == null)
        {
            return;
        }

        exposedEnumerations.AddRange(additionalExposedEnumerations);
    }

    /// <summary>
    /// Gets all exposed enumerations
    /// </summary>
    /// <returns></returns>
    public static List<ExposedEnumeration> GetAllExposedEnumerations() => exposedEnumerations;
}
