using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronosVM_2
{
    public delegate void Recieve(short b);
    public delegate short GetData();

    public class PeripheralBase
    {
        private Dictionary<short, Recieve> listeners = new Dictionary<short, Recieve>();
        private Dictionary<short, GetData> responders = new Dictionary<short, GetData>();
        private VM vm;

        public PeripheralBase(VM vm)
        {
            this.vm = vm;
        }

        public void addBusListener(short bus, Recieve recieve)
        {
            listeners.Add(bus, recieve);
        }

        public void addBusResponder(short bus, GetData getData)
        {
            responders.Add(bus, getData);
        }

        public void outb(short bus, byte b)
        {
            listeners[bus](b);
        }

        public void outw(short bus, short data)
        {
            listeners[bus](data);
        }

        public byte inb(short bus)
        {
            return (byte)responders[bus]();
        }

        public short inw(short bus)
        {
            return responders[bus]();
        }
    }
}
