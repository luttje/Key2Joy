using Key2Joy.Mapping.Actions.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Key2Joy.Tests.Core.Mapping.Actions.Scripting;

public class MockExposedMethod : ExposedMethod
{
    public MockExposedMethod()
        : base("functionName", "methodName")
    { }

    public MockExposedMethod(string functionName, string methodName)
        : base(functionName, methodName)
    { }

    public override IList<Type> GetParameterTypes(out IList<object> parameterDefaultValues, out bool isLastParameterParams)
        => throw new NotImplementedException();

    public override object InvokeMethod(object[] transformedParameters)
        => transformedParameters;

    public object GetInstance() => this.Instance;
}

public class MockType
{
    public void MethodWithNoParameters()
    { }

    public string MethodThatConcatsParameters(string p1, int p2)
        => p1 + p2;

    public string MethodThatConcatsFloatParameters(float p1, float p2)
        => $"{p1.ToString(new CultureInfo("en_US"))}|{p2.ToString(new CultureInfo("en_US"))}";

    public string MethodThatConcatsParametersAndHasParams(string p1, int p2, params string[] p3)
        => p1 + p2 + string.Join("/", p3);

    public string MethodWithDefaultParameters(string p1, int p2 = 2)
        => $"{p1}|{p2}";
}

[TestClass]
public class ExposedMethodTests
{
    [TestMethod]
    public void Test_Prepare_SetsInstanceAndParameterTypes()
    {
        var exposedMethod = new Mock<MockExposedMethod>() { CallBase = true };

        IList<object> _;
        bool __;
        exposedMethod.Setup(m => m.GetParameterTypes(out _, out __)).Returns(new List<Type> { typeof(string), typeof(int) });

        var instance = new object();
        exposedMethod.Object.Prepare(instance);

        Assert.AreEqual(instance, exposedMethod.Object.GetInstance());

        var resultingParameters = (object[])exposedMethod.Object.TransformAndRedirect("test", 2);
        Assert.IsInstanceOfType(resultingParameters[0], typeof(string));
        Assert.IsInstanceOfType(resultingParameters[1], typeof(int));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test_RegisterParameterTransformer_FailsIfNotPrepared()
    {
        var exposedMethod = new Mock<MockExposedMethod>() { CallBase = true };

        exposedMethod.Object.RegisterParameterTransformer<int>((p, t) => p * 2);
    }

    [TestMethod]
    public void Test_RegisterParameterTransformer_ReplacesExistingTransformer()
    {
        var exposedMethod = new Mock<MockExposedMethod>() { CallBase = true };

        IList<object> _;
        bool __;
        exposedMethod.Setup(m => m.GetParameterTypes(out _, out __)).Returns(new List<Type> { typeof(int) });
        var instance = new object();
        exposedMethod.Object.Prepare(instance);

        exposedMethod.Object.RegisterParameterTransformer<int>((p, t) => p * 2);
        exposedMethod.Object.RegisterParameterTransformer<int>((p, t) => p * 3);

        var resultingParameters = (object[])exposedMethod.Object.TransformAndRedirect(1);
        Assert.AreEqual(3, resultingParameters[0]); // Ensures the transformer was replaced, not added
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test_TransformAndRedirect_FailsIfNotPrepared()
    {
        var exposedMethod = new Mock<MockExposedMethod>() { CallBase = true };

        IList<object> _;
        bool __;
        exposedMethod.Setup(m => m.GetParameterTypes(out _, out __)).Returns(new List<Type> { typeof(DayOfWeek) });
        exposedMethod.Setup(m => m.InvokeMethod(It.IsAny<object[]>())).Returns<object[]>(p => p[0]);

        var resultingParameters = (object[])exposedMethod.Object.TransformAndRedirect(nameof(DayOfWeek.Monday));
    }

    [TestMethod]
    public void Test_TransformAndRedirect_EnumConversion()
    {
        var exposedMethod = new Mock<MockExposedMethod>() { CallBase = true };

        IList<object> _;
        bool __;
        exposedMethod.Setup(m => m.GetParameterTypes(out _, out __)).Returns(new List<Type> { typeof(DayOfWeek) });

        var instance = new object();
        exposedMethod.Object.Prepare(instance);

        var resultingParameters = (object[])exposedMethod.Object.TransformAndRedirect(nameof(DayOfWeek.Monday));
        var resultingParameters2 = (object[])exposedMethod.Object.TransformAndRedirect((int)DayOfWeek.Monday);

        Assert.AreEqual(DayOfWeek.Monday, resultingParameters[0]);
        Assert.AreEqual(DayOfWeek.Monday, resultingParameters2[0]);
    }

    [TestMethod]
    public void Test_TransformAndRedirect_ObjectArrayConversion()
    {
        var exposedMethod = new Mock<MockExposedMethod>() { CallBase = true };

        IList<object> _;
        bool __;
        exposedMethod.Setup(m => m.GetParameterTypes(out _, out __)).Returns(new List<Type> { typeof(string[]), typeof(int[]) });

        var instance = new object();
        exposedMethod.Object.Prepare(instance);

        var objectArray = new object[] { "string", "anotherString" };
        var intArray = new int[] { 1, 2 };
        var resultingParameters = (object[])exposedMethod.Object.TransformAndRedirect(objectArray, intArray);

        Assert.IsInstanceOfType<string[]>(resultingParameters[0]);
        Assert.AreEqual(((string[])resultingParameters[0])[1], "anotherString");

        Assert.IsInstanceOfType<int[]>(resultingParameters[1]);
        Assert.AreEqual(((int[])resultingParameters[1])[1], 2);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test_TransformAndRedirect_InvalidOperationException()
    {
        var exposedMethod = new Mock<MockExposedMethod>() { CallBase = true };

        IList<object> _;
        bool __;
        exposedMethod.Setup(m => m.GetParameterTypes(out _, out __)).Returns(new List<Type> { typeof(MockExposedMethod) });

        exposedMethod.Object.TransformAndRedirect(new object());
    }

    [TestMethod]
    public void Test_TypeExposedMethod_GetParameterTypesIfNone()
    {
        var exposedMethod = new TypeExposedMethod("functionName", nameof(MockType.MethodWithNoParameters), typeof(MockType));

        var parameterTypes = exposedMethod.GetParameterTypes(out var _, out var _);

        Assert.IsFalse(parameterTypes.Any());
    }

    [TestMethod]
    public void Test_TypeExposedMethod_GetParameterTypes()
    {
        var exposedMethod = new TypeExposedMethod("functionName", nameof(MockType.MethodThatConcatsParameters), typeof(MockType));

        var parameterTypes = exposedMethod.GetParameterTypes(out var _, out var _).ToList();

        CollectionAssert.AreEqual(parameterTypes, new Type[] { typeof(string), typeof(int) });
    }

    [TestMethod]
    public void Test_TypeExposedMethod_InvokeMethod()
    {
        var exposedMethod = new TypeExposedMethod("functionName", nameof(MockType.MethodThatConcatsParameters), typeof(MockType));

        var instance = new MockType();
        exposedMethod.Prepare(instance);

        var result = exposedMethod.InvokeMethod(new object[] { "1", 23 });

        Assert.AreEqual("123", result);
    }

    [TestMethod]
    public void Test_TypeExposedMethod_InvokeMethodLenientCast()
    {
        var exposedMethod = new TypeExposedMethod("functionName", nameof(MockType.MethodThatConcatsFloatParameters), typeof(MockType));

        var instance = new MockType();
        exposedMethod.Prepare(instance);

        var result = (string)exposedMethod.TransformAndRedirect(new object[] { 1.5d, 2.5d });

        Assert.AreEqual("1.5|2.5", result);
    }

    [TestMethod]
    public void Test_TypeExposedMethod_InvokeMethodImpliedEmptyParams()
    {
        var exposedMethod = new TypeExposedMethod("functionName", nameof(MockType.MethodThatConcatsParametersAndHasParams), typeof(MockType));

        var instance = new MockType();
        exposedMethod.Prepare(instance);

        var result = (string)exposedMethod.TransformAndRedirect(new object[] { "1", 23 }); // No params

        Assert.AreEqual("123", result);
    }

    [TestMethod]
    public void Test_TypeExposedMethod_InvokeMethodWithDefaultParameters()
    {
        var exposedMethod = new TypeExposedMethod("functionName", nameof(MockType.MethodWithDefaultParameters), typeof(MockType));

        var instance = new MockType();
        exposedMethod.Prepare(instance);

        var result = (string)exposedMethod.TransformAndRedirect(new object[] { "1" });

        Assert.AreEqual("1|2", result);
    }
}
