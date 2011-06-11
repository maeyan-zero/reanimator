namespace Hellgate.Xml
{
    class XmlBlendRLE : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "pRuns",
                DefaultValue = 0,
                ElementType = ElementType.TableArrayVariable,
                ChildType = typeof(XmlBlendRun)
            }
        };

        public XmlBlendRLE()
        {
            RootElement = "BLEND_RLE";
            base.Elements.AddRange(Elements);
        }
    }
}