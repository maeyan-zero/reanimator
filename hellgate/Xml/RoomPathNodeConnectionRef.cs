namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "ROOM_PATH_NODE_CONNECTION_REF")]
    public class RoomPathNodeConnectionRef
    {
        [XmlCookedAttribute(
            Name = "nConnectionIndex",
            ElementType = ElementType.Int32,
            DefaultValue = 0)] // INVALID_ID
        public int ConnectionIndex;

        [XmlCookedAttribute(
            Name = "pConnection",
            ElementType = ElementType.UnknownPTypeD_0x0D00,
            DefaultValue = 0)]
        public RoomPathNodeConnection Connection; // guessing this is what the UnknownPTypeD_0x0D00 refers to (pointer etc)
    }
}