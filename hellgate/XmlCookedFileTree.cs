using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Hellgate.Xml;

namespace Hellgate
{
    class XmlCookedFileTree
    {
        public UInt32 DefinitionNameHash { get { return (Definition == null) ? 0 : Definition.RootHash; } }
        public XmlCookElement CookElement { get; private set; }
        public XmlDefinition Definition { get; private set; }

        private readonly List<KeyValuePair<UInt32, Object>> _elements;
        public int Count {get { return (_elements == null) ? -1 : _elements.Count; } }

        public XmlCookedFileTree Parent { get; private set; }
        public XmlCookedFileTree Root { get { return (Parent == null) ? this : Parent.Root; } }

        private readonly XmlCookedFileTree _twinTree;
        public XmlCookedFileTree TwinRoot { get { return (_twinTree == null) ? this : _twinTree.TwinRoot; } }

        /// <summary>
        /// Primary constructor used for root node types.
        /// </summary>
        /// <param name="xmlDefinition">The root XML Definition.</param>
        public XmlCookedFileTree(XmlDefinition xmlDefinition)
        {
            Definition = xmlDefinition;
            _elements = new List<KeyValuePair<UInt32, Object>>();
        }

        /// <summary>
        /// Secondary constructor used for child node types.
        /// </summary>
        /// <param name="xmlDefinition">The child tree XML Definition.</param>
        /// <param name="xmlCookElement">The XML Element type (should be of .ElementType = Table or TableCount).</param>
        /// <param name="parentTree">The parent XML tree.</param>
        public XmlCookedFileTree(XmlDefinition xmlDefinition, XmlCookElement xmlCookElement, XmlCookedFileTree parentTree)
        {
            Definition = xmlDefinition;
            CookElement = xmlCookElement;
            Parent = parentTree;
            _elements = new List<KeyValuePair<UInt32, Object>>();
        }

        /// <summary>
        /// Tertiary constructor used for twin/duplicate XML Definitions within a file.
        /// </summary>
        /// <param name="xmlCookElement">The differing XML Element for this twin.</param>
        /// <param name="parentTree">he parent XML tree.</param>
        /// <param name="twinTree">The original XML tree.</param>
        private XmlCookedFileTree(XmlCookElement xmlCookElement, XmlCookedFileTree parentTree, XmlCookedFileTree twinTree)
        {
            CookElement = xmlCookElement;
            Parent = parentTree;
            Definition = twinTree.Definition;
            _twinTree = twinTree;
        }

        /// <summary>
        /// Add an element to the tree.
        /// </summary>
        /// <param name="xmlCookElement">The XML Element to add to the tree.</param>
        public void AddElement(XmlCookElement xmlCookElement)
        {
            _elements.Add(new KeyValuePair<UInt32, Object>(xmlCookElement.NameHash, xmlCookElement));
        }

        /// <summary>
        /// Replaces last added element with tree.
        /// </summary>
        /// <param name="xmlCookedFileTree">The tree to the previous element.</param>
        public void AddTree(XmlCookedFileTree xmlCookedFileTree)
        {
            _elements[Count - 1] = new KeyValuePair<UInt32, Object>(xmlCookedFileTree.DefinitionNameHash, xmlCookedFileTree);
        }

        /// <summary>
        /// Replaces last added element with tree. Automatically links the XML Element to tree.
        /// </summary>
        /// <param name="elementHash">The element name hash of the existing tree.</param>
        public void AddExistingTree(UInt32 elementHash)
        {
            XmlCookedFileTree tree = _GetExistingTree(Root, elementHash);
            tree = tree.TwinRoot;
            Debug.Assert(tree != null, "AddExistingTree: tree not found!");

            XmlCookElement xmlCookElement = this[Count - 1];
            XmlCookedFileTree newTree = new XmlCookedFileTree(xmlCookElement, this, tree);
            AddTree(newTree);
        }

        private static XmlCookedFileTree _GetExistingTree(XmlCookedFileTree xmlTree, UInt32 elementHash)
        {
            XmlCookedFileTree existingTree = xmlTree.GetTree(elementHash); // check current tree
            if (existingTree != null) return existingTree;

            foreach (XmlCookedFileTree xmlChildTree in xmlTree._elements.Select(keyValuePair => keyValuePair.Value).OfType<XmlCookedFileTree>())
            {
                if (xmlChildTree.DefinitionNameHash == elementHash) return xmlChildTree;

                existingTree = _GetExistingTree(xmlChildTree, elementHash);
                if (existingTree != null) return existingTree;
            }

            return null;
        }

        public bool ContainsElement(UInt32 elementHash)
        {
            foreach (KeyValuePair<UInt32, Object> keyValuePair in _elements)
            {
                XmlCookElement xmlCookElement = keyValuePair.Value as XmlCookElement;
                if (xmlCookElement == null)
                {
                    XmlCookedFileTree xmlCookedFileTree = keyValuePair.Value as XmlCookedFileTree;
                    if (xmlCookedFileTree != null && xmlCookedFileTree.CookElement.NameHash == elementHash) return true;
                }
                else
                {
                    if (xmlCookElement.NameHash == elementHash) return true;
                }
            }

            return false;
        }

        public XmlCookedFileTree GetTree(UInt32 elementHash)
        {
            // todo: recursive?
            foreach (XmlCookedFileTree xmlChildTree in
                _elements.Select(keyValuePair => keyValuePair.Value).OfType<XmlCookedFileTree>())
            {
                if (xmlChildTree.DefinitionNameHash == elementHash) return xmlChildTree;
            }

            return null;
        }

        public XmlCookElement this[int i]
        {
            get
            {
                // if we have a twin/original tree link, then use that for the elements as we have none
                //if (_twinTree != null) return _twinTree[i];

                KeyValuePair<UInt32, Object> keyValuePair = _elements[i];

                XmlCookElement xmlCookElement = keyValuePair.Value as XmlCookElement;
                if (xmlCookElement != null) return xmlCookElement;

                XmlCookedFileTree xmlCookedFileTree = keyValuePair.Value as XmlCookedFileTree;
                return xmlCookedFileTree == null ? null : xmlCookedFileTree.CookElement;
            }
        }

        public XmlCookedFileTree GetTree(int i)
        {
            return _elements[i].Value as XmlCookedFileTree;
        }
    }
}