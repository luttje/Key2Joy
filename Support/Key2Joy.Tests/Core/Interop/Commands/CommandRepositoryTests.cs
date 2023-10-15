using System;
using System.Data;
using Key2Joy.Interop.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Interop.Commands;

[TestClass]
public class CommandRepositoryTests
{
    [TestMethod]
    public void TestCommandRegistration()
    {
        var repository = new CommandRepository(false);
        byte id = 1;
        var type = typeof(string); // Using System.String type for the sake of example

        repository.Register(id, type);
        var commandInfo = repository.GetCommandInfoTypes();

        Assert.IsTrue(commandInfo.ContainsKey(id));
        Assert.AreEqual(type, commandInfo[id].StructType);
    }

    [TestMethod]
    [ExpectedException(typeof(DuplicateNameException))]
    public void TestDuplicateCommandRegistration()
    {
        var repository = new CommandRepository(false);
        byte id = 1;
        var type = typeof(string);

        repository.Register(id, type);
        repository.Register(id, type); // Duplicate registration
    }

    [TestMethod]
    public void TestCommandUnregistration()
    {
        var repository = new CommandRepository(false);
        byte id = 1;
        var type = typeof(string);
        repository.Register(id, type);

        repository.Unregister(id);
        var commandInfo = repository.GetCommandInfoTypes();

        Assert.IsFalse(commandInfo.ContainsKey(id));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestRetrieveNonRegisteredCommand()
    {
        var repository = new CommandRepository(false);
        byte id = 2; // Non-registered command ID

        var commandInfo = repository.GetCommandInfo(id); // This should throw an exception
    }
}
