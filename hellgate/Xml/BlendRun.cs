namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "BLEND_RUN")]
    public class BlendRun
    {
        [XmlCookedAttribute(
            Name = "nTotalAlpha",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int TotalAlpha;

        [XmlCookedAttribute(
            Name = "nBlockStart",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int BlockStart;

        [XmlCookedAttribute(
            Name = "nBlockRun",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int BlockRun;

        [XmlCookedAttribute(
            Name = "pbAlpha",
            DefaultValue = 0,
            ElementType = ElementType.ByteArrayVariable)]
        public byte[] Alpha;
    }
}