using System;
using System.Collections.Generic;
using System.Reflection;

namespace Key2Joy.Config;

/// <summary>
/// Provides an abstraction for retrieving property information and custom attributes.
/// </summary>
public interface IAttributeProvider
{
    /// <summary>
    /// Retrieves all properties from the given type.
    /// </summary>
    /// <param name="type">The type whose properties need to be retrieved.</param>
    /// <returns>An enumerable of <see cref="PropertyInfo"/> representing the properties of the given type.</returns>
    IEnumerable<PropertyInfo> GetProperties(Type type);

    /// <summary>
    /// Retrieves the custom <see cref="ConfigControlAttribute"/> associated with the given property.
    /// </summary>
    /// <param name="property">The property whose custom attribute needs to be retrieved.</param>
    /// <returns>The <see cref="ConfigControlAttribute"/> associated with the property, or null if no such attribute exists.</returns>
    ConfigControlAttribute GetCustomConfigControlAttribute(PropertyInfo property);
}
