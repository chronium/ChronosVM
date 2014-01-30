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
        public short instruction;
        public AsmRegister reg;
        public AsmRegister reg1;
        public AsmRegister reg2;
        public AsmRegister reg3;
        public short value;
        public short value1;
        public short value2;
        public short value3;
        public short type;
        public char char1;
        public char char2;

        public void interpretOpCode(Ram ram, ref long index)
        {
            instruction = ram.readShort(index);
            reg = (AsmRegister)ram.readByte(index + 2);
            reg1 = (AsmRegister)ram.readByte(index + 4);
            reg2 = (AsmRegister)ram.readByte(index + 6);
            reg3 = (AsmRegister)ram.readByte(index + 8);

            type = ram.readByte(index + 2);

            value = ram.readShort(index + 2);
            value1 = ram.readShort(index + 4);
            value2 = ram.readShort(index + 6);
            value3 = ram.readShort(index + 8);

            char1 = (char)ram.readShort(index + 4);
            char2 = (char)ram.readShort(index + 6);

            index += Program.instructionSize;
        }
    }
}
