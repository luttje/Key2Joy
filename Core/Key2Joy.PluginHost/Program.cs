using System;
using System.Collections;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Windows.Threading;
using Key2Joy.Contracts.Plugins.Remoting;
using static Key2Joy.PluginHost.Native;

namespace Key2Joy.PluginHost;

internal class Program
{
    public static Dispatcher AppDispatcher { get; private set; }

    private static string portName;

    [STAThread]
    private static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Usage: PluginHost name");
            return;
        }

        SetConsoleCtrlHandler(ProcessExitHandler, true);

        try
        {
            portName = args[0];
            Console.WriteLine("Starting PluginHost {0}", portName);

            AppDispatcher = Dispatcher.CurrentDispatcher;

            BinaryServerFormatterSinkProvider serverProvider = new() { TypeFilterLevel = TypeFilterLevel.Full };
            BinaryClientFormatterSinkProvider clientProvider = new();
            Hashtable properties = new();
            properties["portName"] = portName;

            // Note:https://github.com/microsoft/win32-app-isolation/issues/16
            IpcChannel channel = new(properties, clientProvider, serverProvider);
            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(PluginHost), nameof(PluginHost), WellKnownObjectMode.Singleton);

            var pipeClientStream = new NamedPipeClientStream(
                ".",
                RemotePipe.GetClientPipeName(portName),
                PipeDirection.InOut);
            pipeClientStream.Connect();

            RemoteEventSubscriber.InitClient(pipeClientStream);
            Console.WriteLine($"Connected to pipe @ {RemotePipe.GetClientPipeName(portName)}");

            Dispatcher.Run();
        }
        catch (Exception ex)
        {
            Debugger.Launch();
            var mostInnerException = ex;

            while (mostInnerException.InnerException != null)
            {
                mostInnerException = mostInnerException.InnerException;
            }

            RemoteEventSubscriber.ExitClient();
            Console.WriteLine(mostInnerException.Message);
        }
    }

    private static bool ProcessExitHandler(CtrlType sig)
    {
        RemoteEventSubscriber.ExitClient();
        Console.WriteLine("Sending close signal...");
        return false;
    }
}
