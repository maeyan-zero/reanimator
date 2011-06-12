using System;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "ROOM_PATH_NODE_SET")]
    public class RoomPathNodeSet
    {
        [XmlCookedAttribute(
            Name = "pszIndex",
            ElementType = ElementType.String,
            DefaultValue = null)]
        public String Index;

        [XmlCookedAttribute(
            Name = "pPathNodes",
            ElementType = ElementType.TableArrayVariable,
            DefaultValue = null,
            ChildType = typeof(RoomPathNode))]
        public RoomPathNode[] PathNodes;

        [XmlCookedAttribute(
             Name = "pHappyNodes",
             ElementType = ElementType.Int32ArrayVariable,
             DefaultValue = 0)]
        public int[] HappyNodes;

        [XmlCookedAttribute(
             Name = "dwFlags",
             ElementType = ElementType.Int32,
             DefaultValue = 0)]
        public int Flags; // is this type PathFlags??

        [XmlCookedAttribute(
             Name = "nEdgeNodeCount",
             ElementType = ElementType.NonCookedInt32,
             DefaultValue = 0)]
        public int EdgeNodeCount;

        [XmlCookedAttribute(
             Name = "pEdgeNodes",
             ElementType = ElementType.NonCookedInt32,
             DefaultValue = 0)] // NULL
        public int EdgeNodes;

        [XmlCookedAttribute(
             Name = "fMinX",
             ElementType = ElementType.NonCookedFloat,
             DefaultValue = 0.0f)]
        public float MinX;

        [XmlCookedAttribute(
             Name = "fMaxX",
             ElementType = ElementType.NonCookedFloat,
             DefaultValue = 0.0f)]
        public float MaxX;

        [XmlCookedAttribute(
             Name = "fMinY",
             ElementType = ElementType.NonCookedFloat,
             DefaultValue = 0.0f)]
        public float MinY;

        [XmlCookedAttribute(
             Name = "fMaxY",
             ElementType = ElementType.NonCookedFloat,
             DefaultValue = 0.0f)]
        public float MaxY;

        [XmlCookedAttribute(
             Name = "nArraySize",
             ElementType = ElementType.NonCookedInt32,
             DefaultValue = 0)]
        public int ArraySize;

        [XmlCookedAttribute(
             Name = "pNodeHashArray",
             ElementType = ElementType.NonCookedInt32,
             DefaultValue = 0)] // FALSE
        public float NodeHashArray;

        [XmlCookedAttribute(
             Name = "nHashLengths",
             ElementType = ElementType.NonCookedInt32,
             DefaultValue = 0)]
        public int HashLengths;
    }
}