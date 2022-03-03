using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy
{
    public partial class BindingForm : Form
    {
        private List<RadioButton> radioButtonGroup = new List<RadioButton>();

        public BindingForm()
        {
            InitializeComponent();

            radioButtonGroup.Add(radKeyBind);
            radioButtonGroup.Add(radMouseBind);

            foreach (var radioButton in radioButtonGroup)
            {
                radioButton.CheckedChanged += RadioButton_CheckedChanged;
            }
        }

        private void BindingForm_Load(object sender, EventArgs e)
        {
        }

        private void txtKeyBind_Leave(object sender, EventArgs e)
        {
            if (radKeyBind.Checked)
                txtKeyBind.Focus();
        }

        private void BindingForm_Activated(object sender, EventArgs e)
        {
            txtKeyBind.Focus();
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                foreach (RadioButton other in radioButtonGroup)
                {
                    if (other == radioButton)
                        continue;

                    other.Checked = false;
                }
            }
        }
    }
}
