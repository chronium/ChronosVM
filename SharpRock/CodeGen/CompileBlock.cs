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
                        asm.Emit(new Jump(2));
                        Program.symbols.DeclareWord((node as Declaration).name);

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
                            asm.Emit(new Print(Program.symbols[(asmi as PrintV).symbol]));
                        }
                        else if (asmi is Crx)
                        {
                            asm.Emit(new AssemblerLib.Crx(Program.symbols[(asmi as Crx).label]));
                        }
                    }
                }
                else if (node is FunctionCall)
                {
                    foreach (Expression exp in (node as FunctionCall).args)
                    {
                        CompileExpression("blah", exp, true);
                    }
                    asm.Emit(new Call((node as FunctionCall).target));
                }
            }
        }
    }
}
