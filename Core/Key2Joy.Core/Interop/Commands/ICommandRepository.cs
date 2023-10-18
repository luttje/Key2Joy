using System;
using System.Collections.Generic;

namespace Key2Joy.Interop.Commands;

public interface ICommandRepository
{
    void Register(byte id, Type type);

    void Unregister(byte id);

    CommandInfo GetCommandInfo(byte commandId);

    CommandInfo GetCommandInfo<TCommandType>();

    IReadOnlyDictionary<byte, CommandInfo> GetCommandInfoTypes();
}
