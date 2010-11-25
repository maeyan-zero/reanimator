using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.IO;
using Hellgate.Xml;
using Revival.Common;

namespace Hellgate
{
    public partial class XmlCookedFile
    {
        public const String FileExtention = "xml.cooked";
        private const UInt32 FileMagicWord = 0x6B304F43; // 'CO0k'
        private const Int32 RequiredVersion = 8;
        private const UInt32 DataMagicWord = 0x41544144;
        private static XmlDefinition[] _xmlDefinitions;
        private static FileManager _fileManager;

        private int _offset;
        private byte[] _data;
        public XmlDocument XmlDoc { get; set; }

        public XmlCookedFile()
        {
        }

        /// <summary>
        /// Cooks an XML document to a .xml.cooked.
        /// </summary>
        /// <param name="xmlDocument">An XML Document to Cook.</param>
        /// <returns>The cooked bytes, or null upon failure.</returns>
        public static byte[] CookXmlDocument(XmlDocument xmlDocument)
        {
            if (_xmlDefinitions == null) throw new Exceptions.NotInitializedException();
            if (!xmlDocument.HasChildNodes) return null;

            XmlNode rootElement = xmlDocument.FirstChild;
            UInt32 xmlDefinitionHash = Crypt.GetStringHash(rootElement.Name);

            XmlDefinition xmlDefinition = _GetXmlDefinition(xmlDefinitionHash);
            if (xmlDefinition == null) throw new Exceptions.NotSupportedFileDefinitionException();


            byte[] buffer = new byte[1024];
            int offset = 0;


            // header details //
            XmlCookFileHeader xmlCookFileHeader = new XmlCookFileHeader
            {
                MagicWord = FileMagicWord,
                Version = RequiredVersion,
                XmlRootDefinition = xmlDefinitionHash,
                XmlRootElementCount = xmlDefinition.ElementCount
            };
            FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookFileHeader);


            // write default element array //
            List<UInt32> cookedDefinitions = new List<UInt32>();
            int bytesWritten = _CookXmlDefinition(ref buffer, offset, xmlDefinition, cookedDefinitions);
            if (bytesWritten == 0) return null;
            offset += bytesWritten;


            // write data segment //
            FileTools.WriteToBuffer(ref buffer, ref offset, DataMagicWord);
            XmlNode xmlNode = xmlDocument.FirstChild;
            bytesWritten = _CookXmlData(ref buffer, offset, xmlDefinition, xmlNode);
            if (bytesWritten == 0) return null;
            offset += bytesWritten;

            byte[] cookedBytes = new byte[offset];
            Buffer.BlockCopy(buffer, 0, cookedBytes, 0, offset);

            return cookedBytes;
        }

        private static int _CookXmlData(ref byte[] buffer, int offset, XmlDefinition xmlDefinition, XmlNode xmlNode)
        {
            int offsetStart = offset;
            int elementCount = xmlDefinition.Elements.Count;

            // bitField info
            int bitFieldOffset = offset;
            int bitFieldByteCount = (elementCount - 1 >> 3) + 1; // -1 as 16 >> 3 = 2 + 1 = 3, but should only be 2 bytes, and +1 as 7 >> 3 = 0
            offset += bitFieldByteCount;

            // todo: this is a bit excessive for a couple of offsets, but meh, fix it later
            Hashtable bitFieldOffsts = new Hashtable();
            int bitIndex = -1;
            foreach (XmlCookElement xmlCookElement in xmlDefinition.Elements)
            {
                bitIndex++;

                // ElementType.Table must be cooked - so xmlNode might be null if not defined
                if (xmlNode == null)
                {
                    // only things written in for blank tables
                    switch (xmlCookElement.ElementType)
                    {
                        case ElementType.ExcelIndex:
                            const byte noEntry = 0xFF;
                            FileTools.WriteToBuffer(ref buffer, ref offset, noEntry);
                            _FlagBit(buffer, bitFieldOffset, bitIndex);
                            break;

                        case ElementType.TableCount:
                            const Int32 zeroTables = 0;
                            FileTools.WriteToBuffer(ref buffer, ref offset, zeroTables);
                            _FlagBit(buffer, bitFieldOffset, bitIndex);
                            break;
                    }

                    continue;
                }

                // todo: name replace Name with TrueName when saved and when uncooking
                String elementName = xmlCookElement.Name;
                if (xmlCookElement.ElementType == ElementType.TableCount ||
                    xmlCookElement.ElementType == ElementType.UnknownFloatT)
                {
                    elementName += "Count";
                }

                XmlElement xmlElement = xmlNode[elementName];
                String elementText = String.Empty;
                if (xmlElement == null &&
                    xmlCookElement.ElementType != ElementType.ExcelIndex && // want excel index even if null/empty
                    xmlCookElement.ElementType != ElementType.Table &&      // want table even if null/empty
                    xmlCookElement.ElementType != ElementType.TableCount    // want table count even if null/empty
                    ) continue;

                if (xmlElement != null)
                {
                    elementText = xmlElement.InnerText;
                }

                switch (xmlCookElement.ElementType)
                {
                    case ElementType.NonCookedInt32: // 0x0700
                    case ElementType.UnknownFloat: // 0x0800
                    case ElementType.UnknownPTypeD: // 0x0D00
                        // not cooked.... (I think...)
                        continue;


                    case ElementType.Int32: // 0x0000
                    case ElementType.UnknownPType: // 0x0007    // I think this is just an int32...
                        Int32 intValue = Convert.ToInt32(elementText);
                        if ((Int32)xmlCookElement.DefaultValue == intValue) continue;
                        FileTools.WriteToBuffer(ref buffer, ref offset, intValue);
                        break;


                    case ElementType.RGBADoubleWordArray: // 0x0006     // found in colorsets.xml.cooked
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

                        if (dwAllDefault) continue;

                        for (int i = 0; i < dwArrayCount; i++)
                        {
                            UInt32 dwWrite = (UInt32)xmlCookElement.DefaultValue;
                            if (i < dwElements.Count)
                            {
                                dwWrite = dwElements[i];
                            }

                            FileTools.WriteToBuffer(ref buffer, ref offset, dwWrite);
                        }
                        break;


                    case ElementType.Float: // 0x0100
                        float floatValue = Convert.ToSingle(elementText);
                        if ((float)xmlCookElement.DefaultValue == floatValue) continue;
                        FileTools.WriteToBuffer(ref buffer, ref offset, floatValue);
                        break;


                    case ElementType.String: // 0x0200
                        if ((String)xmlCookElement.DefaultValue == elementText) continue;

                        Int32 strLen = elementText.Length;
                        if (strLen == 0 && xmlCookElement.DefaultValue == null) continue;

                        byte[] strBytes;
                        if (xmlCookElement.TreatAsData)
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

                        FileTools.WriteToBuffer(ref buffer, ref offset, strLen + 1);
                        FileTools.WriteToBuffer(ref buffer, ref offset, strBytes);

                        offset++; // \0

                        break;


                    case ElementType.UnknownFloatT: // 0x0600
                        Int32 count = Convert.ToInt32(elementText);
                        FileTools.WriteToBuffer(ref buffer, ref offset, count);

                        int floatTCount = xmlCookElement.Count;
                        List<float> floatTValues = new List<float>();
                        bool floatTAllDefault = true;

                        int totalFloatTCount = count * floatTCount;
                        foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
                        {
                            if (xmlChildNode.Name != xmlCookElement.Name) continue;

                            String arrayElementText = xmlChildNode.InnerText;
                            float fValue = Convert.ToSingle(arrayElementText);
                            if (fValue == 0 && arrayElementText == "-0")
                                fValue = -1.0f * 0.0f;
                            floatTValues.Add(fValue);

                            if ((float)xmlCookElement.DefaultValue != fValue)
                            {
                                floatTAllDefault = false;
                            }

                            if (floatTValues.Count == totalFloatTCount) break;
                        }

                        if (floatTAllDefault) continue;

                        for (int i = 0; i < count; i++)
                        {
                            for (int j = 0; j < floatTCount; j++)
                            {
                                float fWrite;

                                int index = i * floatTCount + j;
                                if (index < floatTValues.Count)
                                {
                                    fWrite = floatTValues[index];
                                }
                                else
                                {
                                    fWrite = (float)xmlCookElement.DefaultValue;
                                }

                                FileTools.WriteToBuffer(ref buffer, ref offset, fWrite);
                            }
                        }
                        break;


                    case ElementType.Flag: // 0x0B01
                    case ElementType.BitFlag: // 0x0C02
                        bool flagged = elementText == "0" ? false : true;
                        if ((bool)xmlCookElement.DefaultValue == flagged) continue;

                        int flagIndex = xmlCookElement.FlagId - 1;
                        if (xmlCookElement.ElementType == ElementType.BitFlag) flagIndex = 0;

                        // has it been written yet
                        if (xmlDefinition.BitFields[flagIndex] == -1 || bitFieldOffsts[flagIndex] == null)
                        {
                            bitFieldOffsts.Add(flagIndex, offset);
                            xmlDefinition.BitFields[flagIndex] = 0;
                            offset += 4; // bit field bytes
                        }


                        UInt32 flag = (UInt32)xmlDefinition.BitFields[flagIndex];
                        if (xmlCookElement.ElementType == ElementType.Flag)
                        {
                            flag |= xmlCookElement.BitMask;
                        }
                        else
                        {
                            flag |= ((UInt32)1 << xmlCookElement.BitIndex);
                        }

                        int writeOffset = (int)bitFieldOffsts[flagIndex];
                        FileTools.WriteToBuffer(ref buffer, ref writeOffset, flag);
                        xmlDefinition.BitFields[flagIndex] = (Int32)flag;
                        break;


                    case ElementType.Table: // 0x0308
                        XmlDefinition xmlTableDefinition = _GetXmlDefinition(xmlCookElement.ChildTypeHash);
                        XmlNode xmlTable = xmlNode[xmlTableDefinition.RootElement];

                        int tableBytesWritten = _CookXmlData(ref buffer, offset, xmlTableDefinition, xmlTable);
                        offset += tableBytesWritten;
                        break;


                    case ElementType.TableCount: // 0x030A
                        int tableCount = String.IsNullOrEmpty(elementText) ? 0 : Convert.ToInt32(elementText);
                        FileTools.WriteToBuffer(ref buffer, ref offset, tableCount); // table count is always written
                        if (tableCount == 0) break;

                        XmlDefinition xmlTableCountDefinition = _GetXmlDefinition(xmlCookElement.ChildTypeHash);
                        int tablesAdded = 0;
                        foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
                        {
                            if (xmlChildNode.Name != xmlTableCountDefinition.RootElement) continue;

                            int tableCountBytes = _CookXmlData(ref buffer, offset, xmlTableCountDefinition, xmlChildNode);
                            offset += tableCountBytes;
                            tablesAdded++;

                            if (tableCount == tablesAdded) break;
                        }
                        if (tablesAdded < tableCount) return 0;

                        break;


                    case ElementType.ExcelIndex: // 0x0903
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
                            FileTools.WriteToBuffer(ref buffer, ref offset, byteLen);
                        }
                        else
                        {
                            FileTools.WriteToBuffer(ref buffer, ref offset, byteLen);
                            byte[] stringBytes = FileTools.StringToASCIIByteArray(excelString);
                            FileTools.WriteToBuffer(ref buffer, ref offset, stringBytes);
                            // no \0
                        }

                        break;


                    case ElementType.FloatArray: // 0x0106
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

                        if (allDefault) continue;

                        for (int i = 0; i < arrayCount; i++)
                        {
                            float fWrite = (float)xmlCookElement.DefaultValue;
                            if (i < elements.Count)
                            {
                                fWrite = elements[i];
                            }

                            FileTools.WriteToBuffer(ref buffer, ref offset, fWrite);
                        }
                        break;
                }

                _FlagBit(buffer, bitFieldOffset, bitIndex);
            }

            return offset - offsetStart;
        }

        /// <summary>
        /// Cooks an XML Definition and its children into the supplied byte array.
        /// </summary>
        /// <param name="buffer">A reference to the byte array buffer.</param>
        /// <param name="offset">The buffer offset to begin writing.</param>
        /// <param name="xmlDefinition">The XML Definition to cook.</param>
        /// <param name="cookedDefinitions">A list to track already cooked XML Definitions (stops recursion).</param>
        /// <returns>The amount of bytes written.</returns>
        private static int _CookXmlDefinition(ref byte[] buffer, int offset, XmlDefinition xmlDefinition, List<UInt32> cookedDefinitions)
        {
            cookedDefinitions.Add(xmlDefinition.RootHash);

            int offsetStart = offset;
            foreach (XmlCookElement xmlCookElement in xmlDefinition.Elements)
            {
                // write name hash and token
                FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.NameHash);
                FileTools.WriteToBuffer(ref buffer, ref offset, (ushort)xmlCookElement.ElementType);

                switch (xmlCookElement.ElementType)
                {
                    // 0 bytes
                    case ElementType.UnknownPTypeD: // 0x0D00
                        break;

                    // 4 bytes
                    case ElementType.Int32: // 0x0000
                    case ElementType.Float: // 0x0100
                    case ElementType.UnknownFloatT: // 0x0600
                    case ElementType.NonCookedInt32: // 0x0700
                    case ElementType.UnknownFloat: // 0x0800
                    case ElementType.UnknownPType: // 0x0007
                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.DefaultValue);
                        break;

                    // 1 + x bytes
                    case ElementType.String: // 0x0200
                        byte strLen = (byte)((xmlCookElement.DefaultValue == null)
                                         ? 0
                                         : ((String)xmlCookElement.DefaultValue).Length);
                        FileTools.WriteToBuffer(ref buffer, ref offset, strLen);

                        if (xmlCookElement.DefaultValue == null) continue;


                        byte[] stringBytes = FileTools.StringToASCIIByteArray(xmlCookElement.DefaultValue.ToString());
                        FileTools.WriteToBuffer(ref buffer, ref offset, stringBytes);
                        offset++;
                        break;

                    // 8 bytes
                    case ElementType.Flag: // 0x0B01
                        Int32 defaultFlagged = (bool)xmlCookElement.DefaultValue ? 1 : 0;
                        FileTools.WriteToBuffer(ref buffer, ref offset, defaultFlagged);
                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.BitMask);
                        break;

                    // 8 bytes
                    case ElementType.BitFlag: // 0x0C02
                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.BitIndex);
                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.Count);
                        break;

                    // 8 bytes
                    case ElementType.Table: // 0x0308
                    case ElementType.TableCount: // 0x030A
                        XmlDefinition xmlSubDefinition = _GetXmlDefinition(xmlCookElement.ChildTypeHash);
                        if (xmlSubDefinition == null) throw new Exceptions.NotSupportedFileDefinitionException();

                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.ChildTypeHash);

                        // make sure we haven't already cooked it
                        if (cookedDefinitions.Contains(xmlCookElement.ChildTypeHash))
                        {
                            const Int32 alreadyCooked = -1;
                            FileTools.WriteToBuffer(ref buffer, ref offset, alreadyCooked);
                            break;
                        }

                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlSubDefinition.ElementCount);

                        int bytesWritten = _CookXmlDefinition(ref buffer, offset, xmlSubDefinition, cookedDefinitions);
                        if (bytesWritten == 0) return 0;
                        offset += bytesWritten;
                        break;

                    // 4 bytes
                    case ElementType.ExcelIndex: // 0x0903
                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.ExcelTableCode);
                        break;

                    // 8 bytes
                    case ElementType.RGBADoubleWordArray: // 0x0006     // colorsets.xml.cooked (pdwColors)
                    case ElementType.FloatArray: // 0x0106
                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.DefaultValue);
                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.Count);
                        break;
                }
            }

            return offset - offsetStart;
        }

        /// <summary>
        /// Uncooks a file byte array to XmlDoc.<br />
        /// (automatically determines .xml definition)
        /// </summary>
        /// <param name="data">The file bytes to uncook.</param>
        /// <returns>True on success, false otherwise.</returns>
        /// <exception cref="Exceptions.NotInitializedException">XmlCookedFile.Initialize() Not Called.</exception>
        /// <exception cref="Exceptions.UnexpectedMagicWordException">Bad file bytes - wrong file type (first 4 bytes wrong).</exception>
        /// <exception cref="Exceptions.NotSupportedFileVersionException">File version not supported (only version 8 supported).</exception>
        /// <exception cref="Exceptions.NotSupportedFileDefinitionException">The file contains an unsupported XML Definition type.</exception>
        /// <exception cref="Exceptions.UnexpectedTokenException">An error occured during file bytes parsing.</exception>
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
            XmlCookFileHeader header = FileTools.ByteArrayToStructure<XmlCookFileHeader>(_data, ref _offset);

            if (header.MagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();
            if (header.Version != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();

            XmlDefinition xmlDefinition = _GetXmlDefinition(header.XmlRootDefinition);
            if (xmlDefinition == null ||
                xmlDefinition.ElementCount < header.XmlRootElementCount)
                throw new Exceptions.NotSupportedFileDefinitionException();

            XmlDoc = new XmlDocument();
            Hashtable elements = _UncookFileXmlElements(xmlDefinition, header.XmlRootElementCount);

            UInt32 dataMagicWord = FileTools.ByteArrayToUInt32(_data, ref _offset);
            if (dataMagicWord != DataMagicWord) throw new Exceptions.UnexpectedTokenException("'DATA' Token MagicWord expected but not found!");

            _UncookXmlDefinition(xmlDefinition, XmlDoc, elements);

            Debug.Assert(_offset == _data.Length);

            return true;
        }

        private void _UncookXmlDefinition(XmlDefinition xmlDefinition, XmlNode xmlParent, Hashtable elements)
        {
            XmlElement rootElement = XmlDoc.CreateElement(xmlDefinition.RootElement);
            xmlParent.AppendChild(rootElement);

            // actual present element count - not total count possible
            int elementCount = elements.Count;

            // bitField info
            int bitFieldOffset = _offset;
            int bitFieldByteCount = (elementCount - 1 >> 3) + 1; // -1 as 16 >> 3 = 2 + 1 = 3, but should only be 2 bytes
            _offset += bitFieldByteCount;


            // loop through all elements
            for (int i = 0; i < xmlDefinition.ElementCount; i++)
            {
                // is the field present?
                if (!_TestBit(_data, bitFieldOffset, i)) continue;

                XmlCookElement xmlCookElement = xmlDefinition[i];

                // sanity check - ensure it was in the definition segment
                Debug.Assert(elements.ContainsKey(xmlCookElement.NameHash));

                //if (xmlCookElement.Name == "tAttachmentDef.eType")
                //{
                //    int bp = 0;
                //});

                switch (xmlCookElement.ElementType)
                {
                    case ElementType.Int32: // 0x0000
                        Int32 iValue = _ReadInt32(rootElement, xmlCookElement.Name);
                        Debug.Assert((Int32)xmlCookElement.DefaultValue != iValue);
                        break;

                    case ElementType.Float: // 0x0100
                        float fValue = _ReadFloat(rootElement, xmlCookElement.Name);
                        Debug.Assert((float)xmlCookElement.DefaultValue != fValue);
                        break;

                    case ElementType.String: // 0x0200
                        String szValue = _ReadZeroString(rootElement, xmlCookElement);
                        Debug.Assert((String)xmlCookElement.DefaultValue != szValue);
                        break;

                    case ElementType.UnknownFloatT: // 0x0600
                        // todo: not tested with HGL cooking it
                        _ReadTFloatArray(rootElement, xmlCookElement);
                        break;


                    // todo: any point this being here?
                    case ElementType.NonCookedInt32: // 0x0700
                    case ElementType.UnknownFloat: // 0x0800
                    case ElementType.UnknownPTypeD: // 0x0D00
                        int bp = 0;
                        break;

                    case ElementType.Flag: // 0x0B01
                    case ElementType.BitFlag: // 0x0C02
                        if (xmlCookElement.Name == "ROOM_PATH_NODE_DEF_INDOOR_FLAG")
                        {
                            int b2p = 0;
                        }

                        bool bValue = _ReadBitField(rootElement, xmlDefinition, xmlCookElement);

                        // note: do flags need to be included for cooking no matter their default value??
                        // ROOM_PATH_NODE_DEF_INDOOR_FLAG in set to 0 for some reason in catacombs_test_path.xml.cooked
                        // and caves_d_path.xml.cooked - old version cooking?
                        //Debug.Assert((bool)xmlCookElement.DefaultValue != bValue);
                        break;

                    case ElementType.Table: // 0x0308
                    case ElementType.TableCount: // 0x030A
                        _ReadTable(rootElement, xmlCookElement, elements);
                        break;

                    case ElementType.ExcelIndex: // 0x0903
                        _ReadExcelIndex(rootElement, xmlCookElement);
                        break;

                    case ElementType.FloatArray: // 0x0106
                        for (int fIndex = 0; fIndex < xmlCookElement.Count; fIndex++)
                        {
                            _ReadFloat(rootElement, xmlCookElement.Name);
                        }
                        break;

                    case ElementType.UnknownPType: // 0x0007
                        Int32 pValue = _ReadInt32(rootElement, xmlCookElement.Name);
                        //Debug.Assert((Int32)xmlCookElement.DefaultValue != pValue);
                        break;

                    case ElementType.RGBADoubleWordArray: // 0x0006     // found in colorsets.xml.cooked pdwColors
                        for (int dwIndex = 0; dwIndex < xmlCookElement.Count; dwIndex++)
                        {
                            _ReadUInt32(rootElement, xmlCookElement.Name);
                        }
                        break;

                    default:
                        Debug.Assert(false, "ElementType not set!");
                        break;
                }
            }

            // needs to be added above so we have access to it in _ReadExcelIndex()
            // so if empty here, then we remove it
            if (rootElement.ChildNodes.Count == 0 && rootElement.Name != xmlDefinition.RootElement)
            {
                xmlParent.RemoveChild(rootElement);
            }
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

        private Hashtable _UncookFileXmlElements(XmlDefinition xmlDefinition, int xmlDefElementCount)
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

            Hashtable elements = new Hashtable();
            for (int i = 0; i < xmlDefElementCount && _offset < _data.Length; i++)
            {
                uint elementHash = FileTools.ByteArrayToUInt32(_data, ref _offset);

                XmlCookElement xmlCookElement = _GetXmlCookElement(xmlDefinition, elementHash);
                if (xmlCookElement == null) throw new Exceptions.UnexpectedTokenException("Unexpected xml element hash: 0x" + elementHash.ToString("X8"));
                elements.Add(elementHash, xmlCookElement);

                ElementType token = (ElementType)FileTools.ByteArrayToUShort(_data, ref _offset);
                switch (token)
                {
                    case ElementType.UnknownPTypeD:                                     // 0x0D00
                        // nothing to do                                                // found in paths
                        break;                                                          // only token then next element


                    case ElementType.String:                                            // 0x0200
                        String str = _ReadByteString();                                 // default value
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
                        UInt32 bitMask = _ReadUInt32(null, null);                       // bit mask

                        Debug.Assert((bool)xmlCookElement.DefaultValue == defaultFlagged);
                        Debug.Assert(xmlCookElement.BitMask == bitMask);
                        break;


                    case ElementType.BitFlag:                                           // 0x0C02
                        Int32 bitIndex = _ReadInt32(null, null);                        // bit index
                        Int32 totalCount = _ReadInt32(null, null);                      // total count?

                        Debug.Assert(xmlCookElement.BitIndex == bitIndex);
                        break;


                    case ElementType.ExcelIndex:                                        // 0x0903
                        Int32 excelTableCode = _ReadInt32(null, null);                  // excel table code

                        Debug.Assert(xmlCookElement.ExcelTableCode == excelTableCode);
                        break;


                    case ElementType.FloatArray:                                        // 0x0106
                        float defaultFloatArr = _ReadFloat(null, null);                 // default value
                        Int32 arrayCount = _ReadInt32(null, null);                      // count

                        Debug.Assert((float)xmlCookElement.DefaultValue == defaultFloatArr);
                        Debug.Assert(xmlCookElement.Count == arrayCount);
                        break;


                    case ElementType.Table:                                             // 0x0308
                    case ElementType.TableCount:                                        // 0x030A
                        UInt32 stringHash = _ReadUInt32(null, null);                    // table string hash
                        Int32 elementCount = _ReadInt32(null, null);                    // table element count

                        XmlDefinition tableXmlDefition = _GetXmlDefinition(stringHash);
                        if (tableXmlDefition == null ||
                            tableXmlDefition.ElementCount < elementCount)
                            throw new Exceptions.NotSupportedFileDefinitionException();

                        Hashtable childElements = _UncookFileXmlElements(tableXmlDefition, elementCount);
                        elements[elementHash] = childElements;
                        break;

                    case ElementType.RGBADoubleWordArray:                                    // 0x0006   // found in colorsets.xml.cooked
                        UInt32 defaultDoubleWord = _ReadUInt32(null, null);
                        Int32 arraySize = _ReadInt32(null, null);

                        Debug.Assert((UInt32)xmlCookElement.DefaultValue == defaultDoubleWord);
                        Debug.Assert(xmlCookElement.Count == arraySize);
                        break;

                    default:
                        throw new Exceptions.UnexpectedTokenException(
                            "Unexpected .xml.cooked definition array token: 0x" + ((ushort)token).ToString("X4"));
                }
            }

            return elements;
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

            Debug.Assert(descriptionNode != null);
            if (!String.IsNullOrEmpty(descriptionNode.InnerText)) descriptionNode.InnerText += ", ";
            descriptionNode.InnerText += excelString;

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

                _UncookXmlDefinition(xmlCountDefinition, parentNode, childElements);
            }
        }

        private bool _ReadBitField(XmlNode parentNode, XmlDefinition xmlDefinition, XmlCookElement xmlCookElement)
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

        private String _ReadByteString()
        {
            byte strLen = _data[_offset++];
            if (strLen == 0xFF || strLen == 0x00) return null;

            return FileTools.ByteArrayToStringASCII(_data, ref _offset, strLen);
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

        /// <summary>
        /// Save the uncooked XML Document.
        /// </summary>
        /// <param name="path">The path to save the XML Document.</param>
        public void SaveXml(String path)
        {
            if (XmlDoc == null || String.IsNullOrEmpty(path)) return;

            string directory = Path.GetDirectoryName(path);
            if (!(Directory.Exists(directory)))
            {
                Directory.CreateDirectory(directory);
            }

            XmlDoc.Save(path);
        }
    }
}