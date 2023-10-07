using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static Key2Joy.PluginHost.Native;

namespace Key2Joy.PluginHost
{
    internal class Program
    {
        static HandlerRoutine handler;

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
                    typeof(PluginHost), "PluginHost", WellKnownObjectMode.Singleton);

                Signal(portName, "Ready");

                Dispatcher.Run();
            }
            catch (Exception ex)
            {
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
