using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "Wait for a specified duration",
        Visibility = ActionVisibility.UnlessTopLevel,
        OptionsUserControl = typeof(WaitActionControl),
        NameFormat = "Wait for {0}ms"
    )]
    [Util.ObjectListViewGroup(
        Name = "Logic",
        Image = "script_code"
    )]
    internal class WaitAction : BaseAction
    {
        [JsonProperty]
        public TimeSpan WaitTime;

        public WaitAction(string name, string description)
            : base(name, description)
        {
        }

        /// <markdown-doc>
        /// <parent-name>Logic</parent-name>
        /// <path>Api/Logic</path>
        /// </markdown-doc>
        /// <summary>
        /// Wait for the specified duration in milliseconds, then execute the callback
        /// 
        /// <strong>Note: There are issues with this function I have yet to fix. It may not work as expected sometimes!</strong>
        /// </summary>
        /// <param name="callback">Function to execute</param>
        /// <param name="waitTime">Time to wait (in milliseconds)</param>
        /// <name>Wait</name>
        [ExposesScriptingMethod("Wait")]
        public async void ExecuteForScript(Action callback, long waitTime)
        {
            WaitTime = TimeSpan.FromMilliseconds(waitTime);
            await Task.Delay(WaitTime);
            System.Diagnostics.Debug.WriteLine("Test A");
            callback.Invoke();
            System.Diagnostics.Debug.WriteLine("Test B");
        }

        internal override Task Execute(InputBag inputBag = null)
        {
            return Task.Delay(WaitTime);
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", WaitTime.TotalMilliseconds.ToString());
        }

        public override object Clone()
        {
            return new WaitAction(Name, description)
            {
                WaitTime = WaitTime,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
