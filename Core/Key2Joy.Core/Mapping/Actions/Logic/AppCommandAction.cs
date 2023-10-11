using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Mapping.Actions.Scripting;
using Key2Joy.Util;

namespace Key2Joy.Mapping.Actions.Logic;

[Action(
    Description = "App Command",
    NameFormat = "Run App Command '{0}'",
    GroupName = "Logic",
    GroupImage = "application_xp_terminal"
)]
public class AppCommandAction : CoreAction
{
    public AppCommand Command { get; set; }

    public AppCommandAction(string name)
        : base(name)
    { }

    /// <markdown-doc>
    /// <parent-name>Logic</parent-name>
    /// <path>Api/Logic</path>
    /// </markdown-doc>
    /// <summary>
    /// Execute a command in this app
    ///
    /// TODO: Make commands Enumerations and show those in the docs.
    /// </summary>
    /// <param name="command">Command to execute</param>
    /// <name>App.Command</name>
    [ExposesScriptingMethod("App.Command")]
    public async void ExecuteForScript(AppCommand command)
    {
        this.Command = command;

        if (this.Command == AppCommand.ResetScriptEnvironment)
        {
            foreach (var otherAction in this.OtherActions)
            {
                var otherActionType = otherAction.GetType();

                // TODO: This is a bit hacky and could do with less reflection
                if (typeof(BaseScriptActionWithEnvironment<>).IsSubclassOfRawGeneric(otherActionType))
                {
                    var newEnvironmentMethod = otherActionType.GetMethod(nameof(BaseScriptActionWithEnvironment<object>.RetireEnvironment));
                    newEnvironmentMethod.Invoke(otherAction, null);
                }
            }

            return;
        }

        await this.Execute();
    }

    public override async Task Execute(AbstractInputBag inputBag = null)
    {
        if (!Key2JoyManager.RunAppCommand(this.Command))
        {
            throw new NotImplementedException("This app command is invalid!");
        }
    }

    public override string GetNameDisplay() => this.Name.Replace("{0}", Enum.GetName(typeof(AppCommand), this.Command));

    public override bool Equals(object obj)
    {
        if (obj is not AppCommandAction action)
        {
            return false;
        }

        return action.Command == this.Command;
    }
}
