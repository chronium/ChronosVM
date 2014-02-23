using SharpRock.ParserStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock.CodeGen
{
    public class SymbolHelper
    {
        private AssemblerLib.Assembler asm;
        private int scopeIndex = 0;
        public Stack<Scope> scopes = new Stack<Scope>();

        public SymbolHelper(ref AssemblerLib.Assembler asm)
        {
            this.asm = asm;
        }

        public void DeclareWord(string name)
        {
            Variable var = new Variable();
            var.name = name;
            var.realName = GetScopePrefix() + name;
            var.type = VariableTypes.word;
            scopes.Peek().variables.Add(var);
            asm.makeBuffer(var.realName, sizeof(short));
        }

        public void BeginScope()
        {
            BeginScope(scopeIndex.ToString());
            scopeIndex++;
        }

        public void BeginScope(string name)
        {
            Scope sc = new Scope();
            sc.name = name;
            scopes.Push(sc);
        }

        public void EndScope()
        {
            scopes.Pop();
        }

        private string GetScopePrefix()
        {
            StringBuilder sb = new StringBuilder("");
            foreach (Scope scope in this.scopes)
            {
                sb.Append(scope.name);
                sb.Append("_");
            }

            return sb.ToString();
        }

        public string this[string name]
        {
            get
            {
                foreach (Scope s in this.scopes)
                    foreach (Variable v in s.variables)
                        if (v.name == name)
                            return v.realName;
                return "";
            }
        }
    }

    public class Scope
    {
        public string name = "";
        public List<Variable> variables = new List<Variable>();
    }

    public class Variable
    {
        public string name;
        public string realName;
        public VariableTypes type;
    }
}
