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

        private struct ElementStrings
        {
            public const String Config = "XMLConfig";
            public const String Defaults = "Defaults";
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
                //// SP Definitions
                // AI
                new XmlAIDefinition(),
                new XmlAIBehaviorDefinitionTable(),
                new XmlAIBehaviorDefinition(),

                // Appearance
                new XmlAppearanceDefinition(),
                new XmlAnimationDefinition(),
                new XmlAnimEvent(),
                new XmlInventoryViewInfo(),

                // Colorsets (colorsets.xml)
                new XmlColorSetDefinition(),
                new XmlColorDefinition(),

                // Config (config.xml)
                new XmlConfigDefinition(),

                // Demo Levels
                new XmlDemoLevelDefinition(),

                // Environments
                new XmlEnvironmentDefinition(),
                new XmlEnvLightDefinition(),

                // GameGlobalDefinition (gamedefault.xml)
                new XmlGameGlobalDefinition(),

                // GlobalDefinition (default.xml)
                new XmlGlobalDefinition(),

                // Shared (used in States, Skills and Appearance)
                new XmlConditionDefinition(),

                // Level Layout (contains object positions etc)
                new XmlRoomLayoutGroupDefinition(),
                new XmlRoomLayoutGroup(),

                // Level Pathing (huge-ass list of nodes/points)
                new XmlRoomPathNodeDefinition(),
                new XmlRoomPathNodeSet(),
                new XmlRoomPathNode(),
                new XmlRoomPathNodeConnection(),
                new XmlRoomPathNodeConnectionRef(),

                // Lights
                new XmlLightDefinition(),

                // Materials (makes things look like things)
                new XmlMaterial(),

                // Particles
                new XmlParticleSystemDefinition(),

                // Screen Effects
                new XmlScreenEffectDefinition(),

                // Skills (defines skill effect/appearance mostly, not so much the skill itself)
                new XmlSkillEventsDefinition(),
                new XmlSkillEventHolder(),
                new XmlSkillEvent(),

                // Skybox
                new XmlSkyboxDefinition(),
                new XmlSkyboxModel(),

                // Sound Effects
                new XmlSoundEffectDefinition(),
                new XmlSoundEffect(),

                // Sound Reverbs
                new XmlSoundReverbDefinition(),
                new XmlFmodReverbProperties(),

                // States
                new XmlStateDefinition(),
                new XmlStateEvent(),

                // Textures
                new XmlTextureDefinition(),
                new XmlBlendRLE(),
                new XmlBlendRun(),

                //// MP Definitions
                // Sound Address Envelope
                new XmlSoundAdsrEnvelope()
            };

            // create hashes
            foreach (XmlDefinition xmlDefinition in _xmlDefinitions)
            {
                xmlDefinition.RootHash = Crypt.GetStringHash(xmlDefinition.RootElement);

                foreach (XmlCookElement xmlCookElement in xmlDefinition.Elements)
                {
                    if (xmlCookElement.HashOverride != 0x00)
                    {
                        xmlCookElement.NameHash = xmlCookElement.HashOverride;
                        continue;
                    }

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
            if (strLen == 0xFF) return null;
            if (strLen == 0x00) return String.Empty;

            return FileTools.ByteArrayToStringASCII(_buffer, ref _offset, strLen);
        }

        private void _ReadExcelIndex(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            String excelString = _ReadByteString();
            if (excelString == null || parentNode == null) return;

            // get excel table index
            int rowIndex = _fileManager.GetExcelRowIndex(xmlCookElement.ExcelTableCode, excelString);
            if (rowIndex == -1)
            {
                if (ExcelStringsMissing == null) ExcelStringsMissing = new HashSet<String>();
                if (!ExcelStringsMissing.Contains(excelString)) ExcelStringsMissing.Add(excelString);
                
                if (ThrowOnMissingExcelString) throw new Exceptions.UnknownExcelStringException(excelString);
                Console.WriteLine("Warning: Inaccurate uncook - Unknown ExcelString = " + excelString);
            }

            XmlNode grandParentNode = parentNode.ParentNode;
            XmlNode descriptionNode = grandParentNode.LastChild.PreviousSibling;

            if (descriptionNode != null)
            {
                if (!String.IsNullOrEmpty(descriptionNode.InnerText)) descriptionNode.InnerText += ", ";
                descriptionNode.InnerText += excelString;
            }

            _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name, rowIndex.ToString());
        }

        private void _ReadTable(XmlNode parentNode, XmlCookElement xmlCookElement, XmlCookedFileTree xmlTree)
        {
            Debug.Assert(xmlCookElement.ChildType != null);

            int count = 1;
            if (xmlCookElement.ElementType == ElementType.TableArrayVariable)
            {
                count = _ReadInt32(null, null);
                _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name + "Count", count.ToString());

                if (count == 0) parentNode.RemoveChild(parentNode.LastChild);
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
            if (xmlDefinition.Flags[flagIndex] == -1)
            {                                               // (this extra if() block is here for the extra comment space)
                if (_offset == _buffer.Length)              // in a few MP files, the flags are at the end of the file and for some reason the flag bytes
                {                                           // aren't actually saved in the file (e.g. thirdpersononly.x.c and dizzy_reverb)
                    xmlDefinition.Flags[flagIndex] = -1;    // but looking and comparing with similar xml files says they should be high
                }                                           // so easy enough to just set all to high and whatever is high in the bitfield will be high
                else
                {
                    xmlDefinition.Flags[flagIndex] = _ReadInt32(null, null);
                }
                
            }

            bool flagged = false;
            switch (xmlCookElement.ElementType)
            {
                case ElementType.BitFlag:
                    flagged = (xmlDefinition.Flags[flagIndex] & (1 << xmlCookElement.BitFlagIndex)) > 0;
                    break;

                case ElementType.Flag:
                    flagged = (xmlDefinition.Flags[flagIndex] & xmlCookElement.FlagMask) > 0;
                    break;
            }

            if (parentNode != null)
            {
                String innerText = flagged ? "1" : "0";
                _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name, innerText);
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
                    if (_offset == _buffer.Length)
                    {
                        xmlDefinition.BitFlags[i] = 0xFFFFFFFF; // -1       // see _ReadFlag for why 
                    }
                    else
                    {
                        xmlDefinition.BitFlags[i] = _ReadUInt32(null, null);
                    }
                }

                xmlDefinition.NeedToReadBitFlags = false;
            }

            int intIndex = xmlCookElement.BitFlagIndex >> 5;
            int bitOffset = xmlCookElement.BitFlagIndex - (intIndex << 5);
            bool flagged = (xmlDefinition.BitFlags[intIndex] & (1 << bitOffset)) > 0;

            if (parentNode != null)
            {
                String innerText = flagged ? "1" : "0";
                _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name, innerText);
            }

            return flagged;
        }

        private Int32 _ReadInt32(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            if (xmlCookElement != null && xmlCookElement.DefaultValue.GetType() == typeof(UInt32))
            {
                UInt32 uInt32Value = FileTools.ByteArrayToUInt32(_buffer, ref _offset);
                _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name, uInt32Value.ToString());
                return (Int32) uInt32Value;
            }

            Int32 int32Value = FileTools.ByteArrayToInt32(_buffer, ref _offset);

            if (parentNode != null && xmlCookElement != null)
            {
                _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name, int32Value.ToString());
            }

            return int32Value;
        }

        private void _ReadInt32ArrayVariable(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            int count = _ReadInt32(null, null);
            _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name + "Count", count.ToString());

            for (int i = 0; i < count; i++)
            {
                _ReadInt32(parentNode, xmlCookElement);
            }
        }

        private bool _ReadBool32(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            bool value = _ReadInt32(null, null) != 0;

            if (parentNode != null && xmlCookElement != null)
            {
                _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name, value.ToString());
            }

            return value;
        }

        private UInt32 _ReadUInt32(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            //if (_offset >= _buffer.Length) return 0;

            UInt32 value = FileTools.ByteArrayToUInt32(_buffer, ref _offset);

            if (parentNode != null && xmlCookElement != null)
            {
                _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name, value.ToString());
            }

            return value;
        }

        private float _ReadFloat(XmlNode parentNode, XmlCookElement xmlCookElement)
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

            if (parentNode != null && xmlCookElement != null)
            {
                String innerText = (isNegativeZero ? "-" : "") + value.ToString("r");
                _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name, innerText);
            }

            return value;
        }

        private void _ReadFloatArrayVariable(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            int count = _ReadInt32(null, null);
            _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name + "Count", count.ToString());

            for (int i = 0; i < count; i++)
            {
                _ReadFloat(parentNode, xmlCookElement);
            }
        }

        private void _ReadFloatTripletArrayVariable(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            // todo: add default check (particle has defaults)
            _ReadFloatMultipleArrayVariable(parentNode, xmlCookElement, 3);
        }

        private void _ReadFloatQuadArrayVariable(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            // todo: add default check (particle has defaults)
            _ReadFloatMultipleArrayVariable(parentNode, xmlCookElement, 4);
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
                    floatText += fValue.ToString("r");
                    if (f < floatCount - 1) floatText += ", ";
                }

                _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name, floatText);
            }
        }

        private String _ReadString(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            Int32 strLen = FileTools.ByteArrayToInt32(_buffer, ref _offset);
            Debug.Assert(strLen != 0);

            String value;

            byte b = _buffer[_offset];

            // todo: check http://msdn.microsoft.com/en-us/library/system.text.encoding.ascii.aspx
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
                XmlElement element = _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name, value);
                if (manualTreatAsData) element.SetAttribute("manualTreatAsData", "true");
            }

            return value;
        }

        private void _ReadStringArrayFixed(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            _ReadStringArray(parentNode, xmlCookElement, xmlCookElement.Count);
        }

        private void _ReadStringArrayVariable(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            int count = _ReadInt32(null, null);
            _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name + "Count", count.ToString());

            _ReadStringArray(parentNode, xmlCookElement, count);
        }

        private void _ReadStringArray(XmlNode parentNode, XmlCookElement xmlCookElement, int count)
        {
            for (int i = 0; i < count; i++)
            {
                _ReadString(parentNode, xmlCookElement);
            }
        }

        private int _ReadByteArray(XmlNode parentNode, XmlCookElement xmlCookElement)
        {
            Int32 byteCount = FileTools.ByteArrayToInt32(_buffer, ref _offset);
            byte[] data = new byte[byteCount];
            Buffer.BlockCopy(_buffer, _offset, data, 0, byteCount);
            String value = BitConverter.ToString(data);

            _offset += byteCount;

            if (parentNode != null && !String.IsNullOrEmpty(value))
            {
                _AddChildElement(parentNode, xmlCookElement, xmlCookElement.Name, value);
            }

            return byteCount;
        }

        private XmlElement _AddChildElement(XmlNode parentNode, XmlCookElement xmlCookElement, String elementName, String innerText)
        {
            if (parentNode == null) return null;

            XmlElement element = XmlDoc.CreateElement(elementName);
            if (innerText != null) element.InnerText = innerText;
            parentNode.AppendChild(element);

            if (xmlCookElement != null && xmlCookElement.IsTestCentre)
            {
                element.SetAttribute("IsTCv4", "true");    
            }

            return element;
        }
        #endregion

        #region WriteFunctions
        private void _WriteInt322(String elementText, XmlCookedObject.XmlElement xmlCookElement)
        {
            if (xmlCookElement.XmlAttribute.CustomType == ElementType.Bool)
            {
                // check if it's using the old "int format" (0 = false, 1 = true)
                Int32 boolIntValue;
                if (Int32.TryParse(elementText, out boolIntValue))
                {
                    bool boolIntBool = (boolIntValue == 0) ? false : true;
                    if ((bool)xmlCookElement.Default == boolIntBool) return;
                    FileTools.WriteToBuffer(ref _buffer, ref _offset, boolIntBool ? 1 : 0); // we do the double handling to have 0 as false, and != 0 is true (C-style)
                    return;
                }

                // now try as bool type - we try this last to have Convert.ToBool throw an exception to let the user know they have an invalid element
                bool bValue = bool.Parse(elementText);
                if ((bool)xmlCookElement.Default == bValue) return;
                FileTools.WriteToBuffer(ref _buffer, ref _offset, bValue ? 1 : 0);
            }
            else if (xmlCookElement.XmlAttribute.CustomType == ElementType.Unsigned)
            {
                Int64 int64Value = Convert.ToInt64(elementText);
                //Int32 intValue = Convert.ToInt32(elementText);
                UInt32 uintValue = (UInt32)int64Value; // -1 = 0xFFFFFF, but Convert.ToUInt32(-1) throws exception
                if ((UInt32)xmlCookElement.Default == uintValue) return;
                FileTools.WriteToBuffer(ref _buffer, ref _offset, uintValue);
            }
            else
            {
                Int32 intValue = Convert.ToInt32(elementText);
                if ((Int32)xmlCookElement.Default == intValue) return;
                FileTools.WriteToBuffer(ref _buffer, ref _offset, intValue);
            }
        }

        private void _WriteInt32ArrayFixed2<T>(XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode)
        {
            int dwArrayCount = xmlCookElement.XmlAttribute.Count;
            List<T> dwElements = new List<T>();
            bool dwAllDefault = true;
            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.Name != xmlCookElement.Name) continue;

                String arrayElementText = xmlChildNode.InnerText;
                object dwValue = UInt32.Parse(arrayElementText);
                dwElements.Add((T)dwValue);

                if (!xmlCookElement.Default.Equals(dwValue)) dwAllDefault = false;
                if (dwElements.Count == dwArrayCount) break;
            }

            if (dwAllDefault) return;

            for (int i = 0; i < dwArrayCount; i++)
            {
                T dwWrite = (T)xmlCookElement.Default;
                if (i < dwElements.Count)
                {
                    dwWrite = dwElements[i];
                }

                FileTools.WriteToBuffer(ref _buffer, ref _offset, dwWrite);
            }
        }

        private void _WriteInt32ArrayVariable2(XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode, String elementText)
        {
            int arrayCount = int.Parse(elementText);
            bool allDefault = true;
            List<Int32> elements = _GetArrayElementValues2<Int32>(xmlCookElement, xmlNode, ref allDefault, arrayCount);
            //if (allDefault) return;       // written even if all default

            Int32 count = elements.Count;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, count);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, elements.ToArray().ToByteArray());
        }

        private void _WriteFloat2(String elementText, XmlCookedObject.XmlElement xmlCookElement)
        {
            float floatValue = Convert.ToSingle(elementText);
            if (floatValue == 0 && elementText.Contains("-0")) floatValue = -1.0f * 0.0f;
            if ((float)xmlCookElement.Default == floatValue) return;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, floatValue);
        }

        private void _WriteFloatArrayFixed2(XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode)
        {
            int arrayCount = xmlCookElement.XmlAttribute.Count;
            List<float> elements = new List<float>();
            bool allDefault = true;
            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.Name != xmlCookElement.Name) continue;

                String arrayElementText = xmlChildNode.InnerText;
                float fValue = Convert.ToSingle(arrayElementText);
                if (fValue == 0 && arrayElementText.Contains("-0")) fValue = -1.0f * 0.0f;
                elements.Add(fValue);

                if ((float)xmlCookElement.Default != fValue)
                {
                    allDefault = false;
                }

                if (elements.Count == arrayCount) break;
            }

            if (allDefault) return;

            for (int i = 0; i < arrayCount; i++)
            {
                float fWrite = (float)xmlCookElement.Default;
                if (i < elements.Count)
                {
                    fWrite = elements[i];
                }

                FileTools.WriteToBuffer(ref _buffer, ref _offset, fWrite);
            }
        }

        private void _WriteFloatArrayVariable2(XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode, String elementText)
        {
            int arrayCount = int.Parse(elementText);
            bool allDefault = true;
            List<float> elements = _GetArrayElementValues2<float>(xmlCookElement, xmlNode, ref allDefault, arrayCount);
            //if (allDefault) return;       // written even if all default

            Int32 count = elements.Count;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, count);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, elements.ToArray().ToByteArray());
        }

        private void _WriteFloatTripletArrayVariable2(String elementText, XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode)
        {
            // todo: add default check (particle has defaults)
            _WriteFloatMultipleArrayVariable2(elementText, xmlCookElement, xmlNode, 3);
        }

        private void _WriteFloatQuadArrayVariable2(String elementText, XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode)
        {
            // todo: add default check (particle has defaults)
            _WriteFloatMultipleArrayVariable2(elementText, xmlCookElement, xmlNode, 4);
        }

        private void _WriteFloatMultipleArrayVariable2(String elementText, XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode, int floatCount)
        {
            int arrayCount = int.Parse(elementText);
            bool allDefault = true;
            List<String> elements = _GetArrayElementValues2<String>(xmlCookElement, xmlNode, ref allDefault, arrayCount, false);

            Int32 count = elements.Count;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, count);

            for (int i = 0; i < arrayCount && i < count; i++)
            {
                String[] floatStrs = elements[i].Split(',');
                for (int f = 0; f < floatCount; f++)
                {
                    float fValue = 0.0f;
                    if (f < floatStrs.Length) fValue = float.Parse(floatStrs[f]);
                    if (fValue == 0 && floatStrs[f].Contains("-0")) fValue = -1.0f * 0.0f;
                    FileTools.WriteToBuffer(ref _buffer, ref _offset, fValue);
                }
            }
        }

        private void _WriteString2(String elementText, XmlCookedObject.XmlElement xmlCookElement, XmlElement xmlElement)
        {
            // if ((String)xmlCookElement.Default == elementText) return; tm_armor_heavy_fashion_body_appearance has defaults written

            Int32 strLen = elementText.Length;
            if (strLen == 0 && xmlCookElement.Default == null)
            {
                const Int32 nullStrLen = 1; // 1 char = \0
                FileTools.WriteToBuffer(ref _buffer, ref _offset, nullStrLen);
                _offset++; // \0
                return;
            }

            bool overrideTypeAsByteArray = false;
            if (xmlElement != null && (xmlElement.HasAttribute("overrideTypeAsByteArray") || xmlElement.HasAttribute("manualTreatAsData") /*backwards compatibility*/))
            {
                overrideTypeAsByteArray = true;
            }

            byte[] strBytes;
            if (xmlCookElement.XmlAttribute.IsByteArray || overrideTypeAsByteArray)
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

        private void _WriteStringArrayFixed2(XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode)
        {
            int arrayCount = xmlCookElement.XmlAttribute.Count;
            bool allDefault = true;
            List<String> strings = _GetArrayElementValues2<String>(xmlCookElement, xmlNode, ref allDefault, arrayCount);
            if (allDefault) return;

            for (int i = 0; i < arrayCount && i < strings.Count; i++)
            {
                Int32 strLen = strings[i].Length;
                FileTools.WriteToBuffer(ref _buffer, ref _offset, strLen + 1); // \0
                FileTools.WriteToBuffer(ref _buffer, ref _offset, FileTools.StringToASCIIByteArray(strings[i]));
                _offset++; // \0
            }
        }

        private void _WriteStringArrayVariable2(XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode, String elementText)
        {
            int arrayCount = int.Parse(elementText);
            List<System.Xml.XmlElement> elements = _GetArrayElements2(xmlCookElement, xmlNode);

            Int32 elementCount = elements.Count;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, elementCount);

            for (int i = 0; i < arrayCount && i < elementCount; i++)
            {
                _WriteString2(elements[i].InnerText, xmlCookElement, elements[i]);
            }
        }

        private void _WriteFlag2(String elementText, XmlCookedAttribute xmlAttribute, IList<int> flagOffsets)
        {
            // create the initial flag offset if first time
            int flagIndex = xmlAttribute.FlagId - 1;
            if (flagIndex < 0) flagIndex = 0;
            if (flagOffsets[flagIndex] == -1)
            {
                flagOffsets[flagIndex] = _offset;

                if (xmlAttribute.ElementType == ElementType.Flag)
                {
                    _offset += sizeof(Int32);
                }
                else
                {
                    _offset += ((xmlAttribute.BitFlagCount >> 5) + 1) * sizeof(Int32);
                }
            }

            // determine bit index
            int bitIndex = 0;
            if (xmlAttribute.ElementType == ElementType.Flag)
            {
                uint flagBitIndex = xmlAttribute.FlagMask;
                for (; flagBitIndex > 1; flagBitIndex >>= 1, bitIndex++) { }
            }
            else
            {
                bitIndex = xmlAttribute.BitFlagIndex;
            }

            // determine byte index and flagged state
            int byteIndex = bitIndex >> 3;
            int offset = flagOffsets[flagIndex] + byteIndex;
            bool flagged = (elementText == "0") ? false : true;
            bitIndex -= byteIndex * 8;

            // make sure we aren't out of bounds of the _buffer
            if (offset >= _buffer.Length) // the chance of this happening is pretty damn slim, but actually happens in road_ic_layout.xml.cooked, lol
            {
                FileTools.WriteToBuffer(ref _buffer, offset, (byte)0); // write 0 byte to force buffer size increase
            }

            // apply flag state
            if (flagged)
            {
                _buffer[offset] |= (byte)(1 << bitIndex);
            }
            else
            {
                _buffer[offset] ^= (byte)((1 << bitIndex) & 0xFF);
            }
        }

        private int _WriteTable2(XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlElement)
        {
            XmlCookedObject.XmlDefinition xmlTableDefinition = XmlCookedObject.GetXmlDefinition(xmlCookElement.XmlAttribute.ChildTypeHash);

            if (xmlElement == null) // ElementType.Table must be cooked even if null
            {
                return _CookXmlData2(xmlTableDefinition, xmlElement);
            }

            XmlNode xmlTable = xmlElement.NextSibling;
            return _CookXmlData2(xmlTableDefinition, xmlTable);
        }

        private int _WriteTableArrayFixed2(XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode)
        {
            return _WriteTableArray2(xmlCookElement, xmlNode, xmlCookElement.XmlAttribute.Count);
        }

        private int _WriteTableArrayVariable2(String elementText, XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode)
        {
            int tableCount = String.IsNullOrEmpty(elementText) ? 0 : Convert.ToInt32(elementText);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, tableCount); // table count is always written
            if (xmlNode == null) return 0;

            return _WriteTableArray2(xmlCookElement, xmlNode.NextSibling, tableCount);
        }

        private int _WriteTableArray2(XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode, int tableCount)
        {
            XmlCookedObject.XmlDefinition xmlTableCountDefinition = XmlCookedObject.GetXmlDefinition(xmlCookElement.XmlAttribute.ChildTypeHash);
            int tablesAdded = 0;

            for (int i = 0; i < tableCount; i++)
            {
                XmlNode tableNode = xmlNode.NextSibling;
                if (tableNode == null || tableNode.Name != xmlTableCountDefinition.RootElement.Name) return -1;

                if (_CookXmlData2(xmlTableCountDefinition, tableNode) == -1) return -1;
                tablesAdded++;

                xmlNode = tableNode.NextSibling;
                if (xmlNode == null) break;
            }

            if (tablesAdded < tableCount) return -1;
            return tablesAdded;
        }

        private void _WriteExcelIndex2(String elementText, XmlCookedObject.XmlElement xmlCookElement)
        {
            byte byteLen = (byte)elementText.Length;
            // if no chars, then we obviously have no excel index
            if (byteLen == 0x00)
            {
                FileTools.WriteToBuffer(ref _buffer, ref _offset, (byte)0xFF);
                return;
            }

            String excelString = null;
            try
            {
                int index = int.Parse(elementText);
                excelString = _fileManager.GetExcelRowStringFromRowIndex(xmlCookElement.XmlAttribute.TableCode, index);
            }
            catch (Exception)
            {
                Console.WriteLine("Warning: Failed to get excel string for index: " + elementText);
            }

            // if null string, then we had an invalid index
            // if string is empty, we still want to write 0x00 (below) as there are some excel rows with empty strings
            if (excelString == null)
            {
                FileTools.WriteToBuffer(ref _buffer, ref _offset, (byte)0xFF);
                return;
            }

            byteLen = (byte)excelString.Length;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, byteLen);
            byte[] stringBytes = FileTools.StringToASCIIByteArray(excelString);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, stringBytes);
            // no \0
        }

        private void _WriteExcelIndexArrayFixed2(XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode)
        {
            int arrayCount = xmlCookElement.XmlAttribute.Count;
            bool allDefault = true;
            List<String> elements = _GetArrayElementValues2<String>(xmlCookElement, xmlNode, ref allDefault, arrayCount);

            for (int i = 0; i < arrayCount; i++)
            {
                String elementText = i < elements.Count ? elements[i] : String.Empty;
                _WriteExcelIndex2(elementText, xmlCookElement);
            }
        }

        private void _WriteByteArrayVariable2(String elementText, XmlCookedObject.XmlElement xmlCookElement)
        {
            Int32 length = elementText.Length;
            if (length == 0 && (Int32)xmlCookElement.Default == 0) // always need count written
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

        private static List<T> _GetArrayElementValues2<T>(XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode, ref bool allDefault, int arrayCount, bool checkDefault = true)
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
                else if (typeof(T) == typeof(Int32))
                {
                    elementValue = Convert.ToInt32(arrayElementText);
                }
                else
                {
                    Debug.Assert(false, "NotImplemented Type: _GetArrayElements<T>");
                }
                elements.Add((T)elementValue);

                if (checkDefault && allDefault && !elementValue.Equals((T)xmlCookElement.Default)) allDefault = false;

                if (elements.Count == arrayCount) break;
            }

            return elements;
        }

        private static List<System.Xml.XmlElement> _GetArrayElements2(XmlCookedObject.XmlElement xmlCookElement, XmlNode xmlNode)
        {
            List<System.Xml.XmlElement> elements = new List<System.Xml.XmlElement>();

            foreach (System.Xml.XmlElement xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.Name != xmlCookElement.Name) continue;

                elements.Add(xmlChildNode);
            }

            return elements;
        }










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

        private void _WriteInt32ArrayVariable(XmlCookElement xmlCookElement, XmlNode xmlNode, String elementText)
        {
            int arrayCount = int.Parse(elementText);
            bool allDefault = true;
            List<Int32> elements = _GetArrayElementValues<Int32>(xmlCookElement, xmlNode, ref allDefault, arrayCount);
            //if (allDefault) return;       // written even if all default

            Int32 count = elements.Count;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, count);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, elements.ToArray().ToByteArray());
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
            //if (allDefault) return;       // written even if all default

            Int32 count = elements.Count;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, count);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, elements.ToArray().ToByteArray());
        }

        private void _WriteFloatTripletArrayVariable(String elementText, XmlCookElement xmlCookElement, XmlNode xmlNode)
        {
            // todo: add default check (particle has defaults)
            _WriteFloatMultipleArrayVariable(elementText, xmlCookElement, xmlNode, 3);
        }

        private void _WriteFloatQuadArrayVariable(String elementText, XmlCookElement xmlCookElement, XmlNode xmlNode)
        {
            // todo: add default check (particle has defaults)
            _WriteFloatMultipleArrayVariable(elementText, xmlCookElement, xmlNode, 4);
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
            if (xmlDefinition.Flags[flagIndex] == -1 || bitFieldOffsts[flagIndex] == null)
            {
                bitFieldOffsts.Add(flagIndex, _offset);
                xmlDefinition.Flags[flagIndex] = (Int32)xmlDefinition.FlagsBaseMask;
                _offset += 4; // bit field bytes
            }


            UInt32 flag = (UInt32)xmlDefinition.Flags[flagIndex];
            if (xmlCookElement.ElementType == ElementType.Flag)
            {
                flag |= xmlCookElement.FlagMask;
            }
            else
            {
                flag |= ((UInt32)1 << xmlCookElement.BitFlagIndex);
            }

            int writeOffset = (int)bitFieldOffsts[flagIndex];
            FileTools.WriteToBuffer(ref _buffer, ref writeOffset, flag);
            xmlDefinition.Flags[flagIndex] = (Int32)flag;
        }

        private void _WriteBitFlag(String elementText, XmlCookElement xmlCookElement, XmlDefinition xmlDefinition)
        {
            bool bitFlagIsFlagged = elementText == "0" ? false : true;
            if ((bool)xmlCookElement.DefaultValue == bitFlagIsFlagged) return;

            if (xmlDefinition.BitFlagsWriteOffset == -1)
            {
                int intCount = xmlDefinition.BitFlags.Length;

                xmlDefinition.BitFlagsWriteOffset = _offset;
                _offset += intCount * sizeof(UInt32);
            }

            int intIndex = xmlCookElement.BitFlagIndex >> 5;
            int bitFlagIndex = xmlCookElement.BitFlagIndex - (intIndex << 5);
            UInt32 bitFlagField = xmlDefinition.BitFlags[intIndex] | ((UInt32)1 << bitFlagIndex);
            xmlDefinition.BitFlags[intIndex] = bitFlagField;

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

        private int _WriteTableArrayFixed(XmlCookElement xmlCookElement, XmlNode xmlNode)
        {
            return _WriteTableArray(xmlCookElement, xmlNode, xmlCookElement.Count);
        }

        private int _WriteTableArrayVariable(String elementText, XmlCookElement xmlCookElement, XmlNode xmlNode)
        {
            int tableCount = String.IsNullOrEmpty(elementText) ? 0 : Convert.ToInt32(elementText);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, tableCount); // table count is always written
            if (xmlNode == null) return 0;

            return _WriteTableArray(xmlCookElement, xmlNode.NextSibling, tableCount);
        }

        private int _WriteTableArray(XmlCookElement xmlCookElement, XmlNode xmlNode, int tableCount)
        {
            XmlDefinition xmlTableCountDefinition = _GetXmlDefinition(xmlCookElement.ChildTypeHash);
            int tablesAdded = 0;

            for (int i = 0; i < tableCount; i++)
            {
                XmlNode tableNode = xmlNode.NextSibling;
                if (tableNode.Name != xmlTableCountDefinition.RootElement) return -1;

                if (_CookXmlData(xmlTableCountDefinition, tableNode) == -1) return -1;
                tablesAdded++;

                xmlNode = tableNode.NextSibling;
            }

            if (tablesAdded < tableCount) return -1;
            return tablesAdded;
        }

        private void _WriteExcelIndex(String elementText, XmlCookElement xmlCookElement)
        {
            byte byteLen = (byte)elementText.Length;
            // if no chars, then we obviously have no excel index
            if (byteLen == 0x00)
            {
                FileTools.WriteToBuffer(ref _buffer, ref _offset, (byte)0xFF);
                return;
            }

            String excelString = null;
            try
            {
                int index = int.Parse(elementText);
                excelString = _fileManager.GetExcelRowStringFromRowIndex((Xls.TableCodes)xmlCookElement.ExcelTableCode, index);
            }
            catch (Exception)
            {
                Console.WriteLine("Warning: Failed to get excel string for index: " + elementText);
            }

            // if null string, then we had an invalid index
            // if string is empty, we still want to write 0x00 (below) as there are some excel rows with empty strings
            if (excelString == null)
            {
                FileTools.WriteToBuffer(ref _buffer, ref _offset, (byte)0xFF);
                return;
            }

            byteLen = (byte)excelString.Length;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, byteLen);
            byte[] stringBytes = FileTools.StringToASCIIByteArray(excelString);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, stringBytes);
            // no \0
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
                else if (typeof(T) == typeof(Int32))
                {
                    elementValue = Convert.ToInt32(arrayElementText);
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