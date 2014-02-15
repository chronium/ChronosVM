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
        Registers reg;

        public StackStuff(ref Ram ram, ref Registers reg)
        {
            this.ram = ram;
            this.reg = reg;
        }

        public void push(byte b)
        {
            reg.SP -= sizeof(byte);
            ram.writeByte(reg.SP, b);
        }

        public void push(short b)
        {
            reg.SP -= sizeof(short);
            ram.writeShort(reg.SP, b);
        }

        public byte pop(byte b)
        {
            reg.SP += sizeof(byte);
            byte byt = ram.readByte(reg.SP);
            return byt;
        }

        public short pop(short b)
        {
            short byt = ram.readShort(reg.SP);
            reg.SP += sizeof(short);
            return byt;
        }
    }
}
