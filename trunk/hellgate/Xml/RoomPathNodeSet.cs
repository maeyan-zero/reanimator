namespace Hellgate.Xml
{
    class RoomPathNodeSet : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "pszIndex",
                ElementType = ElementType.String,
                DefaultValue = null
            },
            new XmlCookElement
            {
                Name = "pPathNodes",
                ElementType = ElementType.TableCount,
                DefaultValue = null,
                ChildType = typeof (RoomPathNode)
            },
            new XmlCookElement
            {
                Name = "pHappyNodes",
                ElementType = ElementType.UnknownPType,
                DefaultValue = 0
            },
            new XmlCookElement
            {
                Name = "dwFlags",
                ElementType = ElementType.Int32,
                DefaultValue = 0
            },
            new XmlCookElement
            {
                Name = "nEdgeNodeCount",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0
            },
            new XmlCookElement
            {
                Name = "pEdgeNodes",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0 // NULL
            },
            new XmlCookElement
            {
                Name = "fMinX",
                ElementType = ElementType.UnknownFloat,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fMaxX",
                ElementType = ElementType.UnknownFloat,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fMinY",
                ElementType = ElementType.UnknownFloat,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fMaxY",
                ElementType = ElementType.UnknownFloat,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "nArraySize",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0
            },
            new XmlCookElement
            {
                Name = "pNodeHashArray",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0 // FALSE
            },
            new XmlCookElement
            {
                Name = "nHashLengths",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0
            }
        };

        public RoomPathNodeSet()
        {
            RootElement = "ROOM_PATH_NODE_SET";
            base.Elements.AddRange(Elements);
        }
    }
}
