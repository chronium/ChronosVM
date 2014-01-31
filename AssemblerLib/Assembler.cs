﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerLib
{
    public class Assembler
    {
        List<byte> ram;
        List<Instruction> instructions;
        Dictionary<string, short> labels = new Dictionary<string, short>();

        short instruction = 0;

        public static int instructionSize;

        public string path;
        public string fileName;

        public Assembler(int instructionSize, string path, string fileName)
        {
            ram = new List<byte>();
            instructions = new List<Instruction>();
            Assembler.instructionSize = instructionSize;
            this.path = path;
            this.fileName = fileName;
        }

        public void Emit(Instruction instruction)
        {
            this.instruction++;
            instructions.Add(instruction);
        }

        public void addLabel(string name)
        {
            labels.Add(name, instruction);
        }

        public byte[] Release()
        {
            doLabelWork();
            List<byte> ramTemp = new List<byte>(instructions.Count * Assembler.instructionSize);

            foreach (Instruction i in instructions)
            {
                foreach (byte b in i.bytes)
                {
                    ramTemp.Add(b);
                }
            }

            return ramTemp.ToArray();
        }

        public void writeToFile(byte[] byteCode)
        {
            using (FileStream fs = new FileStream(path + '\\' + fileName, FileMode.Create))
            {
                fs.Write(byteCode, 0, byteCode.Length);
            }
        }

        public void doLabelWork()
        {
            foreach (Instruction i in instructions)
                if (i is Call)
                {
                    Call c = i as Call;

                    if (c.isLabel)
                        c.setCall(labels[c.label]);
                }
                else if (i is JumpIfEqual)
                {
                    JumpIfEqual j = i as JumpIfEqual;

                    if (j.isLabel)
                        j.setCall(labels[j.label]);
                }
                else if (i is Jump)
                {
                    Jump j = i as Jump;

                    if (j.isLabel)
                        j.setCall(labels[j.label]);
                }
        }
    }

    public abstract class Instruction
    {
        public byte[] bytes;
        public string name;

        public Instruction(string name)
        {
            bytes = new byte[Assembler.instructionSize];

            this.name = name;
        }

        public abstract byte[] emit();

        /// <summary>
        /// Set Byte[0]
        /// </summary>
        /// <param name="b">Instruction number</param>
        public void setInstruction(short b)
        {
            this.bytes[0] = BitConverter.GetBytes(b)[0];
            this.bytes[1] = BitConverter.GetBytes(b)[1];
        }

        /// <summary>
        /// Set Register byte[2]
        /// </summary>
        /// <param name="reg">Expected register</param>
        public void setReg(AsmRegister reg)
        {
            this.bytes[2] = (byte)reg;
        }

        /// <summary>
        /// Sets register at byte[4]
        /// </summary>
        /// <param name="reg">Desired register</param>
        public void setReg1(AsmRegister reg)
        {
            this.bytes[4] = (byte)reg;
        }

        /// <summary>
        /// Sets register at byte[6]
        /// </summary>
        /// <param name="reg">Desired register</param>
        public void setReg2(AsmRegister reg)
        {
            this.bytes[6] = (byte)reg;
        }

        /// <summary>
        /// Sets register at byte[8]
        /// </summary>
        /// <param name="reg">Desired register</param>
        public void setReg3(AsmRegister reg)
        {
            this.bytes[8] = (byte)reg;
        }

        public void setVal0(short val1)
        {
            this.bytes[2] = BitConverter.GetBytes(val1)[0];
            this.bytes[3] = BitConverter.GetBytes(val1)[1];
        }

        /// <summary>
        /// Sets short at bytes[4]
        /// </summary>
        /// <param name="val3">Desired short</param>
        public void setVal1(short val2)
        {
            this.bytes[4] = BitConverter.GetBytes(val2)[0];
            this.bytes[5] = BitConverter.GetBytes(val2)[1];
        }

        /// <summary>
        /// Sets short at bytes[6]
        /// </summary>
        /// <param name="val3">Desired short</param>
        public void setVal2(short val3)
        {
            this.bytes[6] = BitConverter.GetBytes(val3)[0];
            this.bytes[7] = BitConverter.GetBytes(val3)[1];
        }

        /// <summary>
        /// Sets short at bytes[8]
        /// </summary>
        /// <param name="val3">Desired short</param>
        public void setVal3(short val3)
        {
            this.bytes[8] = BitConverter.GetBytes(val3)[0];
            this.bytes[9] = BitConverter.GetBytes(val3)[1];
        }

        public void setType(byte type)
        {
            this.bytes[2] = type;
        }
    }

    public class SetReg : Instruction
    {
        public SetReg(AsmRegister reg, short value)
            : base("Set Reg")
        {
            this.setInstruction(0x01);
            this.setReg(reg);
            this.setVal1(value);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class IncReg : Instruction
    {
        public IncReg(AsmRegister reg)
            : base("Inc Reg")
        {
            this.setInstruction(0x02);
            this.setType(0x00);
            this.setReg1(reg);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class DecReg : Instruction
    {
        public DecReg(AsmRegister reg)
            : base("Dec Reg")
        {
            this.setInstruction(0x02);
            this.setType(0x01);
            this.setReg1(reg);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class AddReg : Instruction
    {
        public AddReg(AsmRegister reg, AsmRegister reg1)
            : base("Add Reg")
        {
            this.setInstruction(0x02);
            this.setType(0x02);
            this.setReg1(reg);
            this.setReg2(reg1);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class SubReg : Instruction
    {
        public SubReg(AsmRegister reg, AsmRegister reg1)
            : base("Sub Reg")
        {
            this.setInstruction(0x02);
            this.setType(0x03);
            this.setReg1(reg);
            this.setReg2(reg1);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class MulReg : Instruction
    {
        public MulReg(AsmRegister reg, AsmRegister reg1)
            : base("MulReg")
        {
            this.setInstruction(0x02);
            this.setType(0x04);
            this.setReg1(reg);
            this.setReg2(reg1);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class DivReg : Instruction
    {
        public DivReg(AsmRegister reg, AsmRegister reg1)
            : base("Div Reg")
        {
            this.setInstruction(0x02);
            this.setType(0x05);
            this.setReg1(reg);
            this.setReg2(reg1);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class ReadKey : Instruction
    {
        public ReadKey(AsmRegister reg)
            : base("Read Key")
        {
            this.setInstruction(0x11);
            this.setReg(reg);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class Print : Instruction
    {
        public Print(AsmRegister reg, bool type = false)
            : base("Print")
        {
            this.setInstruction(0x10);
            this.setType(type == false ? (byte)0x00 : (byte)0x02);
            this.setReg1(reg);
        }

        public Print(char s)
            : base("Write")
        {
            this.setInstruction(0x10);
            this.setType(0x01);
            this.setVal1((short)s);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class Write : Instruction
    {
        public Write(short seg, short addr, AsmRegister reg)
            : base("Write")
        {
            this.setInstruction(0x40);
            this.setType(0x00);
            this.setVal1(seg);
            this.setVal2(addr);
            this.setReg3(reg);
        }

        public Write(short seg, short addr, short reg)
            : base("Write")
        {
            this.setInstruction(0x40);
            this.setType(0x01);
            this.setVal1(seg);
            this.setVal2(addr);
            this.setVal3(reg);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class Read : Instruction
    {
        public Read(AsmRegister reg, short seg, short addr)
            : base("Read")
        {
            this.setInstruction(0x41);
            this.setType(0x00);
            this.setReg1(reg);
            this.setVal2(seg);
            this.setVal3(addr);
        }

        public Read(AsmRegister reg, short seg, AsmRegister reg1)
            : base("Read")
        {
            this.setInstruction(0x41);
            this.setType(0x01);
            this.setReg1(reg);
            this.setVal2(seg);
            this.setReg3(reg1);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class DumpReg : Instruction
    {
        public DumpReg()
            : base("Dump Reg")
        {
            this.setInstruction(0xFE);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class Jump : Instruction
    {
        public string label = null;
        public bool isLabel = false;

        public Jump(short inst)
            : base("Jump")
        {
            this.setInstruction(0x20);
            this.setVal1(inst);
        }

        public Jump(string label)
            : base("Jump")
        {
            this.setInstruction(0x20);
            this.label = label;
            isLabel = true;
        }

        public void setCall(short inst)
        {
            this.setVal1(inst);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class JumpIfEqual : Instruction
    {
        public string label = null;
        public bool isLabel = false;

        public JumpIfEqual(short inst)
            : base("Jump If Equal")
        {
            this.setInstruction(0x21);
            this.setVal1(inst);
        }

        public JumpIfEqual(string label)
            : base("Jump If Equal")
        {
            this.setInstruction(0x21);
            this.label = label;
            isLabel = true;
        }

        public void setCall(short inst)
        {
            this.setVal1(inst);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class JumpIfNotEqual : Instruction
    {
        public JumpIfNotEqual(short inst)
            : base("Jump If Not Equal")
        {
            this.setInstruction(0x22);
            this.setVal1(inst);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class Compare : Instruction
    {
        public Compare(AsmRegister reg, short val)
            : base("Copmare")
        {
            this.setInstruction(0x30);
            this.setType(0x00);
            this.setReg1(reg);
            this.setVal2(val);
        }

        public Compare(AsmRegister reg, AsmRegister reg2)
            : base("Copmare")
        {
            this.setInstruction(0x30);
            this.setType(0x01);
            this.setReg1(reg);
            this.setReg2(reg2);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class Push : Instruction
    {
        public Push(AsmRegister reg)
            : base("Push")
        {
            this.setInstruction(0x12);
            this.setType(0x00);
            this.setReg1(reg);
        }

        public Push(short val)
            : base("Push")
        {
            this.setInstruction(0x12);
            this.setType(0x01);
            this.setVal1(val);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class Pop : Instruction
    {
        public Pop(AsmRegister reg)
            : base("Pop")
        {
            this.setInstruction(0x13);
            this.setReg(reg);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class Inb : Instruction
    {
        public Inb(short port, AsmRegister reg)
            : base("Inb")
        {
            this.setInstruction(0xFA);
            this.setType(0);
            this.setReg1(reg);
            this.setVal2(port);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class Inw : Instruction
    {
        public Inw(short port, AsmRegister reg)
            : base("Inw")
        {
            this.setInstruction(0xFB);
            this.setType(0);
            this.setReg1(reg);
            this.setVal2(port);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class Outw : Instruction
    {
        public Outw(short port, short value)
            : base("Outw")
        {
            this.setInstruction(0xFD);
            this.setType(0);
            this.setVal1(port);
            this.setVal2(value);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class Call : Instruction
    {
        public bool isLabel = false;
        public string label = null;

        public Call(short inst)
            : base("Call")
        {
            this.setInstruction(0x31);
            this.setType(0);
            this.setVal1(inst);
        }

        public Call(string label)
            : base("Call")
        {
            this.setInstruction(0x31);
            this.setType(0);
            this.label = label;
            isLabel = true;
        }

        public void setCall(short inst)
        {
            this.setVal1(inst);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class Return : Instruction
    {
        public Return()
            : base("Ret")
        {
            this.setInstruction(0x32);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public class Halt : Instruction
    {
        public Halt()
            : base("Halt")
        {
            this.setInstruction(0xFF);
        }

        public override byte[] emit()
        {
            string s = "Emmitted " + this.name + " with the value of: ";

            foreach (byte b in bytes)
                s += b.ToString("X") + ";";

            Console.WriteLine(s);
            return this.bytes;
        }
    }

    public enum AsmRegister
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        X = 5,
        Y = 6,
        IP = 7,
        SP = 8
    }
}
