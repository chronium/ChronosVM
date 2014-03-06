using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronosVM_2
{
    public class StackStuff
    {
        public void push(byte b, ref Registers reg, ref Ram ram)
        {
            reg.SP -= sizeof(byte);
            ram.writeByte(reg.SP, b);
        }

        public void push(short b, ref Registers reg, ref Ram ram)
        {
            reg.SP -= sizeof(short);
            ram.writeShort(reg.SP, b);
        }

        public byte pop(byte b, ref Registers reg, ref Ram ram)
        {
            reg.SP += sizeof(byte);
            byte byt = ram.readByte(reg.SP);
            return byt;
        }

        public short pop(short b, ref Registers reg, ref Ram ram)
        {
            short byt = ram.readShort(reg.SP);
            reg.SP += sizeof(short);
            return byt;
        }
    }
}
