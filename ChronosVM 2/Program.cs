using AssemblerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChronosVM_2
{
    class Program
    {
        public static int instructionSize = 10;

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

            byte[] program = File.ReadAllBytes(args[0] + '\\' + args[1]);

            for (int i = 0; i < program.Length; i++)
            {
                vm.ram.writeByte(i, program[i]);
            }

            Console.Clear();

            screen.startVM();

            //Application.Run(screen);
        }
    }
}
