using Jint.Native;
using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;

namespace Key2Joy.Plugins
{
    public class MappingTypeHelper
    {
        /// <summary>
        /// Ensures the typename is valid, splitting the long variant into its short form
        /// </summary>
        /// <param name="typeInfoTypeName"></param>
        /// <returns></returns>
        public static string EnsureSimpleTypeName(string typeInfoTypeName)
        {
            return typeInfoTypeName.Split(',')[0];
        }

        /// <summary>
        /// Gets the typename, even if the object is a proxy
        /// </summary>
        /// <param name="typeFactories"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetTypeFullName<T>(IDictionary<string, MappingTypeFactory<T>> typeFactories, AbstractMappingAspect instance) 
            where T : AbstractMappingAspect
        {
            string realTypeName;

            if (RemotingServices.IsTransparentProxy(instance))
            {
                var objRef = RemotingServices.GetObjRefForProxy(instance);
                realTypeName = objRef.TypeInfo.TypeName;

                if (!typeFactories.ContainsKey(EnsureSimpleTypeName(realTypeName)))
                {
                    throw new ArgumentException("Only allowed types may be (de)serialized");
                }
            }
            else
            {
                realTypeName = instance.GetType().FullName;

                if (!typeFactories.ContainsKey(realTypeName))
                {
                    throw new ArgumentException("Only allowed types may be (de)serialized");
                }
            }

            return realTypeName;
        }

        /// <summary>
        /// Gets the typename, even if the object is a proxy
        /// </summary>
        /// <param name="typeFactories"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetTypeFullName(IDictionary<string, MappingTypeFactory> typeFactories, AbstractMappingAspect instance)
        {
            string realTypeName;

            if (RemotingServices.IsTransparentProxy(instance))
            {
                var objRef = RemotingServices.GetObjRefForProxy(instance);
                realTypeName = objRef.TypeInfo.TypeName;

                if (!typeFactories.ContainsKey(EnsureSimpleTypeName(realTypeName)))
                {
                    throw new ArgumentException("Only allowed types may be (de)serialized");
                }
            }
            else
            {
                realTypeName = instance.GetType().FullName;

                if (!typeFactories.ContainsKey(realTypeName))
                {
                    throw new ArgumentException("Only allowed types may be (de)serialized");
                }
            }

            return realTypeName;
        }
    }
}
