using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeBlendOpRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        [ExcelOutput(IsBool = true)]
        public Int32 replaceAllParts;//bool;
        [ExcelOutput(IsBool = true)]
        public Int32 noTextureChange;//bool;
        public Int32 removeParts1;
        public Int32 removeParts2;
        public Int32 removeParts3;
        public Int32 removeParts4;
        public Int32 removeParts5;
        public Int32 removeParts6;
        public Int32 removeParts7;
        public Int32 removeParts8;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] removePartsUnused;
        public Int32 addParts1;
        public Int32 addParts2;
        public Int32 addParts3;
        public Int32 addParts4;
        public Int32 addParts5;
        public Int32 addParts6;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] addPartsUnused;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string blend;
        short undefined1;
        Int32 undefined2;
        public Int32 target;
        public Int32 covers1;
        public Int32 covers2;
        public Int32 covers3;
        public Int32 covers4;
        public Int32 covers5;
        public Int32 covers6;
        public Int32 covers7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] coversUnused;
    }
}