using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Key2Joy.Config;
using Moq;
using System.Collections.Generic;
using System;

namespace Key2Joy.Tests.Core.Config;

[TestClass]
public class ConfigControlAttributeTests
{
    [TestMethod]
    public void GetAllProperties_ReturnsCorrectProperties()
    {
        var mockProvider = new Mock<IAttributeProvider>();

        var mockProperty1 = new Mock<PropertyInfo>();
        var mockProperty2 = new Mock<PropertyInfo>();
        var mockProperty3 = new Mock<PropertyInfo>();

        mockProvider.Setup(m => m.GetProperties(It.IsAny<Type>())).Returns(new List<PropertyInfo> { mockProperty1.Object, mockProperty2.Object, mockProperty3.Object });
        mockProvider.Setup(m => m.GetCustomConfigControlAttribute(mockProperty1.Object)).Returns(new BooleanConfigControlAttribute());
        mockProvider.Setup(m => m.GetCustomConfigControlAttribute(mockProperty2.Object)).Returns(new NumericConfigControlAttribute()
        {
            Minimum = 0,
            Maximum = 100
        });
        mockProvider.Setup(m => m.GetCustomConfigControlAttribute(mockProperty3.Object)).Returns(new TextConfigControlAttribute() { MaxLength = 255 });

        var properties = ConfigControlAttribute.GetAllProperties(typeof(ConfigState), mockProvider.Object);

        Assert.AreEqual(3, properties.Count);
        Assert.AreEqual(typeof(BooleanConfigControlAttribute), properties.First().Value.GetType());
        Assert.AreEqual(typeof(NumericConfigControlAttribute), properties.Skip(1).First().Value.GetType());
        Assert.AreEqual(typeof(TextConfigControlAttribute), properties.Last().Value.GetType());
    }
}
