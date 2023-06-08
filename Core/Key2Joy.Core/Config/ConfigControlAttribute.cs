using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Config
{
    /// <summary>
    /// Only applied to <see cref="ConfigManager"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class ConfigControlAttribute : Attribute
    {
        public string Text { get; set; }

        /// <summary>
        /// Gets all configs and their property
        /// </summary>
        /// <returns></returns>
        public static Dictionary<PropertyInfo, ConfigControlAttribute> GetAllProperties()
        {
            return typeof(ConfigState).GetProperties()
                .Where(p => (p.GetCustomAttribute(typeof(ConfigControlAttribute), false) as ConfigControlAttribute) != null)
                .ToDictionary(
                    p => p,
                    p => p.GetCustomAttribute(typeof(ConfigControlAttribute), false) as ConfigControlAttribute
                );
        }
    }
}
