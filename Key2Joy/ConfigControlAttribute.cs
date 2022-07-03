using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy
{
    /// <summary>
    /// Only applied to <see cref="Config"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigControlAttribute : Attribute
    {
        public string Text { get; set; }
        public Type ControlType { get; set; }
        
        /// <summary>
        /// Gets all configs and their property
        /// </summary>
        /// <returns></returns>
        public static Dictionary<PropertyInfo, ConfigControlAttribute> GetAllProperties()
        {
            return typeof(Config).GetProperties()
                .Where(p => (p.GetCustomAttribute(typeof(ConfigControlAttribute), false) as ConfigControlAttribute) != null)
                .ToDictionary(
                    p => p,
                    p => p.GetCustomAttribute(typeof(ConfigControlAttribute), false) as ConfigControlAttribute
                );
        }
    }
}
