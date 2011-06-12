using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "ROOM_PATH_NODE")]
    public class RoomPathNode
    {
        [XmlCookedAttribute(
            Name = "nIndex",
            ElementType = ElementType.Int32,
            DefaultValue = 0)]
        public int Index;

        [XmlCookedAttribute(
            Name = "nEdgeIndex",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = 0)] // INVALID_ID
        public int EdgeIndex;

        [XmlCookedAttribute(
             Name = "vNormal",
             ElementType = ElementType.FloatArrayFixed,
             DefaultValue = 0.0f,
             Count = 3,
             CustomType = ElementType.Vector3)]
        public Vector3 Normal;

        [XmlCookedAttribute(
             Name = "vPosition",
             ElementType = ElementType.FloatArrayFixed,
             DefaultValue = 0.0f,
             Count = 3,
             CustomType = ElementType.Vector3)]
        public Vector3 Position;

        [XmlCookedAttribute(
             Name = "fHeight",
             ElementType = ElementType.Float,
             DefaultValue = 0.0f)]
        public float Height;

        [XmlCookedAttribute(
             Name = "fRadius",
             ElementType = ElementType.Float,
             DefaultValue = 0.0f)]
        public float Radius;

        [XmlCookedAttribute(
             Name = "dwFlags",
             ElementType = ElementType.Int32,
             DefaultValue = 0)]
        public int Flags; // is this type PathFlags??

        [XmlCookedAttribute(
             Name = "pConnections",
             ElementType = ElementType.TableArrayVariable,
             DefaultValue = null,
             ChildType = typeof(RoomPathNodeConnection))]
        public RoomPathNodeConnection[] Connections;

        [XmlCookedAttribute(
             Name = "pLongConnections",
             ElementType = ElementType.TableArrayVariable,
             DefaultValue = null,
             ChildType = typeof(RoomPathNodeConnectionRef))]
        public RoomPathNodeConnectionRef[] LongConnections;

        [XmlCookedAttribute(
             Name = "pShortConnections",
             ElementType = ElementType.TableArrayVariable,
             DefaultValue = null,
             ChildType = typeof(RoomPathNodeConnectionRef))]
        public RoomPathNodeConnectionRef[] ShortConnections;
    }
}