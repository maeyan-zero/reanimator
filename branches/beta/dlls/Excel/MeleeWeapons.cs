using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class MeleeWeapons : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MeleeWeaponsTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string name2;
            public Int32 undefined1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string name3;
            public Int32 undefined2;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyHit;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined3;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyDefault;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined3a;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined3b;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined3c;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined3d;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 tinySpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined3e;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined3f;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyPoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            Int32[] undefined3g;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 softHit;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined9;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 softDefault;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined9a;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 softPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined9b;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 softFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined9c;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 softElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined9d;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 softSpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined9e;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 softToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined9f;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 softPoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            Int32[] undefined9g;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 unknown1;//should be mediumHit
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined4;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 unknown2;//should be mediumDefault
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined4a;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 mediumPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined4b;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 mediumFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined4c;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 mediumElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined4d;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 mediumSpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined4e;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 unknown3;//but should be mediumToxic
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined4f;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 unknown4;//going by the other offsets in order this should be mediumPoison
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            Int32[] undefined4g;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 hardHit;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined5;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 hardDefault;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined5a;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 hardPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined5b;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 hardFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined5c;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 hardElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined5d;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 hardSpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined5e;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 hardToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined5f;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 hardPoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            Int32[] undefined5g;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 kill;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined6;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 killDefault;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined6a;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 killPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined6b;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 killFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined6c;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 killElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined6d;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 killSpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined6e;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 killToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined6f;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 killPoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            Int32[] undefined6g;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 block;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined7;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 blockDefault;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined7a;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 blockPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined7b;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 blockFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined7c;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 blockElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined7d;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 blockSpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined7e;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 blockToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined7f;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 blockPoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            Int32[] undefined7g;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 fumble;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined8;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 fumbleDefault;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined8a;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 fumblePhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined8b;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 fumbleFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined8c;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 fumbleElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined8d;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 fumbleSpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined8e;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 fumbleToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined8f;
            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 fumblePoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            Int32[] undefined8g;

        }

        public MeleeWeapons(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MeleeWeaponsTable>(data, ref offset, Count);
        }
    }
}
