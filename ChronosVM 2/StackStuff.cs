using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronosVM_2
{
    public class StackStuff
    {
        Ram ram;
        public short SP;

        public StackStuff(ref Ram ram)
        {
            this.ram = ram;
            this.SP = 0;
        }

        public void push(byte b)
        {
            SP -= sizeof(byte);
            ram.writeByte(SP, b);
        }

        public void push(short b)
        {
            SP -= sizeof(short);
            ram.writeShort(SP, b);
        }

        public byte pop(byte b)
        {
            SP += sizeof(byte);
            byte byt = ram.readByte(SP);
            return byt;
        }

        public short pop(short b)
        {
            short byt = ram.readShort(SP);
            SP += sizeof(short);
            return byt;
        }
    }
}
