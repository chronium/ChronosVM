using AssemblerLib;
using SharpRock.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock.CodeGen
{
    public partial class Compiler
    {
        public AssemblerLib.Assembler asm;
        private List<Node> tree = new List<Node>();
        public List<string> errors = new List<string>();

        public Compiler(AssemblerLib.Assembler asm, List<Node> tree)
        {
            this.asm = asm;
            this.tree = tree;
        }

        public void Compile()
        {
            asm.Emit(new Call("main"));
            asm.Emit(new Jump("9i1j248u235jik12490i-124jin235j9824nui129124nui219012in423k905234mi51230i12mio4120i952356mi40i1242m3i5634k09634im5213k094123jmi523k0653246nui239k0451n4ui230k4213nui5230k512nu4120i423inu523k0-"));
            Program.symbols.BeginScope();
            Compile(tree);
            Program.symbols.EndScope();
            asm.addLabel("9i1j248u235jik12490i-124jin235j9824nui129124nui219012in423k905234mi51230i12mio4120i952356mi40i1242m3i5634k09634im5213k094123jmi523k0653246nui239k0451n4ui230k4213nui5230k512nu4120i423inu523k0-");

            if (this.errors.Count == 0)
                asm.writeToFile(asm.Release());
        }
    }
}
