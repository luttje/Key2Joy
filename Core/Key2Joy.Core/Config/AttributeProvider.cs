using System;
using System.Collections.Generic;
using System.Reflection;

namespace Key2Joy.Config;

/// <summary>
/// Provides methods for retrieving property information and custom attributes.
/// Implements the <see cref="IAttributeProvider"/> interface.
/// </summary>
public class AttributeProvider : IAttributeProvider
{
    /// <summary>
    /// Retrieves all properties from the given type.
    /// </summary>
    /// <param name="type">The type whose properties need to be retrieved.</param>
    /// <returns>An enumerable of <see cref="PropertyInfo"/> representing the properties of the given type.</returns>
    public IEnumerable<PropertyInfo> GetProperties(Type type)
        => type.GetProperties();

    /// <summary>
    /// Retrieves the custom <see cref="ConfigControlAttribute"/> associated with the given property.
    /// </summary>
    /// <param name="property">The property whose custom attribute needs to be retrieved.</param>
    /// <returns>The <see cref="ConfigControlAttribute"/> associated with the property, or null if no such attribute exists.</returns>
    public ConfigControlAttribute GetCustomConfigControlAttribute(PropertyInfo property)
        => property.GetCustomAttribute(typeof(ConfigControlAttribute), false) as ConfigControlAttribute;
}
