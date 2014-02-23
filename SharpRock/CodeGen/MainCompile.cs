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
    public partial class Compiler
    {
        public void Compile(List<Node> tree)
        {
            foreach (Node n in tree)
            {
                Program.symbols.BeginScope();
                if (n is Method)
                {
                    asm.addLabel((n as Method).Name);
                    Program.symbols.BeginScope();

                    foreach (Declaration decl in (n as Method).args)
                    {
                        if ((decl as Declaration).type == VariableTypes.word)
                        {
                            asm.Emit(new Jump(2));
                            Program.symbols.DeclareWord((decl as Declaration).name);
                        }
                        //else if ((decl as Declaration).type == VariableTypes.bytev)
                        //{
                        //    asm.Emit(new Jump(1));
                        //    Program.symbols.DeclareWord((decl as Declaration).name);
                        //}
                        asm.Emit(new Push(Program.symbols[(decl as Declaration).name]));
                        asm.Emit(new Pop(true));
                    }

                    CompileBlock((n as Method).block);
                    Program.symbols.EndScope();
                    asm.Emit(new Return());
                }
                Program.symbols.EndScope();
            }
        }

        private void CompileExpression(string name, Expression expression, bool b = false)
        {
            if (Program.symbols[name] == "" && !b)
            {
                errors.Add(name + " does not exist in the current scope");
            }
            else
            {
                foreach (Node no in expression.value)
                {
                    if (no is ShortLiteral)
                        asm.Emit(new Push((no as ShortLiteral).value));
                    else if (no is Operator)
                        switch ((no as Operator).op)
                        {
                            case '+':
                                asm.Emit(new AddS());
                                break;
                            case '-':
                                asm.Emit(new SubS());
                                break;
                            case '*':
                                asm.Emit(new MulS());
                                break;
                            case '/':
                                asm.Emit(new DivS());
                                break;
                        }
                    else if (no is VarPlaceholder)
                        asm.Emit(new Push(Program.symbols[(no as VarPlaceholder).name], true));
                }

                if (!b)
                {
                    asm.Emit(new Push(Program.symbols[name]));
                    asm.Emit(new Pop(true));
                }
            }
        }
    }
}
