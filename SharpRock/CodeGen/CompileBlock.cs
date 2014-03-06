using AssemblerLib;
using SharpRock.Language;
using SharpRock.Language.ControlFlow;
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
        public void CompileBlock(Block block)
        {
            foreach (Node node in block.body)
            {
                if (node is Assignment)
                {
                    CompileExpression((node as Assignment).name, (node as Assignment).value);
                }
                else if (node is Declaration)
                {
                    if ((node as Declaration).type == VariableTypes.word)
                    {
                        Program.symbols.DeclareWord((node as Declaration).name, true);

                        if ((node as Declaration).e != null)
                        {
                            CompileExpression((node as Declaration).name, (node as Declaration).e);
                        }
                    }
                }
                else if (node is ILAssembly)
                {
                    foreach (Node asmi in (node as ILAssembly).IL)
                    {
                        if (asmi is PrintC)
                        {
                            asm.Emit(new Print((asmi as PrintC).c));
                        }
                        else if (asmi is PrintV)
                        {
                            asm.Emit(new Read(AsmRegister.C, AsmRegister.BP, Program.symbols.getIndex((asmi as PrintV).symbol)));
                            asm.Emit(new Print(AsmRegister.C));
                        }
                        else if (asmi is Crx)
                        {
                            asm.Emit(new Read(AsmRegister.C, AsmRegister.BP, (short)-Program.symbols.getIndex((asmi as Crx).label)));
                            asm.Emit(new AssemblerLib.Crx(AsmRegister.C));
                        }
                    }
                }
                else if (node is FunctionCall)
                {
                    short idvsavdasdasd = 0;
                    List<Argument> args = new List<Argument>();
                    foreach (Expression exp in (node as FunctionCall).args)
                    {
                        args.Add(new Argument(idvsavdasdasd, exp));
                        idvsavdasdasd += 2;
                    }

                    foreach (Argument arg in args)
                    {
                        CompileExpression("blah", arg.e, true);
                        asm.Emit(new Pop(AsmRegister.D));
                        asm.Emit(new Write(AsmRegister.BP, (short)-arg.id, AsmRegister.D));
                    }

                    asm.Emit(new Call((node as FunctionCall).target));
                }
            }
        }
    }

    public class Argument
    {
        public short id;
        public Expression e;

        public Argument(short id, Expression e)
        {
            this.id = id;
            this.e = e;
        }
    }
}
