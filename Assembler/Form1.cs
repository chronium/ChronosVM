using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assembler
{
    public partial class Form1 : Form
    {
        string ipath = null;
        string opath = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Assembly file |*.asm | Test file |*.txt";
            ofd.Title = "Select a file to assemble";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                ipath = ofd.FileName;

            textBox1.Text = ipath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Binary File |*.bin";
            sfd.Title = "Select an output file";

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                opath = sfd.FileName;

            textBox2.Text = sfd.FileName;
        }
    }
}
