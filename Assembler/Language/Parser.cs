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
                if (peek().ToString().ToLower() == "print")
                {
                    read();
                    if (peek() is Tokens.IntLiteral)
                        asm.Emit(new Print((char)(read() as Tokens.IntLiteral).Value));
                    else
                    {
                        asm.Emit(new Print(getReg(read())));
                    }
                }
                else if (peek().ToString().ToLower() == "write")
                {
                    read();
                    short seg = Convert.ToInt16((read() as Tokens.IntLiteral).Value);
                    read();
                    if (peek() is Tokens.IntLiteral)
                    {
                        short addr = Convert.ToInt16((read() as Tokens.IntLiteral).Value);

                        checkForComma();

                        if (peek() is Tokens.Statement)
                        {
                            AsmRegister reg = getReg(read());

                            asm.Emit(new Write(seg, addr, reg));
                        }
                        else if (peek() is Tokens.IntLiteral)
                        {
                            Tokens.IntLiteral val = read() as Tokens.IntLiteral;

                            asm.Emit(new Write(seg, addr, Convert.ToInt16(val.Value)));

                        }
                    }
                    else if (peek() is Tokens.Statement)
                    {
                        AsmRegister reg1 = getReg(read());

                        checkForComma();

                        if (peek() is Tokens.Statement)
                        {
                            AsmRegister reg = getReg(read());

                            asm.Emit(new Write(seg, reg1, reg));
                        }
                        else if (peek() is Tokens.IntLiteral)
                        {
                            Tokens.IntLiteral val = read() as Tokens.IntLiteral;

                            asm.Emit(new Write(seg, reg1, Convert.ToInt16(val.Value)));

                        }
                    }
                }
                else if (peek().ToString().ToLower() == "read")
                {
                    read();
                    AsmRegister reg = getReg(read());

                    checkForComma();

                    short seg = Convert.ToInt16((read() as Tokens.IntLiteral).Value);
                    read();
                    if (peek() is Tokens.Statement)
                    {
                        AsmRegister reg1 = getReg(read());

                        asm.Emit(new Read(reg, seg, reg1));
                    }
                    else
                    {
                        short addr = Convert.ToInt16((read() as Tokens.IntLiteral).Value);

                        asm.Emit(new Read(reg, seg, addr));
                    }
                }
                else if (peek().ToString().ToLower() == "halt")
                {
                    read();
                    asm.Emit(new Halt());
                }
                else if (peek().ToString().ToLower() == "set")
                {
                    read();
                    AsmRegister reg = getReg(read());

                    checkForComma();

                    if (!(peek() is Tokens.IntLiteral) && !(peek() is Tokens.Dot) && !(peek() is Tokens.Statement))
                    {
                        MessageBox.Show("Expected stuff somewhere in the program!");
                        Application.Exit();
                    }

                    if (peek() is Tokens.IntLiteral)
                    {
                        asm.Emit(new SetReg(reg, Convert.ToInt16((read() as Tokens.IntLiteral).Value)));
                    }
                    else if (peek() is Tokens.Dot)
                    {
                        read();

                        Tokens.Statement label = read() as Tokens.Statement;

                        asm.Emit(new SetReg(reg, label.Name));
                    }
                    else if (peek() is Tokens.Statement)
                    {
                        asm.Emit(new SetReg(reg, getReg(read())));
                    }
                }
                else if (peek().ToString().ToLower() == "jump")
                {
                    read();

                    if (!(peek() is Tokens.IntLiteral))
                    {
                        if (!(peek() is Tokens.Dot))
                        {
                            MessageBox.Show("Expected a label somewhere in the program!");
                            Application.Exit();
                        }
                        else if (!(peek() is Tokens.IntLiteral) && !(peek() is Tokens.Dot))
                        {
                            MessageBox.Show("Expected an address somewhere in the program!");
                            Application.Exit();
                        }
                    }

                    if (peek() is Tokens.IntLiteral)
                    {
                        Tokens.IntLiteral inst = read() as Tokens.IntLiteral;

                        asm.Emit(new Jump(Convert.ToInt16(inst.Value)));
                    }
                    else if (peek() is Tokens.Dot)
                    {
                        read();

                        Tokens.Statement label = read() as Tokens.Statement;

                        asm.Emit(new Jump(label.Name));
                    }
                }
                else if (peek().ToString().ToLower() == "ret")
                {
                    read();
                    asm.Emit(new Return());
                }
                else if (peek().ToString().ToLower() == "call")
                {
                    read();

                    if (!(peek() is Tokens.IntLiteral))
                    {
                        if (!(peek() is Tokens.Dot))
                        {
                            MessageBox.Show("Expected a label somewhere in the program!");
                            Application.Exit();
                        }
                        else if (!(peek() is Tokens.IntLiteral) && !(peek() is Tokens.Dot))
                        {
                            MessageBox.Show("Expected an address somewhere in the program!");
                            Application.Exit();
                        }
                    }

                    if (peek() is Tokens.IntLiteral)
                    {
                        Tokens.IntLiteral inst = read() as Tokens.IntLiteral;

                        asm.Emit(new Call(Convert.ToInt16(inst.Value)));
                    }
                    else if (peek() is Tokens.Dot)
                    {
                        read();

                        Tokens.Statement label = read() as Tokens.Statement;

                        asm.Emit(new Call(label.Name));
                    }
                }
                else if (peek() is Tokens.Dot)
                {
                    read();
                    string name = (read() as Tokens.Statement).Name;
                    if (!(peek() is Tokens.Colon))
                    {
                        MessageBox.Show("Expected a colon somewhere in the program!");
                        Application.Exit();
                    }
                    read();

                    asm.addLabel(name);
                }
                else if (peek().ToString().ToLower() == "dumpreg")
                {
                    read();
                    asm.Emit(new DumpReg());
                }
                else if (peek().ToString().ToLower() == "inc")
                {
                    read();

                    asm.Emit(new IncReg(getReg(read())));
                }
                else if (peek().ToString().ToLower() == "add")
                {
                    read();

                    AsmRegister reg1 = getReg(read());

                    checkForComma();

                    if (peek() is Tokens.Statement)
                    {
                        AsmRegister reg2 = getReg(read());

                        asm.Emit(new AddReg(reg1, reg2));
                    }
                    else if (peek() is Tokens.IntLiteral)
                    {
                        asm.Emit(new AddReg(reg1, (short)(read() as Tokens.IntLiteral).Value));
                    }
                }
                else if (peek().ToString().ToLower() == "sub")
                {
                    read();

                    AsmRegister reg1 = getReg(read());

                    checkForComma();

                    if (peek() is Tokens.Statement)
                    {
                        AsmRegister reg2 = getReg(read());

                        asm.Emit(new SubReg(reg1, reg2));
                    }
                    else if (peek() is Tokens.IntLiteral)
                    {
                        asm.Emit(new SubReg(reg1, (short)(read() as Tokens.IntLiteral).Value));
                    }
                }
                else if (peek().ToString().ToLower() == "mul")
                {
                    read();

                    AsmRegister reg1 = getReg(read());

                    checkForComma();

                    if (peek() is Tokens.Statement)
                    {
                        AsmRegister reg2 = getReg(read());

                        asm.Emit(new MulReg(reg1, reg2));
                    }
                    else if (peek() is Tokens.IntLiteral)
                    {
                        asm.Emit(new MulReg(reg1, (short)(read() as Tokens.IntLiteral).Value));
                    }
                }
                else if (peek().ToString().ToLower() == "cmp")
                {
                    read();

                    AsmRegister reg1 = getReg(read());

                    checkForComma();

                    if (peek() is Tokens.Statement)
                    {
                        AsmRegister reg2 = getReg(read());

                        asm.Emit(new Compare(reg1, reg2));
                    }
                    else
                    {
                        Tokens.IntLiteral val = read() as Tokens.IntLiteral;

                        asm.Emit(new Compare(reg1, Convert.ToInt16(val.Value)));
                    }
                }
                else if (peek().ToString().ToLower() == "je")
                {
                    read();

                    if (!(peek() is Tokens.IntLiteral))
                    {
                        if (!(peek() is Tokens.Dot))
                        {
                            MessageBox.Show("Expected a label somewhere in the program!");
                            Application.Exit();
                        }
                        else if (!(peek() is Tokens.IntLiteral) && !(peek() is Tokens.Dot))
                        {
                            MessageBox.Show("Expected an address somewhere in the program!");
                            Application.Exit();
                        }
                    }

                    if (peek() is Tokens.IntLiteral)
                    {
                        Tokens.IntLiteral inst = read() as Tokens.IntLiteral;

                        asm.Emit(new JumpIfEqual(Convert.ToInt16(inst.Value)));
                    }
                    else if (peek() is Tokens.Dot)
                    {
                        read();

                        Tokens.Statement label = read() as Tokens.Statement;

                        asm.Emit(new JumpIfEqual(label.Name));
                    }
                }
                else if (peek().ToString().ToLower() == "jne")
                {
                    read();

                    if (!(peek() is Tokens.IntLiteral))
                    {
                        if (!(peek() is Tokens.Dot))
                        {
                            MessageBox.Show("Expected a label somewhere in the program!");
                            Application.Exit();
                        }
                        else if (!(peek() is Tokens.IntLiteral) && !(peek() is Tokens.Dot))
                        {
                            MessageBox.Show("Expected an address somewhere in the program!");
                            Application.Exit();
                        }
                    }

                    if (peek() is Tokens.IntLiteral)
                    {
                        Tokens.IntLiteral inst = read() as Tokens.IntLiteral;

                        asm.Emit(new JumpIfNotEqual(Convert.ToInt16(inst.Value)));
                    }
                    else if (peek() is Tokens.Dot)
                    {
                        read();

                        Tokens.Statement label = read() as Tokens.Statement;

                        asm.Emit(new JumpIfNotEqual(label.Name));
                    }
                }
                else if (peek() is Tokens.Colon)
                {
                    read();
                    string name = (read() as Tokens.Statement).Name;
                    asm.addLabel(name, asm.instruction);
                    if (peek().ToString().ToLower() == "db")
                    {
                        read();

                        string s = (read() as Tokens.StringLiteral).Value;

                        asm.addStringToFile(asm.instruction, s);

                        foreach (char c in s)
                            asm.instruction += sizeof(char);

                        if (peek() is Tokens.Comma)
                        {
                            read();
                            asm.instruction += sizeof(char);
                            read();
                        }
                    }
                    else if (peek().ToString().ToLower() == "mb")
                    {
                        read();

                        Tokens.IntLiteral size = read() as Tokens.IntLiteral;

                        asm.makeBuffer(asm.instruction, name);
                        asm.instruction += (short)size.Value;
                    }
                }
                else if (peek().ToString().ToLower() == "gls")
                {
                    read();

                    AsmRegister reg1 = getReg(read());

                    checkForComma();
                    checkForDot();

                    string label = (read() as Tokens.Statement).Name;

                    asm.Emit(new SetReg(reg1, label, false));
                }
                else if (peek().ToString().ToLower() == "gla")
                {
                    read();

                    AsmRegister reg1 = getReg(read());

                    checkForComma();
                    checkForDot();

                    string label = (read() as Tokens.Statement).Name;

                    asm.Emit(new SetReg(reg1, label, true));
                }
                else if (peek().ToString().ToLower() == "crx")
                {
                    read();

                    asm.Emit(new Crx(getReg(read())));
                }
                else if (peek().ToString().ToLower() == "cry")
                {
                    read();

                    asm.Emit(new Cry(getReg(read())));
                }
                else if (peek().ToString().ToLower() == "getkey")
                {
                    read();

                    asm.Emit(new ReadKey(getReg(read())));
                }
                else if (peek().ToString().ToLower() == "cls")
                {
                    read();
                    asm.Emit(new Cls());
                }
                else if (peek().ToString().ToLower() == "outw")
                {
                    read();
                    Tokens.IntLiteral intl = read() as Tokens.IntLiteral;
                    
                    checkForComma();

                    asm.Emit(new Outw((short)intl.Value, getReg(read())));
                }
                else if (peek().ToString().ToLower() == "push")
                {
                    read();
                    asm.Emit(new Push(getReg(read())));
                }
                else if (peek().ToString().ToLower() == "pop")
                {
                    read();
                    asm.Emit(new Pop(getReg(read())));
                }
                else
                {
                    MessageBox.Show("Unknown instruction " + peek().ToString() + "!");
                    Application.Exit();
                }
            }
            //asm.Refactor();
            //asm.Assemble();
        }

        private void checkForDot()
        {
            if (!(peek() is Tokens.Dot))
            {
                MessageBox.Show("Expected a label somewhere in the program!");
                Application.Exit();
            }

            read();
        }

        private void checkForComma()
        {
            if (!(peek() is Tokens.Comma))
            {
                MessageBox.Show("Expected comma somewhere in the program!");
                Application.Exit();
            }

            read();
        }

        public AsmRegister getReg(Token t)
        {
            string regName = (t as Tokens.Statement).Name.ToUpper();
            AsmRegister reg;

            if (!(Enum.TryParse(regName, out reg)))
            {
                MessageBox.Show("Unknown register " + regName + "!");
                Application.Exit();
            }

            return reg;
        }
    }
}
