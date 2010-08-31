using System;

namespace Reanimator.XmlDefinitions
{
    public enum ElementType
    {
        Int32 = 0x0000,
        Float = 0x0100,
        String = 0x0200,
        NonCookedInt32 = 0x0700,
        Flag = 0x0B01,
        BitFlag = 0x0C02,
        Table = 0x0803,
        ExcelIndex = 0x0903,
        FloatArray = 0x0106,
        TableCount = 0x030A
    }

    public class XmlCookElement
    {
        public String Name;
        public String TrueName; // some element names have illegal xml chars in them (bad FSS), so we need the "true" name if we want the correct string hash
        public Object DefaultValue;
        public Type ChildType;
        public UInt32 ExcelTableCode;
        public ElementType ElementType;
        public UInt32 FlagId;
        public Int32 ArrayCount;
    }
}