using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XmlElement = Hellgate.XmlCookedObject.XmlElement;
using XmlDefinition = Hellgate.XmlCookedObject.XmlDefinition;

namespace Hellgate
{
    public class XmlCookedTree
    {
        public uint DefinitionNameHash { get { return Definition.RootElement.NameHash; } }
        public XmlElement Element { get; private set; }
        public XmlDefinition Definition { get; private set; }

        //private readonly Dictionary<uint, XmlElement> _elements;
        private readonly List<KeyValuePair<UInt32, XmlElement>> _elements;
        public int Count { get; private set; }

        public XmlCookedTree Parent { get; private set; }
        public XmlCookedTree Root { get { return (Parent == null) ? this : Parent.Root; } }

        private readonly XmlCookedTree _twinTree;
        public XmlCookedTree TwinRoot { get { return (_twinTree == null) ? this : _twinTree.TwinRoot; } }

        /// <summary>
        /// Primary constructor used for root node types.
        /// </summary>
        /// <param name="xmlDefinition">The root XML Definition.</param>
        /// <param name="elementCount">Count of elements in this definition in this XML File.</param>
        public XmlCookedTree(XmlDefinition xmlDefinition, int elementCount)
        {
            Definition = xmlDefinition;
            Count = elementCount;
            _elements = new List<KeyValuePair<UInt32, XmlElement>>();
        }

        /// <summary>
        /// Secondary constructor used for child node types.
        /// </summary>
        /// <param name="xmlDefinition">The child tree XML Definition.</param>
        /// <param name="xmlElement">The XML Element type (should be of .ElementType = Table or TableCount).</param>
        /// <param name="parentTree">The parent XML tree.</param>
        /// <param name="elementCount">Count of elements in this definition in this XML File.</param>
        public XmlCookedTree(XmlDefinition xmlDefinition, XmlElement xmlElement, XmlCookedTree parentTree, int elementCount)
        {
            Definition = xmlDefinition;
            Element = xmlElement;
            Parent = parentTree;
            Count = elementCount;
            _elements = new List<KeyValuePair<UInt32, XmlElement>>();
        }

        /// <summary>
        /// Tertiary constructor used for twin/duplicate XML Definitions within a file.
        /// </summary>
        /// <param name="xmlElement">The differing XML Element for this twin.</param>
        /// <param name="parentTree">he parent XML tree.</param>
        /// <param name="twinTree">The original XML tree.</param>
        private XmlCookedTree(XmlElement xmlElement, XmlCookedTree parentTree, XmlCookedTree twinTree)
        {
            Element = xmlElement;
            Parent = parentTree;
            Definition = twinTree.Definition;
            _twinTree = twinTree;
        }

        /// <summary>
        /// Add an element to the tree.
        /// </summary>
        /// <param name="xmlElement">The XML Element to add to the tree.</param>
        public void AddElement(XmlElement xmlElement)
        {
            _elements.Add(new KeyValuePair<UInt32, XmlElement>(xmlElement.NameHash, xmlElement));
        }

        /// <summary>
        /// Replaces last added element with tree.
        /// </summary>
        /// <param name="xmlCookedFileTree">The tree to the previous element.</param>
        public void AddTree(XmlCookedTree xmlCookedFileTree)
        {
            //_elements.Remove(_elements.Last().Key);
            //_elements.Add(xmlCookedFileTree.DefinitionNameHash, new XmlElement(xmlCookedFileTree));
            _elements[_elements.Count - 1] = new KeyValuePair<UInt32, XmlElement>(xmlCookedFileTree.DefinitionNameHash, new XmlElement(xmlCookedFileTree));
        }

        /// <summary>
        /// Replaces last added element with tree. Automatically links the XML Element to tree.
        /// </summary>
        /// <param name="elementHash">The element name hash of the existing tree.</param>
        public void AddExistingTree(UInt32 elementHash)
        {
            XmlCookedTree tree = _GetExistingTree(Root, elementHash);
            Debug.Assert(tree != null, "AddExistingTree: tree not found!");
            tree = tree.TwinRoot;
            Debug.Assert(tree != null, "AddExistingTree: tree not found!");

            XmlElement xmlElement = this[_elements.Count - 1];
            XmlCookedTree newTree = new XmlCookedTree(xmlElement, this, tree);
            AddTree(newTree);
        }

        public XmlElement GetElement(uint nameHash)
        {
            return Definition.GetElement(nameHash);
        }

        public XmlElement GetElement(String name)
        {
            return Definition.GetElement(name);
        }

        /// <summary>
        /// todo: this function is gross and executes a lot of unnecessary - optimize me
        /// </summary>
        /// <param name="xmlTree"></param>
        /// <param name="elementHash"></param>
        /// <returns></returns>
        private XmlCookedTree _GetExistingTree(XmlCookedTree xmlTree, UInt32 elementHash)
        {
            // check current branch
            foreach (XmlElement xmlCookedElement in _elements.Select(keyValuePair => keyValuePair.Value))
            {
                if (xmlCookedElement.XmlTree == null) continue;
                if (xmlCookedElement.XmlTree.DefinitionNameHash == elementHash) return xmlCookedElement.XmlTree;
            }
            //XmlCookedTree existingTree = xmlTree.GetTree(elementHash);
            //if (existingTree != null) return existingTree;

            // check entire tree set
            foreach (XmlElement xmlCookedElement in xmlTree._elements.Select(keyValuePair => keyValuePair.Value))
            {
                if (xmlCookedElement.XmlTree == null) continue;
                if (xmlCookedElement.XmlTree.DefinitionNameHash == elementHash) return xmlCookedElement.XmlTree;

                XmlCookedTree existingTree = _GetExistingTree(xmlCookedElement.XmlTree, elementHash);
                if (existingTree != null) return existingTree;
            }

            //foreach (XmlElement xmlCookedElement in xmlTree._elements.Values)
            //{
            //    if (xmlCookedElement.XmlTree == null) continue;
            //    if (xmlCookedElement.XmlTree.DefinitionNameHash == elementHash) return xmlCookedElement.XmlTree;

            //    existingTree = _GetExistingTree(xmlCookedElement.XmlTree, elementHash);
            //    if (existingTree != null) return existingTree;
            //}

            return null;
        }

        public bool ContainsElement(uint elementHash)
        {
            //if (_elements.ContainsKey(elementHash)) return true;

            //foreach (XmlElement xmlElement in _elements.Values)
            //{
            //    if (xmlElement.XmlTree == null) continue;
            //    if (xmlElement.XmlTree.Element.NameHash == elementHash) return true;
            //}

            //return false;

            foreach (KeyValuePair<UInt32, XmlElement> keyValuePair in _elements)
            {
                if (keyValuePair.Key == elementHash) return true;
                if (keyValuePair.Value.XmlTree == null) continue;
                //if (keyValuePair.Value.XmlTree.DefinitionNameHash == elementHash) return true;
                if (keyValuePair.Value.XmlTree.Element.NameHash == elementHash) return true;

                //if (keyValuePair.Value.NameHash == elementHash) return true;
                //if (keyValuePair.Value.)

                //XmlCookedAttribute xmlCookElement = keyValuePair.Value as XmlCookedAttribute;
                //if (xmlCookElement == null)
                //{
                //    XmlCookedFileTree xmlCookedFileTree = keyValuePair.Value as XmlCookedFileTree;
                //    if (xmlCookedFileTree != null && xmlCookedFileTree.CookElement.NameHash == elementHash) return true;
                //}
                //else
                //{
                //    if (xmlCookElement.NameHash == elementHash) return true;
                //}
            }

            return false;
        }

        public XmlCookedTree GetTree(UInt32 elementHash)
        {
            //XmlElement xmlElement;
            //return (_elements.TryGetValue(elementHash, out xmlElement)) ? xmlElement.XmlTree : null;

            foreach (XmlElement xmlCookedElement in _elements.Select(keyValuePair => keyValuePair.Value))
            {
                if (xmlCookedElement.XmlTree == null) continue;
                if (xmlCookedElement.XmlTree.DefinitionNameHash == elementHash) return xmlCookedElement.XmlTree;
            }

            //foreach (XmlCookedTree xmlChildTree in _elements.Select(keyValuePair => keyValuePair.Value).OfType<XmlCookedTree>())
            //{
            //    if (xmlChildTree.DefinitionNameHash == elementHash) return xmlChildTree;
            //}

            return null;
        }

        public XmlElement this[int i]
        {
            get
            {
                //XmlElement xmlElement = _elements.Values.ElementAt(i);
                //return (xmlElement.XmlTree == null) ? xmlElement : xmlElement.XmlTree.Element;

                XmlElement xmlElement = _elements[i].Value;
                return (xmlElement.XmlTree == null) ? xmlElement : xmlElement.XmlTree.Element;

                //KeyValuePair<UInt32, Object> keyValuePair = _elements[i];

                //XmlElement xmlElement = keyValuePair.Value as XmlElement;
                //if (xmlElement != null) return xmlElement;

                //XmlCookedTree xmlCookedFileTree = keyValuePair.Value as XmlCookedTree;
                //return xmlCookedFileTree == null ? null : xmlCookedFileTree.Element;
            }
        }

        public XmlCookedTree GetTree(int i)
        {
            //XmlElement xmlElement = _elements.Values.ElementAt(i);
            //return xmlElement.XmlTree;

            return _elements[i].Value.XmlTree;
        }

    }
}