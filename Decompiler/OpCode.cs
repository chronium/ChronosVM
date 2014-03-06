using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decompiler
{
    public class OpCode
    {
        public short instruction;
        public byte reg;
        public byte reg1;
        public byte reg2;
        public byte reg3;
        public short value;
        public short value1;
        public short value2;
        public short value3;
        public short type;
        public byte char1;
        public byte char2;

        public string dump;

        public void interpretOpCode(Ram ram, ref ushort index)
        {
            instruction = ram.readShort(index);
            reg = ram.readByte(index + 2);
            reg1 = ram.readByte(index + 4);
            reg2 = ram.readByte(index + 6);
            reg3 = ram.readByte(index + 8);

            type = ram.readByte(index + 2);

            value = ram.readShort(index + 2);
            value1 = ram.readShort(index + 4);
            value2 = ram.readShort(index + 6);
            value3 = ram.readShort(index + 8);

            char1 = ram.readByte(index + 4);
            char2 = ram.readByte(index + 6);

            for (int i = index; i < index + 10; i++)
                dump += ram.readByte(i) + ", ";

            dump = dump.Substring(0, dump.Length - 3);

            index += 10;
        }
    }
}
