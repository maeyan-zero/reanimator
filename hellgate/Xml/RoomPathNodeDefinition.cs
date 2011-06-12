using System;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "ROOM_PATH_NODE_DEFINITION")]
    public class RoomPathNodeDefinition
    {
        [Flags]
        public enum PathFlags : uint
        {
            ROOM_PATH_NODE_DEF_INDOOR_FLAG = (1 << 0),
            ROOM_PATH_NODE_DEF_NO_PATHNODES_FLAG = (1 << 1),
            //ROOM_PATH_NODE_DEF_USE_TUGBOAT = (1 << 1),
            [XmlCookedAttribute(IsResurrection = true)]
            ROOM_PATH_NODE_DEF_USE_TUGBOAT_FLAG = (1 << 2) // field appears to be renamed and new valued (either that or I read in above wrong to start with... tood?)
        }

        [XmlCookedAttribute(
            Name = "pPathNodeSets",
            ElementType = ElementType.TableArrayVariable,
            DefaultValue = null,
            ChildType = typeof(RoomPathNodeSet))]
        public RoomPathNodeSet[] PathNodeSets;

        [XmlCookedAttribute(
            Name = "Flags",
            ElementType = ElementType.Flag,
            FlagId = 1,
            DefaultValue = false)]
        public PathFlags Flags;

        [XmlCookedAttribute(
            Name = "fRadius",
            ElementType = ElementType.Float,
            DefaultValue = 1.5f)]
        public float Radius;

        [XmlCookedAttribute(
            Name = "fNodeFrequencyX",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f)]
        public float NodeFrequencyX;

        [XmlCookedAttribute(
            Name = "fNodeFrequencyY",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f)]
        public float NodeFrequencyY;

        [XmlCookedAttribute(
            Name = "fDiagDistBetweenNodesSq",
            ElementType = ElementType.NonCookedFloat,
            DefaultValue = 1.0f)]
        public float DiagDistBetweenNodesSq;

        [XmlCookedAttribute(
            Name = "fDiagDistBetweenNodes",
            ElementType = ElementType.NonCookedFloat,
            DefaultValue = 1.0f)]
        public float DiagDistBetweenNodes;

        [XmlCookedAttribute(
            Name = "fNodeOffsetX",
            ElementType = ElementType.NonCookedFloat,
            DefaultValue = 0.0f)]
        public float NodeOffsetX;

        [XmlCookedAttribute(
            Name = "fNodeOffsetY",
            ElementType = ElementType.NonCookedFloat,
            DefaultValue = 0.0f)]
        public float NodeOffsetY;

        [XmlCookedAttribute(
            Name = "bExists",
            ElementType = ElementType.Int32,
            DefaultValue = false,
            CustomType = ElementType.Bool)]
        public bool Exists;

        [XmlCookedAttribute(
            Name = "fNodeMinZ",
            ElementType = ElementType.Float,
            DefaultValue = -0.5f)]
        public float NodeMinZ;

        [XmlCookedAttribute(
            Name = "fNodeMaxZ",
            ElementType = ElementType.Float,
            DefaultValue = 0.5f)]
        public float NodeMaxZ;

        [XmlCookedAttribute(
            Name = "vCorner",
            ElementType = ElementType.FloatArrayFixed,
            DefaultValue = 0.0f,
            Count = 3,
            IsTestCentre = true,
            CustomType = ElementType.Vector3)]
        public Vector3 Corner;
    }
}