using KeyToJoy.Input;
using KeyToJoy.Mapping;
using SimWinInput;
using System;
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
            var defaultPreset = LoadActions();

            // Load presets from documents
            var presets = MappingPreset.LoadAll();

            foreach (var preset in presets)
            {
                MappingPreset.Add(preset, false);
            }

            if (MappingPreset.All.Count == 0)
                MappingPreset.Add(defaultPreset);

            SimGamePad.Instance.Initialize();
            Program.GoToNextForm(new MainForm());
        }

        private MappingPreset LoadActions()
        {
            var defaultPreset = new MappingPreset("Default");

            // Commands for this application
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new AppCommandAction(null, "abort", "Stop simulating GamePad Input")),
                Binding = new KeyboardTrigger(Keys.Escape)
            });

            // Top of controller
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_LB", GamePadControl.LeftShoulder)),
                Binding = new KeyboardTrigger(Keys.Q)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_LT", GamePadControl.LeftTrigger)),
                Binding = new KeyboardTrigger(Keys.D1)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_RB", GamePadControl.RightShoulder)),
                Binding = new KeyboardTrigger(Keys.E)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_RT", GamePadControl.RightTrigger)),
                Binding = new KeyboardTrigger(Keys.D2)
            });

            // Left half of controller
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Left_Stick_Up", GamePadControl.LeftStickUp)),
                Binding = new KeyboardTrigger(Keys.W)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Left_Stick_Right", GamePadControl.LeftStickRight)),
                Binding = new KeyboardTrigger(Keys.D)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Left_Stick_Down", GamePadControl.LeftStickDown)),
                Binding = new KeyboardTrigger(Keys.S)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Left_Stick_Left", GamePadControl.LeftStickLeft)),
                Binding = new KeyboardTrigger(Keys.A)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Left_Stick_Click", GamePadControl.LeftStickClick)),
                Binding = new KeyboardTrigger(Keys.LControlKey)
            });

            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Dpad_Up", GamePadControl.DPadUp)),
                Binding = new KeyboardTrigger(Keys.Up)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Dpad_Right", GamePadControl.DPadRight)),
                Binding = new KeyboardTrigger(Keys.Right)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Dpad_Down", GamePadControl.DPadDown)),
                Binding = new KeyboardTrigger(Keys.Down)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Dpad_Left", GamePadControl.DPadLeft)),
                Binding = new KeyboardTrigger(Keys.Left)
            });

            // Right half of controller
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Right_Stick_Up", GamePadControl.RightStickUp)),
                Binding = new MouseMoveTrigger(AxisDirection.Up)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Right_Stick_Right", GamePadControl.RightStickRight)),
                Binding = new MouseMoveTrigger(AxisDirection.Right)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Right_Stick_Down", GamePadControl.RightStickDown)),
                Binding = new MouseMoveTrigger(AxisDirection.Down)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Right_Stick_Left", GamePadControl.RightStickLeft)),
                Binding = new MouseMoveTrigger(AxisDirection.Left)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Right_Stick_Click", GamePadControl.RightStickClick)),
                Binding = new KeyboardTrigger(Keys.RControlKey)
            });

            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_X", GamePadControl.X)),
                Binding = new KeyboardTrigger(Keys.X)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Y", GamePadControl.Y)),
                Binding = new KeyboardTrigger(Keys.Y)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_A", GamePadControl.A)),
                Binding = new KeyboardTrigger(Keys.F)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_B", GamePadControl.B)),
                Binding = new KeyboardTrigger(Keys.Z)
            });

            // Start and select
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Start", GamePadControl.Start)),
                Binding = new KeyboardTrigger(Keys.F1)
            });
            defaultPreset.AddOption(new MappedOption
            {
                Action = BaseAction.Register(new GamePadAction("XboxSeriesX_Select", GamePadControl.Back)),
                Binding = new KeyboardTrigger(Keys.F2)
            });

            return defaultPreset;
        }
    }
}
