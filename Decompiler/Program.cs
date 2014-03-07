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
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
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
                    writeInstruction("set");
                    string inst = "";
                    switch (opcode.type)
                    {
                        case 0:
                            switch (opcode.reg1)
                            {
                                case (byte)AsmRegister.SP:
                                    writeRegister(opcode.reg1);

                                    Console.Write(", ");

                                    writeValue((ushort)opcode.value2);

                                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                    Console.Write(" (0x" + ((ushort)opcode.value2).ToString("X") + ")");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    break;
                                default:
                                    writeRegister(opcode.reg1);
                                    inst = ", " + opcode.value2;
                                    break;
                            }
                            break;
                        case 1:
                            writeRegister(opcode.reg1);
                            Console.Write(", ");
                            writeRegister(opcode.reg2);
                            break;
                        case 2:
                            writeRegister(opcode.reg1);
                            Console.Write(", ");
                            writeValue(ip + opcode.value2);
                            break;
                    }
                    Console.WriteLine(inst);
                }
                else if (opcode.instruction == 0x3)
                {
                    string inst = "";
                    switch (opcode.type)
                    {
                        case 0:
                            writeInstruction("inc");
                            writeRegister(opcode.reg1);
                            break;
                        case 1:
                            writeInstruction("dec");
                            writeRegister(opcode.reg1);
                            break;
                        case 2:
                            writeInstruction("add");
                            writeRegister(opcode.reg1);
                            Console.Write(", ");
                            writeValue(opcode.value2);
                            break;
                        case 3:
                            writeInstruction("sub");
                            writeRegister(opcode.reg1);
                            Console.Write(", ");
                            writeValue(opcode.value2);
                            break;
                        case 4:
                            writeInstruction("mul");
                            writeRegister(opcode.reg1);
                            Console.Write(", ");
                            writeValue(opcode.value2);
                            break;
                        case 5:
                            writeInstruction("div");
                            writeRegister(opcode.reg1);
                            Console.Write(", ");
                            writeValue(opcode.value2);
                            break;
                    }
                    Console.WriteLine(inst);
                }
                else if (opcode.instruction == 0x10)
                {
                    switch (opcode.type)
                    {
                        case 0:
                            writeInstruction("print");
                            writeRegister(opcode.reg1);
                            break;
                        case 1:
                            writeInstruction("print");
                            writeChar(opcode.char1);
                            break;
                        default:
                            Console.WriteLine(opcode.dump);
                            break;
                    }
                    Console.WriteLine();
                }
                else if (opcode.instruction == 0x12)
                {
                    writeInstruction("push");
                    string inst = "";
                    switch (opcode.type)
                    {
                        case 0:
                            writeRegister(opcode.reg1);
                            break;
                        case 1:
                            writeValue(opcode.value1);
                            break;
                        case 2:
                            writeValue(ip + opcode.value1);
                            break;
                        case 3:
                            Console.WriteLine("ram[");
                            Console.WriteLine(ip + opcode.value1);
                            Console.WriteLine("]");
                            break;
                    }
                    Console.WriteLine(inst);
                }
                else if (opcode.instruction == 0x13)
                {
                    writeInstruction("pop");
                    string inst = "";
                    writeRegister(opcode.reg);
                    Console.WriteLine(inst);
                }
                else if (opcode.instruction == 0x20)
                {
                    writeInstruction("jump");
                    string inst = "";
                    Console.Write("offset ");
                    writeValue(opcode.value1);
                    Console.WriteLine(inst);
                }
                else if (opcode.instruction == 0x31)
                {
                    writeInstruction("call");
                    string inst = "";
                    Console.Write("offset ");
                    writeValue(opcode.value1);
                    Console.WriteLine(inst);
                }
                else if (opcode.instruction == 0x32)
                {
                    writeInstruction("ret");
                    string inst = "";
                    Console.WriteLine(inst);
                }
                else if (opcode.instruction == 0x40)
                {
                    writeInstruction("write");
                    string inst = "";
                    switch (opcode.type)
                    {
                        case 0:
                            inst += (ushort)opcode.value1 + " + " + (short)opcode.value2 + ", " + (AsmRegister)opcode.reg3;
                            break;
                        case 1:
                            inst += (ushort)opcode.value1 + " + " + (short)opcode.value2 + ", " + opcode.value3;
                            break;
                        case 2:
                            inst += (AsmRegister)opcode.reg1 + " + " + (short)opcode.value2 + ", " + (AsmRegister)opcode.reg3;
                            break;
                        case 3:
                            inst += (AsmRegister)opcode.reg1 + " + " + (short)opcode.value2 + ", " + opcode.value3;
                            break;
                    }
                    Console.WriteLine(inst);
                }
                else if (opcode.instruction == 0x41)
                {
                    writeInstruction("read");
                    string inst = "";
                    switch (opcode.type)
                    {
                        case 0:
                            writeRegister(opcode.reg1);
                            Console.Write(", ");
                            writeValue(opcode.value2);
                            Console.Write(" + ");
                            writeValue(opcode.value3);
                            break;
                        case 1:
                            writeRegister(opcode.reg1);
                            Console.Write(", ");
                            writeRegister(opcode.reg2);
                            Console.Write(" + ");
                            writeValue(opcode.value3);
                            break;
                    }
                    Console.WriteLine(inst);
                }
                else if (opcode.instruction == 0) { }
                else Console.WriteLine(opcode.dump);
            }

            Console.Read();
        }

        private static void writeChar(byte c)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("'" + (char)c + "' ");
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public static void writeInstruction(string text)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(text + " ");
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public static void writeRegister(byte reg)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write((AsmRegister)reg);
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public static void writeValue(object val)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(val);
            Console.ForegroundColor = ConsoleColor.Black;
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
