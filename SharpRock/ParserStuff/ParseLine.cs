using SharpRock.CodeGen;
using SharpRock.Language;
using SharpRock.Language.ControlFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock.ParserStuff
{
    public partial class Parser
    {
        public Node parseLine()
        {
            Node ret = null;
            while (!(peek() is Tokens.SemiColon))
            {
                if (peek().ToString() == "word")
                {
                    read();
                    string name = read().ToString();

                    if (peek() is Tokens.Assign)
                    {
                        if (!(peek() is Tokens.Assign))
                        {
                            errors.Add("Expected an equal sign after " + name + "!!!");
                            break;
                        }
                        read();
                        List<Token> exp = new List<Token>();
                        while (!(peek() is Tokens.SemiColon))
                        {
                            exp.Add(read());
                        }
                        ret = new Declaration(name, VariableTypes.word, parseExpression(exp));
                    }
                    else
                        ret = new Declaration(name, VariableTypes.word);
                }
                else if (peek().ToString() == "byte")
                {
                    read();
                    string name = read().ToString();

                    if (peek() is Tokens.Assign)
                    {
                        if (!(peek() is Tokens.Assign))
                        {
                            errors.Add("Expected an equal sign after " + name + "!!!");
                            break;
                        }
                        read();
                        List<Token> exp = new List<Token>();
                        while (!(peek() is Tokens.SemiColon))
                        {
                            exp.Add(read());
                        }
                        ret = new Declaration(name, VariableTypes.bytev, parseExpression(exp));
                    }
                    else
                        ret = new Declaration(name, VariableTypes.bytev);
                }
                else if (peek() is Tokens.Statement && peek(1) is Tokens.openParenthesis)
                {
                    List<Expression> args = new List<Expression>();
                    string name = read().ToString();
                    read();
                    while (!(peek() is Tokens.closeParenthesis))
                    {
                        List<Token> toks = new List<Token>();
                        while (!(peek() is Tokens.Comma) && !(peek() is Tokens.closeParenthesis))
                            toks.Add(read());
                        args.Add(parseExpression(toks));

                        if (!(peek() is Tokens.Comma) && !(peek() is Tokens.closeParenthesis))
                        {
                            errors.Add("Expected , or ) " + peek().ToString());
                            break;
                        }
                        else if (peek() is Tokens.Comma)
                            read();
                    }
                    read();
                    read();
                    return new FunctionCall(name, args);
                }
                else if (peek() is Tokens.Statement)
                {
                    string name = read().ToString();
                    if (peek() is Tokens.Assign)
                    {
                        read();
                        List<Token> exp = new List<Token>();
                        while (!(peek() is Tokens.SemiColon))
                        {
                            exp.Add(read());
                        }
                        ret = new Assignment(name, parseExpression(exp));
                    }
                    else if (peek() is Tokens.PlusEquals)
                    {
                        read();
                        List<Token> exp = new List<Token>();
                        Tokens.Statement st = new Tokens.Statement();
                        st.Name = name;
                        exp.Add(st);
                        exp.Add(new Tokens.Add());
                        while (!(peek() is Tokens.SemiColon))
                        {
                            exp.Add(read());
                        }
                        ret = new Assignment(name, parseExpression(exp));
                    }
                    else if (!(peek() is Tokens.Assign))
                    {
                        errors.Add("Expected an equal sign after " + name + "!!!");
                        break;
                    }
                }
                else if (peek() is Tokens.Underline && peek(1).ToString() == "asm")
                {
                    read(); read();
                    if (!(peek() is Tokens.openCurlyBracket))
                    {
                        errors.Add("Expected an open curly bracket after asm");
                        break;
                    }
                    read();
                    List<Token> il = new List<Token>();
                    while (!(peek() is Tokens.closeCurlyBracket))
                        il.Add(read());
                    read();
                    casm assembler = new casm(il, ref asm, this);
                    ret = new ILAssembly(assembler.Parse());
                }
            }
            read();
            return ret;
        }
    }

    public enum VariableTypes
    {
        word,
        bytev
    }
}
