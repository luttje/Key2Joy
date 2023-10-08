using Key2Joy.Contracts.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using static Key2Joy.PluginHost.Native;

namespace Key2Joy.PluginHost
{
    internal class Program
    {
        public static Dispatcher AppDispatcher { get; private set; }

        private static string portName;
        
        [STAThread]
        static void Main(string[] args)
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

                var serverProvider = new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full };
                var clientProvider = new BinaryClientFormatterSinkProvider();
                var properties = new Hashtable();
                properties["portName"] = portName;

                // Note:https://github.com/microsoft/win32-app-isolation/issues/16
                var channel = new IpcChannel(properties, clientProvider, serverProvider);
                ChannelServices.RegisterChannel(channel, false);

                RemotingConfiguration.RegisterWellKnownServiceType(
                    typeof(PluginHost), nameof(PluginHost), WellKnownObjectMode.Singleton);

                var pipeClientStream = new NamedPipeClientStream(
                    ".",
                    RemotePipe.GetClientPipeName(portName),
                    PipeDirection.InOut);

                Console.WriteLine($"Connecting to pipe @ {RemotePipe.GetClientPipeName(portName)}");
                pipeClientStream.Connect();
                Console.WriteLine($"Connected");
                RemoteEventSubscriber.InitClient(pipeClientStream);

                Signal(portName, "Ready");

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

                Signal(portName, "Exit");
                Console.WriteLine(mostInnerException.Message);
            }
        }

        private static void Signal(string name, string eventName)
        {
            try
            {
                var signalEvent = EventWaitHandle.OpenExisting($"{name}.{eventName}");
                signalEvent.Set();
            } catch(WaitHandleCannotBeOpenedException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        private static bool ProcessExitHandler(CtrlType sig)
        {
            Signal(portName, "Exit");
            Console.WriteLine("Sending close signal...");
            return false;
        }
    }
}
