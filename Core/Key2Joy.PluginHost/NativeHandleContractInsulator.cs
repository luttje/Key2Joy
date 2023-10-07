using System;
using System.AddIn.Contract;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Windows;
using Key2Joy.Contracts.Plugins;

namespace Key2Joy.PluginHost
{
    public class NativeHandleContractInsulator : MarshalByRefObject, INativeHandleContract
    {
        private readonly INativeHandleContract source;
        private readonly ViewContractConverter converter;

        public NativeHandleContractInsulator(INativeHandleContract source, ViewContractConverter converter)
        {
            this.source = source;
            this.converter = converter;
        }

        public IntPtr GetHandle()
        {
            return source.GetHandle();
        }

        public int AcquireLifetimeToken()
        {
            return source.AcquireLifetimeToken();
        }

        public int GetRemoteHashCode()
        {
            return source.GetRemoteHashCode();
        }

        public IContract QueryContract(string contractIdentifier)
        {
            return source.QueryContract(contractIdentifier);
        }

        public bool RemoteEquals(IContract contract)
        {
            return source.RemoteEquals(contract);
        }

        public string RemoteToString()
        {
            return source.RemoteToString();
        }

        public void RevokeLifetimeToken(int token)
        {
            source.RevokeLifetimeToken(token);
        }

        public object RemoteInvoke(string methodName, object[] parameters)
        {
            return converter.RemoteInvoke(methodName, parameters);
        }

        public object RemoteInvokeUI(string methodName, object[] parameters)
        {
            return Program.AppDispatcher.Invoke(new Func<object>(() => converter.RemoteInvoke(methodName, parameters)));
        }

        public event EventHandler OnEventTest; // TODO: testing only
        private RemoteInvokee handler;
        public void AddRemoteEventHandler(string eventName, RemoteInvokee handler)
        {
            this.handler = handler;
            OnEventTest += NativeHandleContractInsulator_OnEventTest;
            Program.AppDispatcher.Invoke(new Action(() => converter.AddRemoteEventHandler(eventName, new RemoteInvokee(Converter_OnEventTest))));
        }

        private void Converter_OnEventTest()
        {
            OnEventTest?.Invoke(null, EventArgs.Empty);
        }

        private void NativeHandleContractInsulator_OnEventTest(object sender, EventArgs e)
        {
            MessageBox.Show($"{AppDomain.CurrentDomain.FriendlyName} received event from {sender}");
            handler?.Invoke();
        }

        public override object InitializeLifetimeService()
        {
            return null; // live forever
        }
    }
}