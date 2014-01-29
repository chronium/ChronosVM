using AssemblerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronosVM_2
{
    public class OpCode
    {
        public byte instruction;
        public AsmRegister reg1;
        public AsmRegister reg2;
        public AsmRegister reg3;
        public short value;
        public short value2;
        public short type;
        public char char1;
        public char char2;

        public void interpretOpCode(Ram ram, ref long index)
        {
            instruction = ram.readByte(index);
            reg1 = (AsmRegister)ram.readByte(index + 1);
            reg2 = (AsmRegister)ram.readByte(index + 3);
            reg3 = (AsmRegister)ram.readByte(index + 5);

            type = ram.readByte(index + 1);

            value = ram.readShort(index + 3);
            value2 = ram.readShort(index + 5);

            char1 = (char)ram.readShort(index + 3);
            char2 = (char)ram.readShort(index + 5);

            index += Program.instructionSize;
        }
    }
}
