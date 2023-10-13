using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Key2Joy.Interop;

public delegate void CommandHandler<CommandType>(CommandType command);

public class CommandRepository
{
    public static CommandRepository Instance { get; } = new CommandRepository();

    private readonly Dictionary<byte, CommandInfo> commandTypes;

    private CommandRepository()
    {
        this.commandTypes = new Dictionary<byte, CommandInfo>();

        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0);

        foreach (var type in types)
        {
            var attribute = type.GetCustomAttribute<CommandAttribute>();

            this.Register(attribute.Id, type);
        }
    }

    public void Register(byte id, Type structType)
    {
        if (this.commandTypes.ContainsKey(id))
        {
            throw new DuplicateNameException($"Command with id {id} already registered.");
        }

        CommandInfo commandInfo = new()
        {
            Id = id,
            StructType = structType,
        };

        this.commandTypes.Add(id, commandInfo);
    }

    public void Unregister(byte id)
    {
        if (!this.commandTypes.ContainsKey(id))
        {
            throw new KeyNotFoundException($"Command with id {id} not registered.");
        }

        this.commandTypes.Remove(id);
    }

    public CommandInfo GetCommandInfo<CommandType>()
    {
        var commandInfo = this.commandTypes.Values.FirstOrDefault(c => c.StructType == typeof(CommandType))
            ?? throw new ArgumentException("Command type not found in repository");
        return commandInfo;
    }

    public CommandInfo GetCommandInfo(byte id)
    {
        if (!this.commandTypes.ContainsKey(id))
        {
            throw new ArgumentException("Command with id " + id + " not registered");
        }

        return this.commandTypes[id];
    }
}
