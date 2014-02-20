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

    public class Definition : Node
    {
        public string name;
        public VariableTypes type;

        public Definition(string name, VariableTypes type)
        {
            this.name = name;
            this.type = type;
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
}
