using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronosVM_2
{
    public class ScreenDevice : Device
    {
        public Color[] colors;
        public short color = 0;

        Screen screen;

        public override void init(PeripheralBase pBase)
        {
            colors = new Color[16];
            colors[0] = Color.Black;
            colors[1] = Color.DarkBlue;
            colors[2] = Color.DarkGreen;
            colors[3] = Color.DarkCyan;
            colors[4] = Color.DarkRed;
            colors[5] = Color.DarkMagenta;
            colors[6] = ColorTranslator.FromHtml("#CC7722");
            colors[7] = Color.Gray;
            colors[8] = Color.DarkGray;
            colors[9] = Color.Blue;
            colors[10] = Color.Green;
            colors[11] = Color.Cyan;
            colors[12] = Color.Red;
            colors[13] = Color.Magenta;
            colors[14] = Color.Yellow;
            colors[15] = Color.White;

            pBase.addBusListener(0x02, port0x02InterpretCommand);
            pBase.addBusResponder(0x03, port0x03GetColor);
        }

        public void setScreen(Screen screen)
        {
            this.screen = screen;
        }

        public void port0x02InterpretCommand(short command)
        {
            switch (command)
            {
                case 0:
                    color = (short)vm.reg.getRegister(0);
                    break;
                case 1:
                    screen.clearScreen(colors[color]);
                    break;
            }
        }

        public short port0x03GetColor()
        {
            return color;
        }
    }
}
