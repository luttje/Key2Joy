using System;
using System.AddIn.Contract;
using System.Runtime.Remoting;
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

        public void RemoteInvoke(string methodName, object[] parameters)
        {
            converter.RemoteInvoke(methodName, parameters);
        }

        public void RemoteInvokeUI(string methodName, object[] parameters)
        {
            Program.AppDispatcher.Invoke(new Action(() => converter.RemoteInvoke(methodName, parameters)));
        }

        public override object InitializeLifetimeService()
        {
            return null; // live forever
        }
    }
}