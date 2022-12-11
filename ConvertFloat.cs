using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TRAINER___2023___ZELDA
{
    class ConvertFloat
    {

  
[StructLayout(LayoutKind.Explicit)]
struct UnionInt
        {
            [FieldOffset(0)] public byte byte0;
            [FieldOffset(1)] public byte byte1;
            [FieldOffset(2)] public byte byte2;
            [FieldOffset(3)] public byte byte3;

            [FieldOffset(0)] public float AsSingle;
            [FieldOffset(0)] public int AsInt32;

            public void FromBigEndian(byte[] bigEndianData)
            {
                this.byte0 = bigEndianData[3];
                this.byte1 = bigEndianData[2];
                this.byte2 = bigEndianData[1];
                this.byte3 = bigEndianData[0];
            }
        }




    }
}
