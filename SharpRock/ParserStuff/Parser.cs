﻿using SharpRock.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock.ParserStuff
{
    public partial class Parser
    {
        private List<Token> tokens;
        public List<Node> AST;

        public List<string> errors = new List<string>();

        AssemblerLib.Assembler asm;

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

        public Parser(List<Token> tokens, ref AssemblerLib.Assembler asm)
        {
            this.tokens = tokens;
            this.asm = asm;
            AST = new List<Node>();
        }

        public void Parse()
        {
            while (i < tokens.Count)
            {
                if (peek() is Tokens.Statement && peek(1) is Tokens.Statement && peek(2) is Tokens.openParenthesis)
                {
                    List<Declaration> args = new List<Declaration>();
                    string retType = read().ToString();
                    string name = read().ToString();
                    Console.WriteLine(retType + " " + name);
                    if (!(peek() is Tokens.openParenthesis))
                    {
                        errors.Add("Expected an open and closed parenthesis after the method declaration!!!");
                        break;
                    }
                    read();
                    while (!(peek() is Tokens.closeParenthesis))
                    {
                        string ret = read().ToString();
                        args.Add(new Declaration(read().ToString(), ret == "word" ? VariableTypes.word : VariableTypes.bytev));
                        if (peek() is Tokens.Comma)
                            read();
                        else if (!(peek() is Tokens.closeParenthesis))
                        {
                            errors.Add("Unexpected " + peek().ToString());
                        }
                    }

                    read();
                    AST.Add(new Method(name, retType, parseBlock(), args));
                }
            }
        }
    }
}
