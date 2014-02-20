using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock.Language
{
    public class Expression : Node
    {
        public List<Node> value = new List<Node>();
    }

    public class Operator : Node
    {
        public char op;

        public Operator(char op)
        {
            this.op = op;
        }

        public override string ToString()
        {
            return op.ToString();
        }
    }
}
