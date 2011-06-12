using Hellgate.Excel.JapaneseBeta;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "SKILL_EVENT_HOLDER")]
    public class SkillEventHolder
    {
        [XmlCookedAttribute(
            Name = "nUnitMode",
            DefaultValue = null,
            TableCode = Xls.TableCodes.UNITMODES, // 0x00006630 (26160) UNITMODES
            ElementType = ElementType.ExcelIndex)]
        public UnitModesRow UnitMode;
        public int UnitModeRowIndex;

        [XmlCookedAttribute(
            Name = "fDuration",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Duration;

        [XmlCookedAttribute(
            Name = "pEvents",
            DefaultValue = 0,
            ChildType = typeof(SkillEvent),
            ElementType = ElementType.TableArrayVariable)]
        public SkillEvent[] Events;
    }
}