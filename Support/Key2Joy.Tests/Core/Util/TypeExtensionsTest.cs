using System;
using System.Collections.Generic;
using Key2Joy.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Util;

public class CoolList<T> : List<T>
{ }

[TestClass]
public class TypeExtensionsTest
{
    [TestMethod]
    [DataRow(typeof(List<int>))]
    [DataRow(typeof(List<string>))]
    [DataRow(typeof(List<bool>))]
    [DataRow(typeof(List<object>))]
    public void IsList_DetectList(Type listType) => Assert.IsTrue(listType.IsList());

    [TestMethod]
    [DataRow(typeof(Dictionary<int, string>))]
    [DataRow(typeof(string))]
    [DataRow(typeof(bool))]
    [DataRow(typeof(object))]
    public void IsList_DetectNotList(Type notListType) => Assert.IsFalse(notListType.IsList());

    [TestMethod]
    public void IsSubclassOfRawGeneric_Should_Return_True_For_Subclass_Of_Generic_Type()
    {
        var genericType = typeof(List<>);
        var subclassType = typeof(List<int>);

        var result = genericType.IsSubclassOfRawGeneric(subclassType);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsSubclassOfRawGeneric_Should_Return_False_For_Non_Subclass_Of_Generic_Type()
    {
        var genericType = typeof(List<>);
        var nonSubclassType = typeof(HashSet<int>);

        var result = genericType.IsSubclassOfRawGeneric(nonSubclassType);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsSubclassOfRawGeneric_Should_Return_False_For_Null_Input()
    {
        var genericType = typeof(List<>);

        var result = genericType.IsSubclassOfRawGeneric(null);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void CopyArrayToNewType_ShouldCopyStringArrayToIntegerArray()
    {
        string[] originalArray = { "1", "2", "3" };
        var targetType = typeof(int);

        var newArray = originalArray.CopyArrayToNewType(targetType);

        Assert.IsInstanceOfType(newArray, targetType.MakeArrayType());
        CollectionAssert.AreEqual(new int[] { 1, 2, 3 }, newArray);
    }

    [TestMethod]
    public void CopyArrayToNewType_ShouldCopyIntArrayToStringArray()
    {
        object[] originalArray = { 1, 2, 3 };
        var targetType = typeof(string);

        var newArray = originalArray.CopyArrayToNewType(targetType);

        Assert.IsInstanceOfType(newArray, targetType.MakeArrayType());
        CollectionAssert.AreEqual(new string[] { "1", "2", "3" }, newArray);
    }

    [TestMethod]
    public void CopyArrayToNewType_ShouldCopyObjectArrayToDoubleArray()
    {
        object[] originalArray = { 1.1, 2.2, 3.3 };
        var targetType = typeof(double);

        var newArray = originalArray.CopyArrayToNewType(targetType);

        Assert.IsInstanceOfType(newArray, targetType.MakeArrayType());
        CollectionAssert.AreEqual(new double[] { 1.1, 2.2, 3.3 }, newArray);
    }

    [TestMethod]
    public void CopyArrayToNewType_ShouldCopyEmptyArray()
    {
        string[] originalArray = { };
        var targetType = typeof(int);

        var newArray = originalArray.CopyArrayToNewType(targetType);

        Assert.IsInstanceOfType(newArray, targetType.MakeArrayType());
        Assert.AreEqual(0, newArray.Length);
    }
}
