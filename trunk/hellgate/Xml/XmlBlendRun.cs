namespace Hellgate.Xml
{
    internal class XmlBlendRun : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "nTotalAlpha",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nBlockStart",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nBlockRun",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "pbAlpha",
                DefaultValue = 0,
                ElementType = ElementType.ByteArrayVariable
            }
        };

        public XmlBlendRun()
        {
            RootElement = "BLEND_RUN";
            base.Elements.AddRange(Elements);
        }
    }
}