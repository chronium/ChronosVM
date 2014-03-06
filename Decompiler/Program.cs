using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decompiler
{
    class Program
    {
        public static bool running = true;
        static void Main(string[] args)
        {
            Console.Clear();
            byte[] program = File.ReadAllBytes(args[0] + '\\' + args[1]);

            ushort ip = 0;

            Ram ram = new Ram(0xFFFF);

            for (int i = 0; i < program.Length; i++)
            {
                ram.writeByte(i, program[i]);
            }

            while (running)
            {
                OpCode opcode = new OpCode();
                opcode.interpretOpCode(ram, ref ip);

                if (opcode.instruction == 0x01)
                {
                    string inst = "set ";

                    switch (opcode.type)
                    {
                        case 0:
                            switch (opcode.reg1)
                            {
                                case (byte)AsmRegister.SP: 
                                    inst += (AsmRegister)opcode.reg1 + ", " + (ushort)opcode.value2;
                                    break;
                                default:
                                    inst += (AsmRegister)opcode.reg1 + ", " + opcode.value2;
                                    break;
                            }
                            break;
                        case 1:
                            inst += (AsmRegister)opcode.reg1 + ", " + (AsmRegister)opcode.reg2;
                            break;
                        case 2:
                            inst += (AsmRegister)opcode.reg1 + ", " + (ip + opcode.value2);
                            break;
                    }

                    Console.WriteLine(inst);
                }
                else if (opcode.instruction == 0x31)
                {
                    string inst = "call ";

                    inst += "offset " + opcode.value1;

                    Console.WriteLine(inst);

                }
                else if (opcode.instruction == 0x20)
                {
                    string inst = "jump ";

                    inst += "offset " + opcode.value1;

                    Console.WriteLine(inst);
                }
                else if (opcode.instruction == 0)
                    Console.Write("NOP");
                else Console.WriteLine(opcode.dump);
            }

            Console.Read();
        }
    }

    public enum AsmRegister
    {
        AL = 0,
        AH = 1,
        A = 2,
        BL = 3,
        BH = 4,
        B = 5,
        CL = 6,
        CH = 7,
        C = 8,
        DL = 9,
        DH = 10,
        D = 11,
        EL = 12,
        EH = 13,
        E = 14,
        FL = 15,
        FH = 16,
        F = 17,
        GL = 18,
        GH = 19,
        G = 20,
        X = 21,
        Y = 22,
        SP = 24,
        BP = 25
    }
}
