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
        static void Main(string[] args)
        {
            AssemblerLib.Assembler asm = new AssemblerLib.Assembler(10, args[2], args[3]);
            string program = File.ReadAllText(args[0] + '\\' + args[1]);

            //asm.writeToFile(asm.Release());

            Lexer l = new Lexer();
            l.Scan(new StringReader(program));

            Parser p = new Parser(l.tokens, ref asm);
            p.Parse();

            foreach (Node n in p.AST)
            {
                if (n is Assignment)
                {
                    Console.WriteLine(n);
                }
                else if (n is Definition)
                {
                    Console.WriteLine(n);
                }
            }

            Console.Read();
        }
    }
}
