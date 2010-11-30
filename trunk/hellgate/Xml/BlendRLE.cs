namespace Hellgate.Xml
{
    class BlendRLE : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "pRuns",
                DefaultValue = 0,
                ElementType = ElementType.TableMultiple,
                ChildType = typeof(BlendRun)
            }
        };

        public BlendRLE()
        {
            RootElement = "BLEND_RLE";
            base.Elements.AddRange(Elements);
        }
    }
}