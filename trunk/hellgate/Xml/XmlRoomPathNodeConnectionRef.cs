namespace Hellgate.Xml
{
    class XmlRoomPathNodeConnectionRef : XmlDefinition
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
                    ElementType = ElementType.UnknownPTypeD_0x0D00,
                    DefaultValue = 0
            }
        };

        public XmlRoomPathNodeConnectionRef()
        {
            RootElement = "ROOM_PATH_NODE_CONNECTION_REF";
            base.Elements.AddRange(Elements);
        }
    }
}