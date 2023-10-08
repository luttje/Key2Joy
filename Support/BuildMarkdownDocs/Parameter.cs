using System;
using System.Xml.Linq;

namespace BuildMarkdownDocs
{
    internal class Parameter
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string Description { get; set; }

        public bool IsOptional { get; set; }
        private Type nullableType = null;

        public static Parameter FromXml(XElement element, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("Parameter type should be provided!");
            }

            Parameter parameter = new()
            {
                Name = element.Attribute("name").Value,
                Description = element.Value,
                Type = type,

                nullableType = Nullable.GetUnderlyingType(type)
            };
            parameter.IsOptional = parameter.nullableType != null;

            return parameter;
        }

        internal object GetTypeName(bool includeOptionalMarkerIfApplicable = true)
        {
            if (this.IsOptional)
            {
                return $"{this.nullableType.Name}" + (includeOptionalMarkerIfApplicable ? "?" : string.Empty);
            }

            return this.Type.Name;
        }
    }
}
