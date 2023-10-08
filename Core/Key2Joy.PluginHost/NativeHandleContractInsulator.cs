using Key2Joy.Contracts.Plugins;
using System;
using System.AddIn.Contract;

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

        public override object InitializeLifetimeService()
        {
            return null; // live forever
        }
    }
}