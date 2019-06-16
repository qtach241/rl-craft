using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOverlay
{
    public partial class FormMain : Form
    {
        FormOverlay frm = new FormOverlay();

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            frm.Show();
        }

        private void checkBoxShowOverlay_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxShowOverlay.Checked == true)
            {
                frm.Show();
            }
            else
            {
                frm.Hide();
            }
        }
    }
}
