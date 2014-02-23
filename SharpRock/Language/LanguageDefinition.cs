using SharpRock.ParserStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock.Language
{
    public class Node
    {
    }

    public class ShortLiteral : Node
    {
        public short value;

        public ShortLiteral(short val)
        {
            this.value = val;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }

    public class VarPlaceholder : Node
    {
        public string name;

        public VarPlaceholder(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }

    public class Declaration : Node
    {
        public string name;
        public VariableTypes type;
        public Expression e;

        public Declaration(string name, VariableTypes type)
        {
            this.name = name;
            this.type = type;
        }

        public Declaration(string name, VariableTypes type, Expression e)
        {
            this.name = name;
            this.type = type;
            this.e = e;
        }

        public override string ToString()
        {
            return "Defining " + name + " as a " + (type == VariableTypes.bytev ? "byte" : "word");
        }
    }

    public class Assignment : Node
    {
        public string name;
        public Expression value;

        public Assignment(string name, Expression exp)
        {
            this.name = name;
            this.value = exp;
        }

        public override string ToString()
        {
            string expression = "";
            foreach (Node no in value.value)
            {
                expression += no.ToString() + " ";
            }
            expression = expression.Trim();
            return "Setting " + name + " to [" + expression + "]";
        }
    }

    public class ILAssembly : Node
    {
        public List<Node> IL;

        public ILAssembly(List<Node> IL)
        {
            this.IL = IL;
        }
    }

    public class Block : Node
    {
        public List<Node> body = new List<Node>();
    }
}
