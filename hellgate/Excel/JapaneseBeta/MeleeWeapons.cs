using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MeleeWeaponsBeta
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string swingEvents;
        public Int32 undefined1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string particleFolder;
        public Int32 undefined2;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 tinyHit;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] undefined3;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 tinyDefault;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined3a;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 tinyPhysical;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined3b;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 tinyFire;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined3c;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 tinyElectric;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined3d;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 tinySpectral;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined3e;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 tinyToxic;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined3f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 softHit;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] undefined9;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 softDefault;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined9a;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 softPhysical;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined9b;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 softFire;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined9c;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 softElectric;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined9d;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 softSpectral;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined9e;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 softToxic;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined9f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mediumHit;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] undefined4;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mediumDefault;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined4a;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mediumPhysical;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined4b;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mediumFire;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined4c;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mediumElectric;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined4d;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mediumSpectral;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined4e;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mediumToxic;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined4f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 hardHit;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] undefined5;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 hardDefault;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined5a;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 hardPhysical;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined5b;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 hardFire;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined5c;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 hardElectric;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined5d;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 hardSpectral;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined5e;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 hardToxic;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined5f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 kill;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] undefined6;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 killDefault;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined6a;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 killPhysical;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined6b;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 killFire;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined6c;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 killElectric;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined6d;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 killSpectral;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined6e;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 killToxic;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined6f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 block;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] undefined7;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 blockDefault;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined7a;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 blockPhysical;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined7b;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 blockFire;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined7c;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 blockElectric;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined7d;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 blockSpectral;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined7e;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 blockToxic;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined7f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 fumble;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] undefined8;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 fumbleDefault;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined8a;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 fumblePhysical;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined8b;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 fumbleFire;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined8c;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 fumbleElectric;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined8d;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 fumbleSpectral;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined8e;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 fumbleToxic;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined8f;
    }
}
