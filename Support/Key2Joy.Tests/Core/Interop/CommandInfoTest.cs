using Key2Joy.Interop;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Key2Joy.Tests.Core.Interop;

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
public class CommandInfoTest
{
    [TestInitialize]
    public void Initialize() => CommandRepository.Instance.Register(TestCommand.Id, typeof(TestCommand));

    [TestCleanup]
    public void Cleanup() => CommandRepository.Instance.Unregister(TestCommand.Id);

    [TestMethod]
    public void GetCommandInfo_ByCommandType_ReturnsCorrectInfo()
    {
        var result = CommandRepository.Instance.GetCommandInfo<TestCommand>();

        Assert.IsNotNull(result);
        Assert.AreEqual(TestCommand.Id, result.Id);
    }

    [TestMethod]
    public void GetCommandInfo_ById_ReturnsCorrectInfo()
    {
        var result = CommandRepository.Instance.GetCommandInfo(TestCommand.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual(TestCommand.Id, result.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetCommandInfo_InvalidCommandType_ThrowsArgumentException()
        => CommandRepository.Instance.GetCommandInfo<UnregisteredCommand>();

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetCommandInfo_InvalidId_ThrowsArgumentException()
    {
        byte invalidCommandId = 0xAA; // An ID that is not registered

        CommandRepository.Instance.GetCommandInfo(invalidCommandId);
    }
}
