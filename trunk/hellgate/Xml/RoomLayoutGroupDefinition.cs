namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "ROOM_LAYOUT_GROUP_DEFINITION")]
    public class RoomLayoutGroupDefinition
    {
        [XmlCookedAttribute(
            Name = "tGroup",
            ElementType = ElementType.Table,
            DefaultValue = null,
            ChildType = typeof(RoomLayoutGroup))]
        public RoomLayoutGroup Group;

        [XmlCookedAttribute(
            Name = "bShowIcons",
            ElementType = ElementType.Int32,
            DefaultValue = false,
            CustomType = ElementType.Bool)]
        public bool ShowIcons;

        [XmlCookedAttribute(
            Name = "nEditType",
            ElementType = ElementType.Int32,
            DefaultValue = 0)] // EDIT_TYPE_POLY_PICK
        public int EditType;

        [XmlCookedAttribute(
            Name = "bExists",
            ElementType = ElementType.Int32,
            DefaultValue = false,
            CustomType = ElementType.Bool)]
        public bool Exists;

        [XmlCookedAttribute(
            Name = "bFixup",
            ElementType = ElementType.Int32,
            DefaultValue = false,
            CustomType = ElementType.Bool)]
        public bool Fixup;
    }
}