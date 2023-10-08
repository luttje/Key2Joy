using Key2Joy.Mapping;

namespace Key2Joy
{
    public interface IAcceptAppCommands
    {
        bool RunAppCommand(AppCommand command);
    }
}
