using System;
using Hellgate.Excel.JapaneseBeta;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "DEMO_LEVEL_DEFINITION")]
    public class DemoLevelDefinition
    {
        [XmlCookedAttribute(
            Name = "dwFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.Int32,
            CustomType = ElementType.Unsigned)]
        public UInt32 Flags;

        [XmlCookedAttribute(
            Name = "nLevelDefinition",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.LEVEL)] // 29233 LEVEL
        public LevelRow LevelDefinition;
        public Int32 LevelDefinitionRowIndex;

        [XmlCookedAttribute(
            Name = "nDRLGDefinition",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.LEVEL_DRLGS)] // 21553 LEVEL_DRLGS
        public LevelDrlgsRow DRLGDefinition;
        public Int32 DRLGDefinitionRowIndex;

        [XmlCookedAttribute(
            Name = "dwDRLGSeed",
            DefaultValue = (UInt32)1,
            ElementType = ElementType.Int32,
            CustomType = ElementType.Unsigned)]
        public UInt32 DRLGSeed;

        [XmlCookedAttribute(
            Name = "nCameraType",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 CameraType;

        [XmlCookedAttribute(
            Name = "fMetersPerSecond",
            DefaultValue = 5.0f,
            ElementType = ElementType.Float)]
        public float MetersPerSecond;
    }
}