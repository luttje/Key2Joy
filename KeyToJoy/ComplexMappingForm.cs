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
    public partial class ComplexMappingForm : Form
    {
        public ComplexMappingForm()
        {
            InitializeComponent();

            var context = new MappingContext();
            ndcLogic.Context = context;
        }

        private void ComplexMappingForm_Load(object sender, EventArgs e)
        {
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            ndcLogic.Execute();
        }
    }
}
