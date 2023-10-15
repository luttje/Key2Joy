using System;

namespace Key2Joy;

public interface IHaveHandleAndInvoke
{
    IntPtr Handle { get; }

    object Invoke(Delegate method);

    object Invoke(Delegate method, params object[] arguments);
}
