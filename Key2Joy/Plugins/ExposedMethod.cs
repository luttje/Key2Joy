using Key2Joy.Contracts.Mapping;
using Key2Joy.Util;
using System;
using System.Linq;
using System.Reflection;

namespace Key2Joy.Plugins
{
    public abstract class ExposedMethod
    {
        public string FunctionName { get; protected set; }
        public string MethodName { get; protected set; }

        public ExposedMethod(string functionName, string methodName)
        {
            FunctionName = functionName;
            MethodName = methodName;
        }

        public abstract Delegate CreateDelegate(AbstractAction instance);
    }

    public class TypeExposedMethod : ExposedMethod
    {
        public Type Type { get; protected set; }

        public TypeExposedMethod(string functionName, string methodName, Type type) 
            : base(functionName, methodName)
        {
            Type = type;
        }

        public override Delegate CreateDelegate(AbstractAction instance)
        {
            var method = Type.GetMethod(MethodName);

            return method.CreateDelegate(instance);
        }
    }

    public class AppDomainExposedMethod : ExposedMethod
    {
        public AppDomain AppDomain { get; protected set; }
        public string AssemblyPath { get; protected set; }
        public string TypeName { get; protected set; }

        public AppDomainExposedMethod(AppDomain appDomain, string assemblyPath, string typeName, string functionName, string methodName)
            : base(functionName, methodName)
        {
            AppDomain = appDomain;
            AssemblyPath = assemblyPath;
            TypeName = typeName;
        }

        public override Delegate CreateDelegate(AbstractAction instance)
        {
            throw new NotImplementedException();
        }
    }
}
