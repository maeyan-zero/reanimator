namespace Hellgate.Xml
{
    class XmlRoomPathNode : XmlDefinition
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
                    ElementType = ElementType.TableArrayVariable,
                    DefaultValue = null,
                    ChildType = typeof (XmlRoomPathNodeConnection)
            },
            new XmlCookElement
            {
                    Name = "pLongConnections",
                    ElementType = ElementType.TableArrayVariable,
                    DefaultValue = null,
                    ChildType = typeof (XmlRoomPathNodeConnectionRef)
            },
            new XmlCookElement
            {
                    Name = "pShortConnections",
                    ElementType = ElementType.TableArrayVariable,
                    DefaultValue = null,
                    ChildType = typeof (XmlRoomPathNodeConnectionRef)
            }
        };

        public XmlRoomPathNode()
        {
            RootElement = "ROOM_PATH_NODE";
            base.Elements.AddRange(Elements);
        }
    }
}
