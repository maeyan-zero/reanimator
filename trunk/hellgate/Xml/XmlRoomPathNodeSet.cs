﻿namespace Hellgate.Xml
{
    class XmlRoomPathNodeSet : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "pszIndex",
                ElementType = ElementType.String,
                DefaultValue = null
            },
            new XmlCookElement
            {
                Name = "pPathNodes",
                ElementType = ElementType.TableArrayVariable,
                DefaultValue = null,
                ChildType = typeof (XmlRoomPathNode)
            },
            new XmlCookElement
            {
                Name = "pHappyNodes",
                ElementType = ElementType.Int32ArrayVariable,
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
                Name = "nEdgeNodeCount",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0
            },
            new XmlCookElement
            {
                Name = "pEdgeNodes",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0 // NULL
            },
            new XmlCookElement
            {
                Name = "fMinX",
                ElementType = ElementType.NonCookedFloat,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fMaxX",
                ElementType = ElementType.NonCookedFloat,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fMinY",
                ElementType = ElementType.NonCookedFloat,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fMaxY",
                ElementType = ElementType.NonCookedFloat,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "nArraySize",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0
            },
            new XmlCookElement
            {
                Name = "pNodeHashArray",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0 // FALSE
            },
            new XmlCookElement
            {
                Name = "nHashLengths",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0
            }
        };

        public XmlRoomPathNodeSet()
        {
            RootElement = "ROOM_PATH_NODE_SET";
            base.Elements.AddRange(Elements);
        }
    }
}