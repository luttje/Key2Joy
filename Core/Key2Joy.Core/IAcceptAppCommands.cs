using Key2Joy.Mapping.Actions.Logic;

namespace Key2Joy;

public interface IAcceptAppCommands
{
    bool RunAppCommand(AppCommand command);
}
