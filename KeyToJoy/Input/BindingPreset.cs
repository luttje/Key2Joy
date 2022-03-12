using KeyToJoy.Properties;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Input
{
    internal class BindingPreset
    {
        public List<BindingSetting> Bindings { get; set; } = new List<BindingSetting>();
        public string Name { get; set; }

        public BindingPreset(string name)
        {
            Name = name;
        }

        internal static BindingPreset Default
        {
            get
            {
                var defaultPreset = new BindingPreset("Default");

                // Top of controller
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.LeftShoulder,
                    HighlightImage = Resources.XboxSeriesX_LB,
                    DefaultKeyBind = Keys.Q
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.LeftTrigger,
                    HighlightImage = Resources.XboxSeriesX_LT,
                    DefaultKeyBind = Keys.D1
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.RightShoulder,
                    HighlightImage = Resources.XboxSeriesX_RB,
                    DefaultKeyBind = Keys.E
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.RightTrigger,
                    HighlightImage = Resources.XboxSeriesX_RT,
                    DefaultKeyBind = Keys.D2
                });

                // Left half of controller
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.LeftStickUp,
                    HighlightImage = Resources.XboxSeriesX_Left_Stick_Up,
                    DefaultKeyBind = Keys.W
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.LeftStickRight,
                    HighlightImage = Resources.XboxSeriesX_Left_Stick_Right,
                    DefaultKeyBind = Keys.D
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.LeftStickDown,
                    HighlightImage = Resources.XboxSeriesX_Left_Stick_Down,
                    DefaultKeyBind = Keys.S
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.LeftStickLeft,
                    HighlightImage = Resources.XboxSeriesX_Left_Stick_Left,
                    DefaultKeyBind = Keys.A
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.LeftStickClick,
                    HighlightImage = Resources.XboxSeriesX_Left_Stick_Click,
                    DefaultKeyBind = Keys.LControlKey
                });

                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.DPadUp,
                    HighlightImage = Resources.XboxSeriesX_Dpad_Up,
                    DefaultKeyBind = Keys.Up
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.DPadRight,
                    HighlightImage = Resources.XboxSeriesX_Dpad_Right,
                    DefaultKeyBind = Keys.Right
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.DPadDown,
                    HighlightImage = Resources.XboxSeriesX_Dpad_Down,
                    DefaultKeyBind = Keys.Down
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.DPadLeft,
                    HighlightImage = Resources.XboxSeriesX_Dpad_Left,
                    DefaultKeyBind = Keys.Left
                });

                // Right half of controller
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.RightStickUp,
                    HighlightImage = Resources.XboxSeriesX_Right_Stick_Up,
                    //DefaultKeyBind = Keys.I,
                    DefaultAxisBind = AxisDirection.Up
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.RightStickRight,
                    HighlightImage = Resources.XboxSeriesX_Right_Stick_Right,
                    //DefaultKeyBind = Keys.L,
                    DefaultAxisBind = AxisDirection.Right
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.RightStickDown,
                    HighlightImage = Resources.XboxSeriesX_Right_Stick_Down,
                    //DefaultKeyBind = Keys.K,
                    DefaultAxisBind = AxisDirection.Down
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.RightStickLeft,
                    HighlightImage = Resources.XboxSeriesX_Right_Stick_Left,
                    //DefaultKeyBind = Keys.J,
                    DefaultAxisBind = AxisDirection.Left
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.RightStickClick,
                    HighlightImage = Resources.XboxSeriesX_Right_Stick_Click,
                    DefaultKeyBind = Keys.RControlKey
                });

                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.X,
                    HighlightImage = Resources.XboxSeriesX_X,
                    DefaultKeyBind = Keys.X
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.Y,
                    HighlightImage = Resources.XboxSeriesX_Y,
                    DefaultKeyBind = Keys.Y
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.A,
                    HighlightImage = Resources.XboxSeriesX_A,
                    DefaultKeyBind = Keys.F
                });
                defaultPreset.Bindings.Add(new BindingSetting
                {
                    Control = GamePadControl.B,
                    HighlightImage = Resources.XboxSeriesX_B,
                    DefaultKeyBind = Keys.Z
                });

                return defaultPreset;
            }
        }
    }
}
