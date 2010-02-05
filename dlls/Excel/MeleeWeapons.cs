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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string name;
            public Int32 undefined1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string name;
            public Int32 undefined2;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyHit;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined3;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyDefault;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined3a;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined3b;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined3c;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined3d;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 tinySpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined3e;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined3f;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyPoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            Int32[] undefined3g;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 softHit;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined9;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 softDefault;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined9a;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 softPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined9b;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 softFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined9c;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 softElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined9d;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 softSpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined9e;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 softToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined9f;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 softPoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            Int32[] undefined9g;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 unknown1;//should be mediumHit
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined4;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 unknown2;//should be mediumDefault
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined4a;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 mediumPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined4b;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 mediumFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined4c;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 mediumElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined4d;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 mediumSpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined4e;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 unknown3;//but should be mediumToxic
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined4f;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 unknown4;//going by the other offsets in order this should be mediumPoison
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            Int32[] undefined4g;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 hardHit;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined5;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 hardDefault;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined5a;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 hardPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined5b;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 hardFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined5c;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 hardElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined5d;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 hardSpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined5e;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 hardToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined5f;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 hardPoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            Int32[] undefined5g;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 kill;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined6;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 killDefault;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined6a;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 killPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined6b;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 killFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined6c;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 killElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined6d;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 killSpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined6e;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 killToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined6f;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 killPoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            Int32[] undefined6g;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 block;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined7;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 blockDefault;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined7a;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 blockPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined7b;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 blockFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined7c;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 blockElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined7d;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 blockSpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined7e;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 blockToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined7f;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 blockPoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            Int32[] undefined7g;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 fumble;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined8;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 fumbleDefault;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined8a;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 fumblePhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined8b;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 fumbleFire;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined8c;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 fumbleElectric;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined8d;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 fumbleSpectral;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined8e;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 fumbleToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            Int32[] undefined8f;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
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
