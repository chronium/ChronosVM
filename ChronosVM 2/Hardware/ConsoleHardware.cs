using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronosVM_2.Hardware
{
    class ConsoleHardware : Device
    {
        public ConsoleLine[] lines = new ConsoleLine[60];
        public int x = 0, y = 0;

        public override void init(PeripheralBase pBase)
        {
            pBase.addBusListener(0x04, setX);
            pBase.addBusListener(0x05, setY);
            pBase.addBusListener(0x06, writeChar);
            pBase.addBusListener(0x07, shiftStuff);

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = new ConsoleLine();
            }
        }

        public void setX(short x)
        {
            this.x = x;
        }

        public void setY(short y)
        {
            this.y = y;
        }

        public void writeChar(short c)
        {
            lines[y].addLetter((char)c, x);
        }

        public void shiftStuff(short val)
        {
            ConsoleLine[] newStuff = new ConsoleLine[lines.Length];

            for (int i = 1; i < lines.Length; i++)
            {
                newStuff[i - 1] = lines[i];
            }

            this.lines = newStuff;
        }

        public void clearConsole(short s)
        {
            foreach (ConsoleLine line in lines)
            {
                line.clearLine();
            }
        }
    }

    public class ConsoleLine
    {
        public const int length = 120;
        public char[] text = new char[length];

        public void addLetter(char c, int index)
        {
            if (index >= length)
                throw new Exception("Console line out of bounds!");
            text[index] = c;
        }

        public void clearLine()
        {
            for (int i = 0; i < length; i++)
            {
                text[i] = '\0';
            }
        }
    }
}
