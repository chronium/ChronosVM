using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronosVM_2
{
    public partial class VM
    {
        public void DoStackOperations(OpCode opcode)
        {
            short op2 = stack.pop((short)0);
            short op1 = stack.pop((short)0);
            switch (opcode.type)
            {
                case 0:
                    stack.push((short)(op1 + op2));
                    break;
                case 1:
                    stack.push((short)(op1 - op2));
                    break;
                case 2:
                    stack.push((short)(op1 * op2));
                    break;
                case 3:
                    stack.push((short)(op1 / op2));
                    break;
            }
        }
    }
}
