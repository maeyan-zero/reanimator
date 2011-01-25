using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AffixPickBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string affixPick;
		public Int32 affix1Chance;
		public Int32 affix1SetNumRequirement;
		public Int32 affix1Type1Weight;
		public Int32 affix1Type2Weight;
		public Int32 affix1Type3Weight;
		public Int32 affix1Type4Weight;
		public Int32 affix1Type5Weight;
		public Int32 affix1Type6Weight;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix1Type1;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix1Type2;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix1Type3;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix1Type4;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix1Type5;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
		public Int32 affix1Type6;//(33)
		public Int32 affix2Chance;
		public Int32 affix2SetNumRequirement;
		public Int32 affix2Type1Weight;
		public Int32 affix2Type2Weight;
		public Int32 affix2Type3Weight;
		public Int32 affix2Type4Weight;
		public Int32 affix2Type5Weight;
        public Int32 affix2Type6Weight;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix2Type1;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix2Type2;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix2Type3;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix2Type4;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix2Type5;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
		public Int32 affix2Type6;//(33)
		public Int32 affix3Chance;
		public Int32 affix3SetNumRequirement;
		public Int32 affix3Type1Weight;
		public Int32 affix3Type2Weight;
		public Int32 affix3Type3Weight;
		public Int32 affix3Type4Weight;
		public Int32 affix3Type5Weight;
        public Int32 affix3Type6Weight;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix3Type1;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix3Type2;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix3Type3;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix3Type4;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix3Type5;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
		public Int32 affix3Type6;//(33)
		public Int32 affix4Chance;
		public Int32 affix4SetNumRequirement;
		public Int32 affix4Type1Weight;
		public Int32 affix4Type2Weight;
		public Int32 affix4Type3Weight;
		public Int32 affix4Type4Weight;
		public Int32 affix4Type5Weight;
        public Int32 affix4Type6Weight;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix4Type1;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix4Type2;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix4Type3;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix4Type4;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix4Type5;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
		public Int32 affix4Type6;//(33)
		public Int32 affix5Chance;
		public Int32 affix5SetNumRequirement;
		public Int32 affix5Type1Weight;
		public Int32 affix5Type2Weight;
		public Int32 affix5Type3Weight;
		public Int32 affix5Type4Weight;
		public Int32 affix5Type5Weight;
        public Int32 affix5Type6Weight;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix5Type1;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix5Type2;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix5Type3;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix5Type4;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix5Type5;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
		public Int32 affix5Type6;//(33)
		public Int32 affix6Chance;
		public Int32 affix6SetNumRequirement;
		public Int32 affix6Type1Weight;
		public Int32 affix6Type2Weight;
		public Int32 affix6Type3Weight;
		public Int32 affix6Type4Weight;
		public Int32 affix6Type5Weight;
        public Int32 affix6Type6Weight;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix6Type1;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix6Type2;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix6Type3;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix6Type4;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix6Type5;//(33)
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
		public Int32 affix6Type6;//(33)

        
    }
}
