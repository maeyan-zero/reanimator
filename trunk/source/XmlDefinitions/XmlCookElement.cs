using System;

namespace Reanimator.XmlDefinitions
{
    public enum ElementType : ushort 
    {
        Int32 = 0x0000,
        Float = 0x0100,
        String = 0x0200,
        UnknownFloatT = 0x0600,
        NonCookedInt32 = 0x0700,
        UnknownFloat = 0x0800,
        UnknownPTypeD = 0x0D00,
        Flag = 0x0B01,
        BitFlag = 0x0C02,
        Table = 0x0308,
        ExcelIndex = 0x0903,
        FloatArray = 0x0106,
        UnknownPType = 0x0007,
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
        public Int32 FlagId;
        public Int32 BitIndex;
        public UInt32 BitMask;
        public Int32 ArrayCount;
        public UInt32 NameHash;
    }
}