﻿using Assembler.Language;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assembler
{
    static class Program
    {
        static void Main(string[] args)
        {
            AssemblerLib.Assembler asm = new AssemblerLib.Assembler(10, args[2], args[3]);
            string program = File.ReadAllText(args[0] + '\\' + args[1]);
            Lexer l = new Lexer();
            l.Scan(new StringReader(program));

            Parser p = new Parser(l.tokens, ref asm);

            p.Parse();

            asm.writeToFile(asm.Release());
        }
    }
}
