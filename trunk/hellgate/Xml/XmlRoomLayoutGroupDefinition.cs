namespace Hellgate.Xml
{
    class XmlRoomLayoutGroupDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "tGroup",
                ElementType = ElementType.Table,
                DefaultValue = null,
                ChildType = typeof (XmlRoomLayoutGroup)
            },
            new XmlCookElement
            {
                Name = "bShowIcons",
                ElementType = ElementType.Int32,
                DefaultValue = 0, // FALSE
            },
            new XmlCookElement
            {
                Name = "nEditType",
                ElementType = ElementType.Int32,
                DefaultValue = 0, // EDIT_TYPE_POLY_PICK
            },
            new XmlCookElement
            {
                Name = "bExists",
                ElementType = ElementType.Int32,
                DefaultValue = 0, // FALSE
            },
            new XmlCookElement
            {
                Name = "bFixup",
                ElementType = ElementType.Int32,
                DefaultValue = 0, // FALSE
            }
        };

        public XmlRoomLayoutGroupDefinition()
        {
            RootElement = "ROOM_LAYOUT_GROUP_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}