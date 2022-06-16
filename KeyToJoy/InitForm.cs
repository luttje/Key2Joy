using KeyToJoy.Input;
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
            var presets = BindingPreset.LoadAll();

            foreach (var preset in presets)
            {
                BindingPreset.Add(preset, false);
            }

            if (BindingPreset.All.Count == 0)
                BindingPreset.Add(defaultPreset);

            SimGamePad.Instance.Initialize();
            Program.GoToNextForm(new MainForm());
        }

        private BindingPreset LoadActions()
        {
            var defaultPreset = new BindingPreset("Default");

            // Commands for this application
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new AppCommandAction(null, "abort", "Stop simulating GamePad Input")),
                Binding = new KeyboardBinding(Keys.Escape)
            });

            // Top of controller
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_LB", GamePadControl.LeftShoulder)),
                Binding = new KeyboardBinding(Keys.Q)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_LT", GamePadControl.LeftTrigger)),
                Binding = new KeyboardBinding(Keys.D1)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_RB", GamePadControl.RightShoulder)),
                Binding = new KeyboardBinding(Keys.E)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_RT", GamePadControl.RightTrigger)),
                Binding = new KeyboardBinding(Keys.D2)
            });

            // Left half of controller
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Left_Stick_Up", GamePadControl.LeftStickUp)),
                Binding = new KeyboardBinding(Keys.W)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Left_Stick_Right", GamePadControl.LeftStickRight)),
                Binding = new KeyboardBinding(Keys.D)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Left_Stick_Down", GamePadControl.LeftStickDown)),
                Binding = new KeyboardBinding(Keys.S)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Left_Stick_Left", GamePadControl.LeftStickLeft)),
                Binding = new KeyboardBinding(Keys.A)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Left_Stick_Click", GamePadControl.LeftStickClick)),
                Binding = new KeyboardBinding(Keys.LControlKey)
            });

            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Dpad_Up", GamePadControl.DPadUp)),
                Binding = new KeyboardBinding(Keys.Up)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Dpad_Right", GamePadControl.DPadRight)),
                Binding = new KeyboardBinding(Keys.Right)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Dpad_Down", GamePadControl.DPadDown)),
                Binding = new KeyboardBinding(Keys.Down)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Dpad_Left", GamePadControl.DPadLeft)),
                Binding = new KeyboardBinding(Keys.Left)
            });

            // Right half of controller
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Right_Stick_Up", GamePadControl.RightStickUp)),
                Binding = new MouseAxisBinding(AxisDirection.Up)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Right_Stick_Right", GamePadControl.RightStickRight)),
                Binding = new MouseAxisBinding(AxisDirection.Right)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Right_Stick_Down", GamePadControl.RightStickDown)),
                Binding = new MouseAxisBinding(AxisDirection.Down)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Right_Stick_Left", GamePadControl.RightStickLeft)),
                Binding = new MouseAxisBinding(AxisDirection.Left)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Right_Stick_Click", GamePadControl.RightStickClick)),
                Binding = new KeyboardBinding(Keys.RControlKey)
            });

            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_X", GamePadControl.X)),
                Binding = new KeyboardBinding(Keys.X)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Y", GamePadControl.Y)),
                Binding = new KeyboardBinding(Keys.Y)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_A", GamePadControl.A)),
                Binding = new KeyboardBinding(Keys.F)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_B", GamePadControl.B)),
                Binding = new KeyboardBinding(Keys.Z)
            });

            // Start and select
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Start", GamePadControl.Start)),
                Binding = new KeyboardBinding(Keys.F1)
            });
            defaultPreset.AddOption(new BindingOption
            {
                Action = BindableAction.Register(new GamePadAction("XboxSeriesX_Select", GamePadControl.Back)),
                Binding = new KeyboardBinding(Keys.F2)
            });

            return defaultPreset;
        }
    }
}
