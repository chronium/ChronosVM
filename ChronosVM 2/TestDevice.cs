using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronosVM_2
{
    class TestDevice : Device
    {
        public short whatToReturn = 0;

        public override void init(PeripheralBase pBase)
        {
            pBase.addBusResponder(0x00, test);
            pBase.addBusListener(0x01, setWhatToReturn);
        }

        public short test()
        {
            return whatToReturn;
        }

        public void setWhatToReturn(short what)
        {
            this.whatToReturn = what;
        }
    }
}
