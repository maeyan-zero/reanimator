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
                ElementType = ElementType.TableCount,
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