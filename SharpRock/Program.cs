using SharpRock.CodeGen;
using SharpRock.Language;
using SharpRock.ParserStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock
{
    class Program
    {
        public static SymbolHelper symbols;

        static void Main(string[] args)
        {
            AssemblerLib.Assembler asm = new AssemblerLib.Assembler(10, args[2], args[3]);
            string program = File.ReadAllText(args[0] + '\\' + args[1]);

            symbols = new SymbolHelper(ref asm);

            //asm.writeToFile(asm.Release());

            Lexer l = new Lexer();
            l.Scan(new StringReader(program));

            Parser p = new Parser(l.tokens, ref asm);
            p.Parse();

            if (p.errors.Count <= 0)
            {
                Console.WriteLine("Starting compilation...");
                Compiler c = new Compiler(asm, p.AST);
                c.Compile();

                foreach (string error in c.errors)
                    Console.WriteLine(error);
                Console.WriteLine("Compilation finished :)");
            }
            else
            {
                Console.WriteLine("Could not compile because of parser errors");
                foreach (string error in p.errors)
                    Console.WriteLine(error);
            }

            Console.Read();
        }
    }
}
