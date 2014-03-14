using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock.Language.ControlFlow
{
    public class Return : Node
    {
        public Expression ret;

        public Return(Expression ret)
        {
            this.ret = ret;
        }
    }
}
