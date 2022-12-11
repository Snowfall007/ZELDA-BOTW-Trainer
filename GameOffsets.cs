using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRAINER___2023___ZELDA
{
    internal class GameOffsets
    {
        /**
        * In quarters. 3 Heart = 12. Max = 80
        */
        //public static readonly Offset MAX_HEALTH = new Offset(Program.Globals.baseAdress+0x430216FA, 2, MemoryType.BigEndian);
        public static readonly Offset HEALTH = new Offset(Program.Globals.baseAdress + 0x430216FA, 2, MemoryType.BigEndian);
        //public static readonly Offset HEALTH = new Offset(0x276FFF2CE90, 2, MemoryType.BigEndian);

        //Rupees
        public static readonly Offset RUPEES = new Offset(Program.Globals.baseAdress + 0x3F8CB48A, 2, MemoryType.BigEndian);

        //Speed
        public static readonly Offset Speed = new Offset(Program.Globals.baseAdress + 0x430F6544, 2, MemoryType.BigEndian);

        //Position - coordinates
        public static readonly Offset Xposition = new Offset(Program.Globals.baseAdress + 0x113445C0, 2, MemoryType.BigEndian);
        public static readonly Offset Yposition = new Offset(Program.Globals.baseAdress + 0x113445C4, 2, MemoryType.BigEndian);
        public static readonly Offset Zposition = new Offset(Program.Globals.baseAdress + 0x113445C8, 2, MemoryType.BigEndian);

        /**
         * 0 for None
         * 16 for the base bar
         * 32 for max
         */
        public static readonly Offset STAMINA = new Offset(Program.Globals.baseAdress + 0x41C132A8, 2, MemoryType.BigEndian);

        /**
         * ID 1 and ID 2 is used for the Swift (fast) sail and the Wind Waker (music instrument)
         */
        //public static readonly Offset INVENTORY_ITEM_ID_0 = new Offset(0x145B7BBC, 1, MemoryType.BigEndian);
        //public static readonly Offset INVENTORY_ITEM_ID_SWIFT_SAIL = new Offset(0x145B7BBD, 1, MemoryType.BigEndian); // ITEM ID = 119
        //public static readonly Offset INVENTORY_ITEM_ID_WIND_WAKER = new Offset(0x145B7BBE, 1, MemoryType.BigEndian); // ITEM_ID = 34
        //public static readonly Offset INVENTORY_ITEM_ID_3 = new Offset(0x145B7BBF, 1, MemoryType.BigEndian);
    }
}