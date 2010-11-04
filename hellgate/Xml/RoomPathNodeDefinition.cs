using System;

namespace Hellgate.Xml
{
    class RoomPathNodeDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "pPathNodeSets",
                ElementType = ElementType.TableCount,
                DefaultValue = null,
                ChildType = typeof (RoomPathNodeSet)
            },
            new XmlCookElement
            {
                Name = "ROOM_PATH_NODE_DEF_INDOOR_FLAG",
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 0), // 1
                DefaultValue = false
            },
            new XmlCookElement
            {
                Name = "ROOM_PATH_NODE_DEF_NO_PATHNODES_FLAG",
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 1), // 2
                DefaultValue = false
            },
            new XmlCookElement
            {
                Name = "ROOM_PATH_NODE_DEF_USE_TUGBOAT",
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 1), // 2
                DefaultValue = false
            },
            new XmlCookElement
            {
                Name = "fRadius",
                ElementType = ElementType.Float,
                DefaultValue = 1.5f
            },
            new XmlCookElement
            {
                Name = "fNodeFrequencyX",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fNodeFrequencyY",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fDiagDistBetweenNodesSq",
                ElementType = ElementType.UnknownFloat,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fDiagDistBetweenNodes",
                ElementType = ElementType.UnknownFloat,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fNodeOffsetX",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fNodeOffsetY",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "bExists",
                ElementType = ElementType.Int32,
                DefaultValue = 0 // FALSE
            },
            new XmlCookElement
            {
                Name = "fNodeMinZ",
                ElementType = ElementType.Float,
                DefaultValue = -0.5f
            },
            new XmlCookElement
            {
                Name = "fNodeMaxZ",
                ElementType = ElementType.Float,
                DefaultValue = 0.5f
            },
        };

        public RoomPathNodeDefinition()
        {
            RootElement = "ROOM_PATH_NODE_DEFINITION";
            base.Elements.AddRange(Elements);
            BitFields = new Int32[] {-1};
        }
    }
}