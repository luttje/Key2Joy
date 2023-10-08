using System;
using System.AddIn.Contract;
using System.Runtime.Remoting.Messaging;

namespace Key2Joy.Contracts.Plugins
{
    public interface IPluginHost
    {
        event RemoteEventHandlerCallback AnyEvent;

        /// <summary>
        /// This method can't just return the PluginBase, since it's created in an AppDomain in the PluginHost process. Passing it 
        /// upwards to the main app would not work, since it has no remote connection to it. Therefor we store the plugin and let
        /// the main app call functions on this loader to interact with it.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="assemblyName"></param>
        /// <param name="loadedChecksum">The checksum after the plugin was loaded</param>
        /// <param name="expectedChecksum">The checksum the plugin must have to be loaded</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="PluginLoadException">Throws when the plugin failed to load</exception>
        void LoadPlugin(string assemblyPath, string assemblyName, out string loadedChecksum, string expectedChecksum = null);

        INativeHandleContract CreateFrameworkElementContract(string controlTypeName, SubscriptionInfo[] eventSubscriptions = null);
        PluginActionInsulator CreateAction(string fullTypeName, object[] constructorArguments);
        PluginTriggerInsulator CreateTrigger(string fullTypeName, object[] constructorArguments);

        [OneWay]
        void Terminate();

        string GetPluginName();

        string GetPluginAuthor();

        string GetPluginWebsite();
    }
}