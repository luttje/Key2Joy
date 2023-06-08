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
    public class TextConfigControlAttribute : ConfigControlAttribute
    {
        public int MaxLength { get; set; }
    }
}
