using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRAINER___2023___ZELDA
{
    internal struct Offset
    {
        public long Address { get; }
        public int Size { get; }
        public MemoryType MemoryType { get; }

        public Offset(long address, int size, MemoryType memoryType)
        {
            Address = address;
            Size = size;
            MemoryType = memoryType;
        }
    }
}
