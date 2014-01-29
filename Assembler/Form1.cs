using Assembler.Language;
using AssemblerLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            ofd.Filter = "Assembly file |*.asm";
            ofd.Title = "Select a file to assemble";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                ipath = ofd.FileName;

            textBox1.Text = ipath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ipath != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Binary File |*.bin";
                sfd.Title = "Select an output file";

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    opath = sfd.FileName;

                AssemblerLib.Assembler asm = new AssemblerLib.Assembler(7, Path.GetDirectoryName(opath), Path.GetFileName(opath));

                //asm.Emit(new Call(2));
                //asm.Emit(new Halt());
                //asm.Emit(new Write('b'));
                //asm.Emit(new Ret());

                Lexer l = new Lexer();

                string test = File.ReadAllText(ipath);

                l.Scan(new StringReader(test));

                Parser p = new Parser(l.tokens, ref asm);

                p.Parse();

                asm.writeToFile(asm.Release());

                MessageBox.Show("File assembled!");
            }
            else
                MessageBox.Show("Make sure that all of the paths are set!");
        }
    }
}
