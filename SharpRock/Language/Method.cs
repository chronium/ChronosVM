using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock.Language
{
    public class Method : Node
    {
        public string Name;
        public string ReturnType;
        public Block block;
        public List<Declaration> args = new List<Declaration>();

        public Method(string name, string type, Block block, List<Declaration> args)
        {
            this.Name = name;
            this.ReturnType = type;
            this.block = block;
            this.args = args;
        }
    }
}
