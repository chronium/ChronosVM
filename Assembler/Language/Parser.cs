using AssemblerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assembler.Language
{
    public partial class Parser
    {
        private List<Token> tokens;

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
        }

        public void Parse()
        {
            while (i != tokens.ToArray().Length)
            {
                if (peek().ToString().ToLower() == "write")
                {
                    read();
                    //if (peek() is Tokens.IntLiteral)
                        //asm.Emit(new Write((char)(read() as Tokens.IntLiteral).Value));
                    //else
                    //{
                        AsmRegister reg;

                        string regName = (read() as Tokens.Statement).Name.ToUpper();

                        if (Enum.TryParse(regName, out reg))
                        {
                            //asm.Emit(new Write(reg));
                        }
                        else
                        {
                            MessageBox.Show("Unknown register " + regName + "!");
                        }
                    //}
                }
                else if (peek().ToString().ToLower() == "halt")
                {
                    read();
                    asm.Emit(new Halt());
                }
                else if (peek().ToString().ToLower() == "set")
                {
                    read();
                    string regName = (read() as Tokens.Statement).Name.ToUpper();
                    AsmRegister reg = (AsmRegister)Enum.Parse(typeof(AsmRegister), regName);
                    if (!(peek() is Tokens.Comma))
                    {
                        MessageBox.Show("Expected comma somewhere in the program!");
                        Application.Exit();
                    }

                    read();

                    asm.Emit(new SetReg(reg, Convert.ToInt16((read() as Tokens.IntLiteral).Value)));
                }
            }
            //asm.Refactor();
            //asm.Assemble();
        }
    }
}
