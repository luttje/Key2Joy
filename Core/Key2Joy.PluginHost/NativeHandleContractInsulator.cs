using System;
using System.AddIn.Contract;
using Key2Joy.Contracts.Plugins.Remoting;

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
            return this.source.GetHandle();
        }

        public int AcquireLifetimeToken()
        {
            return this.source.AcquireLifetimeToken();
        }

        public int GetRemoteHashCode()
        {
            return this.source.GetRemoteHashCode();
        }

        public IContract QueryContract(string contractIdentifier)
        {
            return this.source.QueryContract(contractIdentifier);
        }

        public bool RemoteEquals(IContract contract)
        {
            return this.source.RemoteEquals(contract);
        }

        public string RemoteToString()
        {
            return this.source.RemoteToString();
        }

        public void RevokeLifetimeToken(int token)
        {
            this.source.RevokeLifetimeToken(token);
        }

        public object RemoteInvoke(string methodName, object[] parameters)
        {
            return this.converter.RemoteInvoke(methodName, parameters);
        }

        public object RemoteInvokeUI(string methodName, object[] parameters)
        {
            return Program.AppDispatcher.Invoke(new Func<object>(() => this.converter.RemoteInvoke(methodName, parameters)));
        }

        public override object InitializeLifetimeService()
        {
            return null; // live forever
        }
    }
}