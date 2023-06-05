using System;

namespace Key2Joy.Contracts.Util
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ObjectListViewGroupAttribute : Attribute
    {
        /// <summary>
        /// Display name for the image group
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Resource name of the image
        /// </summary>
        public string Image { get; set; }
    }
}
