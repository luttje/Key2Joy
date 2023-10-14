using System;

namespace Key2Joy.Contracts.Mapping;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ExposesEnumerationAttribute : MappingAttribute
{
    public Type EnumerationType { get; set; }

    public ExposesEnumerationAttribute(Type enumerationType)
    {
        this.EnumerationType = enumerationType;
    }
}
