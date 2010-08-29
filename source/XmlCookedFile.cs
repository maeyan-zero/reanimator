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

            // this isn't actually (at least, I don't think it is) a type specifier, but seeing
            // as (from what I've seen so far) the unknown array is constant per-type, might as well use it as such
            uint cookedType = FileTools.ByteArrayTo<uint>(_data, fileTypeOffset);

            switch(cookedType)
            {
                case 0x400053E7: // skills
                    //_xmlCooked = new XmlCookedSkill();
                    _xmlFile = new SkillEventsDefinition();
                    break;

                //case 0x3A048D4A: // states
                //    _xmlCooked = new XmlCookedState();
                //    break;

                default:
                    MessageBox.Show("Not implemented type!\nOnly skills and states supported.\n\nunknown = 0x" + cookedType.ToString("X8"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Debug.Assert(false);
                    return false;
            }

            return _ParseCookedData();
        }

        private bool _ParseCookedData()
        {
            if (_xmlFile == null) return false;
            _offset = 0;

            // skip over ("parse") default value array
            // todo: check me?
            if (!_ParseCookedDefaultValues()) return false;

            return _DoDefinition(_xmlFile, XmlDoc);
        }

        private bool _DoDefinition(XmlDefinition xmlDefinition, XmlNode xmlParent)
        {
            XmlElement rootElement = XmlDoc.CreateElement(xmlDefinition.RootElement);
            xmlParent.AppendChild(rootElement);

            int elementCount = xmlDefinition.Elements.Count;

            // generate bitField info
            int bitFieldByteCount = (elementCount-1 >> 3) + 1; // -1 as 16 >> 3 = 2 + 1 = 3, but should only be 2 bytes

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

                // is the field a count parameter?
                String elementName = xmlDefinition[i].Name;
                if (xmlDefinition[i].ChildType != null)
                {
                    int count = 1;
                    if (xmlDefinition[i].IsCount)
                    {
                        elementName += "Count";
                        count = _ReadInt32(rootElement, elementName);
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
                    case ElementType.UInt32:
                        _ReadUInt32(rootElement, xmlDefinition[i].Name);
                        break;
                    case ElementType.Float:
                        _ReadFloat(rootElement, xmlDefinition[i].Name);
                        break;
                    case ElementType.String:
                        _ReadZeroString(rootElement, xmlDefinition[i].Name);
                        break;
                    case ElementType.ExcelIndex:
                        _ReadByteString(rootElement, xmlDefinition[i].Name, xmlDefinition[i].DefaultValue); // todo: do proper conversion
                        break;
                    case ElementType.Flag:
                        UInt32 flagId = xmlDefinition[i].FlagId;
                        if (flags.ContainsKey(flagId))
                        {
                            UInt32 flag = (UInt32)flags[flagId];
                            bool flagged = (flag & (1 << i)) > 0;
                            bool defaultFlagged = (bool) xmlDefinition[i].DefaultValue;
                            if (flagged != defaultFlagged)
                            {
                                XmlElement xmlElement = XmlDoc.CreateElement(xmlDefinition[i].Name);
                                xmlElement.InnerText = flagged ? "1" : "0";
                                rootElement.AppendChild(xmlElement);
                            }
                        }
                        else
                        {
                            UInt32 flag = _ReadUInt32(rootElement, xmlDefinition[i].Name);
                            flags.Add(flagId, flag);
                        }
                        break;
                    default:
                        Debug.Assert(false, "ElementType not set!");
                        break;
                }
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

            //XmlElement mainElement = XmlDoc.CreateElement("CO0k");
            //XmlDoc.AppendChild(mainElement);

            //XmlElement versionElement = XmlDoc.CreateElement("version");
            //versionElement.InnerText = "8";
            //mainElement.AppendChild(versionElement);


            /* ==Strange Array==
             * 4 bytes		unknown
             * 2 bytes		Type Token
             * *remaining based on token*
             * 
             * =Token=		=Followed By=
             *  00 00       4 bytes (Int32?  Or UInt32 because 00 07 appears to have -1?)
             *  00 01       4 bytes (Float)
             *  00 02       1 byte  (Str Length (NOT inc \0) - if != 0, string WITH \0)
             *  00 06       4 bytes (Float)
             *  00 07       4 bytes (Int32?  Or possibly an array with -1 = end?)
             *  01 0B       8 bytes	(Null Int32?,  32 bit Bitmask)
             *  02 0C       8 bytes	(Int32 index?,  Int32 value?)
             *  03 00       2 bytes	(ShortInt?)
             *  03 09       4 bytes	(Int32?)
             *  05 00       2 bytes (??)
             *  08 03       8 bytes	(Int32??,  Int32??)
             *  09 03       4 bytes (Int32)
             *  0A 03       8 bytes	(Int32??,  Int32??)
             *  C0 00       2 bytes (??)
             *  
             *  // extras found in particle effects
             *  8D 00       2 bytes
             *  00 0A       4 bytes (Int32?)
             *  06 01       8 bytes (Int32?, Int32?)
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

                    case 0x0003:    // skills
                        //case 0x0005:
                        //case 0x00C0:
                        //case 0x008D: // found in particle effects
                        _ReadShort(null, null);
                        //_AddUnknown(unknown, token, srt);
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

            //XmlElement dataElement = XmlDoc.CreateElement("DATA");
            //mainElement.AppendChild(dataElement);

            //if (!ParseDataSegment(dataElement)) return false;

            //if (ReadOffset != Data.Length)
            //{
            //    MessageBox.Show("Entire file not parsed!\noffset != data.Length\n\noffset = " + ReadOffset +
            //                    "\ndata.Length = " + Data.Length, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return false;
            //}
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
                //element.SetAttribute("asHex", "0x" + value.ToString("X4"));
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
                //element.SetAttribute("asHex", "0x" + value.ToString("X8"));
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
                //element.SetAttribute("asHex", "0x" + value.ToString("X8"));
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
                //element.SetAttribute("asHex", "0x" + value.ToString("X8"));
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