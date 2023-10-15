using System.Linq;
using Key2Joy.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Config;

public class TestConfigState
{
    [BooleanConfigControl(
        Text = "Mute informative message about this app minimizing by default"
    )]
    public bool MuteCloseExitMessage { get; set; }

    [BooleanConfigControl(
        Text = "Override default behaviour when trigger action is executed"
    )]
    public bool OverrideDefaultTriggerBehaviour { get; set; }

    [TextConfigControl(
        Text = "Last loaded mapping profile file location"
    )]
    public string LastLoadedProfile { get; set; }
}

[TestClass]
public class AttributeProviderTests
{
    private IAttributeProvider attributeProvider;

    [TestInitialize]
    public void Setup() => this.attributeProvider = new AttributeProvider();

    [TestMethod]
    public void GetProperties_ReturnsAllPropertiesOfType()
    {
        var properties = this.attributeProvider.GetProperties(typeof(TestConfigState));

        Assert.IsTrue(properties.Any(p => p.Name == "MuteCloseExitMessage"));
        Assert.IsTrue(properties.Any(p => p.Name == "OverrideDefaultTriggerBehaviour"));
        Assert.IsTrue(properties.Any(p => p.Name == "LastLoadedProfile"));
    }

    [TestMethod]
    public void GetCustomConfigControlAttribute_ReturnsCorrectAttribute()
    {
        var property = typeof(TestConfigState).GetProperty("MuteCloseExitMessage");

        var attribute = this.attributeProvider.GetCustomConfigControlAttribute(property);

        Assert.IsNotNull(attribute);
        Assert.IsInstanceOfType(attribute, typeof(BooleanConfigControlAttribute));
    }

    // A dummy class without any attributes for this test
    private class TestClass
    {
        public string NoAttributeProperty { get; set; }
    }

    [TestMethod]
    public void GetCustomConfigControlAttribute_ReturnsNullWhenNoAttribute()
    {
        var property = typeof(TestClass).GetProperty("NoAttributeProperty");

        var attribute = this.attributeProvider.GetCustomConfigControlAttribute(property);

        Assert.IsNull(attribute);
    }
}
