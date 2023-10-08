﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                throw new ArgumentNullException("Parameter type should be provided!");
            
            var parameter = new Parameter();
            parameter.Name = element.Attribute("name").Value;
            parameter.Description = element.Value;
            parameter.Type = type;
            
            parameter.nullableType = Nullable.GetUnderlyingType(type);
            parameter.IsOptional = parameter.nullableType != null;

            return parameter;
        }

        internal object GetTypeName(bool includeOptionalMarkerIfApplicable = true)
        {
            if (IsOptional)
                return $"{nullableType.Name}" + (includeOptionalMarkerIfApplicable ? "?" : string.Empty);

            return Type.Name;
        }
    }
}