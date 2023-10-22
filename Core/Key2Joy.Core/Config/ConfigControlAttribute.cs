using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Key2Joy.Config;

/// <summary>
/// Only applied to <see cref="ConfigState"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public abstract class ConfigControlAttribute : Attribute
{
    public string Text { get; set; }
    public string Hint { get; set; } = null;

    /// <summary>
    /// Gets all configs and their property
    /// </summary>
    /// <returns></returns>
    public static Dictionary<PropertyInfo, ConfigControlAttribute> GetAllProperties(Type configType, IAttributeProvider attributeProvider)
        => attributeProvider.GetProperties(configType)
            .Where(p => attributeProvider.GetCustomConfigControlAttribute(p) != null)
            .ToDictionary(
                p => p,
                attributeProvider.GetCustomConfigControlAttribute
            );
}
