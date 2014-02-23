using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock.Language.ControlFlow
{
    class FunctionCall : Node
    {
        public string target;
        public List<Expression> args = new List<Expression>();

        public FunctionCall(string target, List<Expression> args)
        {
            this.target = target;
            this.args = args;
        }
    }
}
