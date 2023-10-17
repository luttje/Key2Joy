using BuildMarkdownDocs.Util;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Linq;

namespace Key2Joy.Tests.BuildMarkdownDocs.Util;

[TestClass]
public class TypeUtilTests
{
    public void TestParameterlessMethod()
    { }

    public void TestMethodWithParameters(string p1, int p2)
    { }

    public void TestMethodWithParameters2(int p1, string p2)
    { }

    public static void TestStaticMethodWithParameters(bool p1, string p2)
    { }

    public class NestedType
    { }

    public static MethodInfo GetMethodInfoWithSignature(string targetMethodInThisClass, out string signature)
    {
        var thisType = typeof(TypeUtilTests);
        var thisFullClassName = thisType.FullName;

        signature = $"M:{thisFullClassName}.{targetMethodInThisClass}";

        var methodInfo = thisType.GetMethod(targetMethodInThisClass);
        return methodInfo;
    }

    [TestMethod]
    public void CleanTypeName_WithNullable()
    {
        var typeName = "System.Nullable{System.Int32}";
        var result = TypeUtil.CleanTypeName(typeName);
        Assert.AreEqual("System.Nullable`1[System.Int32]", result);
    }

    [TestMethod]
    public void CleanTypeName_WithoutNullable()
    {
        var typeName = "System.String";
        var result = TypeUtil.CleanTypeName(typeName);
        Assert.AreEqual("System.String", result);
    }

    [TestMethod]
    public void GetType_WithExistingType()
    {
        var typeName = "System.String";
        var type = TypeUtil.GetType(typeName);
        Assert.IsNotNull(type);
        Assert.AreEqual(typeof(string), type);
    }

    [TestMethod]
    public void GetType_WithNonExistingType()
    {
        var typeName = "Non.Existing.Type";
        var type = TypeUtil.GetType(typeName);
        Assert.IsNull(type);
    }

    [TestMethod]
    public void GetType_WithNestedType()
    {
        var thisClassName = typeof(TypeUtilTests).FullName;
        var typeName = $"{thisClassName}+NestedType";
        var type = TypeUtil.GetType(typeName);
        Assert.IsNotNull(type);
        Assert.AreEqual(typeof(NestedType), type);
    }

    [TestMethod]
    public void ParseParameterTypes_WithMultipleTypes()
    {
        var parameters = "System.Int32, System.String";
        var types = TypeUtil.ParseParameterTypes(parameters);
        Assert.AreEqual(2, types.Length);
        Assert.IsTrue(types.Contains(typeof(int)));
        Assert.IsTrue(types.Contains(typeof(string)));
    }

    [TestMethod]
    public void ParseParameterTypes_WithEmptyString()
    {
        var parameters = "";
        var types = TypeUtil.ParseParameterTypes(parameters);
        Assert.AreEqual(0, types.Length);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetMethodInfo_WithInvalidType()
    {
        var signature = "M:Invalid.Type.Method";
        TypeUtil.GetMethodInfo(signature);
    }

    [TestMethod]
    public void GetMethodInfo_ValidSignature_ReturnsMethodInfo()
    {
        var expectedMethodInfo = GetMethodInfoWithSignature(nameof(TestParameterlessMethod), out var signature);
        var methodInfo = TypeUtil.GetMethodInfo(signature);

        Assert.IsNotNull(methodInfo);
        Assert.AreEqual(expectedMethodInfo, methodInfo);
    }

    [TestMethod]
    public void GetMethodInfo_ValidSignatureWithEmptyParams_ReturnsMethodInfo()
    {
        var expectedMethodInfo = GetMethodInfoWithSignature(nameof(TestParameterlessMethod), out var signature);
        signature += "()";
        var methodInfo = TypeUtil.GetMethodInfo(signature);

        Assert.IsNotNull(methodInfo);
        Assert.AreEqual(expectedMethodInfo, methodInfo);
    }

    [TestMethod]
    public void GetMethodInfo_ValidSignatureWithParams_ReturnsMethodInfo()
    {
        var expectedMethodInfo = GetMethodInfoWithSignature(nameof(TestMethodWithParameters), out var signature);
        signature += "(System.String,System.Int32)";
        var methodInfo = TypeUtil.GetMethodInfo(signature);

        Assert.IsNotNull(methodInfo);
        Assert.AreEqual(expectedMethodInfo, methodInfo);
    }

    [TestMethod]
    public void GetMethodInfo_ValidSignatureWithParams2_WrongParams_ThrowsArgumentException()
    {
        var expectedMethodInfo = GetMethodInfoWithSignature(nameof(TestMethodWithParameters2), out var signature);
        var invalidSignature = $"{signature}(System.String,System.Int32)"; // wrong order (doesnt exist)

        Assert.ThrowsException<ArgumentException>(() => TypeUtil.GetMethodInfo(invalidSignature));
    }

    [TestMethod]
    public void GetMethodInfo_ValidSignatureWithParams2_ReturnsMethodInfo()
    {
        var expectedMethodInfo = GetMethodInfoWithSignature(nameof(TestMethodWithParameters2), out var signature);
        signature += "(System.Int32,System.String)";
        var methodInfo = TypeUtil.GetMethodInfo(signature);

        Assert.IsNotNull(methodInfo);
        Assert.AreEqual(expectedMethodInfo, methodInfo);
    }

    [TestMethod]
    public void GetMethodInfo_ValidSignatureWithParamsStatic_ReturnsMethodInfo()
    {
        var expectedMethodInfo = GetMethodInfoWithSignature(nameof(TestStaticMethodWithParameters), out var signature);
        signature += "(System.Boolean,System.String)";
        var methodInfo = TypeUtil.GetMethodInfo(signature);

        Assert.IsNotNull(methodInfo);
        Assert.AreEqual(expectedMethodInfo, methodInfo);
    }

    [TestMethod]
    public void GetMethodInfo_InvalidSignature_ThrowsArgumentException()
    {
        var invalidSignature = "InvalidSignature";

        Assert.ThrowsException<ArgumentException>(() => TypeUtil.GetMethodInfo(invalidSignature));
    }

    [TestMethod]
    public void GetMethodInfo_SignatureWithInvalidType_ThrowsArgumentException()
    {
        var invalidTypeSignature = "M:InvalidType.MethodName";

        Assert.ThrowsException<ArgumentException>(() => TypeUtil.GetMethodInfo(invalidTypeSignature));
    }

    [TestMethod]
    public void GetMethodInfo_SignatureWithInvalidMethod_ThrowsArgumentException()
    {
        var invalidMethodSignature = "M:Namespace.ClassName.InvalidMethod";

        Assert.ThrowsException<ArgumentException>(() => TypeUtil.GetMethodInfo(invalidMethodSignature));
    }

    [TestMethod]
    public void GetMethodInfo_SignatureWithInvalidParameter_ThrowsArgumentException()
    {
        var invalidParameterSignature = "M:Namespace.ClassName.MethodName(InvalidParameter)";

        Assert.ThrowsException<ArgumentException>(() => TypeUtil.GetMethodInfo(invalidParameterSignature));
    }

    [TestMethod]
    public void GetMethodInfo_SignatureWithNonexistentType_ThrowsArgumentException()
    {
        var nonexistentTypeSignature = "M:NonexistentNamespace.NonexistentClass.NonexistentMethod()";

        Assert.ThrowsException<ArgumentException>(() => TypeUtil.GetMethodInfo(nonexistentTypeSignature));
    }
}
