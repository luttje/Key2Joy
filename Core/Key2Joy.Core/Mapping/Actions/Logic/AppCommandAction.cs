using Key2Joy.Contracts.Mapping;
using Key2Joy.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Key2Joy.LowLevelInput.Mouse;

namespace Key2Joy.Mapping
{
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
            Command = command;

            if(command == AppCommand.ResetScriptEnvironment)
            {
                // Keep track of new environments to pass to all related script actions
                Dictionary<Type, object> newEnvironments = new Dictionary<Type, object>();
                
                foreach (var otherAction in otherActions)
                {
                    var otherActionType = otherAction.GetType();

                    // TODO: This is a bit hacky and could do with less reflection
                    if (typeof(BaseScriptActionWithEnvironment<>).IsSubclassOfRawGeneric(otherActionType))
                    {
                        if (!newEnvironments.TryGetValue(otherActionType, out object environment))
                        {
                            var newEnvironmentMethod = otherActionType.GetMethod(nameof(BaseScriptActionWithEnvironment<object>.SetupEnvironment));
                            environment = newEnvironmentMethod.Invoke(otherAction, null);
                            newEnvironments.Add(otherActionType, environment);
                        }

                        var replaceEnvironmentMethod = otherActionType.GetMethod(nameof(BaseScriptActionWithEnvironment<object>.ReplaceEnvironment));
                        replaceEnvironmentMethod.Invoke(otherAction, new object[] { environment });
                    }
                }
                
                return;
            }

            // Wait a frame so we don't get an Access Violation on the lua.DoString
            // TODO: Figure out if there's a nicer way
            // TODO: Do we still need this?
            await Task.Delay(0);

            await this.Execute();
        }

        public override async Task Execute(AbstractInputBag inputBag = null)
        {
            if (!Key2JoyManager.RunAppCommand(Command))
            {
                throw new NotImplementedException("This app command is invalid!");
            }
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", Enum.GetName(typeof(AppCommand), Command));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AppCommandAction action))
                return false;

            return action.Command == Command;
        }
    }
}
