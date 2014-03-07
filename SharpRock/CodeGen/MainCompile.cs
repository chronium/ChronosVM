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

                    int locals = 0;

                    int argsize = 0;

                    foreach (Declaration decl in ((Method)n).args)
                        argsize += 2;

                    asm.Emit(new SubReg(AsmRegister.SP, (short)argsize));
                    asm.Emit(new Push(AsmRegister.BP));
                    asm.Emit(new SetReg(AsmRegister.BP, AsmRegister.SP));

                    SymbolHelper.localIndex = (0 - argsize);

                    foreach (Declaration decl in ((Method)n).args)
                        Program.symbols.DeclareWord(decl.name, true);

                    SymbolHelper.localIndex = 0;

                    foreach (Node d in ((Method)n).block.body)
                        if (d is Declaration)
                            locals += 2;

                    asm.Emit(new SubReg(AsmRegister.SP, (short)locals));

                    CompileBlock((n as Method).block);
                    Program.symbols.EndScope();
                    asm.Emit(new AddReg(AsmRegister.SP, (short)locals));
                    asm.Emit(new Pop(AsmRegister.BP));
                    asm.Emit(new Return());
                }
                Program.symbols.EndScope();
            }
        }

        private void CompileExpression(string name, Expression expression, bool b = false)
        {
            if (Program.symbols[name] == "" && !b)
            {
                errors.Add(name + " does not exist in the current context");
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
                    {
                        asm.Emit(new Read(AsmRegister.C, AsmRegister.BP, (short)-Program.symbols.getIndex((no as VarPlaceholder).name)));
                        asm.Emit(new Push(AsmRegister.C));
                    }
                }
                if (!b)
                {
                    asm.Emit(new Pop(AsmRegister.D));
                    asm.Emit(new Write(AsmRegister.BP, (short)-Program.symbols.getIndex(name), AsmRegister.D));
                }
            }
        }
    }
}
