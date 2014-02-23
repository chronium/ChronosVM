using AssemblerLib;
using SharpRock.Language;
using SharpRock.ParserStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock.CodeGen
{
    public class casm
    {
        private List<Token> tokens;
        private AssemblerLib.Assembler asm;
        private Parser parser;

        int i = 0;

        private Token peek(int offset = 0)
        {
            if (i <= tokens.Count)
            {
                try
                {
                    return tokens[i + offset];
                }
                catch { }
                Tokens.Statement stat = new Tokens.Statement();
                stat.Name = "";
                return stat;
            }
            else
            {
                Tokens.Statement stat = new Tokens.Statement();
                stat.Name = "";
                return stat;
            }
        }

        private Token read()
        {
            if (i <= tokens.Count)
            {
                try
                {
                    return tokens[i++];
                }
                catch { }
                Tokens.Statement stat = new Tokens.Statement();
                stat.Name = "";
                return stat;
            }
            else
            {
                Tokens.Statement stat = new Tokens.Statement();
                stat.Name = "";
                return stat;
            }
        }

        public casm(List<Token> tokens, ref AssemblerLib.Assembler asm, Parser parser)
        {
            this.tokens = tokens;
            this.asm = asm;
            this.parser = parser;
        }

        public List<Node> Parse()
        {
            List<Node> nodes = new List<Node>();
            while (i < tokens.Count)
            {
                if (peek().ToString().ToLower() == "print")
                {
                    read();
                    if (peek() is Tokens.IntLiteral)
                    {
                        nodes.Add(new PrintC((char)(read() as Tokens.IntLiteral).Value));
                    }
                    else if (peek() is Tokens.Dollah)
                    {
                        read();
                        nodes.Add(new PrintV(read().ToString()));
                    }
                }
                else if (peek().ToString().ToLower() == "crx")
                {
                    read();
                    read();
                    nodes.Add(new Crx(read().ToString()));
                }
            }

            return nodes;
        }
    }

    public class PrintC : Node
    {
        public char c;

        public PrintC(char c)
        {
            this.c = c;
        }
    }

    public class PrintV : Node
    {
        public string symbol;

        public PrintV(string symbol)
        {
            this.symbol = symbol;
        }
    }

    public class Crx : Node
    {
        public string label;

        public Crx(string label)
        {
            this.label = label;
        }
    }
}
