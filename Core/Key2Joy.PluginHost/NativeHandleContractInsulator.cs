using System;
using System.AddIn.Contract;
using Key2Joy.Contracts.Plugins.Remoting;

namespace Key2Joy.PluginHost;

public class NativeHandleContractInsulator : MarshalByRefObject, INativeHandleContract
{
    private readonly INativeHandleContract source;
    private readonly ViewContractConverter converter;

    public NativeHandleContractInsulator(INativeHandleContract source, ViewContractConverter converter)
    {
        this.source = source;
        this.converter = converter;
    }

    public IntPtr GetHandle() => this.source.GetHandle();

    public int AcquireLifetimeToken() => this.source.AcquireLifetimeToken();

    public int GetRemoteHashCode() => this.source.GetRemoteHashCode();

    public IContract QueryContract(string contractIdentifier) => this.source.QueryContract(contractIdentifier);

    public bool RemoteEquals(IContract contract) => this.source.RemoteEquals(contract);

    public string RemoteToString() => this.source.RemoteToString();

    public void RevokeLifetimeToken(int token) => this.source.RevokeLifetimeToken(token);

    public object RemoteInvoke(string methodName, object[] parameters) => this.converter.RemoteInvoke(methodName, parameters);

    public object RemoteInvokeUI(string methodName, object[] parameters) => Program.AppDispatcher.Invoke(new Func<object>(() => this.converter.RemoteInvoke(methodName, parameters)));

    public override object InitializeLifetimeService() => null; // live forever
}