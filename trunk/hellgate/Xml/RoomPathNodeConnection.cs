namespace Hellgate.Xml
{
    class RoomPathNodeConnection : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                    Name = "nConnectionIndex",
                    ElementType = ElementType.Int32,
                    DefaultValue = 0 // INVALID_ID
            },
            new XmlCookElement
            {
                    Name = "pConnection",
                    ElementType = ElementType.UnknownPTypeD,
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
                    Name = "fDistance",
                    ElementType = ElementType.Float,
                    DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                    Name = "fHeight",
                    ElementType = ElementType.Float,
                    DefaultValue = 0.0f
            }
        };

        public RoomPathNodeConnection()
        {
            RootElement = "ROOM_PATH_NODE_CONNECTION";
            base.Elements.AddRange(Elements);
        }
    }
}