using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using Hellgate.Excel;
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

                // Demo Levels
                new DemoLevelDefinition(),

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

                // Lights
                new LightDefinition(),

                // Materials (makes things look like things)
                new Material(),

                // Particles
                new ParticleSystemDefinition(),

                // Screen Effects
                new ScreenEffectDefinition(),

                // Skills (defines skill effect/appearance mostly, not so much the skill itself)
                new SkillEventsDefinition(),
                new SkillEventHolder(),
                new SkillEvent(),

                // Sound Effects
                new SoundEffectDefinition(),
                new SoundEffect(),

                // Sound Reverbs
                new SoundReverbDefinition(),
                new FmodReverbProperties(),

                // States
                new StateDefinition(),
                new StateEvent(),

                // Textures
                new TextureDefinition(),
                new BlendRLE(),
                new BlendRun()
            };

            // create hashes
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

        #region ReadFunctions
        private String _ReadByteString()
        {
            byte strLen = _buffer[_offset++];
            if (strLen == 0xFF || strLen == 0x00) return null;

            return FileTools.ByteArrayToStringASCII(_buffer, ref _offset, strLen);
        }

        private void _ReadExcelIndex(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            XmlElement element = XmlDoc.CreateElement(xmlCookElement.Name);
            String excelString = _ReadByteString();
            if (String.IsNullOrEmpty(excelString)) // the empty element is always written
            {
                parentNode.AppendChild(element);
                return;
            }

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

            element.InnerText = rowIndex.ToString();
            parentNode.AppendChild(element);
        }

        private void _ReadTable(XmlNode parentNode, XmlCookElement xmlCookElement, XmlCookedFileTree xmlTree)
        {
            String elementName = xmlCookElement.Name;
            Debug.Assert(xmlCookElement.ChildType != null);

            int count = 1;
            if (xmlCookElement.ElementType == ElementType.TableMultiple)
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

                _UncookXmlData(xmlTree.Definition, parentNode, xmlTree.TwinRoot);
            }
        }

        private bool _ReadFlag(XmlNode parentNode, XmlDefinition xmlDefinition, XmlCookElement xmlCookElement)
        {
            int flagIndex = xmlCookElement.FlagId;
            if (xmlCookElement.ElementType == ElementType.Flag) flagIndex--;

            Debug.Assert(flagIndex >= 0);
            if (xmlDefinition.BitFlags[flagIndex] == -1)
            {
                xmlDefinition.BitFlags[flagIndex] = _ReadInt32(null, null);
            }

            bool flagged = false;
            switch (xmlCookElement.ElementType)
            {
                case ElementType.BitFlag:
                    flagged = (xmlDefinition.BitFlags[flagIndex] & (1 << xmlCookElement.BitIndex)) > 0;
                    break;

                case ElementType.Flag:
                    flagged = (xmlDefinition.BitFlags[flagIndex] & xmlCookElement.BitMask) > 0;
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
                int intCount = xmlDefinition.Flags.Length;
                for (int i = 0; i < intCount; i++)
                {
                    xmlDefinition.Flags[i] = _ReadUInt32(null, null);
                }

                xmlDefinition.NeedToReadBitFlags = false;
            }

            int intIndex = xmlCookElement.BitIndex >> 5;
            int bitOffset = xmlCookElement.BitIndex - (intIndex << 5);
            bool flagged = (xmlDefinition.Flags[intIndex] & (1 << bitOffset)) > 0;

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
            Int32 value = FileTools.ByteArrayToInt32(_buffer, ref _offset);

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
            UInt32 value = FileTools.ByteArrayToUInt32(_buffer, ref _offset);

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
            float value = FileTools.ByteArrayToFloat(_buffer, ref _offset);

            // is the float value a negative zero?
            bool isNegativeZero = false;
            if (value == 0)
            {
                if (_TestBit(_buffer, _offset - 1, 7))
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

        private void _ReadFloatArrayVariable(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            int count = _ReadInt32(null, null);
            // Debug.Assert(count != 0);    // always written

            XmlElement countElement = XmlDoc.CreateElement(xmlCookElement.Name + "Count");
            countElement.InnerText = count.ToString();
            parentNode.AppendChild(countElement);

            for (int i = 0; i < count; i++)
            {
                _ReadFloat(parentNode, xmlCookElement.Name);
            }
        }

        private void _ReadFloatTripletArrayVariable(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            // todo: add default check (particle has defaults)
            _ReadFloatMultipleArrayVariable(parentNode, xmlCookElement, 3);

            //int count = _ReadInt32(null, null);
            //Debug.Assert(count != 0);

            //XmlElement countElement = XmlDoc.CreateElement(xmlCookElement.Name + "Count");
            //countElement.InnerText = count.ToString();
            //parentNode.AppendChild(countElement);

            //for (int i = 0; i < count; i++)
            //{
            //    float value1 = FileTools.ByteArrayToFloat(_buffer, ref _offset);
            //    float value2 = FileTools.ByteArrayToFloat(_buffer, ref _offset);
            //    float value3 = FileTools.ByteArrayToFloat(_buffer, ref _offset);

            //    XmlElement element = XmlDoc.CreateElement(xmlCookElement.Name);
            //    element.InnerText = String.Format("{0}, {1}, {2}", value1, value2, value3);
            //    parentNode.AppendChild(element);
            //}
        }

        private void _ReadFloatQuadArrayVariable(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            // todo: add default check (particle has defaults)
            _ReadFloatMultipleArrayVariable(parentNode, xmlCookElement, 4);

            //int count = _ReadInt32(null, null);
            //Debug.Assert(count == 1); // not seen with anything other than 1 - not even sure if it's a count

            //XmlElement countElement = XmlDoc.CreateElement(xmlCookElement.Name + "Count");
            //countElement.InnerText = count.ToString();
            //parentNode.AppendChild(countElement);

            //for (int i = 0; i < count; i++)
            //{
            //    for (int f = 0; f < 4; f++)
            //    {
            //        _ReadFloat(parentNode, xmlCookElement.Name);
            //    }
            //}
        }

        private void _ReadFloatMultipleArrayVariable(XmlNode parentNode, XmlCookElement xmlCookElement, int floatCount)
        {
            int count = _ReadInt32(null, null);
            Debug.Assert(count != 0);

            XmlElement countElement = XmlDoc.CreateElement(xmlCookElement.Name + "Count");
            countElement.InnerText = count.ToString();
            parentNode.AppendChild(countElement);

            for (int i = 0; i < count; i++)
            {
                String floatText = String.Empty;
                for (int f = 0; f < floatCount; f++)
                {
                    float fValue = FileTools.ByteArrayToFloat(_buffer, ref _offset);
                    floatText += fValue.ToString();
                    if (f < floatCount - 1) floatText += ", ";
                }

                XmlElement element = XmlDoc.CreateElement(xmlCookElement.Name);
                element.InnerText = floatText;
                parentNode.AppendChild(element);
            }
        }

        private String _ReadString(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            Int32 strLen = FileTools.ByteArrayToInt32(_buffer, ref _offset);
            Debug.Assert(strLen != 0);

            String value;

            byte b = _buffer[_offset];
            bool manualTreatAsData = ((b < 0x20 || b > 0x7F) && b != 0x00); // if not a valid string, then treat as data

            if ((xmlCookElement.TreatAsData && strLen > 0) || manualTreatAsData)
            {
                byte[] data = new byte[strLen];
                Buffer.BlockCopy(_buffer, _offset, data, 0, strLen);
                value = BitConverter.ToString(data);
            }
            else
            {
                value = strLen == 1 ? String.Empty : FileTools.ByteArrayToStringASCII(_buffer, _offset);
            }

            _offset += strLen;

            if (parentNode != null)
            {
                XmlElement element = XmlDoc.CreateElement(xmlCookElement.Name);
                element.InnerText = value;
                parentNode.AppendChild(element);

                if (manualTreatAsData) element.SetAttribute("manualTreatAsData", "true");
            }

            return value;
        }

        private void _ReadStringArray(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            int count = _ReadInt32(null, null);
            // Debug.Assert(count != 0); // always written?

            XmlElement countElement = XmlDoc.CreateElement(xmlCookElement.Name + "Count");
            countElement.InnerText = count.ToString();
            parentNode.AppendChild(countElement);

            for (int i = 0; i < count; i++)
            {
                _ReadString(parentNode, xmlCookElement);
            }
        }

        private int _ReadByteArray(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            Int32 byteCount = FileTools.ByteArrayToInt32(_buffer, ref _offset);
            // Debug.Assert(byteCount != 0); // always written?

            byte[] data = new byte[byteCount];
            Buffer.BlockCopy(_buffer, _offset, data, 0, byteCount);
            String value = BitConverter.ToString(data);

            _offset += byteCount;

            if (parentNode != null && !String.IsNullOrEmpty(value))
            {
                XmlElement element = XmlDoc.CreateElement(xmlCookElement.Name);
                element.InnerText = value;
                parentNode.AppendChild(element);
            }

            return byteCount;
        }
        #endregion

        #region WriteFunctions
        private void _WriteInt32(String elementText, XmlCookElement xmlCookElement)
        {
            Int32 intValue = Convert.ToInt32(elementText);
            if ((Int32)xmlCookElement.DefaultValue == intValue) return;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, intValue);
        }

        private void _WriteInt32ArrayFixed(XmlCookElement xmlCookElement, XmlNode xmlNode)
        {
            int dwArrayCount = xmlCookElement.Count;
            List<UInt32> dwElements = new List<UInt32>();
            bool dwAllDefault = true;
            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.Name != xmlCookElement.Name) continue;

                String arrayElementText = xmlChildNode.InnerText;
                UInt32 dwValue = UInt32.Parse(arrayElementText);
                dwElements.Add(dwValue);

                if ((UInt32)xmlCookElement.DefaultValue != dwValue)
                {
                    dwAllDefault = false;
                }

                if (dwElements.Count == dwArrayCount) break;
            }

            if (dwAllDefault) return;

            for (int i = 0; i < dwArrayCount; i++)
            {
                UInt32 dwWrite = (UInt32)xmlCookElement.DefaultValue;
                if (i < dwElements.Count)
                {
                    dwWrite = dwElements[i];
                }

                FileTools.WriteToBuffer(ref _buffer, ref _offset, dwWrite);
            }
        }

        private void _WriteFloat(String elementText, XmlCookElement xmlCookElement)
        {
            float floatValue = Convert.ToSingle(elementText);
            if ((float)xmlCookElement.DefaultValue == floatValue) return;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, floatValue);
        }

        private void _WriteFloatArrayFixed(XmlCookElement xmlCookElement, XmlNode xmlNode)
        {
            int arrayCount = xmlCookElement.Count;
            List<float> elements = new List<float>();
            bool allDefault = true;
            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.Name != xmlCookElement.Name) continue;

                String arrayElementText = xmlChildNode.InnerText;
                float fValue = Convert.ToSingle(arrayElementText);
                if (fValue == 0 && arrayElementText == "-0")
                    fValue = -1.0f * 0.0f;
                elements.Add(fValue);

                if ((float)xmlCookElement.DefaultValue != fValue)
                {
                    allDefault = false;
                }

                if (elements.Count == arrayCount) break;
            }

            if (allDefault) return;

            for (int i = 0; i < arrayCount; i++)
            {
                float fWrite = (float)xmlCookElement.DefaultValue;
                if (i < elements.Count)
                {
                    fWrite = elements[i];
                }

                FileTools.WriteToBuffer(ref _buffer, ref _offset, fWrite);
            }
        }

        private void _WriteFloatArrayVariable(XmlCookElement xmlCookElement, XmlNode xmlNode, String elementText)
        {
            int arrayCount = int.Parse(elementText);
            bool allDefault = true;
            List<float> elements = _GetArrayElementValues<float>(xmlCookElement, xmlNode, ref allDefault, arrayCount);
            //if (allDefault) return;       // appears to be written even if all default

            Int32 count = elements.Count;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, count);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, elements.ToArray().ToByteArray());
        }

        private void _WriteFloatTripletArrayVariable(String elementText, XmlCookElement xmlCookElement, XmlNode xmlNode)
        {
            // todo: add default check (particle has defaults)
            _WriteFloatMultipleArrayVariable(elementText, xmlCookElement, xmlNode, 3);

            //int arrayCount = int.Parse(elementText);
            //bool allDefault = true;
            //List<String> elements = _GetArrayElementValues<String>(xmlCookElement, xmlNode, ref allDefault, arrayCount, false);

            //Int32 count = elements.Count;
            //FileTools.WriteToBuffer(ref _buffer, ref _offset, count);

            //for (int i = 0; i < arrayCount && i < count; i++)
            //{
            //    String[] floatStrs = elements[i].Split(',');
            //    for (int j = 0; j < 3; j++)
            //    {
            //        float fValue = 0.0f;
            //        if (j < floatStrs.Length) fValue = float.Parse(floatStrs[j]);
            //        FileTools.WriteToBuffer(ref _buffer, ref _offset, fValue);
            //    }
            //}
        }

        private void _WriteFloatQuadArrayVariable(String elementText, XmlCookElement xmlCookElement, XmlNode xmlNode)
        {
            // todo: add default check (particle has defaults)
            _WriteFloatMultipleArrayVariable(elementText, xmlCookElement, xmlNode, 4);

            //Int32 count = Convert.ToInt32(elementText);
            //FileTools.WriteToBuffer(ref _buffer, ref _offset, count);

            //int floatTCount = xmlCookElement.Count;
            //List<float> floatTValues = new List<float>();
            //bool floatTAllDefault = true;

            //int totalFloatTCount = count * floatTCount;
            //foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            //{
            //    if (xmlChildNode.Name != xmlCookElement.Name) continue;

            //    String arrayElementText = xmlChildNode.InnerText;
            //    float fValue = Convert.ToSingle(arrayElementText);
            //    if (fValue == 0 && arrayElementText == "-0")
            //        fValue = -1.0f * 0.0f;
            //    floatTValues.Add(fValue);

            //    if ((float)xmlCookElement.DefaultValue != fValue)
            //    {
            //        floatTAllDefault = false;
            //    }

            //    if (floatTValues.Count == totalFloatTCount) break;
            //}

            //if (floatTAllDefault) return;

            //for (int i = 0; i < count; i++)
            //{
            //    for (int j = 0; j < floatTCount; j++)
            //    {
            //        float fWrite;

            //        int index = i * floatTCount + j;
            //        if (index < floatTValues.Count)
            //        {
            //            fWrite = floatTValues[index];
            //        }
            //        else
            //        {
            //            fWrite = (float)xmlCookElement.DefaultValue;
            //        }

            //        FileTools.WriteToBuffer(ref _buffer, ref _offset, fWrite);
            //    }
            //}
        }

        private void _WriteFloatMultipleArrayVariable(String elementText, XmlCookElement xmlCookElement, XmlNode xmlNode, int floatCount)
        {
            int arrayCount = int.Parse(elementText);
            bool allDefault = true;
            List<String> elements = _GetArrayElementValues<String>(xmlCookElement, xmlNode, ref allDefault, arrayCount, false);

            Int32 count = elements.Count;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, count);

            for (int i = 0; i < arrayCount && i < count; i++)
            {
                String[] floatStrs = elements[i].Split(',');
                for (int f = 0; f < floatCount; f++)
                {
                    float fValue = 0.0f;
                    if (f < floatStrs.Length) fValue = float.Parse(floatStrs[f]);
                    FileTools.WriteToBuffer(ref _buffer, ref _offset, fValue);
                }
            }
        }

        private void _WriteString(String elementText, XmlCookElement xmlCookElement, XmlElement xmlElement)
        {
            if ((String)xmlCookElement.DefaultValue == elementText) return;

            Int32 strLen = elementText.Length;
            if (strLen == 0 && xmlCookElement.DefaultValue == null)
            {
                const Int32 nullStrLen = 1; // 1 char = \0
                FileTools.WriteToBuffer(ref _buffer, ref _offset, nullStrLen);
                _offset++; // \0
                return;
            }

            bool manualTreatAsData = false;
            if (xmlElement != null && xmlElement.HasAttribute("manualTreatAsData"))
            {
                manualTreatAsData = true;
            }

            byte[] strBytes;
            if (xmlCookElement.TreatAsData || manualTreatAsData)
            {
                String[] dataStrBytes = elementText.Split('-');
                strLen = dataStrBytes.Length - 1; //+1 done down below

                strBytes = new byte[strLen];
                for (int i = 0; i < strLen; i++)
                {
                    strBytes[i] = Byte.Parse(dataStrBytes[i], System.Globalization.NumberStyles.HexNumber);
                }
            }
            else
            {
                strBytes = FileTools.StringToASCIIByteArray(elementText);
            }

            FileTools.WriteToBuffer(ref _buffer, ref _offset, strLen + 1); // \0
            FileTools.WriteToBuffer(ref _buffer, ref _offset, strBytes);

            _offset++; // \0
        }

        private void _WriteStringArrayFixed(XmlCookElement xmlCookElement, XmlNode xmlNode)
        {
            int arrayCount = xmlCookElement.Count;
            bool allDefault = true;
            List<String> strings = _GetArrayElementValues<String>(xmlCookElement, xmlNode, ref allDefault, arrayCount);
            if (allDefault) return;

            for (int i = 0; i < arrayCount && i < strings.Count; i++)
            {
                Int32 strLen = strings[i].Length;
                FileTools.WriteToBuffer(ref _buffer, ref _offset, strLen + 1); // \0
                FileTools.WriteToBuffer(ref _buffer, ref _offset, FileTools.StringToASCIIByteArray(strings[i]));
                _offset++; // \0
            }
        }

        private void _WriteStringArrayVariable(XmlCookElement xmlCookElement, XmlNode xmlNode, String elementText)
        {
            int arrayCount = int.Parse(elementText);
            List<XmlElement> elements = _GetArrayElements(xmlCookElement, xmlNode);

            Int32 elementCount = elements.Count;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, elementCount);

            for (int i = 0; i < arrayCount && i < elementCount; i++)
            {
                _WriteString(elements[i].InnerText, xmlCookElement, elements[i]);
            }
        }

        private void _WriteFlag(String elementText, XmlCookElement xmlCookElement, XmlDefinition xmlDefinition, Hashtable bitFieldOffsts)
        {
            // todo: fix up/remove hashtable
            bool flagged = elementText == "0" ? false : true;
            if ((bool)xmlCookElement.DefaultValue == flagged) return;

            int flagIndex = xmlCookElement.FlagId - 1;
            if (xmlCookElement.ElementType == ElementType.BitFlag) flagIndex = 0;

            // has it been written yet
            if (xmlDefinition.BitFlags[flagIndex] == -1 || bitFieldOffsts[flagIndex] == null)
            {
                bitFieldOffsts.Add(flagIndex, _offset);
                xmlDefinition.BitFlags[flagIndex] = (Int32)xmlDefinition.BitFlagsBaseMask;
                _offset += 4; // bit field bytes
            }


            UInt32 flag = (UInt32)xmlDefinition.BitFlags[flagIndex];
            if (xmlCookElement.ElementType == ElementType.Flag)
            {
                flag |= xmlCookElement.BitMask;
            }
            else
            {
                flag |= ((UInt32)1 << xmlCookElement.BitIndex);
            }

            int writeOffset = (int)bitFieldOffsts[flagIndex];
            FileTools.WriteToBuffer(ref _buffer, ref writeOffset, flag);
            xmlDefinition.BitFlags[flagIndex] = (Int32)flag;
        }

        private void _WriteBitFlag(String elementText, XmlCookElement xmlCookElement, XmlDefinition xmlDefinition)
        {
            bool bitFlagIsFlagged = elementText == "0" ? false : true;
            if ((bool)xmlCookElement.DefaultValue == bitFlagIsFlagged) return;

            if (xmlDefinition.BitFlagsWriteOffset == -1)
            {
                int intCount = xmlDefinition.Flags.Length;

                xmlDefinition.BitFlagsWriteOffset = _offset;
                _offset += intCount * sizeof(UInt32);
            }

            int intIndex = xmlCookElement.BitIndex >> 5;
            int bitFlagIndex = xmlCookElement.BitIndex - (intIndex << 5);
            UInt32 bitFlagField = xmlDefinition.Flags[intIndex] | ((UInt32)1 << bitFlagIndex);
            xmlDefinition.Flags[intIndex] = bitFlagField;

            int bitFlagWriteOffset = xmlDefinition.BitFlagsWriteOffset + (intIndex << 2);
            FileTools.WriteToBuffer(ref _buffer, ref bitFlagWriteOffset, bitFlagField);
        }

        private int _WriteTable(XmlCookElement xmlCookElement, XmlNode xmlElement)
        {
            XmlDefinition xmlTableDefinition = _GetXmlDefinition(xmlCookElement.ChildTypeHash);

            if (xmlElement == null) // ElementType.Table must be cooked even if null
            {
                return _CookXmlData(xmlTableDefinition, xmlElement);
            }

            XmlNode xmlTable = xmlElement.NextSibling;
            return _CookXmlData(xmlTableDefinition, xmlTable);
        }

        private int _WriteTableCount(String elementText, XmlCookElement xmlCookElement, XmlNode xmlNode)
        {
            int tableCount = String.IsNullOrEmpty(elementText) ? 0 : Convert.ToInt32(elementText);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, tableCount); // table count is always written
            if (tableCount == 0) return 0;

            XmlDefinition xmlTableCountDefinition = _GetXmlDefinition(xmlCookElement.ChildTypeHash);
            int tablesAdded = 0;
            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.Name != xmlTableCountDefinition.RootElement) continue;

                if (_CookXmlData(xmlTableCountDefinition, xmlChildNode) == -1) return -1;
                tablesAdded++;

                if (tableCount == tablesAdded) break;
            }

            if (tablesAdded < tableCount) return -1;
            return tablesAdded;
        }

        private void _WriteExcelIndex(String elementText, XmlCookElement xmlCookElement)
        {
            String excelString = String.Empty;
            byte byteLen = (byte)elementText.Length;
            if (byteLen > 0)
            {
                int index = int.Parse(elementText);
                excelString = _fileManager.GetExcelRowStringFromIndex(xmlCookElement.ExcelTableCode, index);

                if (String.IsNullOrEmpty(excelString))
                {
                    byteLen = 0; // not found
                }
                else
                {
                    byteLen = (byte)excelString.Length;
                }
            }

            if (byteLen == 0)
            {
                byteLen = 0xFF;
                FileTools.WriteToBuffer(ref _buffer, ref _offset, byteLen);
            }
            else
            {
                FileTools.WriteToBuffer(ref _buffer, ref _offset, byteLen);
                byte[] stringBytes = FileTools.StringToASCIIByteArray(excelString);
                FileTools.WriteToBuffer(ref _buffer, ref _offset, stringBytes);
                // no \0
            }
        }

        private void _WriteExcelIndexArrayFixed(XmlCookElement xmlCookElement, XmlNode xmlNode)
        {
            int arrayCount = xmlCookElement.Count;
            bool allDefault = true;
            List<String> elements = _GetArrayElementValues<String>(xmlCookElement, xmlNode, ref allDefault, arrayCount);

            for (int i = 0; i < arrayCount; i++)
            {
                String elementText = i < elements.Count ? elements[i] : String.Empty;
                _WriteExcelIndex(elementText, xmlCookElement);
            }
        }

        private void _WriteByteArrayVariable(String elementText, XmlCookElement xmlCookElement)
        {
            Int32 length = elementText.Length;
            if (length == 0 && (Int32)xmlCookElement.DefaultValue == 0) // always need count written
            {
                FileTools.WriteToBuffer(ref _buffer, ref _offset, length);
                return;
            }

            String[] strBytes = elementText.Split('-');
            length = strBytes.Length;

            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                bytes[i] = Byte.Parse(strBytes[i], System.Globalization.NumberStyles.HexNumber);
            }

            FileTools.WriteToBuffer(ref _buffer, ref _offset, length);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, bytes);
        }

        private static List<T> _GetArrayElementValues<T>(XmlCookElement xmlCookElement, XmlNode xmlNode, ref bool allDefault, int arrayCount, bool checkDefault = true)
        {
            List<T> elements = new List<T>();

            allDefault = true;
            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.Name != xmlCookElement.Name) continue;

                String arrayElementText = xmlChildNode.InnerText;

                Object elementValue = null;
                if (typeof(T) == typeof(String))
                {
                    elementValue = arrayElementText;
                }
                else if (typeof(T) == typeof(float))
                {
                    float fValue = Convert.ToSingle(arrayElementText);
                    if (fValue == 0 && arrayElementText == "-0")
                        fValue = -1.0f * 0.0f;
                    elementValue = fValue;
                }
                else
                {
                    Debug.Assert(false, "NotImplemented Type: _GetArrayElements<T>");
                }
                elements.Add((T)elementValue);

                if (checkDefault && allDefault && !elementValue.Equals((T)xmlCookElement.DefaultValue)) allDefault = false;

                if (elements.Count == arrayCount) break;
            }

            return elements;
        }

        private static List<XmlElement> _GetArrayElements(XmlCookElement xmlCookElement, XmlNode xmlNode)
        {
            List<XmlElement> elements = new List<XmlElement>();

            foreach (XmlElement xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.Name != xmlCookElement.Name) continue;

                elements.Add(xmlChildNode);
            }

            return elements;
        }
        #endregion
    }
}
