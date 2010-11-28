using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using Hellgate.Xml;
using Revival.Common;

namespace Hellgate
{
    public partial class XmlCookedFile
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class XmlCookFileHeader
        {
            public UInt32 MagicWord;
            public Int32 Version;
            public UInt32 XmlRootDefinition;
            public Int32 XmlRootElementCount;
        }

        /// <summary>
        /// Initialize XML Definitions and generate String Hash values for xml element names.
        /// Must be called before usage of the class.
        /// </summary>
        /// <param name="fileManager">The loaded table data set of excel/strings for excel lookups.</param>
        public static void Initialize(FileManager fileManager)
        {
            Debug.Assert(fileManager != null);

            _fileManager = fileManager;
            _xmlDefinitions = new XmlDefinition[]
            {
                // AI
                new AIDefinition(),
                new AIBehaviorDefinitionTable(),
                new AIBehaviorDefinition(),

                // Appearance
                new AppearanceDefinition(),
                new AnimationDefinition(),
                new AnimEvent(),
                new InventoryViewInfo(),

                // Colorsets (colorsets.xml)
                new ColorSetDefinition(),
                new ColorDefinition(),

                // Config (config.xml)
                new ConfigDefinition(),

                // GameGlobalDefinition (gamedefault.xml)
                new GameGlobalDefinition(),

                // GlobalDefinition (default.xml)
                new GlobalDefinition(),

                // Shared (used in States, Skills amd Appearance)
                new ConditionDefinition(),

                // Level Layout (contains object positions etc)
                new RoomLayoutGroupDefinition(),
                new RoomLayoutGroup(),

                // Level Pathing (huge-ass list of nodes/points)
                // todo: not all are completely parsing
                //new RoomPathNodeDefinition(),
                //new RoomPathNodeSet(),
                //new RoomPathNode(),
                //new RoomPathNodeConnection(),
                //new RoomPathNodeConnectionRef(),

                // Material (makes things look like things)
                new Material(),

                // Skills (defines skill effect/appearance mostly, not so much the skill itself)
                new SkillEventsDefinition(),
                new SkillEventHolder(),
                new SkillEvent(),

                // Sound Effects
                new SoundEffectDefinition(),
                new SoundEffect(),

                // States
                new StateDefinition(),
                new StateEvent(),

                // Textures
                new TextureDefinition(),
                new BlendRLE(),
                new BlendRun()
            };

            // create hashes;
            foreach (XmlDefinition xmlDefinition in _xmlDefinitions)
            {
                xmlDefinition.RootHash = Crypt.GetStringHash(xmlDefinition.RootElement);

                foreach (XmlCookElement xmlCookElement in xmlDefinition.Elements)
                {
                    String stringToHash = String.IsNullOrEmpty(xmlCookElement.TrueName) ? xmlCookElement.Name : xmlCookElement.TrueName;
                    xmlCookElement.NameHash = Crypt.GetStringHash(stringToHash);
                }
            }

            // assign child table hashes
            foreach (XmlDefinition xmlDefinition in _xmlDefinitions)
            {
                foreach (XmlCookElement xmlCookElement in xmlDefinition.Elements)
                {
                    if (xmlCookElement.ChildType == null) continue;

                    Type childType = xmlCookElement.ChildType;
                    foreach (XmlDefinition xmlDef in _xmlDefinitions.Where(def => def.GetType() == childType))
                    {
                        xmlCookElement.ChildTypeHash = xmlDef.RootHash;
                        break;
                    }

                    Debug.Assert(xmlCookElement.ChildTypeHash != 0);
                }
            }
        }

        /// <summary>
        /// Searches for a known XML Definition using their Root Element String Hash.
        /// </summary>
        /// <param name="stringHash">The String Hash of the Root Element to find.</param>
        /// <returns>Found XML Definition or null if not found.</returns>
        private static XmlDefinition _GetXmlDefinition(UInt32 stringHash)
        {
            XmlDefinition xmlDefinition = _xmlDefinitions.FirstOrDefault(xmlDef => xmlDef.RootHash == stringHash);
            if (xmlDefinition != null) xmlDefinition.ResetFields();

            return xmlDefinition;
        }

        /// <summary>
        /// Searches for an XML Cook Element in an XML Definition for an Element with a particular String Hash.
        /// </summary>
        /// <param name="xmlDefinition">The XML Definition to search through.</param>
        /// <param name="stringHash">The String Hash of the Element Name to find.</param>
        /// <returns>Found XML Cook Element or null if not found.</returns>
        private static XmlCookElement _GetXmlCookElement(XmlDefinition xmlDefinition, UInt32 stringHash)
        {
            return xmlDefinition.Elements.FirstOrDefault(xmlCookElement => xmlCookElement.NameHash == stringHash);
        }

        private static bool _TestBit(IList<byte> bitField, int byteOffset, int bitOffset)
        {
            byteOffset += bitOffset >> 3;
            bitOffset &= 0x07;

            return (bitField[byteOffset] & (1 << bitOffset)) >= 1;
        }

        private static void _FlagBit(IList<byte> bitField, int byteOffset, int bitOffset)
        {
            byteOffset += bitOffset >> 3;
            bitOffset &= 0x07;

            bitField[byteOffset] |= (byte)(1 << bitOffset);
        }


        private String _ReadByteString()
        {
            byte strLen = _data[_offset++];
            if (strLen == 0xFF || strLen == 0x00) return null;

            return FileTools.ByteArrayToStringASCII(_data, ref _offset, strLen);
        }

        private void _ReadExcelIndex(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            String excelString = _ReadByteString();
            if (String.IsNullOrEmpty(excelString)) return;

            if (parentNode == null) return;

            // get excel table index
            int rowIndex = _fileManager.GetExcelRowIndex(xmlCookElement.ExcelTableCode, excelString);
            if (rowIndex == -1) throw new Exceptions.UnknownExcelElementException("excelString = " + excelString);

            XmlNode grandParentNode = parentNode.ParentNode;
            XmlNode descriptionNode = grandParentNode.LastChild.PreviousSibling;

            if (descriptionNode != null)
            {
                if (!String.IsNullOrEmpty(descriptionNode.InnerText)) descriptionNode.InnerText += ", ";
                descriptionNode.InnerText += excelString;
            }

            XmlElement element = XmlDoc.CreateElement(xmlCookElement.Name);
            element.InnerText = rowIndex.ToString();
            parentNode.AppendChild(element);
        }

        private void _ReadTable(XmlNode parentNode, XmlCookElement xmlCookElement, Hashtable elements)
        {
            String elementName = xmlCookElement.Name;
            Debug.Assert(xmlCookElement.ChildType != null);

            int count = 1;
            if (xmlCookElement.ElementType == ElementType.TableCount)
            {
                elementName += "Count";
                count = _ReadInt32(parentNode, elementName);

                if (count == 0)
                {
                    parentNode.RemoveChild(parentNode.LastChild);
                }
            }

            for (int i = 0; i < count; i++)
            {
                XmlElement tableDesc = XmlDoc.CreateElement(xmlCookElement.Name);
                parentNode.AppendChild(tableDesc);

                XmlDefinition xmlCountDefinition = (XmlDefinition)Activator.CreateInstance(xmlCookElement.ChildType);
                Hashtable childElements = (Hashtable)elements[xmlCookElement.NameHash];

                _UncookXmlData(xmlCountDefinition, parentNode, childElements);
            }
        }

        private bool _ReadFlag(XmlNode parentNode, XmlDefinition xmlDefinition, XmlCookElement xmlCookElement)
        {
            int flagIndex = xmlCookElement.FlagId;
            if (xmlCookElement.ElementType == ElementType.Flag) flagIndex--;

            Debug.Assert(flagIndex >= 0);
            if (xmlDefinition.BitFields[flagIndex] == -1)
            {
                xmlDefinition.BitFields[flagIndex] = _ReadInt32(null, null);
            }

            bool flagged = false;
            switch (xmlCookElement.ElementType)
            {
                case ElementType.BitFlag:
                    flagged = (xmlDefinition.BitFields[flagIndex] & (1 << xmlCookElement.BitIndex)) > 0;
                    break;

                case ElementType.Flag:
                    flagged = (xmlDefinition.BitFields[flagIndex] & xmlCookElement.BitMask) > 0;
                    break;
            }

            if (parentNode != null)
            {
                XmlElement element = XmlDoc.CreateElement(xmlCookElement.Name);
                element.InnerText = flagged ? "1" : "0";

                parentNode.AppendChild(element);
            }

            return flagged;
        }

        private bool _ReadBitFlag(XmlNode parentNode, XmlDefinition xmlDefinition, XmlCookElement xmlCookElement)
        {
            if (xmlDefinition.NeedToReadBitFlags)
            {
                int intCount = xmlDefinition.BitFlags.Length;
                for (int i = 0; i < intCount; i++)
                {
                    xmlDefinition.BitFlags[i] = _ReadUInt32(null, null);
                }

                xmlDefinition.NeedToReadBitFlags = false;
            }

            int intIndex = xmlCookElement.BitIndex >> 5;
            int bitOffset = xmlCookElement.BitIndex - (intIndex << 5);
            bool flagged = (xmlDefinition.BitFlags[intIndex] & (1 << bitOffset)) > 0;

            if (parentNode != null)
            {
                XmlElement element = XmlDoc.CreateElement(xmlCookElement.Name);
                element.InnerText = flagged ? "1" : "0";

                parentNode.AppendChild(element);
            }

            return flagged;
        }

        private Int32 _ReadInt32(XmlNode parentNode, String elementName)
        {
            Int32 value = FileTools.ByteArrayToInt32(_data, ref _offset);

            if (parentNode != null && elementName != null)
            {
                XmlElement element = XmlDoc.CreateElement(elementName);
                element.InnerText = value.ToString();
                parentNode.AppendChild(element);
            }

            return value;
        }

        private bool _ReadBool32(XmlNode parentNode, String elementName)
        {
            bool value = _ReadInt32(null, null) != 0;

            if (parentNode != null && elementName != null)
            {
                XmlElement element = XmlDoc.CreateElement(elementName);
                element.InnerText = value.ToString();
                parentNode.AppendChild(element);
            }

            return value;
        }

        private UInt32 _ReadUInt32(XmlNode parentNode, String elementName)
        {
            UInt32 value = FileTools.ByteArrayToUInt32(_data, ref _offset);

            if (parentNode != null && elementName != null)
            {
                XmlElement element = XmlDoc.CreateElement(elementName);
                element.InnerText = value.ToString();
                parentNode.AppendChild(element);
            }

            return value;
        }

        private float _ReadFloat(XmlNode parentNode, String elementName)
        {
            float value = FileTools.ByteArrayToFloat(_data, ref _offset);

            // is the float value a negative zero?
            bool isNegativeZero = false;
            if (value == 0)
            {
                if (_TestBit(_data, _offset - 1, 7))
                {
                    isNegativeZero = true;
                }
            }

            if (parentNode != null && elementName != null)
            {
                XmlElement element = XmlDoc.CreateElement(elementName);
                element.InnerText = (isNegativeZero ? "-" : "") + value;
                parentNode.AppendChild(element);
            }

            return value;
        }

        private String _ReadZeroString(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            Int32 strLen = FileTools.ByteArrayToInt32(_data, ref _offset);
            Debug.Assert(strLen != 0);

            String value;
            if (xmlCookElement.TreatAsData && strLen > 0)
            {
                byte[] data = new byte[strLen];
                Buffer.BlockCopy(_data, _offset, data, 0, strLen);
                value = BitConverter.ToString(data);
            }
            else
            {
                value = FileTools.ByteArrayToStringASCII(_data, _offset);
            }

            _offset += strLen;

            if (parentNode != null)
            {
                XmlElement element = XmlDoc.CreateElement(xmlCookElement.Name);
                element.InnerText = value;
                parentNode.AppendChild(element);
            }

            return value;
        }

        private void _ReadTFloatArray(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            int count = _ReadInt32(null, null);
            Debug.Assert(count == 1); // not tested with anything other than 1 - not even sure if it's a count

            XmlElement countElement = XmlDoc.CreateElement(xmlCookElement.Name + "Count");
            countElement.InnerText = count.ToString();
            parentNode.AppendChild(countElement);

            for (int i = 0; i < count; i++)
            {
                for (int f = 0; f < 4; f++)
                {
                    _ReadFloat(parentNode, xmlCookElement.Name);
                }
            }
        }
    }
}
