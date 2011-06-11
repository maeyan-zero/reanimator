using System;

namespace Hellgate.Xml
{
    class XmlRoomPathNodeDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "pPathNodeSets",
                ElementType = ElementType.TableArrayVariable,
                DefaultValue = null,
                ChildType = typeof (XmlRoomPathNodeSet)
            },
            new XmlCookElement
            {
                Name = "ROOM_PATH_NODE_DEF_INDOOR_FLAG",
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 0), // 1
                DefaultValue = false
            },
            new XmlCookElement
            {
                Name = "ROOM_PATH_NODE_DEF_NO_PATHNODES_FLAG",
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 1), // 2
                DefaultValue = false
            },
            new XmlCookElement
            {
                Name = "ROOM_PATH_NODE_DEF_USE_TUGBOAT",
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 1), // 2
                DefaultValue = false
            },
            new XmlCookElement
            {
                Name = "ROOM_PATH_NODE_DEF_USE_TUGBOAT_FLAG",
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 2), // 4
                DefaultValue = false,
                IsResurrection = true // field appears to be renamed and new valued
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
                ElementType = ElementType.NonCookedFloat,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fDiagDistBetweenNodes",
                ElementType = ElementType.NonCookedFloat,
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
            new XmlCookElement
            {
                Name = "vCorner",
                ElementType = ElementType.FloatArrayFixed,
                DefaultValue = 0.0f,
                Count = 3,
                IsTestCentre = true
            }
        };

        public XmlRoomPathNodeDefinition()
        {
            RootElement = "ROOM_PATH_NODE_DEFINITION";
            base.Elements.AddRange(Elements);
            Flags = new Int32[] {-1};
        }
    }
}