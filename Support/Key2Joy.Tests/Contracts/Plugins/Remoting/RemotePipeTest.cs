using Key2Joy.Contracts.Plugins.Remoting;
using System.IO.Pipes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Key2Joy.Tests.Contracts.Plugins.Remoting;

[TestClass]
public class RemotePipeTest
{
    private const string pipeName = "TestPipe";

    [TestMethod]
    public void GetAbsolutePipeName_ShouldReturnCorrectFormat()
    {
        var result = RemotePipe.GetAbsolutePipeName(pipeName);

        Assert.AreEqual($@"\\.\EventPipe.{pipeName}", result);
    }

    [TestMethod]
    public void GetClientPipeName_ShouldReturnCorrectFormat()
    {
        var result = RemotePipe.GetClientPipeName(pipeName);

        Assert.AreEqual($"EventPipe.{pipeName}", result);
    }

    [TestMethod]
    public async Task ReadMessage_ShouldReadMessageFromPipe()
    {
        var absolutePipeName = RemotePipe.GetAbsolutePipeName(pipeName);
        var clientPipeName = RemotePipe.GetClientPipeName(pipeName);
        using var serverPipe = new NamedPipeServerStream(absolutePipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Message);
        using var clientPipe = new NamedPipeClientStream(".", clientPipeName, PipeDirection.InOut);

        var serverTask = Task.Run(() =>
        {
            serverPipe.WaitForConnection();
            RemotePipe.WriteMessage(serverPipe, "Hello, Pipe!");
        });

        await clientPipe.ConnectAsync();
        clientPipe.ReadMode = PipeTransmissionMode.Message;
        var result = await Task.Run(() => RemotePipe.ReadMessage(clientPipe));

        await serverTask;
        Assert.AreEqual("Hello, Pipe!", result);
    }

    [TestMethod]
    public async Task WriteMessage_ShouldWriteMessageToPipe()
    {
        var absolutePipeName = RemotePipe.GetAbsolutePipeName(pipeName);
        var clientPipeName = RemotePipe.GetClientPipeName(pipeName);
        using var serverPipe = new NamedPipeServerStream(absolutePipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Message);
        using var clientPipe = new NamedPipeClientStream(".", clientPipeName, PipeDirection.InOut);

        var clientTask = Task.Run(() =>
        {
            clientPipe.Connect();
            clientPipe.ReadMode = PipeTransmissionMode.Message;
            RemotePipe.WriteMessage(clientPipe, "Hello, Pipe!");
        });

        await serverPipe.WaitForConnectionAsync();
        var result = await Task.Run(() => RemotePipe.ReadMessage(serverPipe));

        await clientTask;
        Assert.AreEqual("Hello, Pipe!", result);
    }
}
