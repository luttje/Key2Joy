using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Key2Joy.Util
{
    public static class ExpressionUtil
    {
        /// <summary>
        /// Creates a delegate matching the given signature.
        /// </summary>
        /// <param name="parameterTypes">An array of parameter types for the delegate.</param>
        /// <param name="returnType">The return type of the delegate.</param>
        /// <returns>A Type representing the created delegate type.</returns>
        public static Type GetDelegateType(Type[] parameterTypes, Type returnType)
        {
            // Create a dynamic assembly
            AssemblyName assemblyName = new("DynamicAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

            // Create a dynamic type
            var typeBuilder = moduleBuilder.DefineType(
                "DynamicDelegateType",
                TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.AutoLayout,
                typeof(MulticastDelegate));

            // Define a constructor for the dynamic type
            var constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new[] { typeof(object), typeof(IntPtr) });

            constructorBuilder.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            // Define the Invoke method for the dynamic type
            var methodBuilder = typeBuilder.DefineMethod(
                "Invoke",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                returnType,
                parameterTypes);

            methodBuilder.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            // Create the dynamic type
            var dynamicDelegateType = typeBuilder.CreateType();

            return dynamicDelegateType;
        }
    }
}