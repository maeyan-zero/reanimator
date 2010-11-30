namespace Hellgate.Xml
{
    class RoomPathNode : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                    Name = "nIndex",
                    ElementType = ElementType.Int32,
                    DefaultValue = 0
            },
            new XmlCookElement
            {
                    Name = "nEdgeIndex",
                    ElementType = ElementType.NonCookedInt32,
                    DefaultValue = 0 // INVALID_ID
            },
            new XmlCookElement
            {
                    Name = "vPosition",
                    ElementType = ElementType.FloatArrayFixed,
                    DefaultValue = 0.0f,
                    Count = 3
            },
            new XmlCookElement
            {
                    Name = "vNormal",
                    ElementType = ElementType.FloatArrayFixed,
                    DefaultValue = 0.0f,
                    Count = 3
            },
            new XmlCookElement
            {
                    Name = "fHeight",
                    ElementType = ElementType.Float,
                    DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                    Name = "fRadius",
                    ElementType = ElementType.Float,
                    DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                    Name = "dwFlags",
                    ElementType = ElementType.Int32,
                    DefaultValue = 0
            },
            new XmlCookElement
            {
                    Name = "pConnections",
                    ElementType = ElementType.TableCount,
                    DefaultValue = null,
                    ChildType = typeof (RoomPathNodeConnection)
            },
            new XmlCookElement
            {
                    Name = "pLongConnections",
                    ElementType = ElementType.TableCount,
                    DefaultValue = null,
                    ChildType = typeof (RoomPathNodeConnectionRef)
            },
            new XmlCookElement
            {
                    Name = "pShortConnections",
                    ElementType = ElementType.TableCount,
                    DefaultValue = null,
                    ChildType = typeof (RoomPathNodeConnectionRef)
            },
        };

        public RoomPathNode()
        {
            RootElement = "ROOM_PATH_NODE";
            base.Elements.AddRange(Elements);
        }
    }
}
