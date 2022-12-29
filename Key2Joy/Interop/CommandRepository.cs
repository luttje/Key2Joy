using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Interop
{
    public delegate void CommandHandler<CommandType>(CommandType command);

    public class CommandRepository
    {
        public static CommandRepository Instance { get; } = new CommandRepository();

        private readonly Dictionary<byte, CommandInfo> commandTypes;

        private CommandRepository()
        {
            commandTypes = new Dictionary<byte, CommandInfo>();

            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0);

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<CommandAttribute>();
                var commandInfo = new CommandInfo
                {
                    Id = attribute.Id,
                    StructType = type,
                };

                commandTypes.Add(attribute.Id, commandInfo);
            }
        }

        public CommandInfo GetCommandInfo<CommandType>(CommandType command)
        {
            var commandInfo = commandTypes.Values.FirstOrDefault(c => c.StructType == command.GetType());
            if (commandInfo == null)
                throw new ArgumentException("Command type not found in repository");

            return commandInfo;
        }

        public CommandInfo GetCommandInfo(byte id)
        {
            if (!commandTypes.ContainsKey(id))
            {
                throw new ArgumentException("Command with id " + id + " not registered");
            }

            return commandTypes[id];
        }
    }
}
