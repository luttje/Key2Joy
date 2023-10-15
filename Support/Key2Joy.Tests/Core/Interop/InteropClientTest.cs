using Key2Joy.Interop;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Pipes;
using Moq;
using System.Threading.Tasks;
using Key2Joy.Tests.Testing;
using Key2Joy.Interop.Commands;

namespace Key2Joy.Tests.Core.Interop;

public class MockInteropClient : InteropClient
{
    public MockInteropClient(ICommandRepository commandRepository)
        : base(commandRepository)
    {
    }

    protected override string GetPipeName() => InteropClientTest.TestPipeName;

    internal static MockInteropClient CreateTestInstance(ICommandRepository commandRepository)
        => new(commandRepository);
}

[TestClass]
public class InteropClientTest
{
    internal const string TestPipeName = "Key2Joy.Tets.InteropClientTest.TestPipe";

    private Mock<CommandRepository> commandRepositoryMock;

    [TestInitialize]
    public void SetUp()
        => this.commandRepositoryMock = new Mock<CommandRepository>(true);

    [TestMethod]
    public async Task SendCommand_Successful()
    {
        var client = MockInteropClient.CreateTestInstance(this.commandRepositoryMock.Object);
        object receivedCommand = null;
        var tcs = new TaskCompletionSource<object>();

        using var pipeServer = new NamedPipeServerStream(
            TestPipeName,
            PipeDirection.InOut,
            1,
            PipeTransmissionMode.Message,
            PipeOptions.Asynchronous);

        var mock = new Mock<AsyncCallback>();
        mock.Setup(x => x.Invoke(It.IsAny<IAsyncResult>()))
            .Callback((IAsyncResult asyncResult) =>
            {
                pipeServer.EndWaitForConnection(asyncResult);

                var commandId = InteropServer.ReadCommandId(pipeServer);
                var commandInfo = this.commandRepositoryMock.Object.GetCommandInfo(commandId);
                receivedCommand = InteropServer.ReadCommand(pipeServer, commandInfo);

                // Set the result of the task completion source when the command is received
                tcs.SetResult(receivedCommand);
            });

        pipeServer.BeginWaitForConnection(mock.Object, pipeServer);

        var command = new EnableCommand
        {
            ProfilePath = "Path to profile (test)"
        };
        var dbg = this.commandRepositoryMock.Object.GetCommandInfoTypes();
        client.SendCommand(command);

        await TestUtilities.TestAsyncMethodWithTimeout(
            tcs.Task,
            TimeSpan.FromMilliseconds(50) // The result should return almost instantly, but let's give laggy tests some space to breathe
        );

        Assert.IsNotNull(receivedCommand);
        Assert.IsInstanceOfType(receivedCommand, typeof(EnableCommand));
        Assert.AreEqual(command.ProfilePath, ((EnableCommand)receivedCommand).ProfilePath);
    }
}
