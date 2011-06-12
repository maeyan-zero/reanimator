using System;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "SKILL_EVENTS_DEFINITION")]
    public class SkillEventsDefinition
    {
        [XmlCookedAttribute(
            Name = "szPreviewAppearance",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String PreviewAppearance;

        [XmlCookedAttribute(
            Name = "nPreviewAppearance",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public int PreviewAppearanceCount;

        [XmlCookedAttribute(
            Name = "pEventHolders",
            DefaultValue = 0,
            ChildType = typeof(SkillEventHolder),
            ElementType = ElementType.TableArrayVariable)]
        public SkillEventHolder[] EventHolders;
    }
}