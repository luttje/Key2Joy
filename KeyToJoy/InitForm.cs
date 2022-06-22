using KeyToJoy.Input;
using KeyToJoy.Mapping;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyToJoy
{
    public partial class InitForm : Form
    {
        public InitForm()
        {
            InitializeComponent();
        }

        private void InitForm_Load(object sender, EventArgs e)
        {
            // TODO: Refactor
            //var defaultPreset = LoadActions();

            //// Load presets from documents
            //var presets = MappingPreset.LoadAll();

            //foreach (var preset in presets)
            //{
            //    MappingPreset.Add(preset, false);
            //}

            //if (MappingPreset.All.Count == 0)
            //    MappingPreset.Add(defaultPreset);

            SimGamePad.Instance.Initialize();
            Program.GoToNextForm(new MainForm());
        }

        private MappingPreset LoadActions()
        {
            var defaultPreset = new MappingPreset("Default");

            //// TODO: Refactor presets after refactoring of actions and triggers
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(new SequenceAction("Sequence of Actions", null, new List<BaseAction>
            //    {
            //        new GamePadAction(
            //            "Left Shoulder",
            //            "XboxSeriesX_LB",
            //            GamePadControl.LeftShoulder
            //        ),
            //        new WaitAction(
            //            "Wait",
            //            null,
            //            TimeSpan.FromSeconds(1)
            //        ),
            //        new GamePadAction(
            //            "Right Shoulder",
            //            "XboxSeriesX_RB",
            //            GamePadControl.RightShoulder
            //        )
            //})),
            //    Trigger = new KeyboardTrigger(Keys.F12)
            //});
            
            //// Commands for this application
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(new AppCommandAction("Stop Mapping Keys", null, "abort")),
            //    Trigger = new KeyboardTrigger(Keys.Escape)
            //});

            //// Top of controller
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Left Shoulder",
            //            "XboxSeriesX_LB", 
            //            GamePadControl.LeftShoulder
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.Q)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Left Trigger",
            //            "XboxSeriesX_LT", 
            //            GamePadControl.LeftTrigger
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.D1)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Right Shoulder",
            //            "XboxSeriesX_RB", 
            //            GamePadControl.RightShoulder
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.E)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Right Trigger",
            //            "XboxSeriesX_RT", 
            //            GamePadControl.RightTrigger
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.D2)
            //});

            //// Left half of controller
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Left Stick Up",
            //            "XboxSeriesX_Left_Stick_Up", 
            //            GamePadControl.LeftStickUp
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.W)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Left Stick Right",
            //            "XboxSeriesX_Left_Stick_Right", 
            //            GamePadControl.LeftStickRight
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.D)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Left Stick Down",
            //            "XboxSeriesX_Left_Stick_Down", 
            //            GamePadControl.LeftStickDown
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.S)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Left Stick Left",
            //            "XboxSeriesX_Left_Stick_Left", 
            //            GamePadControl.LeftStickLeft
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.A)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Left Stick Click",
            //            "XboxSeriesX_Left_Stick_Click", 
            //            GamePadControl.LeftStickClick
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.LControlKey)
            //});

            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "DPad Up",
            //            "XboxSeriesX_Dpad_Up", 
            //            GamePadControl.DPadUp
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.Up)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "DPad Right",
            //            "XboxSeriesX_Dpad_Right", 
            //            GamePadControl.DPadRight
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.Right)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "DPad Down",
            //            "XboxSeriesX_Dpad_Down", 
            //            GamePadControl.DPadDown
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.Down)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "DPad Left",
            //            "XboxSeriesX_Dpad_Left", 
            //            GamePadControl.DPadLeft
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.Left)
            //});

            //// Right half of controller
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Right Stick Up",
            //            "XboxSeriesX_Right_Stick_Up", 
            //            GamePadControl.RightStickUp
            //            )
            //        ),
            //    Trigger = new MouseMoveTrigger(AxisDirection.Up)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Right Stick Right",
            //            "XboxSeriesX_Right_Stick_Right", 
            //            GamePadControl.RightStickRight
            //            )
            //        ),
            //    Trigger = new MouseMoveTrigger(AxisDirection.Right)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Right Stick Down",
            //            "XboxSeriesX_Right_Stick_Down", 
            //            GamePadControl.RightStickDown
            //            )
            //        ),
            //    Trigger = new MouseMoveTrigger(AxisDirection.Down)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Right Stick Left",
            //            "XboxSeriesX_Right_Stick_Left", 
            //            GamePadControl.RightStickLeft
            //            )
            //        ),
            //    Trigger = new MouseMoveTrigger(AxisDirection.Left)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Right Stick Click",
            //            "XboxSeriesX_Right_Stick_Click", 
            //            GamePadControl.RightStickClick
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.RControlKey)
            //});

            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "X",
            //            "XboxSeriesX_X", 
            //            GamePadControl.X
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.X)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Y",
            //            "XboxSeriesX_Y", 
            //            GamePadControl.Y
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.Y)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "A",
            //            "XboxSeriesX_A", 
            //            GamePadControl.A
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.F)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "B",
            //            "XboxSeriesX_B", 
            //            GamePadControl.B
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.Z)
            //});

            //// Start and select
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Start",
            //            "XboxSeriesX_Start", 
            //            GamePadControl.Start
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.F1)
            //});
            //defaultPreset.AddMapping(new MappedOption
            //{
            //    Action = BaseAction.Register(
            //        new GamePadAction(
            //            "Select",
            //            "XboxSeriesX_Select", 
            //            GamePadControl.Back
            //            )
            //        ),
            //    Trigger = new KeyboardTrigger(Keys.F2)
            //});

            return defaultPreset;
        }
    }
}
