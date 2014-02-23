using SharpRock.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock.ParserStuff
{
    public partial class Parser
    {
        public Block parseBlock()
        {
            Block block = new Block();

            if (!(peek() is Tokens.openCurlyBracket))
                block.body.Add(parseLine());
            else
            {
                read();
                while (!(peek() is Tokens.closeCurlyBracket))
                    block.body.Add(parseLine());
                read();
            }

            return block;
        }
    }
}
