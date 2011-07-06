namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "BLEND_RLE")]
    public class BlendRLE
    {
        [XmlCookedAttribute(
            Name = "pRuns",
            DefaultValue = 0,
            ElementType = ElementType.TableArrayVariable,
            ChildType = typeof(BlendRun))]
        public BlendRun[] Runs;
    }
}