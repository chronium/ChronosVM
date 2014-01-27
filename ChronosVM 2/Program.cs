﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChronosVM_2
{
    class Program
    {
        public static int instructionSize = 7;

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        public static VM vm;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Attach to the parent process via AttachConsole SDK call
            AttachConsole(ATTACH_PARENT_PROCESS);

            Screen screen = new Screen();
            vm = new VM(4680 * instructionSize, screen);
            Assembler asm = new Assembler(instructionSize);

            asm.Emit(new Call(2));
            asm.Emit(new Halt());
            asm.Emit(new Write('a'));
            asm.Emit(new Ret());

            byte[] program = asm.Release();

            for (int i = 0; i < program.Length; i++)
            {
                vm.ram.writeByte(i, program[i]);
            }

            screen.startVM();

            Application.Run(screen);
        }
    }
}