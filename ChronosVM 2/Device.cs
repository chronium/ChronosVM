using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronosVM_2
{
    public abstract class Device
    {
        protected Ram ram;
        protected VM vm;

        public virtual void onTick() { }

        public void init(VM vm)
        {
            this.ram = vm.ram;
            this.vm = vm;
            init(vm.peripheralBase);
        }

        public virtual void init(PeripheralBase pBase) { }
    }
}
