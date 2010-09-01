using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Reanimator.XmlDefinitions;
using System.Runtime.InteropServices;

namespace Reanimator
{
    partial class XmlCookedFile
    {
        public const String FileExtention = "xml.cooked";
        private const UInt32 FileMagicWord = 0x6B304F43; // 'CO0k'
        private const Int32 RequiredVersion = 8;
        private const UInt32 DataMagicWord = 0x41544144;
        private static XmlDefinition[] _xmlDefinitions;

        private int _offset;
        private byte[] _data;
        private XmlDocument XmlDoc { get; set; }

        public XmlCookedFile()
        {
            XmlDoc = new XmlDocument();
        }

        public bool Uncook(byte[] data)
        {
            if (_xmlDefinitions == null) throw new Exceptions.NotInitializedException();

            if (data == null) return false;
            _data = data;

            /* Header Structure
             * UInt32           Magic Word          'CO0k'
             * Int32            Version             Must be 8
             */

            // check file infos
            XmlCookFileHeader header = (XmlCookFileHeader)FileTools.ByteArrayToStructure(_data, typeof(XmlCookFileHeader), _offset);
            _offset += Marshal.SizeOf(typeof(XmlCookFileHeader));

            if (header.MagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();
            if (header.Version != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();

            XmlDefinition xmlDefinition = GetXmlDefinition(header.XmlRootDefinition);
            if (xmlDefinition == null ||
                xmlDefinition.ElementCount < header.XmlRootElementCount)
                throw new Exceptions.NotSupportedFileDefinitionException();

            _UncookFileXmlElements(xmlDefinition, header.XmlRootElementCount);

            UInt32 dataMagicWord = FileTools.ByteArrayTo<UInt32>(_data, ref _offset);
            if (dataMagicWord != DataMagicWord) throw new Exceptions.UnexpectedTokenException("'DATA' Token MagicWord expected but not found!");

            _UncookXmlDefinition(xmlDefinition, XmlDoc);

            return true;
        }

        private void _UncookXmlDefinition(XmlDefinition xmlDefinition, XmlNode xmlParent)
        {
            XmlElement rootElement = XmlDoc.CreateElement(xmlDefinition.RootElement);
            int elementCount = xmlDefinition.Elements.Count;


            // bitField info
            int bitFieldOffset = _offset;
            int bitFieldByteCount = (elementCount - 1 >> 3) + 1; // -1 as 16 >> 3 = 2 + 1 = 3, but should only be 2 bytes
            _offset += bitFieldByteCount;


            // loop through elements
            for (int i = 0; i < elementCount; i++)
            {
                // is the field present?
                if (!_TestBit(_data, bitFieldOffset, i)) continue;

                XmlCookElement xmlCookElement = xmlDefinition[i];

                //if (xmlCookElement.Name == "tAttachmentDef.eType")
                //{
                //    int bp = 0;
                //}

                switch (xmlCookElement.ElementType)
                {
                    case ElementType.Int32:
                        _ReadInt32(rootElement, xmlCookElement.Name);
                        break;

                    case ElementType.Float:
                        _ReadFloat(rootElement, xmlCookElement.Name);
                        break;

                    case ElementType.String:
                        _ReadZeroString(rootElement, xmlCookElement.Name);
                        break;

                    case ElementType.NonCookedInt32: // todo: any point this being here?
                        int bp1 = 0;
                        break;

                    case ElementType.Flag:
                    case ElementType.BitFlag:
                        _ReadBitField(rootElement, xmlDefinition, xmlCookElement);
                        break;

                    case ElementType.ExcelIndex:
                        _ReadByteString(rootElement, xmlCookElement.Name, xmlCookElement.DefaultValue); // todo: do proper conversion
                        break;

                    case ElementType.FloatArray:
                        for (int fIndex = 0; fIndex < xmlCookElement.ArrayCount; fIndex++)
                        {
                            _ReadFloat(rootElement, xmlCookElement.Name);
                        }
                        break;

                    case ElementType.UnknownPType:
                        _ReadInt32(rootElement, xmlCookElement.Name);
                        break;

                    case ElementType.Table:
                    case ElementType.TableCount:
                        _ReadTable(rootElement, xmlCookElement);
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
        }

        private void _ReadTable(XmlNode parentNode, XmlCookElement xmlCookElement)
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
                XmlDefinition xmlCountDefinition = (XmlDefinition)Activator.CreateInstance(xmlCookElement.ChildType);
                _UncookXmlDefinition(xmlCountDefinition, parentNode);
            }
        }

        private static bool _TestBit(IList<byte> bitField, int byteOffset, int bitOffset)
        {
            byteOffset += bitOffset >> 3;
            bitOffset &= 0x07;

            return (bitField[byteOffset] & (1 << bitOffset)) >= 1;
        }

        private void _UncookFileXmlElements(XmlDefinition xmlDefinition, int xmlDefElementCount)
        {
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

            for (int i = 0; i < xmlDefElementCount && _offset < _data.Length; i++)
            {
                uint elementHash = FileTools.ByteArrayTo<UInt32>(_data, ref _offset);

                XmlCookElement xmlCookElement = GetXmlCookElement(xmlDefinition, elementHash);
                if (xmlCookElement == null) throw new Exceptions.UnexpectedTokenException("Unexpected xml element hash: 0x" + elementHash.ToString("X8"));


                ElementType token = (ElementType)FileTools.ByteArrayTo<ushort>(_data, ref _offset);
                switch (token)
                {
                    case ElementType.UnknownPTypeD:                                     // 0x0D00
                        // nothing to do                                                // found in paths
                        break;                                                          // only token then next element


                    case ElementType.String:                                            // 0x0200
                        String str = _ReadByteString(null, null, null);                 // default value
                        if (str != null) _offset++; // +1 for \0

                        Debug.Assert((String)xmlCookElement.DefaultValue == str);
                        break;


                    case ElementType.Int32:                                             // 0x0000
                    case ElementType.NonCookedInt32:                                    // 0x0700
                    case ElementType.UnknownPType:                                      // 0x0007
                        Int32 defaultInt32 = _ReadInt32(null, null);                    // default value

                        Debug.Assert((Int32)xmlCookElement.DefaultValue == defaultInt32);
                        break;


                    case ElementType.Float:                                             // 0x0100
                    case ElementType.UnknownFloat:                                      // 0x0800
                    case ElementType.UnknownFloatT:                                     // 0x0600 //materials "tScatterColor"
                        float defaultFloat = _ReadFloat(null, null);                    // default value

                        Debug.Assert((float)xmlCookElement.DefaultValue == defaultFloat);
                        break;


                    case ElementType.Flag:                                              // 0x0B01
                        bool defaultFlagged = _ReadBool32(null, null);                  // default value
                        UInt32 bitMask = _ReadBitField(null, null);                     // bit mask

                        Debug.Assert((bool)xmlCookElement.DefaultValue == defaultFlagged);
                        Debug.Assert(xmlCookElement.BitMask == bitMask);
                        break;


                    case ElementType.BitFlag:                                           // 0x0C02
                        Int32 bitIndex = _ReadInt32(null, null);                        // bit index
                        _ReadBitField(null, null);                                      // total count?

                        Debug.Assert(xmlCookElement.BitIndex == bitIndex);
                        break;


                    case ElementType.ExcelIndex:                                        // 0x0903
                        Int32 excelTableCode =_ReadInt32(null, null);                   // excel table code

                        Debug.Assert(xmlCookElement.ExcelTableCode == excelTableCode);
                        break;


                    case ElementType.FloatArray:                                        // 0x0106
                        float defaultFloatArr = _ReadFloat(null, null);                 // default value
                        Int32 arrayCount = _ReadInt32(null, null);                      // count

                        Debug.Assert((float)xmlCookElement.DefaultValue == defaultFloatArr);
                        Debug.Assert(xmlCookElement.ArrayCount == arrayCount);
                        break;


                    case ElementType.Table:                                             // 0x0308
                    case ElementType.TableCount:                                        // 0x030A
                        UInt32 stringHash = _ReadUInt32(null, null);                    // table string hash
                        Int32 elementCount = _ReadInt32(null, null);                    // table element count

                        XmlDefinition tableXmlDefition = GetXmlDefinition(stringHash);
                        if (tableXmlDefition == null ||
                            tableXmlDefition.ElementCount < elementCount)
                            throw new Exceptions.NotSupportedFileDefinitionException();

                        _UncookFileXmlElements(tableXmlDefition, elementCount);
                        break;


                    default:
                        throw new Exceptions.UnexpectedTokenException(
                            "Unexpected .xml.cooked definition array token: 0x" + ((ushort)token).ToString("X4"));
                }
            }
        }

        private void _ReadBitField(XmlNode parentNode, XmlDefinition xmlDefinition, XmlCookElement xmlCookElement)
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

            Debug.Assert(flagged != (bool)xmlCookElement.DefaultValue);

            if (parentNode == null) return;
            XmlElement element = XmlDoc.CreateElement(xmlCookElement.Name);
            element.InnerText = flagged ? "1" : "0"; ;
            parentNode.AppendChild(element);
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