using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Key2Joy.Contracts.Util;

namespace Key2Joy.Contracts.Mapping;

public abstract class AbstractMappingAspect : MarshalByRefObject, ICloneable, IComparable<AbstractMappingAspect>
{
    public string Name { get; set; }

    public AbstractMappingAspect(string name) => this.Name = name;

    public virtual string GetNameDisplay() => this.Name;

    public override string ToString() => this.GetNameDisplay();

    private PropertyInfo[] GetProperties()
    {
        var type = this.GetType();
        var properties = type.GetProperties();

        return properties.Where(p => p.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Length == 0).ToArray();
    }

    public virtual MappingAspectOptions SaveOptions()
    {
        MappingAspectOptions options = new();
        var properties = this.GetProperties();

        foreach (var property in properties)
        {
            this.SaveOptionsGetProperty(options, property);
        }

        return options;
    }

    /// <summary>
    /// Can be overridden by child actions or triggers to allow saving more complex types.
    /// </summary>
    /// <param name="property"></param>
    /// <param name="value"></param>
    protected virtual void SaveOptionsGetProperty(MappingAspectOptions options, PropertyInfo property)
    {
        var value = property.GetValue(this);

        switch (value)
        {
            case DateTime dateTime:
                value = dateTime.Ticks;
                break;

            default:
                break;
        }

        options.Add(property.Name, value);
    }

    public virtual void LoadOptions(MappingAspectOptions options)
    {
        var properties = this.GetProperties();

        foreach (var property in properties)
        {
            if (!options.ContainsKey(property.Name))
            {
                continue;
            }

            this.LoadOptionSetProperty(options, property);
        }
    }

    /// <summary>
    /// Can be overridden by child actions or triggers to allow loading more complex types.
    /// </summary>
    /// <param name="property"></param>
    /// <param name="value"></param>
    protected virtual void LoadOptionSetProperty(MappingAspectOptions options, PropertyInfo property)
    {
        var propertyType = property.PropertyType;
        var value = options[property.Name];
        var genericTypeDefinition = propertyType.IsGenericType ? propertyType.GetGenericTypeDefinition() : null;

        propertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (propertyType == typeof(DateTime))
        {
            value = new DateTime(Convert.ToInt64(value));
        }
        else
        {
            value = TypeConverter.ConvertToType(value, propertyType);
        }

        property.SetValue(this, value);
    }

    public virtual int CompareTo(AbstractMappingAspect other) => this.ToString().CompareTo(other.ToString());

    public static bool operator ==(AbstractMappingAspect a, AbstractMappingAspect b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(AbstractMappingAspect a, AbstractMappingAspect b) => !(a == b);

    public virtual object Clone() => this.MemberwiseClone();
}
