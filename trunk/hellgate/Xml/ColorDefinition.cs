using System;
using Hellgate.Excel.SinglePlayer;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "COLOR_DEFINITION")]
    public class ColorDefinition
    {
        [XmlCookedAttribute(
            Name = "nId",
            DefaultValue = null,
            TableCode = Xls.TableCodes.COLORSETS, // 13360 COLORSETS
            ElementType = ElementType.ExcelIndex)]
        public ColorSets ColorSet;
        public int ColorSetRowIndex;

        [XmlCookedAttribute(
            Name = "nUnittype",
            DefaultValue = null,
            TableCode = Xls.TableCodes.UNITTYPES, // 21040 UNITTYPES
            ElementType = ElementType.ExcelIndex)]
        public UnitTypes UnitType;
        public int UnitTypeRowIndex;

        [XmlCookedAttribute(
            Name = "pdwColors",
            ElementType = ElementType.Int32ArrayFixed,
            DefaultValue = (UInt32)0x00000000,
            Count = 6,
            CustomType = ElementType.Unsigned)]
        public UInt32[] Colors;

        public ColorDefinition()
        {
            ColorSet = null;
            ColorSetRowIndex = -1;
            UnitType = null;
            UnitTypeRowIndex = -1;
            Colors = null;
        }
    }
}