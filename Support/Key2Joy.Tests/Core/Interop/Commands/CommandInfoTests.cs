using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Key2Joy.Interop.Commands;
using CommonServiceLocator;

namespace Key2Joy.Tests.Core.Interop.Commands;

[Command(Id)]
[StructLayout(LayoutKind.Sequential)]
public struct TestCommand
{
    public const int Id = 0xFF;
}

[Command(Id)]
[StructLayout(LayoutKind.Sequential)]
public struct UnregisteredCommand
{
    public const int Id = 0xAA;
}

[TestClass]
public class CommandInfoTests
{
    private ICommandRepository commandRepository;

    [TestInitialize]
    public void Initialize()
    {
        this.commandRepository = ServiceLocator.Current.GetInstance<ICommandRepository>();
        this.commandRepository.Register(TestCommand.Id, typeof(TestCommand));
    }

    [TestCleanup]
    public void Cleanup() => this.commandRepository.Unregister(TestCommand.Id);

    [TestMethod]
    public void GetCommandInfo_ByCommandType_ReturnsCorrectInfo()
    {
        var result = this.commandRepository.GetCommandInfo<TestCommand>();

        Assert.IsNotNull(result);
        Assert.AreEqual(TestCommand.Id, result.Id);
    }

    [TestMethod]
    public void GetCommandInfo_ById_ReturnsCorrectInfo()
    {
        var result = this.commandRepository.GetCommandInfo(TestCommand.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual(TestCommand.Id, result.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetCommandInfo_InvalidCommandType_ThrowsArgumentException()
        => this.commandRepository.GetCommandInfo<UnregisteredCommand>();

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetCommandInfo_InvalidId_ThrowsArgumentException()
    {
        byte invalidCommandId = 0xAA; // An ID that is not registered

        this.commandRepository.GetCommandInfo(invalidCommandId);
    }
}
