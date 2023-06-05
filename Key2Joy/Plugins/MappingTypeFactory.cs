using Key2Joy.Contracts.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Plugins
{
    /// <summary>
    /// Creates instances of types, simply using Activator.CreateInstance
    /// </summary>
    public class MappingTypeFactory
    {
        public string FullTypeName { get; private set; }

        public MappingAttribute Attribute { get; private set; }
        public IEnumerable<ExposedMethod> ExposedMethods { get; private set; }

        public MappingTypeFactory(string fullTypeName, MappingAttribute attribute, IEnumerable<ExposedMethod> exposedMethods = null)
        {
            FullTypeName = fullTypeName;
            Attribute = attribute;
            ExposedMethods = exposedMethods ?? new List<ExposedMethod>();
        }

        public virtual T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(ToType(), new object[] 
            { 
                Attribute.NameFormat,
                Attribute.Description,
            });
        }

        public virtual Type ToType()
        {
            return Type.GetType(FullTypeName);
        }
    }
    
    /// <summary>
    /// Creates instances of types, simply using Activator.CreateInstance
    /// </summary>
    public class MappingTypeFactory<T> : MappingTypeFactory where T : AbstractMappingAspect
    {
        public MappingTypeFactory(string fullTypeName, MappingAttribute attribute, IEnumerable<ExposedMethod> exposedMethods = null)
            : base(fullTypeName, attribute, exposedMethods)
        {
        }

        public virtual T CreateInstance()
        {
            return base.CreateInstance<T>();
        }
    }

    /// <summary>
    /// Creates a type using the specified AppDomain.
    /// </summary>
    public class AppDomainMappingTypeFactory<T> : MappingTypeFactory<T> where T : AbstractMappingAspect
    {
        private AppDomain appDomain;
        private Type hostBaseType;
        private string pluginAssemblyPath;

        public AppDomainMappingTypeFactory(AppDomain appDomain, string pluginAssemblyPath, Type hostBaseType, string fullTypeName, MappingAttribute attribute, IReadOnlyList<ExposedMethod> exposedMethods = null)
            : base(fullTypeName, attribute, exposedMethods)
        {
            this.appDomain = appDomain;
            this.pluginAssemblyPath = pluginAssemblyPath;
            this.hostBaseType = hostBaseType;
        }

        public override T CreateInstance()
        {
            return (T)appDomain.CreateInstanceFromAndUnwrap(pluginAssemblyPath, FullTypeName);
        }

        /// <summary>
        /// Since we can't get the Type in the other appdomain, we return the host/contract class it derives from instead.
        /// </summary>
        /// <returns></returns>
        public override Type ToType()
        {
            return hostBaseType;
        }
    }
}
