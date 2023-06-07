using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    public abstract class AbstractMappingAspect : MarshalByRefObject, ICloneable, IComparable<AbstractMappingAspect>
    {
        public string Name { get; set; }

        public AbstractMappingAspect(string name)
        {
            Name = name;
        }

        public virtual MappingAspectOptions SaveOptions()
        {
            var type = GetType();
            var properties = type.GetProperties();
            var options = new MappingAspectOptions();

            foreach (var property in properties)
            {
                SaveOptionsGetProperty(options, property);
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

            switch(value)
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
            var type = GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (!options.ContainsKey(property.Name))
                {
                    continue;
                }

                LoadOptionSetProperty(options, property);
            }
        }

        /// <summary>
        /// Can be overridden by child actions or triggers to allow loading more complex types.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        protected virtual void LoadOptionSetProperty(MappingAspectOptions options, PropertyInfo property)
        {
            var value = options[property.Name];
            
            if (property.PropertyType.IsEnum)
            {
                value = Enum.Parse(property.PropertyType, (string)value);
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                value = new DateTime(Convert.ToInt64(value));
            }

            property.SetValue(this, value);
        }

        public virtual int CompareTo(AbstractMappingAspect other)
        {
            return ToString().CompareTo(other.ToString());
        }

        public static bool operator ==(AbstractMappingAspect a, AbstractMappingAspect b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null)
                || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(AbstractMappingAspect a, AbstractMappingAspect b) => !(a == b);

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
