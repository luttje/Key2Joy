using KeyToJoy.Input;
using SimWinInput;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // Load presets from documents
            var presets = BindingPreset.LoadAll();

            foreach (var preset in presets)
            {
                BindingPreset.Add(preset, false);
            }

            if(BindingPreset.All.Count == 0)
            {
                var preset = BindingPreset.Default;
                BindingPreset.Add(preset);
            }

            // Setup resource names for each controller button
            // Top of controller
            BindingOption.AddControllerImage(GamePadControl.LeftShoulder, "XboxSeriesX_LB");
            BindingOption.AddControllerImage(GamePadControl.LeftTrigger, "XboxSeriesX_LT");
            BindingOption.AddControllerImage(GamePadControl.RightShoulder, "XboxSeriesX_RB");
            BindingOption.AddControllerImage(GamePadControl.RightTrigger, "XboxSeriesX_RT");

            // Left half of controller
            BindingOption.AddControllerImage(GamePadControl.LeftStickUp, "XboxSeriesX_Left_Stick_Up");
            BindingOption.AddControllerImage(GamePadControl.LeftStickRight, "XboxSeriesX_Left_Stick_Right");
            BindingOption.AddControllerImage(GamePadControl.LeftStickDown, "XboxSeriesX_Left_Stick_Down");
            BindingOption.AddControllerImage(GamePadControl.LeftStickLeft, "XboxSeriesX_Left_Stick_Left");
            BindingOption.AddControllerImage(GamePadControl.LeftStickClick, "XboxSeriesX_Left_Stick_Click");
            BindingOption.AddControllerImage(GamePadControl.DPadUp, "XboxSeriesX_Dpad_Up");
            BindingOption.AddControllerImage(GamePadControl.DPadRight, "XboxSeriesX_Dpad_Right");
            BindingOption.AddControllerImage(GamePadControl.DPadDown, "XboxSeriesX_Dpad_Down");
            BindingOption.AddControllerImage(GamePadControl.DPadLeft, "XboxSeriesX_Dpad_Left");

            // Right half of controller
            BindingOption.AddControllerImage(GamePadControl.RightStickUp, "XboxSeriesX_Right_Stick_Up");
            BindingOption.AddControllerImage(GamePadControl.RightStickRight, "XboxSeriesX_Right_Stick_Right");
            BindingOption.AddControllerImage(GamePadControl.RightStickDown, "XboxSeriesX_Right_Stick_Down");
            BindingOption.AddControllerImage(GamePadControl.RightStickLeft, "XboxSeriesX_Right_Stick_Left");
            BindingOption.AddControllerImage(GamePadControl.RightStickClick, "XboxSeriesX_Right_Stick_Click");
            BindingOption.AddControllerImage(GamePadControl.X, "XboxSeriesX_X");
            BindingOption.AddControllerImage(GamePadControl.Y, "XboxSeriesX_Y");
            BindingOption.AddControllerImage(GamePadControl.A, "XboxSeriesX_A");
            BindingOption.AddControllerImage(GamePadControl.B, "XboxSeriesX_B");

            SimGamePad.Instance.Initialize();
            Program.GoToNextForm(new MainForm());
        }
    }
}
