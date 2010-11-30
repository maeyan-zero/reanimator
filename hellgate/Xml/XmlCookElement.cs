using System;

namespace Hellgate.Xml
{
    public enum ElementType : ushort
    {
        Int32 = 0x0000,
        Int32ArrayFixed = 0x0006,           // found in colorsets (pdwColors)
        Int32ArrayVariable = 0x0007,

        Float = 0x0100,
        FloatArrayFixed = 0x0106,
        FloatArrayVariable = 0x0107,        // found in AppearanceDefinition

        String = 0x0200,
        StringArrayFixed = 0x0206,          // found in AppearanceDefinition, like any other array type but as String
        StringArrayVariable = 0x0207,       // found in AppearanceDefinition

        Table = 0x0308,
        TableCount = 0x030A,
        Float_0x0500 = 0x0500,              // found in AppearanceDefinition and children; appears to be treated like normal float but name usually "tName"
        UnknownFloatT = 0x0600,
        NonCookedInt32 = 0x0700,
        UnknownFloat = 0x0800,

        ExcelIndex = 0x0903,
        ExcelIndexArrayFixed = 0x0905,      // found in AppearanceDefinition, like any other array type but as ExcelIndex

        Int32_0x0A00 = 0x0A00,              // found in AppearanceDefinition and children; appears to be treated like normal Int32
        Int32Array_0x0A06 = 0x0A06,         // found in AppearanceDefinition, like any other array type but as Int32_0x0A00; name usually "pnName"
        Flag = 0x0B01,
        BitFlag = 0x0C02,
        UnknownPTypeD_0x0D00 = 0x0D00,      // found in RoomPath and AppearanceDefinition and children; no known use/occurance - name usually "pName"
        ByteArray = 0x1007
    }

    public class XmlCookElement
    {
        public String Name;
        public String TrueName; // some element names have illegal xml chars in them (bad FSS), so we need the "true" name if we want the correct string hash
        public Object DefaultValue;
        public Type ChildType;
        public UInt32 ChildTypeHash;
        public UInt32 ExcelTableCode;
        public ElementType ElementType;
        public Int32 FlagId;
        public Int32 BitIndex;
        public Int32 BitCount; // used for BitIndex, total field BitCount
        public UInt32 BitMask;
        public Int32 Count;
        public UInt32 NameHash;
        public bool TreatAsData;
        public bool IsTCv4;
        public bool IsPresent;
    }
}