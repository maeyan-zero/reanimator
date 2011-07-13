using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Hellgate
{
    public class XmlCookedTree
    {
        private class Branch
        {
            public readonly XmlCookedElement Element;
            public readonly XmlCookedTree Tree;

            public Branch(XmlCookedElement element, XmlCookedTree tree=null)
            {
                Element = element;
                Tree = tree;
            }
        }

        private uint DefinitionHash { get { return Definition.Attributes.NameHash; } }
        private readonly XmlCookedElement _element;
        public XmlCookedDefinition Definition { get; private set; }

        private readonly Dictionary<uint, Branch> _elements; 
        public int Count { get; private set; }

        private XmlCookedTree Parent { get; set; }
        private XmlCookedTree Root { get { return (Parent == null) ? this : Parent.Root; } }

        private readonly XmlCookedTree _twinTree;
        public XmlCookedTree TwinRoot { get { return (_twinTree == null) ? this : _twinTree.TwinRoot; } }

        /// <summary>
        /// Primary constructor used for root node types.
        /// </summary>
        /// <param name="xmlDefinition">The root XML Definition.</param>
        /// <param name="elementCount">Count of elements in this definition in this XML File.</param>
        public XmlCookedTree(XmlCookedDefinition xmlDefinition, int elementCount)
        {
            Definition = xmlDefinition;
            Count = elementCount;
            _elements = new Dictionary<uint, Branch>();
        }

        /// <summary>
        /// Secondary constructor used for child node types.
        /// </summary>
        /// <param name="xmlDefinition">The child tree XML Definition.</param>
        /// <param name="xmlElement">The XML Element type (should be of .ElementType = Table or TableCount).</param>
        /// <param name="parentTree">The parent XML tree.</param>
        /// <param name="elementCount">Count of elements in this definition in this XML File.</param>
        public XmlCookedTree(XmlCookedDefinition xmlDefinition, XmlCookedElement xmlElement, XmlCookedTree parentTree, int elementCount)
        {
            Definition = xmlDefinition;
            _element = xmlElement;
            Parent = parentTree;
            Count = elementCount;
            _elements = new Dictionary<uint, Branch>();
        }

        /// <summary>
        /// Tertiary constructor used for twin/duplicate XML Definitions within a file.
        /// </summary>
        /// <param name="xmlElement">The differing XML Element for this twin.</param>
        /// <param name="parentTree">he parent XML tree.</param>
        /// <param name="twinTree">The original XML tree.</param>
        private XmlCookedTree(XmlCookedElement xmlElement, XmlCookedTree parentTree, XmlCookedTree twinTree)
        {
            _element = xmlElement;
            Parent = parentTree;
            Definition = twinTree.Definition;
            _twinTree = twinTree;
        }

        /// <summary>
        /// Add an element to the tree.
        /// </summary>
        /// <param name="xmlElement">The XML Element to add to the tree.</param>
        public void AddElement(XmlCookedElement xmlElement)
        {
            _elements.Add(xmlElement.NameHash, new Branch(xmlElement));
        }

        /// <summary>
        /// Replaces last added element with tree.
        /// </summary>
        /// <param name="xmlCookedFileTree">The tree to the previous element.</param>
        public void AddTree(XmlCookedTree xmlCookedFileTree)
        {
            _elements[_elements.Last().Key] = new Branch(xmlCookedFileTree._element, xmlCookedFileTree);
        }

        /// <summary>
        /// Replaces last added element with tree. Automatically links the XML Element to tree.
        /// </summary>
        /// <param name="definitionHash">The element name hash of the existing tree.</param>
        public void AddExistingTree(UInt32 definitionHash)
        {
            XmlCookedTree tree = _GetExistingTree(Root, definitionHash);
            Debug.Assert(tree != null, "AddExistingTree: tree not found!");
            tree = tree.TwinRoot;
            Debug.Assert(tree != null, "AddExistingTree: tree not found!");

            XmlCookedElement xmlElement = _elements.Last().Value.Element;
            XmlCookedTree newTree = new XmlCookedTree(xmlElement, this, tree);
            AddTree(newTree);
        }

        public XmlCookedElement GetElement(uint nameHash)
        {
            return Definition.GetElement(nameHash);
        }

        public XmlCookedElement GetElement(String name)
        {
            return Definition.GetElement(name);
        }

        /// <summary>
        /// Get an already defined definition.
        /// </summary>
        /// <param name="xmlTree"></param>
        /// <param name="elementHash"></param>
        /// <returns></returns>
        private static XmlCookedTree _GetExistingTree(XmlCookedTree xmlTree, UInt32 elementHash)
        {
            foreach (Branch branch in xmlTree._elements.Select(keyValuePair => keyValuePair.Value))
            {
                if (branch.Tree == null) continue;
                if (branch.Tree.DefinitionHash == elementHash) return branch.Tree;

                XmlCookedTree existingTree = _GetExistingTree(branch.Tree, elementHash);
                if (existingTree != null) return existingTree;
            }

            return null;
        }

        public bool ContainsElement(uint elementHash)
        {
            return _elements.ContainsKey(elementHash);
        }

        public XmlCookedTree GetTree(int i)
        {
            return _elements.Values.ElementAt(i).Tree;
        }
    }
}