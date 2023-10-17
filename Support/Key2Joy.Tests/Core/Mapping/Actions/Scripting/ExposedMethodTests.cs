using Key2Joy.Mapping.Actions.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
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

    public override IList<Type> GetParameterTypes()
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
}

[TestClass]
public class ExposedMethodTests
{
    [TestMethod]
    public void Test_Prepare_SetsInstanceAndParameterTypes()
    {
        var exposedMethod = new Mock<MockExposedMethod>() { CallBase = true };
        exposedMethod.Setup(m => m.GetParameterTypes()).Returns(new List<Type> { typeof(string), typeof(int) });

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

        Assert.Fail();
    }

    [TestMethod]
    public void Test_RegisterParameterTransformer_ReplacesExistingTransformer()
    {
        var exposedMethod = new Mock<MockExposedMethod>() { CallBase = true };
        exposedMethod.Setup(m => m.GetParameterTypes()).Returns(new List<Type> { typeof(int) });
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
        exposedMethod.Setup(m => m.GetParameterTypes()).Returns(new List<Type> { typeof(DayOfWeek) });
        exposedMethod.Setup(m => m.InvokeMethod(It.IsAny<object[]>())).Returns<object[]>(p => p[0]);

        var resultingParameters = (object[])exposedMethod.Object.TransformAndRedirect(nameof(DayOfWeek.Monday));

        Assert.Fail();
    }

    [TestMethod]
    public void Test_TransformAndRedirect_EnumConversion()
    {
        var exposedMethod = new Mock<MockExposedMethod>() { CallBase = true };
        exposedMethod.Setup(m => m.GetParameterTypes()).Returns(new List<Type> { typeof(DayOfWeek) });

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
        exposedMethod.Setup(m => m.GetParameterTypes()).Returns(new List<Type> { typeof(string[]), typeof(int[]) });

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
        exposedMethod.Setup(m => m.GetParameterTypes()).Returns(new List<Type> { typeof(MockExposedMethod) });

        exposedMethod.Object.TransformAndRedirect(new object());
    }

    [TestMethod]
    public void Test_TypeExposedMethod_GetParameterTypesIfNone()
    {
        var exposedMethod = new TypeExposedMethod("functionName", nameof(MockType.MethodWithNoParameters), typeof(MockType));

        var parameterTypes = exposedMethod.GetParameterTypes();

        Assert.IsFalse(parameterTypes.Any());
    }

    [TestMethod]
    public void Test_TypeExposedMethod_GetParameterTypes()
    {
        var exposedMethod = new TypeExposedMethod("functionName", nameof(MockType.MethodThatConcatsParameters), typeof(MockType));

        var parameterTypes = exposedMethod.GetParameterTypes().ToList();

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
}
