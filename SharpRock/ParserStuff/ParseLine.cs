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
                else if (peek().ToString() == "return")
                {
                    read();
                    List<Token> exp = new List<Token>();
                    while (!(peek() is Tokens.SemiColon))
                    {
                        exp.Add(read());
                    }

                    ret = new Return(parseExpression(exp));
                }
                else if (peek() is Tokens.Statement && peek(1) is Tokens.openParenthesis)
                {
                    List<Expression> args = new List<Expression>();
                    string name = read().ToString();
                    read();

                    int parentheses = 1;

                    while (parentheses > 0)
                    {
                        List<Token> toks = new List<Token>();
                        int startParenth = parentheses - 1;
                        while (!(peek() is Tokens.Comma) && parentheses != startParenth)
                        {
                            if (peek() is Tokens.openParenthesis)
                                parentheses++;
                            else if (peek() is Tokens.closeParenthesis)
                                parentheses--;
                            if (startParenth == parentheses)
                                break;
                            toks.Add(read());
                        }
                        args.Add(parseExpression(toks));
                        if (parentheses == 0)
                            break;
                        if (peek() is Tokens.Comma)
                            read();
                        else if (peek() is Tokens.openParenthesis)
                        {
                            parentheses++;
                            peek();
                        }
                        else if (peek() is Tokens.closeParenthesis)
                        {
                            parentheses--;
                            peek();
                        }
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
