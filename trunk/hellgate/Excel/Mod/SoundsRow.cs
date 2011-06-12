using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SoundsRow
    {
        RowHeader header;
        [ExcelOutput(IsStringOffset = true, SortColumnOrder = 1)]
        public Int32 name; //pchar
        Int32 undefined01;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 extension; //pchar
        Int32 undefined02;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 lqExtension; //pchar
        Int32 undefined03a;
        Int32 undefined03b;
        public Int32 pickType;//internalIndexArray count-1 type-4
        [ExcelOutput(IsTableIndex = true, TableStringId = "LANGUAGE")]
        public Int32 language;//internalIndexArray count-1 type-7
        public Int32 volume;
        public float undefinedFloat02;
        public float minRange;
        public float maxRange;
        public Int32 rollOffType;//internalIndexArray count-1 type-5
        public Int32 reverbSend;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask01 bitmask01;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 directory;//pchar
        Int32 undefined5;
        public Int32 fileName1a;//indexMultipleRelations
		public Int32 fileName1b;
		public Int32 fileName1c;
		public Int32 fileName1d;
		public Int32 fileName1e;
		public Int32 fileName1f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_1;
        Int32 undefined1g;
        public float weight1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined1h;
        public Int32 fileName2a;//indexMultipleRelations
		public Int32 fileName2b;
		public Int32 fileName2c;
		public Int32 fileName2d;
		public Int32 fileName2e;
		public Int32 fileName2f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_2;
        Int32 undefined2g;
        public float weight2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined2h;
        public Int32 fileName3a;//indexMultipleRelations
		public Int32 fileName3b;
		public Int32 fileName3c;
		public Int32 fileName3d;
		public Int32 fileName3e;
		public Int32 fileName3f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_3;
        Int32 undefined3g;
        public float weight3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined3h;
        public Int32 fileName4a;//indexMultipleRelations
		public Int32 fileName4b;
		public Int32 fileName4c;
		public Int32 fileName4d;
		public Int32 fileName4e;
		public Int32 fileName4f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_4;
        Int32 undefined4g;
        public float weight4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined4h;
        public Int32 fileName5a;//indexMultipleRelations
		public Int32 fileName5b;
		public Int32 fileName5c;
		public Int32 fileName5d;
		public Int32 fileName5e;
		public Int32 fileName5f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_5;
        Int32 undefined5g;
        public float weight5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined5h;
        public Int32 fileName6a;//indexMultipleRelations
		public Int32 fileName6b;
		public Int32 fileName6c;
		public Int32 fileName6d;
		public Int32 fileName6e;
		public Int32 fileName6f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_6;
        Int32 undefined6g;
        public float weight6;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined6h;
        public Int32 fileName7a;//indexMultipleRelations
		public Int32 fileName7b;
		public Int32 fileName7c;
		public Int32 fileName7d;
		public Int32 fileName7e;
		public Int32 fileName7f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_7;
        Int32 undefined7g;
        public float weight7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined7h;
        public Int32 fileName8a;//indexMultipleRelations
		public Int32 fileName8b;
		public Int32 fileName8c;
		public Int32 fileName8d;
		public Int32 fileName8e;
		public Int32 fileName8f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_8;
        Int32 undefined8g;
        public float weight8;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined8h;
        public Int32 fileName9a;//indexMultipleRelations
		public Int32 fileName9b;
		public Int32 fileName9c;
		public Int32 fileName9d;
		public Int32 fileName9e;
		public Int32 fileName9f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_9;
        Int32 undefined9g;
        public float weight9;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined9h;
        public Int32 fileName10a;//indexMultipleRelations
		public Int32 fileName10b;
		public Int32 fileName10c;
		public Int32 fileName10d;
		public Int32 fileName10e;
		public Int32 fileName10f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_10;
        Int32 undefined10g;
        public float weight10;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined10h;
        public Int32 fileName11a;//indexMultipleRelations
		public Int32 fileName11b;
		public Int32 fileName11c;
		public Int32 fileName11d;
		public Int32 fileName11e;
		public Int32 fileName11f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_11;
        Int32 undefined11g;
        public float weight11;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined11h;
        public Int32 fileName12a;//indexMultipleRelations
		public Int32 fileName12b;
		public Int32 fileName12c;
		public Int32 fileName12d;
		public Int32 fileName12e;
		public Int32 fileName12f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_12;
        Int32 undefined12g;
        public float weight12;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined12h;
        public Int32 fileName13a;//indexMultipleRelations
		public Int32 fileName13b;
		public Int32 fileName13c;
		public Int32 fileName13d;
		public Int32 fileName13e;
		public Int32 fileName13f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_13;
        Int32 undefined13g;
        public float weight13;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined13h;
        public Int32 fileName14a;//indexMultipleRelations
		public Int32 fileName14b;
		public Int32 fileName14c;
		public Int32 fileName14d;
		public Int32 fileName14e;
		public Int32 fileName14f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_14;
        Int32 undefined14g;
        public float weight14;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined14h;
        public Int32 fileName15a;//indexMultipleRelations
		public Int32 fileName15b;
		public Int32 fileName15c;
		public Int32 fileName15d;
		public Int32 fileName15e;
		public Int32 fileName15f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_15;
        Int32 undefined15g;
        public float weight15;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined15h;
        public Int32 fileName16a;//indexMultipleRelations
		public Int32 fileName16b;
		public Int32 fileName16c;
		public Int32 fileName16d;
		public Int32 fileName16e;
		public Int32 fileName16f;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 byte_943961_16;
        Int32 undefined16g;
        public float weight16;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined16h;
        public Int32 freqVar;
        Int32 undefined34;
        public Int32 volVar;
        public Int32 undefined123;//needs labeling
        public Int32 undefined1234;//needs labeling
        public Int32 maxWithInRad;
        public float radius;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 hardwarePriority;
        public Int32 gamePriority;
        public float fadeOutTime;
        public Int32 undefined40;
        public float fadeInTime;
        public Int32 undefined41;
        public float frontSend;
        public float centerSend;
        public float rearSend;
        public float sideSend;
        public Int32 lfeSend;//lfe ?
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUND_MIXSTATES")]
        public Int32 mixState;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDBUSES")]
        public Int32 bus;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDVCAS")]
        public Int32 vcas1;//list
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDVCAS")]
        public Int32 vcas2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDVCAS")]
        public Int32 vcas3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDVCAS")]
        public Int32 vcas4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDVCAS")]
        public Int32 vcas5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDVCAS")]
        public Int32 vcas6;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 vcas7;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 vcas8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSIC_REF")]
        public Int32 musicRef;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined44;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICSTINGERS")]
        public Int32 stingerRef;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined45;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 effects;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        Int32[] undefined46;

        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            nonblock = (1 << 0),
            stream = (1 << 1),
            is3D = (1 << 2),//it is in fact called just 3D, but that isn't liked.
            unk01 = (1 << 3),
            loops = (1 << 4),
            canCutoff = (1 << 5),
            highlander = (1 << 6),
            groupHighlander = (1 << 7),
            software = (1 << 8),
            headRelative = (1 << 9),
            isMusic = (1 << 10),
            dontRandomizeStart = (1 << 11),
            dontCrossfadeVariations = (1 << 12),
            unk02 = (1 << 13),
            loadAtStartup = (1 << 14)
        }
    }
}