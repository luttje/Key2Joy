using System.Collections.Generic;

namespace Key2Joy.Interop.Commands;

public interface ICommandRepository
{
    CommandInfo GetCommandInfo(byte commandId);

    CommandInfo GetCommandInfo<TCommandType>();

    IReadOnlyDictionary<byte, CommandInfo> GetCommandInfoTypes();
}
