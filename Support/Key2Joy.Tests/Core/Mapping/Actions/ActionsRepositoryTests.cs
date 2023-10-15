using System;
using System.Collections.Generic;
using System.Linq;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Mapping.Actions;
using Key2Joy.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Key2Joy.Tests.Core.Mapping.Actions;

[TestClass]
public class ActionsRepositoryTests
{
    [TestMethod]
    public void Buffer_WithoutFactoriesAndNoDiscovery_BuffersCorrectly()
    {
        ActionsRepository.Buffer(null, false);

        var allFactories = ActionsRepository.GetAllActions();

        Assert.AreEqual(0, allFactories.Count);
    }

    [TestMethod]
    public void Buffer_WithAdditionalFactoriesAndNoDiscovery_BuffersCorrectly()
    {
        var additionalFactories = new List<MappingTypeFactory<AbstractAction>>
        {
            new Mock<MappingTypeFactory<AbstractAction>>("FullTypeNameTest", null, null).Object,
            new Mock<MappingTypeFactory<AbstractAction>>("FullTypeNameTest2", null, null).Object,
        };

        ActionsRepository.Buffer(additionalFactories, false);

        var allFactories = ActionsRepository.GetAllActions();

        Assert.AreEqual(2, allFactories.Count);

        foreach (var factory in additionalFactories)
        {
            Assert.IsTrue(allFactories.ContainsKey(factory.FullTypeName));
        }
    }

    [TestMethod]
    public void Buffer_WithExistingAdditionalActions_OverwritesThem()
    {
        var additionalFactories = new List<MappingTypeFactory<AbstractAction>>
        {
            new Mock<MappingTypeFactory<AbstractAction>>("FullTypeNameTest", null, null).Object,
            new Mock<MappingTypeFactory<AbstractAction>>("FullTypeNameTest2", null, null).Object,
        };

        ActionsRepository.Buffer(additionalFactories, false);

        var allFactories = ActionsRepository.GetAllActions();

        Assert.AreEqual(2, allFactories.Count);

        var additionalFactories2 = new List<MappingTypeFactory<AbstractAction>>
        {
            new Mock<MappingTypeFactory<AbstractAction>>("FullTypeNameTest", null, null).Object,
            new Mock<MappingTypeFactory<AbstractAction>>("FullTypeNameTest2", null, null).Object,
        };

        ActionsRepository.Buffer(additionalFactories2, false);

        allFactories = ActionsRepository.GetAllActions();

        Assert.AreEqual(2, allFactories.Count);

        foreach (var factory in additionalFactories2)
        {
            Assert.IsTrue(allFactories.ContainsKey(factory.FullTypeName));
        }
    }

    [TestMethod]
    public void GetAllActionAttributes_ReturnsAllAttributes()
    {
        var additionalFactories = new List<MappingTypeFactory<AbstractAction>>
        {
            new Mock<MappingTypeFactory<AbstractAction>>("FullTypeNameTest", new Mock<ActionAttribute>().Object, null).Object,
            new Mock<MappingTypeFactory<AbstractAction>>("FullTypeNameTest2", new Mock<ActionAttribute>().Object, null).Object,
        };

        ActionsRepository.Buffer(additionalFactories, false);

        var allAttributes = ActionsRepository.GetAllActionAttributes();

        Assert.AreEqual(2, allAttributes.Count);

        foreach (var attribute in allAttributes)
        {
            Assert.IsTrue(additionalFactories.Any(factory => factory.Attribute == attribute));
        }
    }

    [TestMethod]
    public void GetAllActions_ReturnsAllActions()
    {
        var additionalFactories = new List<MappingTypeFactory<AbstractAction>>
        {
            new Mock<MappingTypeFactory<AbstractAction>>("FullTypeNameTest", null, null).Object,
            new Mock<MappingTypeFactory<AbstractAction>>("FullTypeNameTest2", null, null).Object,
        };

        ActionsRepository.Buffer(additionalFactories, false);

        var allFactories = ActionsRepository.GetAllActions();

        Assert.AreEqual(2, allFactories.Count);

        foreach (var factory in additionalFactories)
        {
            Assert.IsTrue(allFactories.ContainsKey(factory.FullTypeName));
        }
    }

    [TestMethod]
    public void GetAction_WithExistingAction_ReturnsCorrectAction()
    {
        var additionalFactories = new List<MappingTypeFactory<AbstractAction>>
        {
            new Mock<MappingTypeFactory<AbstractAction>>("FullTypeNameTest", null, null).Object,
            new Mock<MappingTypeFactory<AbstractAction>>("FullTypeNameTest2", null, null).Object,
        };

        ActionsRepository.Buffer(additionalFactories, false);
        var allFactories = ActionsRepository.GetAllActions();

        var typeMock = new Mock<Type>();
        typeMock.Setup(t => t.FullName).Returns("FullTypeNameTest2");

        var action = ActionsRepository.GetAction(typeMock.Object);

        Assert.IsNotNull(action);
        Assert.IsNotNull(additionalFactories[1]);
    }
}
