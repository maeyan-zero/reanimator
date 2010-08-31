using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using Reanimator.XmlDefinitions;

namespace Reanimator
{
    class XmlCookedFile
    {
        public const String FileExtention = "xml.cooked";
        private const UInt32 FileHeadToken = 0x6B304F43;
        private const Int32 RequiredVersion = 8;
        private const UInt32 DataSegmentToken = 0x41544144;

        private int _offset;
        private byte[] _data;
        XmlDefinition _xmlFile;
        private XmlDocument XmlDoc { get; set; }

        public XmlCookedFile()
        {
            XmlDoc = new XmlDocument();
        }

        public bool ParseData(byte[] data)
        {
            if (data == null) return false;
            _data = data;

            const int fileTypeOffset = 8;
            if (fileTypeOffset >= _data.Length - 4) return false;

            UInt32 xmlDefinitionHash = FileTools.ByteArrayTo<UInt32>(_data, fileTypeOffset);

            switch (xmlDefinitionHash)
            {
                case 0x400053E7: // skills
                    _xmlFile = new SkillEventsDefinition();
                    break;

                case 0x4C461620: // ai
                    _xmlFile = new AIDefinition();
                    break;

                case 0x3A048D4A: // states
                    _xmlFile = new StateDefinition();
                    break;

                case 0xA51494FA: // sounds/effects
                    _xmlFile = new SoundEffectDefinition();
                    break;

                default:
                    MessageBox.Show("Not implemented type!\nOnly skills and states supported.\n\nxmlDefinitionHash = 0x" + xmlDefinitionHash.ToString("X8"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Debug.Assert(false);
                    return false;
            }

            return _ParseCookedData();
        }

        private bool _ParseCookedData()
        {
            if (_xmlFile == null) return false;
            _offset = 0;

            if (!_ParseCookedDefaultValues()) return false;

            return _DoDefinition(_xmlFile, XmlDoc);
        }

        private bool _DoDefinition(XmlDefinition xmlDefinition, XmlNode xmlParent)
        {
            XmlElement rootElement = XmlDoc.CreateElement(xmlDefinition.RootElement);

            int elementCount = xmlDefinition.Elements.Count;

            // generate bitField info
            int bitFieldByteCount = (elementCount - 1 >> 3) + 1; // -1 as 16 >> 3 = 2 + 1 = 3, but should only be 2 bytes

            // read cooked elements bitField
            byte[] elementField = new byte[bitFieldByteCount];
            Buffer.BlockCopy(_data, _offset, elementField, 0, bitFieldByteCount);
            _offset += bitFieldByteCount;

            // loop through elements
            Hashtable flags = new Hashtable();
            for (int i = 0; i < elementCount; i++)
            {
                // is the field present?
                if (!_TestBit(elementField, i)) continue;

                if (xmlDefinition[i].Name == "tAttachmentDef.eType")
                {
                    int bp = 0;
                }

                // is the field a count parameter?
                String elementName = xmlDefinition[i].Name;
                if (xmlDefinition[i].ChildType != null)
                {
                    int count = 1;
                    if (xmlDefinition[i].ElementType == ElementType.TableCount)
                    {
                        elementName += "Count";
                        count = _ReadInt32(rootElement, elementName);

                        if (count == 0)
                        {
                            rootElement.RemoveChild(rootElement.LastChild);
                        }
                    }

                    for (int e = 0; e < count; e++)
                    {
                        XmlDefinition xmlCountDefinition = (XmlDefinition)Activator.CreateInstance(xmlDefinition[i].ChildType);
                        if (!_DoDefinition(xmlCountDefinition, rootElement)) return false;
                    }

                    continue;
                }

                switch (xmlDefinition[i].ElementType)
                {
                    case ElementType.Int32:
                        _ReadInt32(rootElement, xmlDefinition[i].Name);
                        break;
                    case ElementType.Float:
                        _ReadFloat(rootElement, xmlDefinition[i].Name);
                        break;
                    case ElementType.String:
                        _ReadZeroString(rootElement, xmlDefinition[i].Name);
                        break;
                    case ElementType.NonCookedInt32: // todo: any point this being here?
                        int bp1 = 0;
                        break;
                    case ElementType.BitFlag: // todo: what am I exactly?
                    case ElementType.Flag:
                        UInt32 flagId = xmlDefinition[i].FlagId;
                        Debug.Assert(flagId != 0); // flagId shouldn't be zero (as that's a default value)

                        UInt32 flag;
                        if (!flags.ContainsKey(flagId))
                        {
                            flag = _ReadUInt32(null, null);
                            flags.Add(flagId, flag);
                        }
                        else
                        {
                            flag = (UInt32)flags[flagId];
                        }

                        // this shouldn't be done every time for a flag
                        // todo: save offset or pre-generate
                        // get first flag of flagId offset
                        int flagIndex = 0;
                        for (; flagIndex < xmlDefinition.Elements.Count & flagIndex < i; flagIndex++)
                        {
                            if (xmlDefinition[flagIndex].FlagId != flagId) continue;

                            break;
                        }

                        int flagBitMask = (1 << (i - flagIndex));// (1 >> (i-1)) & 0x01;

                        // dodgy temp fix for testing
                        // todo: fixe me
                        if (xmlDefinition[i].Name == "CONDITION_BIT_CHECK_TARGET")
                        {
                            flagBitMask <<= 1;
                        }
                        else if (xmlDefinition[i].Name == "CONDITION_BIT_CHECK_WEAPON")
                        {
                            flagBitMask >>= 1;
                        }

                        bool flagged = (flag & flagBitMask) > 0;
                        bool defaultFlagged = (bool)xmlDefinition[i].DefaultValue;
                        if (flagged != defaultFlagged)
                        {
                            XmlElement xmlElement = XmlDoc.CreateElement(xmlDefinition[i].Name);
                            xmlElement.InnerText = flagged ? "1" : "0";
                            rootElement.AppendChild(xmlElement);
                        }
                        break;
                    case ElementType.ExcelIndex:
                        _ReadByteString(rootElement, xmlDefinition[i].Name, xmlDefinition[i].DefaultValue); // todo: do proper conversion
                        break;
                    case ElementType.FloatArray:
                        for (int fIndex = 0; fIndex < xmlDefinition[i].ArrayCount; fIndex++)
                        {
                            _ReadFloat(rootElement, xmlDefinition[i].Name);
                        }
                        break;
                    default:
                        Debug.Assert(false, "ElementType not set!");
                        break;
                }
            }

            if (rootElement.ChildNodes.Count > 0)
            {
                xmlParent.AppendChild(rootElement);
            }

            return true;
        }

        private static bool _TestBit(IList<byte> bitField, int bitOffset)
        {
            int byteOffset = bitOffset >> 3;
            bitOffset &= 0x07;

            return (bitField[byteOffset] & (1 << bitOffset)) >= 1;
        }

        private bool _ParseCookedDefaultValues()
        {
            if (_data == null) return false;

            int fileHeadToken = FileTools.ByteArrayTo<Int32>(_data, ref _offset);
            if (fileHeadToken != FileHeadToken) return false;  // 'CO0k'

            int version = FileTools.ByteArrayTo<Int32>(_data, ref _offset);
            if (version != RequiredVersion) return false;

            UInt32 xmlDefinitionHash = _ReadUInt32(null, null);
            Int32 definitionElementCount = _ReadInt32(null, null);

            /* Default Element Values Array & General Structure
             * =Token=		=Type=          =FoundIn=       =Details= (* = variable length)
             * 0x0000       Int32           Skills          4 bytes     e.g. tAttachmentDef.eType = -1 in some cases - also used with tAttachmentDef.dwFlags
             * 0x0100       Float           Skills          4 bytes     Float value
             * 0x0200       String          Skills          1 byte*     Str Length (NOT inc \0), then if != 0, string WITH \0 follows
             * 0x0700       NonCookedInt32  Skills          4 bytes     Used for nPreviewAppearance, but doesn't appear to be cooked by game
             * 0x0B01       Flag            Skills          4 bytes*    First 4 bytes (UInt32) = Bitmask of Flag in field (In DATA segment, Flag is only read in once per "chunk" i.e. After 32 flags, the next flag will be read 4 bytes again)
             * 0x0C02       BitFlag         Skills          8 bytes     First 4 bytes (Int32)  = Bit Index of Flag in field, Second 4 bytes (Int32) = Always appears to be 0x00000007
             * 0x0803       Table           Skills          8 bytes     First 4 bytes (UInt32) = XML Definition Hash, Second 4 bytes (Int32) = XML Definition Element Count - this always exists in cooked file, below TableCount type may not
             * 0x0903       ExcelIndex      Skills          4 bytes     First 4 bytes (UInt32) = Code of Excel Table - Cooking reads index and places from table
             * 0x0106       Float Array     AI              4 bytes     First 4 bytes (Float)  = Default Value, Second 4 bytes (Int32) = Array Size
             * 0x030A       TableCount      Skills          8 bytes     First 4 bytes (UInt32) = XML Definition Hash, Second 4 bytes (Int32) = XML Definition Element Count

             * // extras found in particle effects
             *  00 0A       4 bytes (Int32?)
             *  00 05       4 bytes (Float)
             */

            //XmlElement unknownArray = XmlDoc.CreateElement("unknownArray");
            //mainElement.AppendChild(unknownArray);

            bool foundDataSegment = false;
            while (_offset < _data.Length)
            {
                uint unknown = FileTools.ByteArrayTo<UInt32>(_data, ref _offset);

                if (unknown == DataSegmentToken) // 'DATA'
                {
                    foundDataSegment = true;
                    break;
                }

                ushort token = FileTools.ByteArrayTo<ushort>(_data, ref _offset);

                //XmlElement arrayElement = XmlDoc.CreateElement("0x" + unknown.ToString("X8"));
                //arrayElement.SetAttribute("token", "0x" + token.ToString("X4"));
                //unknownArray.AppendChild(arrayElement);

                switch (token)
                {
                    case 0x0200:    // skills
                        String str = _ReadByteString(null, null, null);
                        if (str != null)
                        {
                            _offset++; // need to include \0 as it isn't included in the strLen byte for some reason
                        }
                        //_AddUnknown(unknown, token, str);
                        break;

                    case 0x0000:    // skills
                    case 0x0309:    // skills
                    case 0x0700:    // skills
                    case 0x0903:    // skills
                        //case 0x0A00:   // particle effects
                        _ReadInt32(null, null);
                        //_AddUnknown(unknown, token, int0);
                        break;

                    case 0x0100:    // skills
                        //case 0x0600:
                        ////case 0x0500: // found in particle effects
                        _ReadFloat(null, null);
                        //_AddUnknown(unknown, token, f);
                        break;

                    case 0x0B01:    // skills
                        _ReadBitField(null, null);
                        _ReadBitField(null, null);
                        //_AddUnknown(unknown, token, int1, bitfield);
                        break;

                    case 0x0308:    // skills
                    case 0x030A:    // skills
                    case 0x0106:    // ai (float array)
                        ////case 0x0106: // found in particle effects
                        _ReadInt32(null, null);
                        _ReadInt32(null, null);
                        //_AddUnknown(unknown, token, int2, int3);
                        break;

                    case 0x0C02:    // skills
                        _ReadInt32(null, null);
                        _ReadInt32(null, null);
                        //_AddUnknown(unknown, token, int4, int5);
                        break;

                    default:
                        MessageBox.Show("Unexpected unknownArray token!\n\ntoken = 0x" + token.ToString("X4"), "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Debug.Assert(false);
                        break;
                }
            }

            return foundDataSegment;
        }

        private String _ReadByteString(XmlNode parentNode, String elementName, Object mustExist)
        {
            String value = null;
            byte strLen = FileTools.ByteArrayTo<byte>(_data, ref _offset);
            if (strLen == 0xFF || strLen == 0x00)
            {
                if (mustExist != null)
                {
                    value = mustExist.ToString();
                }
            }
            else
            {
                value = FileTools.ByteArrayToStringAnsi(_data, ref _offset, strLen);
            }

            if (value == null) return null;

            if (parentNode != null && elementName != null)
            {
                XmlElement element = XmlDoc.CreateElement(elementName);
                element.InnerText = value;
                parentNode.AppendChild(element);
            }

            return value;
        }

        private short _ReadShort(XmlNode parentNode, String elementName)
        {
            short value = FileTools.ByteArrayTo<short>(_data, ref _offset);

            if (parentNode != null && elementName != null)
            {
                XmlElement element = XmlDoc.CreateElement(elementName);
                element.InnerText = value.ToString();
                parentNode.AppendChild(element);
            }

            return value;
        }

        private Int32 _ReadInt32(XmlNode parentNode, String elementName)
        {
            Int32 value = FileTools.ByteArrayTo<Int32>(_data, ref _offset);

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
            UInt32 value = FileTools.ByteArrayTo<UInt32>(_data, ref _offset);

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
            float value = FileTools.ByteArrayTo<float>(_data, ref _offset);

            if (parentNode != null && elementName != null)
            {
                XmlElement element = XmlDoc.CreateElement(elementName);
                element.InnerText = value.ToString("F2");
                parentNode.AppendChild(element);
            }

            return value;
        }

        private UInt32 _ReadBitField(XmlNode parentNode, String elementName)
        {
            UInt32 value = FileTools.ByteArrayTo<UInt32>(_data, ref _offset);

            if (parentNode != null && elementName != null)
            {
                XmlElement element = XmlDoc.CreateElement(elementName);
                element.InnerText = Convert.ToString(value, 2).PadLeft(32, '0');
                parentNode.AppendChild(element);
            }

            return value;
        }

        private String _ReadZeroString(XmlNode parentNode, String elementName)
        {
            Int32 strLen = FileTools.ByteArrayTo<Int32>(_data, ref _offset);
            Debug.Assert(strLen != 0);

            String value = FileTools.ByteArrayToStringAnsi(_data, _offset);
            _offset += strLen;

            if (parentNode != null && elementName != null)
            {
                XmlElement element = XmlDoc.CreateElement(elementName);
                element.InnerText = value;
                parentNode.AppendChild(element);
            }

            return value;
        }

        public void SaveXml(String path)
        {
            if (XmlDoc == null || String.IsNullOrEmpty(path)) return;

            XmlDoc.Save(path);
        }
    }
}