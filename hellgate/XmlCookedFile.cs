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
                XmlRootElementCount = xmlDefinition.Count
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
                    case ElementType.UnknownPTypeD_0x0D00: // 0x0D00
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

                    case ElementType.BitFlag: // 0x0C02
                        bool bitFlagIsFlagged = elementText == "0" ? false : true;
                        if ((bool)xmlCookElement.DefaultValue == bitFlagIsFlagged) continue;

                        if (xmlDefinition.BitFlagsWriteOffset == -1)
                        {
                            int intCount = xmlDefinition.BitFlags.Length;

                            xmlDefinition.BitFlagsWriteOffset = offset;
                            offset += intCount * sizeof(UInt32);
                        }

                        int intIndex = xmlCookElement.BitIndex >> 5;
                        int bitFlagIndex = xmlCookElement.BitIndex - (intIndex << 5);
                        UInt32 bitFlagField = xmlDefinition.BitFlags[intIndex] | ((UInt32)1 << bitFlagIndex);
                        xmlDefinition.BitFlags[intIndex] = bitFlagField;

                        int bitFlagWriteOffset = xmlDefinition.BitFlagsWriteOffset + (intIndex << 2);
                        FileTools.WriteToBuffer(ref buffer, ref bitFlagWriteOffset, bitFlagField);
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
        private static int _CookXmlDefinition(ref byte[] buffer, int offset, XmlDefinition xmlDefinition, ICollection<UInt32> cookedDefinitions)
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
                    case ElementType.UnknownPTypeD_0x0D00:         // 0x0D00
                        break;

                    // 4 bytes
                    case ElementType.Int32:                 // 0x0000
                    case ElementType.Float:                 // 0x0100
                    case ElementType.UnknownFloatT:         // 0x0600
                    case ElementType.NonCookedInt32:        // 0x0700
                    case ElementType.UnknownFloat:          // 0x0800
                    case ElementType.UnknownPType:          // 0x0007
                    case ElementType.ByteArray:             // 0x1007   // found in TEXTURE_DEFINITION child BLEND_RUN
                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.DefaultValue);
                        break;

                    // 1 + x bytes
                    case ElementType.String:                // 0x0200
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
                    case ElementType.Flag:                  // 0x0B01
                        Int32 defaultFlagged = (bool)xmlCookElement.DefaultValue ? 1 : 0;
                        FileTools.WriteToBuffer(ref buffer, ref offset, defaultFlagged);
                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.BitMask);
                        break;

                    // 8 bytes
                    case ElementType.BitFlag:               // 0x0C02
                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.BitIndex);
                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.BitCount);
                        break;

                    // 8 bytes
                    case ElementType.Table:                 // 0x0308
                    case ElementType.TableCount:            // 0x030A
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

                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlSubDefinition.Count);

                        int bytesWritten = _CookXmlDefinition(ref buffer, offset, xmlSubDefinition, cookedDefinitions);
                        if (bytesWritten == 0) return 0;
                        offset += bytesWritten;
                        break;

                    // 4 bytes
                    case ElementType.ExcelIndex:            // 0x0903
                        FileTools.WriteToBuffer(ref buffer, ref offset, xmlCookElement.ExcelTableCode);
                        break;

                    // 8 bytes
                    case ElementType.RGBADoubleWordArray:   // 0x0006     // colorsets.xml.cooked (pdwColors)
                    case ElementType.FloatArray:            // 0x0106
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
             * UInt32           Root XML Element
             * Int32            Element Count
             */

            // check file infos
            XmlCookFileHeader header = FileTools.ByteArrayToStructure<XmlCookFileHeader>(_data, ref _offset);

            if (header.MagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();
            if (header.Version != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();

            XmlDefinition xmlDefinition = _GetXmlDefinition(header.XmlRootDefinition);
            if (xmlDefinition == null) throw new Exceptions.NotSupportedFileDefinitionException();
            if (xmlDefinition.Count < header.XmlRootElementCount) throw new Exceptions.NotSupportedXMLElementCount(xmlDefinition.RootElement);


            XmlDoc = new XmlDocument();
            XmlCookedFileTree xmlTree = new XmlCookedFileTree(xmlDefinition);
            _UncookFileXmlDefinition(xmlDefinition, header.XmlRootElementCount, xmlTree);

            UInt32 dataMagicWord = FileTools.ByteArrayToUInt32(_data, ref _offset);
            if (dataMagicWord != DataMagicWord) throw new Exceptions.UnexpectedTokenException("'DATA' Token MagicWord expected but not found!");

            _UncookXmlData(xmlDefinition, XmlDoc, xmlTree);

            Debug.Assert(_offset == _data.Length);

            return true;
        }

        private void _UncookXmlData(XmlDefinition xmlDefinition, XmlNode xmlParent, XmlCookedFileTree xmlTree)
        {
            XmlElement rootElement = XmlDoc.CreateElement(xmlDefinition.RootElement);
            xmlParent.AppendChild(rootElement);

            // actual present element count - not total count possible
            // i.e. some definitions have fields that aren't always present (e.g. TCv4), but those fields aren't counted unless they appear in the xml definition header
            int elementCount = xmlTree.Count;

            // bitField info
            int bitFieldOffset = _offset;
            int bitFieldByteCount = (elementCount - 1 >> 3) + 1; // -1 as 16 >> 3 = 2 + 1 = 3, but should only be 2 bytes
            _offset += bitFieldByteCount;


            // loop through elements
            //for (int i = 0; i < xmlDefinition.Count; i++)
            for (int i = 0; i < elementCount; i++)
            {
                // is the field present?
                if (!_TestBit(_data, bitFieldOffset, i)) continue;

                //XmlCookElement xmlCookElement = xmlDefinition[i];
                XmlCookElement xmlCookElement = xmlTree[i];

                // sanity check - ensure it was in the definition segment
                Debug.Assert(xmlTree.ContainsElement(xmlCookElement.NameHash));

                //if (xmlCookElement.Name == "tAttachmentDef.eType")
                //{
                //    int bp = 0;
                //});

                switch (xmlCookElement.ElementType)
                {
                    case ElementType.Int32:                         // 0x0000
                        Int32 iValue = _ReadInt32(rootElement, xmlCookElement.Name);
                        Debug.Assert((Int32)xmlCookElement.DefaultValue != iValue);
                        break;

                    case ElementType.RGBADoubleWordArray:           // 0x0006     // found in colorsets.xml.cooked pdwColors
                        for (int dwIndex = 0; dwIndex < xmlCookElement.Count; dwIndex++)
                        {
                            _ReadUInt32(rootElement, xmlCookElement.Name);
                        }
                        break;

                    case ElementType.UnknownPType:                  // 0x0007
                        Int32 pValue = _ReadInt32(rootElement, xmlCookElement.Name);
                        Debug.Assert((Int32)xmlCookElement.DefaultValue != pValue);
                        break;

                    case ElementType.Float:                         // 0x0100
                        float fValue = _ReadFloat(rootElement, xmlCookElement.Name);
                        Debug.Assert((float)xmlCookElement.DefaultValue != fValue);
                        break;

                    case ElementType.FloatArray:                    // 0x0106
                        for (int fIndex = 0; fIndex < xmlCookElement.Count; fIndex++)
                        {
                            _ReadFloat(rootElement, xmlCookElement.Name);
                        }
                        break;

                    case ElementType.FloatArrayUnknown_0x0107:
                        _ReadVariableLengthFloatArray(rootElement, xmlCookElement);
                        break;

                    case ElementType.String:                        // 0x0200
                        String szValue = _ReadZeroString(rootElement, xmlCookElement);
                        Debug.Assert((String)xmlCookElement.DefaultValue != szValue);
                        break;

                    case ElementType.StringArray_0x0206:
                        for (int strIndex = 0; strIndex < xmlCookElement.Count; strIndex++)
                        {
                            _ReadZeroString(rootElement, xmlCookElement);
                        }
                        break;

                    case ElementType.StringArrayUnknown_0x0207:
                        _ReadZeroStringArray(rootElement, xmlCookElement);
                        break;

                    case ElementType.Table:                         // 0x0308
                    case ElementType.TableCount:                    // 0x030A
                        _ReadTable(rootElement, xmlCookElement, xmlTree.GetTree(i));
                        break;

                    case ElementType.UnknownFloatT:                 // 0x0600
                        // todo: not tested with HGL cooking it
                        _ReadTFloatArray(rootElement, xmlCookElement);
                        break;


                    // todo: any point this being here?
                    case ElementType.NonCookedInt32:                // 0x0700
                    case ElementType.UnknownFloat:                  // 0x0800
                    case ElementType.UnknownPTypeD_0x0D00:          // 0x0D00
                        int bp = 0;
                        break;

                    case ElementType.ExcelIndex:                    // 0x0903
                        _ReadExcelIndex(rootElement, xmlCookElement);
                        break;

                    case ElementType.ExcelIndexArray_0x0905:        // 0x0905
                        for (int j = 0; j < xmlCookElement.Count; j++)
                        {
                            _ReadExcelIndex(rootElement, xmlCookElement);
                        }
                        break;

                    case ElementType.Flag:                          // 0x0B01
                        bool flagValue = _ReadFlag(rootElement, xmlDefinition, xmlCookElement);
                        Debug.Assert((bool)xmlCookElement.DefaultValue != flagValue);
                        break;

                    case ElementType.BitFlag:                       // 0x0C02
                        bool bitFlagValue = _ReadBitFlag(rootElement, xmlDefinition, xmlCookElement);
                        Debug.Assert((bool)xmlCookElement.DefaultValue != bitFlagValue);

                        //if (xmlCookElement.Name == "ROOM_PATH_NODE_DEF_INDOOR_FLAG")
                        //{
                        //    int b2p = 0;
                        //}

                        // note: do flags need to be included for cooking no matter their default value??
                        // ROOM_PATH_NODE_DEF_INDOOR_FLAG in set to 0 for some reason in catacombs_test_path.xml.cooked
                        // and caves_d_path.xml.cooked - old version cooking?
                        //Debug.Assert((bool)xmlCookElement.DefaultValue != bValue);
                        break;

                    case ElementType.ByteArray:                     // 0x1007
                        UInt32 byteArraySize = _ReadUInt32(null, null);

                        int bp1 = 0;

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

        /// <summary>
        /// Reads the .xml.cooked header xml definition segment.
        /// Determines what elements and definitions are in use, and returns them as a tree-like hash table.
        /// </summary>
        /// <param name="xmlDefinition">The base XML Definition to check for elements.</param>
        /// <param name="xmlDefElementCount">The number of elements expect to be in use.</param>
        /// <returns>Hashtable of elements in use within the xml file.</returns>
        private void _UncookFileXmlDefinition(XmlDefinition xmlDefinition, int xmlDefElementCount, XmlCookedFileTree xmlTree)
        {
            /* Default Element Values Array & General Structure
             * =Token=		=Type=          =FoundIn=       =Details= (* = variable length)
             * 0x0000       Int32           Skills          4 bytes     e.g. tAttachmentDef.eType = -1 in some cases - also used with tAttachmentDef.dwFlags
             * 0x0100       Float           Skills          4 bytes     Float value
             * 0x0106       Float Array     AI              4 bytes     First 4 bytes (Float)  = Default Value, Second 4 bytes (Int32) = Array Size
             * 0x0200       String          Skills          1 byte*     Str Length (NOT inc \0), then if != 0, string WITH \0 follows
             * 0x030A       TableCount      Skills          8 bytes     First 4 bytes (UInt32) = XML Definition Hash, Second 4 bytes (Int32) = XML Definition Element Count
             * 0x0500       Float           Appearance      4 bytes     Same as Float 0x0100
             * 0x0600       RGBA dwArray    Colorsets       8 bytes     First 4 bytes (UInt32) = Default Value, Second 4 bytes (Int32) = Array Size
             * 0x0700       NonCookedInt32  Skills          4 bytes     Used for nPreviewAppearance, but doesn't appear to be cooked by game
             * 0x0803       Table           Skills          8 bytes     First 4 bytes (UInt32) = XML Definition Hash, Second 4 bytes (Int32) = XML Definition Element Count - this always exists in cooked file, below TableCount type may not
             * 0x0903       ExcelIndex      Skills          4 bytes     First 4 bytes (UInt32) = Code of Excel Table - Cooking reads index and places from table
             * 0x0A00       Int32           Appearance      4 bytes     Same as Int32 0x0000
             * 0x0B01       Flag            Skills          4 bytes*    First 4 bytes (UInt32) = Bitmask of Flag in field (In DATA segment, Flag is only read in once per "chunk" i.e. After 32 flags, the next flag will be read 4 bytes again)
             * 0x0C02       BitFlag         Skills          8 bytes*    First 4 bytes (Int32)  = Bit Index of Flag in field, Second 4 bytes (Int32) = Total bit count of fields, BitFlag is read as int array, 32 bits per int
             * 0x1007       Byte Array      Textures        4 bytes     First 4 bytes (??) = ??
             */

            for (int i = 0; i < xmlDefElementCount && _offset < _data.Length; i++)
            {
                uint elementHash = FileTools.ByteArrayToUInt32(_data, ref _offset);

                XmlCookElement xmlCookElement = _GetXmlCookElement(xmlDefinition, elementHash);
                if (xmlCookElement == null) throw new Exceptions.UnexpectedTokenException("Unexpected xml element hash: 0x" + elementHash.ToString("X8"));
                xmlTree.AddElement(xmlCookElement);

                ElementType token = (ElementType)FileTools.ByteArrayToUShort(_data, ref _offset);
                switch (token)
                {
                    case ElementType.UnknownPTypeD_0x0D00:                              // 0x0D00
                        // nothing to do                                                // found in paths
                        break;                                                          // only token then next element


                    case ElementType.String:                                            // 0x0200
                    case ElementType.StringArrayUnknown_0x0207:                         // 0x0207   // not sure of structure as non-default, but as default has same as String
                        String strValue = _ReadByteString();                            // default value
                        if (strValue != null) _offset++; // +1 for \0

                        Debug.Assert((String)xmlCookElement.DefaultValue == strValue);
                        break;

                    case ElementType.StringArray_0x0206:                                // 0x0206
                        String strArrValue = _ReadByteString();                         // default value
                        if (strArrValue != null) _offset++; // +1 for \0
                        Int32 strArrCount = _ReadInt32(null, null);

                        Debug.Assert((String)xmlCookElement.DefaultValue == strArrValue);
                        Debug.Assert(xmlCookElement.Count == strArrCount);
                        break;


                    case ElementType.Int32:                                             // 0x0000
                    case ElementType.UnknownPType:                                      // 0x0007
                    case ElementType.NonCookedInt32:                                    // 0x0700
                    case ElementType.Int32_0x0A00:                                      // 0x0A00
                        Int32 defaultInt32 = _ReadInt32(null, null);                    // default value

                        Debug.Assert((Int32)xmlCookElement.DefaultValue == defaultInt32);
                        break;


                    case ElementType.RGBADoubleWordArray:                               // 0x0006   // found in colorsets.xml.cooked
                        UInt32 defaultDoubleWord = _ReadUInt32(null, null);
                        Int32 arraySize = _ReadInt32(null, null);

                        Debug.Assert((UInt32)xmlCookElement.DefaultValue == defaultDoubleWord);
                        Debug.Assert(xmlCookElement.Count == arraySize);
                        break;


                    case ElementType.Float:                                             // 0x0100
                    case ElementType.FloatArrayUnknown_0x0107:                          // 0x0107   // not sure of structure as non-default, but as default has same as Float
                    case ElementType.Float_0x0500:                                      // 0x0500
                    case ElementType.UnknownFloatT:                                     // 0x0600 //materials "tScatterColor"
                    case ElementType.UnknownFloat:                                      // 0x0800
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
                        Int32 bitCount = _ReadInt32(null, null);                        // bit count

                        Debug.Assert(xmlCookElement.BitIndex == bitIndex);
                        Debug.Assert(xmlCookElement.BitCount == bitCount);
                        break;


                    case ElementType.ExcelIndex:                                        // 0x0903
                        Int32 excelTableCode = _ReadInt32(null, null);                  // excel table code

                        Debug.Assert(xmlCookElement.ExcelTableCode == excelTableCode);
                        break;

                    case ElementType.ExcelIndexArray_0x0905:                            // 0x0905
                        Int32 excelTableArrCode = _ReadInt32(null, null);               // excel table code
                        Int32 excelTableArrCount = _ReadInt32(null, null);

                        Debug.Assert(xmlCookElement.ExcelTableCode == excelTableArrCode);
                        Debug.Assert(xmlCookElement.Count == excelTableArrCount);
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
                        if (tableXmlDefition == null) throw new Exceptions.NotSupportedFileDefinitionException();
                        if (tableXmlDefition.Count < elementCount) throw new Exceptions.NotSupportedXMLElementCount(tableXmlDefition.RootElement);

                        if (elementCount == -1)
                        {
                            xmlTree.AddExistingTree(stringHash);
                        }
                        else
                        {
                            XmlCookedFileTree xmlChildTree = new XmlCookedFileTree(tableXmlDefition, xmlCookElement, xmlTree);
                            xmlTree.AddTree(xmlChildTree);
                            _UncookFileXmlDefinition(tableXmlDefition, elementCount, xmlChildTree);
                        }
                        break;


                    case ElementType.Int32Array_0x0A06:                                 // 0x0A06
                        Int32 int32ArrDef = _ReadInt32(null, null);
                        Int32 int32ArrCount = _ReadInt32(null, null);

                        Debug.Assert((Int32)xmlCookElement.DefaultValue == int32ArrDef);
                        Debug.Assert(xmlCookElement.Count == int32ArrCount);
                        break;


                    case ElementType.ByteArray:                                         // 0x1007   // found in textures
                        Int32 unknown = _ReadInt32(null, null);

                        Debug.Assert((Int32)xmlCookElement.DefaultValue == unknown);
                        break;


                    default:
                        throw new Exceptions.UnexpectedTokenException(
                            "Unexpected .xml.cooked definition array token: 0x" + ((ushort)token).ToString("X4"));
                }
            }
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