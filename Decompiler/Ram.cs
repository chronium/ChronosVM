using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decompiler
{
    public class Ram
    {
        private byte[] ramArray;

        public Ram(long size)
        {
            ramArray = new byte[size];
            Array.Clear(ramArray, 0, ramArray.Length);
        }

        public byte readByte(long index)
        {
            if (index >= ramArray.Length)
            {
                Program.running = false;
                return 0;
            }
            else
                return ramArray[index];
        }

        public unsafe ushort readUShort(long index)
        {
            fixed (void* ptr = ramArray)
                return *((ushort*)((long)ptr + index));
        }

        public unsafe uint readUInt(long index)
        {
            fixed (void* ptr = ramArray)
                return *((uint*)((long)ptr + index));
        }

        public unsafe ulong readULong(long index)
        {
            fixed (void* ptr = ramArray)
                return *((ulong*)((long)ptr + index));
        }

        public unsafe short readShort(long index)
        {
            fixed (void* ptr = ramArray)
                return *((short*)((long)ptr + index));
        }

        public unsafe int readInt(long index)
        {
            fixed (void* ptr = ramArray)
                return *((int*)((long)ptr + index));
        }

        public unsafe long readLong(long index)
        {
            fixed (void* ptr = ramArray)
                return *((long*)((long)ptr + index));
        }

        public unsafe void writeByte(long index, byte value)
        {
            fixed (void* ptr = ramArray)
                *((byte*)((long)ptr + index)) = value;
        }

        public unsafe void writeUShort(long index, ushort value)
        {
            fixed (void* ptr = ramArray)
                *((ushort*)((long)ptr + index)) = value;
        }

        public unsafe void writeUInt(long index, uint value)
        {
            fixed (void* ptr = ramArray)
                *((uint*)((long)ptr + index)) = value;
        }

        public unsafe void writeULong(long index, ulong value)
        {
            fixed (void* ptr = ramArray)
                *((ulong*)((long)ptr + index)) = value;
        }

        public unsafe void writeShort(long index, short value)
        {
            fixed (void* ptr = ramArray)
                *((short*)((long)ptr + index)) = value;
        }

        public unsafe void writeInt(long index, int value)
        {
            fixed (void* ptr = ramArray)
                *((int*)((long)ptr + index)) = value;
        }

        public unsafe void writeLong(long index, long value)
        {
            fixed (void* ptr = ramArray)
                *((long*)((long)ptr + index)) = value;
        }

        public byte[] getRAM()
        {
            return ramArray;
        }

        public void setRAM(byte[] ram)
        {
            ramArray = ram;
        }
    }
}
