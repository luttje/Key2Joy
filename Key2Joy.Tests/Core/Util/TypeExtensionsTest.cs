using Key2Joy.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Key2Joy.Tests.Core.Util
{
    public class CoolList<T> : List<T> { }


    [TestClass]
    public class TypeExtensionsTest
    {
        [TestMethod]
        [DataRow(typeof(List<int>))]
        [DataRow(typeof(List<string>))]
        [DataRow(typeof(List<bool>))]
        [DataRow(typeof(List<object>))]
        public void IsList_DetectList(Type listType)
        {
            Assert.IsTrue(listType.IsList());
        }

        //[TestMethod]
        //public void IsList_DetectsSubList()
        //{
        //    Assert.IsTrue(typeof(CoolList<int>).IsList());
        //}

        [TestMethod]
        [DataRow(typeof(Dictionary<int, string>))]
        [DataRow(typeof(string))]
        [DataRow(typeof(bool))]
        [DataRow(typeof(object))]
        public void IsList_DetectNotList(Type notListType)
        {
            Assert.IsFalse(notListType.IsList());
        }

    }
}
