using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Reanimator
{
    public partial class ExcelFile
    {
        public const String FolderPath = @"\data_common\excel\";
        public const String FileExtention = "txt.cooked";

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct ExcelHeader
        {
            public Int32 StructureId;   // This is the id used to determine what structure to use to read the table block.
            public Int32 Unknown321;    // This is how the game reads this in...
            public Int32 Unknown322;    // What they do I don't know, lol.
            public Int16 Unknown161;
            public Int16 Unknown162;
            public Int16 Unknown163;
            public Int16 Unknown164;
            public Int16 Unknown165;
            public Int16 Unknown166;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TableHeader
        {
            public Int32 Unknown1;
            public Int32 Unknown2;
            public Int16 VersionMajor;
            public Int16 Reserved1;     // I think...
            public Int16 VersionMinor;
            public Int16 Reserved2;     // I think...
        }

        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public class ExcelOutputAttribute : Attribute
        {
            public bool IsStringOffset { get; set; }
            public bool IsStringIndex { get; set; }

            public bool IsIntOffset { get; set; }
            public Type IntOffsetType { get; set; }
            public int IntOffsetOrder { get; set; }
            public String[] FieldNames { get; set; }
            //public String[] FieldNames { get; set; }
            //public int DefaultIndex { get; set; }


            public bool IsStringId { get; set; }
            public bool IsTableIndex { get; set; }
            public String TableStringId { get; set; }
            public String Column { get; set; }
            public int SortId { get; set; }

            public bool IsBitmask { get; set; }
            public UInt32 DefaultBitmask { get; set; }

            public bool IsBool { get; set; }
        }

        public abstract class IntValueType
        {
            public bool IsGood { get; protected set; }

            protected IntValueType()
            {
                IsGood = false;
            }

            public abstract void ParseBlock(byte[] dataBlock, int offset);

            protected static T ReadBlock<T>(byte[] dataBlock, ref int offset)
            {
                return FileTools.ByteArrayTo<T>(dataBlock, ref offset);
            }
        }

        public abstract class IntValueTypeDef
        {
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public class SingleUInt32Def
            {
                public Int32 Header;
                public UInt32 Value;
                public Int32 Null;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public class SingleInt32Def
            {
                public Int32 Header;
                public Int32 Value;
                public Int32 Null;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public class PropDef
            {
                public Int32 Header;
                public Int32 Value1;
                public Int32 Value2;
                public Int32 Value3;
                public Int32 Null;
            }
        }

        public class SingleUInt32 : IntValueType
        {
            IntValueTypeDef.SingleUInt32Def Data { get; set; }

            public SingleUInt32()
            {
                Data = new IntValueTypeDef.SingleUInt32Def();
            }

            public override void ParseBlock(byte[] dataBlock, int offset)
            {
                Data = ReadBlock<IntValueTypeDef.SingleUInt32Def>(dataBlock, ref offset);
                if (Data.Null == 0) IsGood = true;
            }
        }

        public class SingleInt32 : IntValueType
        {
            IntValueTypeDef.SingleInt32Def Data { get; set; }

            public SingleInt32()
            {
                Data = new IntValueTypeDef.SingleInt32Def();
            }

            public override void ParseBlock(byte[] dataBlock, int offset)
            {
                Data = ReadBlock<IntValueTypeDef.SingleInt32Def>(dataBlock, ref offset);
                if (Data.Null == 0) IsGood = true;
            }
        }

        public class PropData : IntValueType
        {
            List<IntValueTypeDef.PropDef> Data { get; set; }

            public PropData()
            {
                Data = new List<IntValueTypeDef.PropDef>();
            }

            public override void ParseBlock(byte[] dataBlock, int offset)
            {
                IntValueTypeDef.PropDef propData = ReadBlock<IntValueTypeDef.PropDef>(dataBlock, ref offset);
                // todo: FIXME
                //Debug.Assert(propData.Header == 0x1A);

                while (propData.Null != 0)
                {
                    // todo: FIXME
                    //Debug.Assert(propData.Header == 0x1A);
                    Data.Add(propData);
                    offset -= sizeof(Int32);
                    propData = ReadBlock<IntValueTypeDef.PropDef>(dataBlock, ref offset);
                }

                IsGood = true;
            }
        }

        private abstract class FileTokens
        {
            public const Int32 StartOfBlock = 0x68657863;      // 'cxeh'
            public const Int32 TokenRcsh = 0x68736372;         // 'rcsh'
            public const Int32 TokenTysh = 0x68737974;         // 'tysh'
            public const Int32 TokenMysh = 0x6873796D;         // 'mysh'
            public const Int32 TokenDneh = 0x68656E64;         // 'dneh'
        }

        public abstract class ColumnTypeKeys
        {
            public const String IsStringOffset = "IsStringOffset";
            public const String IsStringIndex = "IsStringIndex";
            public const String IsStringId = "IsStringId";
            public const String IsRelationGenerated = "IsRelationGenerated";
            public const String IsTableIndex = "IsTableIndex";
            public const String IsBitmask = "IsBitmask";
            public const String IsBool = "IsBool";
            public const String IsIntOffset = "IsIntOffset";
            public const String IntOffsetType = "IntOffsetType";
            public const String IntOffsetOrder = "IntOffsetOrder";
            public const String SortId = "SortId";
        }
    }
}
