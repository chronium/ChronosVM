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
        public Registers reg;

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
            reg = new Registers();

            peripheralBase = new PeripheralBase(this);

            running = true;

            stack = new StackStuff(ref ram, ref reg);

            this.screen = screen;
            dev = new ScreenDevice();

            dev.setScreen(screen);

            registerDevice(new TestDevice());
            registerDevice(dev);

            Console.SetWindowSize(120, 60);
            Console.SetBufferSize(120, 60);
        }

        public void run()
        {
            while (running)
            {
                OpCode opCode = new OpCode();
                opCode.interpretOpCode(ram, ref reg.IP);

                switch (opCode.instruction)
                {
                    case 0: // NOP
                        break;
                    case 1: // set reg, val
                        switch (opCode.type)
                        {
                            case 0:
                                switch (opCode.reg)
                                {
                                    case 24:
                                        reg.setRegister(24, opCode.value2);
                                        break;
                                    default:
                                        reg.setRegister(opCode.reg1, opCode.value2);
                                        break;
                                }
                                break;
                            case 1:
                                switch (opCode.reg)
                                {
                                    case 24:
                                        reg.setRegister(24, reg.getRegister(opCode.reg2));
                                        break;
                                    default:
                                        reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg2));
                                        break;
                                }
                                break;
                            case 2:
                                switch (opCode.reg)
                                {
                                    case 24:
                                        reg.setRegister(24, reg.IP + opCode.value2);
                                        break;
                                    default:
                                        reg.setRegister(opCode.reg1, reg.IP + opCode.value2);
                                        break;
                                }
                                break;
                        }
                        break;
                    case 2:
                        switch (opCode.type)
                        {
                            case 0: // inc reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) + 1);
                                break;
                            case 1: // dec reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) - 1);
                                break;
                            case 2: // add reg, reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg2) + reg.getRegister(opCode.reg1));
                                break;
                            case 3: // sub reg, reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) - reg.getRegister(opCode.reg2));
                                break;
                            case 4: // mul reg, reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg2) * reg.getRegister(opCode.reg1));
                                break;
                            case 5: // div reg, reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) / reg.getRegister(opCode.reg2));
                                break;
                            case 6: // not reg
                                reg.setRegister(opCode.reg1, ~reg.getRegister(opCode.reg1));
                                break;
                            case 7: // or reg, reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) | reg.getRegister(opCode.reg2));
                                break;
                            case 8: // and reg, reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) & reg.getRegister(opCode.reg2));
                                break;
                            case 9: // xor reg, reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) ^ reg.getRegister(opCode.reg2));
                                break;
                            case 10: // lsh reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) << reg.getRegister(opCode.reg2));
                                break;
                            case 11: // rsh reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) >> reg.getRegister(opCode.reg2));
                                break;
                        }
                        break;
                    case 3:
                        switch (opCode.type)
                        {
                            case 0: // inc reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) + 1);
                                break;
                            case 1: // dec reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) - 1);
                                break;
                            case 2: // add reg, reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) + opCode.value2);
                                break;
                            case 3: // sub reg, reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) - opCode.value2);
                                break;
                            case 4: // mul reg, reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) * opCode.value2);
                                break;
                            case 5: // div reg, reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) / opCode.value2);
                                break;
                            case 6: // not reg
                                reg.setRegister(opCode.reg1, ~opCode.value2);
                                break;
                            case 7: // or reg, reg
                                reg.setRegister(opCode.reg1, (short)reg.getRegister(opCode.reg1) | opCode.value2);
                                break;
                            case 8: // and reg, reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) & opCode.value2);
                                break;
                            case 9: // xor reg, reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) ^ opCode.value2);
                                break;
                            case 10: // lsh reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) << opCode.value2);
                                break;
                            case 11: // rsh reg
                                reg.setRegister(opCode.reg1, reg.getRegister(opCode.reg1) >> opCode.value2);
                                break;
                        }
                        break;
                    case 0x10: // print reg || print char
                        int x = Console.CursorLeft;
                        int y = Console.CursorTop;
                        switch (opCode.type)
                        {
                            case 0:
                                Console.Write((char)(BitConverter.GetBytes(reg.getRegister(opCode.reg1))[0]));
                                break;
                            case 1:
                                Console.Write(opCode.char1);
                                break;
                            case 2:
                                Console.Write(reg.getRegister(opCode.reg1));
                                break;
                            case 3:
                                x = reg.getRegister(opCode.reg1);
                                break;
                            case 4:
                                y = reg.getRegister(opCode.reg1);
                                break;
                            case 5:
                                x = (short)opCode.value1;
                                break;
                            case 6:
                                y = (short)opCode.value1;
                                break;
                            case 7:
                                x = reg.getRegister(opCode.reg1);
                                y = reg.getRegister(opCode.reg2);
                                break;
                            case 8:
                                x = (short)opCode.value1;
                                y = (short)opCode.value2;
                                break;
                            case 9:
                                Console.Clear();
                                break;
                            case 10:
                                //foreground color
                                break;
                            case 11:
                                //backhround color
                                break;
                        }
                        Console.SetCursorPosition(x, y);
                        break;
                    case 0x11: // temp readKey
                        reg.setRegister(opCode.reg, (short)Console.ReadKey(true).KeyChar);
                        break;
                    case 0x12: // push reg || push short
                        switch (opCode.type)
                        {
                            case 0:
                                stack.push((short)reg.getRegister(opCode.reg1));
                                break;
                            case 1:
                                stack.push((short)opCode.value1);
                                break;
                        }
                        break;
                    case 0x13: // pop reg
                        reg.setRegister(opCode.reg, stack.pop((short)0));
                        break;
                    case 0x20: // jump [inst addr]
                        reg.setRegister(23, reg.IP + opCode.value1);
                        break;
                    case 0x21: // je
                        if (reg.EQUAL)
                            reg.setRegister(23, reg.IP + opCode.value1);
                        break;
                    case 0x22: //jne
                        if (!reg.EQUAL)
                            reg.setRegister(23, reg.IP + opCode.value1);
                        break;
                    case 0x30: // cmp reg, val || cmp reg, reg
                        switch (opCode.type)
                        {
                            case 0:
                                if (reg.getRegister(opCode.reg1) == opCode.value2)
                                    reg.EQUAL = true;
                                else reg.EQUAL = false;
                                break;
                            case 1:
                                if (reg.getRegister(opCode.reg1) == reg.getRegister(opCode.reg2))
                                    reg.EQUAL = true;
                                else reg.EQUAL = false;
                                break;
                        }
                        break;
                    case 0x31: // call
                        switch (opCode.type)
                        {
                            case 0:
                                callStack.Push(Convert.ToInt32(reg.IP));
                                reg.setRegister(23, reg.IP + opCode.value1);
                                break;
                        }
                        break;
                    case 0x32: //ret
                        reg.setRegister(23, callStack.Pop());
                        break;
                    case 0x40: // write seg:addr, reg || write seg:addr, val
                        switch (opCode.type)
                        {
                            case 0:
                                ram.writeShort((opCode.value1 * 4096) + opCode.value2, (short)reg.getRegister(opCode.reg3));
                                break;
                            case 1:
                                ram.writeShort((opCode.value1 * 4096) + opCode.value2, opCode.value3);
                                break;
                            case 2:
                                ram.writeShort((opCode.value1 * 4096) + reg.getRegister(opCode.reg2), (short)reg.getRegister(opCode.reg3));
                                break;
                            case 3:
                                ram.writeShort((opCode.value1 * 4096) + reg.getRegister(opCode.reg2), opCode.value3);
                                break;
                        }
                        break;
                    case 0x41: // read reg,seg:addr || read ptr,seg:addr
                        switch (opCode.type)
                        {
                            case 0:
                                reg.setRegister(opCode.reg1, ram.readShort((opCode.value2 * 4096) + opCode.value3));
                                break;
                            case 1:
                                reg.setRegister(opCode.reg1, ram.readShort((opCode.value2 * 4096) + reg.getRegister(opCode.reg3)));
                                break;
                        }
                        break;
                    case 0xFA: // inb port reg || inb port ptr
                        switch (opCode.type)
                        {
                            case 0:
                                reg.setRegister(opCode.reg1, this.peripheralBase.inb(opCode.value2));
                                break;
                        }
                        break;
                    case 0xFB: // inw port reg || inw port ptr
                        switch (opCode.type)
                        {
                            case 0:
                                reg.setRegister(opCode.reg1, this.peripheralBase.inw(opCode.value2));
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
                            case 1:
                                this.peripheralBase.outw(opCode.value1, (short)reg.getRegister(opCode.reg2));
                                break;
                        }
                        break;
                    case 0xFE:
                        break;
                    case 0xFF: // hlt
                        while (true) ;
                }
            }

            System.Windows.Forms.MessageBox.Show("VM Terminated!\nYou may close it now :)");
        }

        public void dumpRam(long size, int shit)
        {
            for (long l = 0; l < size; l++)
            {
                Console.Write(ram.readByte(l).ToString("X") + ',');
            }

            Console.WriteLine();
        }

        public void registerDevice(Device dev)
        {
            dev.init(this);
            this.devices.Add(dev);
        }

        public short allThingsOnCallStack()
        {
            short temp = 0;
            foreach (short s in callStack.ToArray())
            {
                temp += s;
            }
            return temp;
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
        F = 5,
        G = 6,
        X = 7,
        Y = 8,
        IP = 9,
        SP = 10
    }
}
