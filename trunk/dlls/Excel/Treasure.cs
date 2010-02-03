using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    class Treasure: ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class TreasureTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string treasureClass;
            public Int32 allowUnitTypes1;
            public Int32 allowUnitTypes2;
            public Int32 allowUnitTypes3;
            public Int32 allowUnitTypes4;
            public Int32 allowUnitTypes5;
            public Int32 allowUnitTypes6;
            public Int32 globalThemeRequired;
            public Int32 unknown02;
            public Int32 pickTypes;
            public Int32 picks;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            //public byte[] unknown03;
            public Int32 undefined01;
            public Int32 undefined02;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            public byte[] unknown03;
            public Int32 undefined03;//this needed to be changed so that the mutant dyes could drop randomly.
            public Int32 undefined04;
            public Int32 undefined05;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            public byte[] unknown04;
            public float noDrop;
            public Int32 levelBoost;//pointer
            public float moneyChanceMultiplier;
            public float moneyLuckChanceMultiplier;
            public float moneyAmountMultiplier;
            public Int32 item1m;/*this isn't actually defined, but it determines the type of object in item1, whether it's a specific item(01), a unit type(02),
            another treasure class(03), an item quality(04), or something else yet to be determined.*/
            public Int32 item1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]   // this is equal to 19 Int32
            public byte[] unknown05;
            public Int32 value1;
            public Int32 item2m;
            public Int32 item2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown06;
            public Int32 value2;
            public Int32 item3m;
            public Int32 item3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown07;
            public Int32 value3;
            public Int32 item4m;
            public Int32 item4;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown08;
            public Int32 value4;
            public Int32 item5m;
            public Int32 item5;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown09;
            public Int32 value5;
            public Int32 item6m;
            public Int32 item6;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown10;
            public Int32 value6;
            public Int32 item7m;
            public Int32 item7;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown11;
            public Int32 value7;
            public Int32 item8m;
            public Int32 item8;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown12;
            public Int32 value8;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8804)]
            public byte[] unknown13;
            public Int32 spawnCondition;/** bitmask: 'create for all players in level',0 - 'required usable by operator',1 
            'required usable by spawner',2 - 'subscriber only',3 - 'max slots',4 - 'results not required',5
            'stack treasure',6 - 'multiplayer only',7 - 'single player only',8 */
            public Int32 spawnFromMonsterUnitType;
            public Int32 spawnFromLevelTheme;
        }

        public Treasure(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<TreasureTable>(data, ref offset, Count);
        }
    }
}
