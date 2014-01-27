using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChronosVM_2
{
    public partial class Screen : Form
    {
        Bitmap bmp = new Bitmap(320, 200);

        public Screen()
        {
            InitializeComponent();
            this.ClientSize = new Size(320 * 2, 200 * 2);
        }

        public void startVM()
        {
            new Thread(() => Program.vm.run()).Start();
        }

        public void setPixel(int x, int y, Color color)
        {
            bmp.SetPixel(x, y, color);
        }

        public void clearScreen(Color color)
        {
            //lock (bmp)
                using (Graphics gfx = Graphics.FromImage(bmp))
                {
                    using (SolidBrush brush = new SolidBrush(color))
                    {
                        gfx.FillRectangle(brush, 0, 0, 320, 200);
                    }
                }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            lock (bmp)
                e.Graphics.DrawImage(bmp, 0, 0, 320 * 2, 200 * 2);
        }
    }
}
