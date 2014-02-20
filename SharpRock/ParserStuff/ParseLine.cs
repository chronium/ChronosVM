using SharpRock.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock.ParserStuff
{
    public partial class Parser
    {
        public void parseLine()
        {
            while (!(peek() is Tokens.SemiColon))
            {
                if (peek().ToString() == "word")
                {
                    read();
                    string name = read().ToString();

                    AST.Add(new Definition(name, VariableTypes.word));

                    if (peek() is Tokens.Assign)
                    {
                        if (!(peek() is Tokens.Assign))
                        {
                            throw new Exception("Expected an equal sign after " + name + "!!!");
                        }
                        read();
                        List<Token> exp = new List<Token>();
                        while (!(peek() is Tokens.SemiColon))
                        {
                            exp.Add(read());
                        }
                        AST.Add(new Assignment(name, parseExpression(exp)));
                    }
                }
                else if (peek().ToString() == "byte")
                {
                    read();
                    string name = read().ToString();

                    AST.Add(new Definition(name, VariableTypes.bytev));

                    if (peek() is Tokens.Assign)
                    {
                        if (!(peek() is Tokens.Assign))
                        {
                            throw new Exception("Expected an equal sign after " + name + "!!!");
                        }
                        read();
                        List<Token> exp = new List<Token>();
                        while (!(peek() is Tokens.SemiColon))
                        {
                            exp.Add(read());
                        }
                        AST.Add(new Assignment(name, parseExpression(exp)));
                    }
                }
                else if (peek() is Tokens.Statement)
                {
                    string name = read().ToString();
                    if (!(peek() is Tokens.Assign))
                    {
                        throw new Exception("Expected an equal sign after " + name + "!!!");
                    }
                    read();
                    List<Token> exp = new List<Token>();
                    while (!(peek() is Tokens.SemiColon))
                    {
                        exp.Add(read());
                    }
                    AST.Add(new Assignment(name, parseExpression(exp)));
                }
            }
            read();
            return;
        }
    }

    public enum VariableTypes
    {
        word,
        bytev
    }
}
