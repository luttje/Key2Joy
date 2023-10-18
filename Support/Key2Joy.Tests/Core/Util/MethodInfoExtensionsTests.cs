using Key2Joy.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Key2Joy.Tests.Core.Util;

[TestClass]
public class MethodInfoExtensionsTests
{
    private class TestClass
    {
        public int InitialValue { get; set; } = 0;

        public int TestMethod(int x, int y)
            => this.InitialValue + x + y;
    }

    [TestMethod]
    public void CreateDelegate_FromInstanceMethod_CreatesValidDelegate()
    {
        var method = typeof(TestClass).GetMethod(nameof(TestClass.TestMethod));
        var instance = new TestClass()
        {
            InitialValue = 10
        };

        var del = method.CreateDelegate(instance) as Func<int, int, int>;

        Assert.IsNotNull(del);
        Assert.AreEqual(15, del(2, 3));
    }
}
