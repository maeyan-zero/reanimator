using System;

namespace Reanimator.XmlDefinitions
{
    public enum ElementType
    {
        Int32 = 1,
        UInt32,
        Float,
        String,
        ExcelIndex,
        Flag
    }

    public class XmlCookElement
    {
        public String Name;
        public Object DefaultValue;
        public Boolean IsCount;
        public Type ChildType;
        public UInt32 ExcelTableCode;
        public ElementType ElementType;
        public UInt32 FlagId;
    }
}