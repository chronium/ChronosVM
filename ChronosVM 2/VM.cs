using AssemblerLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronosVM_2
{
    public class VM
    {
        public Ram ram;

        public long IP = 0;
        public short[] registers;

        public bool equal = false;

        public bool running = false;

        public StackStuff stack;

        public PeripheralBase peripheralBase;

        public Stack<int> callStack = new Stack<int>(512);

        Screen screen;
        ScreenDevice dev;

        List<Device> devices = new List<Device>();

        public VM(long ramSize, Screen screen)
        {
            ram = new Ram(ramSize, this);
            registers = new short[9];

            peripheralBase = new PeripheralBase(this);

            running = true;

            stack = new StackStuff(ref ram);

            this.screen = screen;
            dev = new ScreenDevice();

            dev.setScreen(screen);

            registerDevice(new TestDevice());
            registerDevice(dev);


        }

        public void run()
        {
            while (running)
            {
                OpCode opCode = new OpCode();
                opCode.interpretOpCode(ram, ref IP);

                switch (opCode.instruction)
                {
                    case 0: // NOP
                        break;
                    case 1: // set reg, val
                        switch (opCode.reg)
                        {
                            case AsmRegister.SP:
                                stack.SP = opCode.value1;
                                break;
                            default:
                                registers[(int)opCode.reg] = opCode.value1;
                                break;
                        }
                        break;
                    case 2: // inc reg
                        switch (opCode.type)
                        {
                            case 0: // inc reg
                                registers[(int)opCode.reg1]++;
                                break;
                            case 1: // dec reg
                                registers[(int)opCode.reg1]--;
                                break;
                            case 2: // add reg, reg
                                registers[(int)opCode.reg1] += registers[(int)opCode.reg2];
                                break;
                            case 3: // sub reg, reg
                                registers[(int)opCode.reg1] -= registers[(int)opCode.reg2];
                                break;
                            case 4: // mul reg, reg
                                registers[(int)opCode.reg1] *= registers[(int)opCode.reg2];
                                break;
                            case 5: // div reg, reg
                                registers[(int)opCode.reg1] /= registers[(int)opCode.reg2];
                                break;
                            case 6: // not reg
                                registers[(int)opCode.reg1] = (short)~registers[(int)opCode.reg1];
                                break;
                            case 7: // or reg, reg
                                registers[(int)opCode.reg1] |= registers[(int)opCode.reg2];
                                break;
                            case 8: // and reg, reg
                                registers[(int)opCode.reg1] &= registers[(int)opCode.reg2];
                                break;
                            case 9: // xor reg, reg
                                registers[(int)opCode.reg1] ^= registers[(int)opCode.reg2];
                                break;
                            case 10: // lsh reg
                                registers[(int)opCode.reg1] <<= registers[(int)opCode.reg1];
                                break;
                            case 11: // rsh reg
                                registers[(int)opCode.reg1] >>= registers[(int)opCode.reg1];
                                break;
                        }
                        break;
                    case 0x10: // print reg || print char
                        switch (opCode.type)
                        {
                            case 0:
                                Console.Write((char)registers[(int)opCode.reg1]);
                                break;
                            case 1:
                                Console.Write(opCode.char1);
                                break;
                            case 2:
                                Console.Write(registers[(int)opCode.reg1]);
                                break;
                        }
                        break;
                    case 0x11:
                        registers[(int)opCode.reg] = (short)Console.ReadKey(true).KeyChar;
                        break;
                    case 0x12: // push reg || push short
                        switch (opCode.type)
                        {
                            case 0:
                                stack.push(registers[(int)opCode.reg1]);
                                break;
                            case 1:
                                stack.push((short)opCode.value1);
                                break;
                        }
                        break;
                    case 0x13: // pop reg
                        registers[(int)opCode.reg] = stack.pop((short)0);
                        break;
                    case 0x20: // jump [inst addr]
                        IP = opCode.value1 * Program.instructionSize;
                        break;
                    case 0x21: // je
                        if (equal)
                            IP = opCode.value1 * Program.instructionSize;
                        break;
                    case 0x22: //jne
                        if (!equal)
                            IP = opCode.value1 * Program.instructionSize;
                        break;
                    case 0x30: // cmp reg, val || cmp reg, reg
                        switch (opCode.type)
                        {
                            case 0:
                                if (registers[(int)opCode.reg1] == opCode.value2)
                                    equal = true;
                                else equal = false;
                                break;
                            case 1:
                                if (registers[(int)opCode.reg1] == registers[(int)opCode.reg2])
                                    equal = true;
                                else equal = false;
                                break;
                        }
                        break;
                    case 0x31: // call
                        switch (opCode.type)
                        {
                            case 0:
                                callStack.Push(Convert.ToInt32(IP));
                                IP = (opCode.value1) * Program.instructionSize;
                                break;
                        }
                        break;
                    case 0x32: //ret
                        IP = (callStack.Pop());
                        break;
                    case 0x40: // write seg:addr, reg || write seg:addr, val
                        switch (opCode.type)
                        {
                            case 0:
                                ram.writeShort((opCode.value1 * 4096) + opCode.value2, registers[(int)opCode.reg3]);
                                break;
                            case 1:
                                ram.writeShort((opCode.value1 * 4096) + opCode.value2, opCode.value3);
                                break;
                            case 2:
                                ram.writeShort((opCode.value1 * 4096) + registers[(int)opCode.reg2], registers[(int)opCode.reg3]);
                                break;
                            case 3:
                                ram.writeShort((opCode.value1 * 4096) + registers[(int)opCode.reg2], opCode.value3);
                                break;
                        }
                        break;
                    case 0x41: // read reg,seg:addr || read ptr,seg:addr
                        switch (opCode.type)
                        {
                            case 0:
                                registers[(int)opCode.reg1] = ram.readShort((opCode.value2 * 4096) + opCode.value3);
                                break;
                            case 1:
                                registers[(int)opCode.reg1] = ram.readShort((opCode.value2 * 4096) + registers[(int)opCode.reg3]);
                                break;
                        }
                        break;
                    case 0xFA: // inb port reg || inb port ptr
                        switch (opCode.type)
                        {
                            case 0:
                                registers[(int)opCode.reg1] = this.peripheralBase.inb(opCode.value2);
                                break;
                        }
                        break;
                    case 0xFB: // inw port reg || inw port ptr
                        switch (opCode.type)
                        {
                            case 0:
                                registers[(int)opCode.reg1] = this.peripheralBase.inw(opCode.value2);
                                break;
                        }
                        break;
                    case 0xFC: // outb port val
                        switch (opCode.type)
                        {
                            case 0:
                                this.peripheralBase.outb(opCode.value2, (byte)opCode.value2);
                                break;
                        }
                        break;
                    case 0xFD: // outw port data || outw port val
                        switch (opCode.type)
                        {
                            case 0:
                                this.peripheralBase.outw(opCode.value1, opCode.value2);
                                break;
                        }
                        break;
                    case 0xFE: // dumpReg
                        dumpRegisters();
                        break;
                    case 0xFF: // hlt
                        while (true) ;
                }
            }

            System.Windows.Forms.MessageBox.Show("VM Terminated!\nYou may close it now :)");
        }

        public void dumpRam(long size)
        {
            for (long l = 0; l < size; l++)
            {
                Console.Write(ram.readByte(l).ToString("X") + ',');
            }

            Console.WriteLine();
        }

        public void dumpRegisters()
        {
            Console.WriteLine("A:{0} B:{1} C:{2}", registers[(int)AsmRegister.A].ToString("X"), registers[(int)AsmRegister.B].ToString("X"), registers[(int)AsmRegister.C].ToString("X"));
            Console.WriteLine("D:{0} E:{1}", registers[(int)AsmRegister.D].ToString("X"), registers[(int)AsmRegister.E].ToString("X"));
            Console.WriteLine("X:{0} Y:{1}", registers[(int)AsmRegister.X].ToString("X"), registers[(int)AsmRegister.Y].ToString("X"));
            Console.WriteLine("IP:{0} SP:{1}", IP, stack.SP);
            Console.WriteLine();
        }

        public void registerDevice(Device dev)
        {
            dev.init(this);
            this.devices.Add(dev);
        }

        private void updateDevices()
        {
            foreach (Device d in devices)
                d.onTick();
        }
    }

    public enum Register
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
