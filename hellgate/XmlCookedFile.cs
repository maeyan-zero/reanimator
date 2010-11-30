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
        public const String FileExtention = ".xml.cooked";
        private const UInt32 FileMagicWord = 0x6B304F43; // 'CO0k'
        private const Int32 RequiredVersion = 8;
        private const UInt32 DataMagicWord = 0x41544144;
        private static XmlDefinition[] _xmlDefinitions;
        private static FileManager _fileManager;

        private int _offset;
        private byte[] _buffer;
        public XmlDocument XmlDoc { get; private set; }

        public XmlCookedFile()
        {
        }

        /// <summary>
        /// Cooks an XML document to a .xml.cooked.
        /// </summary>
        /// <param name="xmlDocument">An XML Document to Cook.</param>
        /// <returns>The cooked bytes, or null upon failure.</returns>
        public byte[] CookXmlDocument(XmlDocument xmlDocument)
        {
            if (_xmlDefinitions == null) throw new Exceptions.NotInitializedException();
            if (!xmlDocument.HasChildNodes) return null;

            XmlNode rootElement = xmlDocument.FirstChild;
            UInt32 xmlDefinitionHash = Crypt.GetStringHash(rootElement.Name);

            XmlDefinition xmlDefinition = _GetXmlDefinition(xmlDefinitionHash);
            if (xmlDefinition == null) throw new Exceptions.NotSupportedFileDefinitionException();

            _offset = 0;
            _buffer = new byte[1024];

            // header details //
            XmlCookFileHeader xmlCookFileHeader = new XmlCookFileHeader
            {
                MagicWord = FileMagicWord,
                Version = RequiredVersion,
                XmlRootDefinition = xmlDefinitionHash,
                XmlRootElementCount = xmlDefinition.Count
            };
            FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookFileHeader);


            // write default element array //
            List<UInt32> cookedDefinitions = new List<UInt32>();
            int bytesWritten = _CookXmlDefinition(xmlDefinition, cookedDefinitions);
            if (bytesWritten == 0) return null;


            // write data segment //
            FileTools.WriteToBuffer(ref _buffer, ref _offset, DataMagicWord);
            XmlNode xmlNode = xmlDocument.FirstChild;
            bytesWritten = _CookXmlData(xmlDefinition, xmlNode);
            if (bytesWritten == -1) return null;

            byte[] cookedBytes = new byte[_offset];
            Buffer.BlockCopy(_buffer, 0, cookedBytes, 0, _offset);

            return cookedBytes;
        }

        private int _CookXmlData(XmlDefinition xmlDefinition, XmlNode xmlNode)
        {
            int offsetStart = _offset;
            int elementCount = xmlDefinition.Elements.Count;

            // bitField info
            int bitFieldOffset = _offset;
            int bitFieldByteCount = (elementCount - 1 >> 3) + 1; // -1 as 16 >> 3 = 2 + 1 = 3, but should only be 2 bytes, and +1 as 7 >> 3 = 0
            _offset += bitFieldByteCount;

            // todo: this is a bit excessive for a couple of offsets, but meh, fix it later
            Hashtable bitFieldOffsts = new Hashtable();
            int bitIndex = -1;
            bool bpTest = true;
            foreach (XmlCookElement xmlCookElement in xmlDefinition.Elements)
            {
                if (bpTest && _offset >= 0x6AA)
                {
                    int bp = 0;
                    bpTest = false;
                }


                bitIndex++;

                // ElementType.Table must be cooked - so xmlNode might be null if not defined
                if (xmlNode == null)
                {
                    // only things written in for blank tables
                    switch (xmlCookElement.ElementType)
                    {
                        case ElementType.ExcelIndex:
                            const byte noEntry = 0xFF;
                            FileTools.WriteToBuffer(ref _buffer, ref _offset, noEntry);
                            _FlagBit(_buffer, bitFieldOffset, bitIndex);
                            break;

                        case ElementType.TableCount:
                            const Int32 zeroTables = 0;
                            FileTools.WriteToBuffer(ref _buffer, ref _offset, zeroTables);
                            _FlagBit(_buffer, bitFieldOffset, bitIndex);
                            break;
                    }

                    continue;
                }

                // todo: name replace Name with TrueName when saved and when uncooking
                String elementName = xmlCookElement.Name;
                if (xmlCookElement.ElementType == ElementType.TableCount ||
                    xmlCookElement.ElementType == ElementType.UnknownFloatT ||
                    xmlCookElement.ElementType == ElementType.StringArrayVariable ||
                    xmlCookElement.ElementType == ElementType.FloatArrayVariable ||
                    xmlCookElement.ElementType == ElementType.FloatTripletArrayVariable)
                {
                    elementName += "Count";
                }

                XmlElement xmlElement = xmlNode[elementName];
                String elementText = String.Empty;
                if (xmlElement == null &&
                    xmlCookElement.ElementType != ElementType.ExcelIndex &&             // want excel index even if null/empty
                    xmlCookElement.ElementType != ElementType.ExcelIndexArrayFixed &&   // want excel index even if null/empty
                    xmlCookElement.ElementType != ElementType.Table &&                  // want table even if null/empty
                    xmlCookElement.ElementType != ElementType.TableCount &&             // want table count even if null/empty
                    xmlCookElement.ElementType != ElementType.ByteArrayVariable &&      // need count written even if null/empty
                    xmlCookElement.ElementType != ElementType.StringArrayVariable &&    // need count written even if null/empty
                    xmlCookElement.ElementType != ElementType.FloatArrayVariable        // need count written even if null/empty
                    ) continue;

                if (xmlElement != null)
                {
                    elementText = xmlElement.InnerText;
                }

                switch (xmlCookElement.ElementType)
                {
                    case ElementType.NonCookedInt32:                    // 0x0700
                    case ElementType.UnknownFloat:                      // 0x0800
                    case ElementType.UnknownPTypeD_0x0D00:              // 0x0D00
                        int bp = 0;     // not cooked.... (I think...)
                        continue;

                    case ElementType.Int32:                             // 0x0000
                        _WriteInt32(elementText, xmlCookElement);
                        break;

                    case ElementType.Int32ArrayFixed:                   // 0x0006       // found in colorsets.xml.cooked
                        _WriteInt32ArrayFixed(xmlCookElement, xmlNode);
                        break;

                    case ElementType.Float:                             // 0x0100
                        _WriteFloat(elementText, xmlCookElement);
                        break;

                    case ElementType.FloatArrayFixed:                   // 0x0106
                        _WriteFloatArrayFixed(xmlCookElement, xmlNode);
                        break;

                    case ElementType.FloatArrayVariable:                // 0x0107
                        _WriteFloatArrayVariable(xmlCookElement, xmlNode, elementText);
                        break;

                    case ElementType.String:                            // 0x0200
                        _WriteString(elementText, xmlCookElement, xmlElement);
                        break;

                    case ElementType.StringArrayFixed:                  // 0x0206
                        _WriteStringArrayFixed(xmlCookElement, xmlNode);
                        break;

                    case ElementType.StringArrayVariable:               // 0x0207
                        _WriteStringArrayVariable(xmlCookElement, xmlNode, elementText);
                        break;

                    case ElementType.Table:                             // 0x0308
                        if (_WriteTable(xmlCookElement, xmlElement) == -1) return -1;
                        break;

                    case ElementType.TableCount:                        // 0x030A
                        if (_WriteTableCount(elementText, xmlCookElement, xmlNode) == -1) return -1; // we found less tables than we were supposed to cook
                        break;

                    case ElementType.FloatTripletArrayVariable:         // 0x0500
                        _WriteFloatTripletArrayVariable(xmlCookElement, xmlNode, elementText);
                        break;

                    case ElementType.ExcelIndexArrayFixed:              // 0x0509
                        _WriteExcelIndexArrayFixed(xmlCookElement, xmlNode);
                        break;

                    case ElementType.UnknownFloatT:                     // 0x0600
                        _WriteUnknownFloatT(elementText, xmlCookElement, xmlNode);
                        break;

                    case ElementType.ExcelIndex:                        // 0x0903
                        _WriteExcelIndex(elementText, xmlCookElement);
                        break;

                    case ElementType.Flag:                              // 0x0B01
                        _WriteFlag(elementText, xmlCookElement, xmlDefinition, bitFieldOffsts);
                        break;

                    case ElementType.BitFlag:                           // 0x0C02
                        _WriteBitFlag(elementText, xmlCookElement, xmlDefinition);
                        break;

                    case ElementType.ByteArrayVariable:                         // 0x1007
                        _WriteByteArrayVariable(elementText, xmlCookElement);
                        break;

                    default:
                        Debug.Assert(false, "ElementType not set!");
                        break;
                }

                _FlagBit(_buffer, bitFieldOffset, bitIndex);
            }

            return _offset - offsetStart;
        }

        /// <summary>
        /// Cooks an XML Definition and its children into the supplied byte array.
        /// </summary>
        /// <param name="xmlDefinition">The XML Definition to cook.</param>
        /// <param name="cookedDefinitions">A list to track already cooked XML Definitions (stops recursion).</param>
        /// <returns>The amount of bytes written.</returns>
        private int _CookXmlDefinition(XmlDefinition xmlDefinition, ICollection<UInt32> cookedDefinitions)
        {
            cookedDefinitions.Add(xmlDefinition.RootHash);

            int offsetStart = _offset;
            foreach (XmlCookElement xmlCookElement in xmlDefinition.Elements)
            {
                // write name hash and token
                FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookElement.NameHash);
                FileTools.WriteToBuffer(ref _buffer, ref _offset, (ushort)xmlCookElement.ElementType);

                switch (xmlCookElement.ElementType)
                {
                    // 0 bytes
                    case ElementType.UnknownPTypeD_0x0D00:          // 0x0D00
                        break;

                    // 4 bytes
                    case ElementType.Int32:                         // 0x0000
                    case ElementType.Int32ArrayVariable:                  // 0x0007
                    case ElementType.Float:                         // 0x0100
                    case ElementType.FloatArrayVariable:      // 0x0107
                    case ElementType.FloatTripletArrayVariable:                  // 0x0500
                    case ElementType.UnknownFloatT:                 // 0x0600
                    case ElementType.NonCookedInt32:                // 0x0700
                    case ElementType.UnknownFloat:                  // 0x0800
                    case ElementType.Int32_0x0A00:                  // 0x0A00
                    case ElementType.ByteArrayVariable:                     // 0x1007       // found in TEXTURE_DEFINITION child BLEND_RUN
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookElement.DefaultValue);
                        break;

                    // 1 + x bytes
                    case ElementType.String:                        // 0x0200
                    case ElementType.StringArrayVariable:     // 0x0207       // AppearanceDefinition - not tested DefaultValue != null
                        String strDefault = xmlCookElement.DefaultValue as String;
                        byte strLen = (byte)((strDefault == null) ? 0 : strDefault.Length);
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, strLen);

                        if (xmlCookElement.DefaultValue == null) continue;

                        byte[] stringBytes = FileTools.StringToASCIIByteArray(xmlCookElement.DefaultValue.ToString());
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, stringBytes);
                        _offset++; // \0
                        break;

                    // 1 + x bytes + 4 bytes
                    case ElementType.StringArrayFixed:            // 0x0206
                        String strArrDefault = xmlCookElement.DefaultValue as String;
                        byte strArrStrLen = (byte)((strArrDefault == null) ? 0 : strArrDefault.Length);
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, strArrStrLen);

                        if (xmlCookElement.DefaultValue != null)
                        {
                            byte[] strArrStringBytes = FileTools.StringToASCIIByteArray(xmlCookElement.DefaultValue.ToString());
                            FileTools.WriteToBuffer(ref _buffer, ref _offset, strArrStringBytes);
                            _offset++; // \0
                        }

                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookElement.Count);
                        break;

                    // 8 bytes
                    case ElementType.Flag:                          // 0x0B01
                        Int32 defaultFlagged = (bool)xmlCookElement.DefaultValue ? 1 : 0;
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, defaultFlagged);
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookElement.BitMask);
                        break;

                    // 8 bytes
                    case ElementType.BitFlag:                       // 0x0C02
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookElement.BitIndex);
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookElement.BitCount);
                        break;

                    // 8 bytes
                    case ElementType.Table:                         // 0x0308
                    case ElementType.TableCount:                    // 0x030A
                        XmlDefinition xmlSubDefinition = _GetXmlDefinition(xmlCookElement.ChildTypeHash);
                        if (xmlSubDefinition == null) throw new Exceptions.NotSupportedFileDefinitionException();

                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookElement.ChildTypeHash);

                        // make sure we haven't already cooked it
                        if (cookedDefinitions.Contains(xmlCookElement.ChildTypeHash))
                        {
                            const Int32 alreadyCooked = -1;
                            FileTools.WriteToBuffer(ref _buffer, ref _offset, alreadyCooked);
                            break;
                        }

                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlSubDefinition.Count);

                        int bytesWritten = _CookXmlDefinition(xmlSubDefinition, cookedDefinitions);
                        if (bytesWritten == 0) return 0;
                        break;

                    // 4 bytes
                    case ElementType.ExcelIndex:                    // 0x0903
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookElement.ExcelTableCode);
                        break;

                    // 8 bytes
                    case ElementType.ExcelIndexArrayFixed:        // 0x0905
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookElement.ExcelTableCode);
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookElement.Count);
                        break;

                    // 8 bytes
                    case ElementType.Int32ArrayFixed:           // 0x0006       // colorsets.xml.cooked (pdwColors)
                    case ElementType.FloatArrayFixed:                    // 0x0106
                    case ElementType.Int32Array_0x0A06:             // 0x0A06       // AppearanceDefinition
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookElement.DefaultValue);
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookElement.Count);
                        break;

                    default:
                        Debug.Assert(false, "ElementType not set!");
                        break;
                }
            }

            return _offset - offsetStart;
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
            _buffer = data;

            /* Header Structure
             * UInt32           Magic Word          'CO0k'
             * Int32            Version             Must be 8
             * UInt32           Root XML Element
             * Int32            Element Count
             */

            // check file infos
            XmlCookFileHeader header = FileTools.ByteArrayToStructure<XmlCookFileHeader>(_buffer, ref _offset);

            if (header.MagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();
            if (header.Version != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();

            XmlDefinition xmlDefinition = _GetXmlDefinition(header.XmlRootDefinition);
            if (xmlDefinition == null) throw new Exceptions.NotSupportedFileDefinitionException();
            if (xmlDefinition.Count < header.XmlRootElementCount) throw new Exceptions.NotSupportedXMLElementCount(xmlDefinition.RootElement);


            XmlDoc = new XmlDocument();
            XmlCookedFileTree xmlTree = new XmlCookedFileTree(xmlDefinition);
            _UncookFileXmlDefinition(xmlDefinition, header.XmlRootElementCount, xmlTree);

            UInt32 dataMagicWord = FileTools.ByteArrayToUInt32(_buffer, ref _offset);
            if (dataMagicWord != DataMagicWord) throw new Exceptions.UnexpectedTokenException("'DATA' Token MagicWord expected but not found!");

            _UncookXmlData(xmlDefinition, XmlDoc, xmlTree);

            Debug.Assert(_offset == _buffer.Length);

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
            bool bpTest = true;
            for (int i = 0; i < elementCount; i++)
            {
                if (bpTest && _offset >= 870)
                {
                    int bp = 0;
                    bpTest = false;
                }

                // is the field present?
                if (!_TestBit(_buffer, bitFieldOffset, i)) continue;

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

                    case ElementType.Int32ArrayFixed:               // 0x0006     // found in colorsets.xml.cooked pdwColors
                        for (int dwIndex = 0; dwIndex < xmlCookElement.Count; dwIndex++)
                        {
                            _ReadUInt32(rootElement, xmlCookElement.Name);
                        }
                        break;

                    case ElementType.Int32ArrayVariable:            // 0x0007
                        Int32 pValue = _ReadInt32(rootElement, xmlCookElement.Name);
                        Debug.Assert((Int32)xmlCookElement.DefaultValue != pValue);
                        break;

                    case ElementType.Float:                         // 0x0100
                        float fValue = _ReadFloat(rootElement, xmlCookElement.Name);
                        Debug.Assert((float)xmlCookElement.DefaultValue != fValue);
                        break;

                    case ElementType.FloatArrayFixed:               // 0x0106
                        for (int fIndex = 0; fIndex < xmlCookElement.Count; fIndex++)
                        {
                            _ReadFloat(rootElement, xmlCookElement.Name);
                        }
                        break;

                    case ElementType.FloatArrayVariable:            // 0x0107
                        _ReadFloatArrayVariable(rootElement, xmlCookElement);
                        break;

                    case ElementType.String:                        // 0x0200
                        String szValue = _ReadString(rootElement, xmlCookElement);
                        Debug.Assert((String)xmlCookElement.DefaultValue != szValue);
                        break;

                    case ElementType.StringArrayFixed:              // 0x0206
                        for (int strIndex = 0; strIndex < xmlCookElement.Count; strIndex++)
                        {
                            _ReadString(rootElement, xmlCookElement);
                        }
                        break;

                    case ElementType.StringArrayVariable:
                        _ReadStringArray(rootElement, xmlCookElement);
                        break;

                    case ElementType.Table:                         // 0x0308
                    case ElementType.TableCount:                    // 0x030A
                        _ReadTable(rootElement, xmlCookElement, xmlTree.GetTree(i));
                        break;

                    case ElementType.FloatTripletArrayVariable:     // 0x0500
                        _ReadFloatTripletArrayVariable(rootElement, xmlCookElement);    // not tested with HGL (un)cooking
                        break;

                    case ElementType.UnknownFloatT:                 // 0x0600
                        // todo: not tested with HGL cooking it
                        _ReadTFloatArray(rootElement, xmlCookElement);
                        break;


                    // todo: any point this being here?
                    case ElementType.NonCookedInt32:                // 0x0700
                    case ElementType.UnknownFloat:                  // 0x0800
                    case ElementType.UnknownPTypeD_0x0D00:          // 0x0D00
                    case ElementType.Int32_0x0A00:                  // 0x0A00
                        int bp = 0;
                        break;

                    case ElementType.ExcelIndex:                    // 0x0903
                        _ReadExcelIndex(rootElement, xmlCookElement);
                        break;

                    case ElementType.ExcelIndexArrayFixed:          // 0x0905
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

                    case ElementType.ByteArrayVariable:             // 0x1007
                        int byteArraySize = _ReadByteArray(rootElement, xmlCookElement);
                        //Debug.Assert((Int32)xmlCookElement.DefaultValue != byteArraySize); // always written?
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

            xmlDefinition.ResetFields();
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

            for (int i = 0; i < xmlDefElementCount && _offset < _buffer.Length; i++)
            {
                uint elementHash = FileTools.ByteArrayToUInt32(_buffer, ref _offset);

                XmlCookElement xmlCookElement = _GetXmlCookElement(xmlDefinition, elementHash);
                if (xmlCookElement == null) throw new Exceptions.UnexpectedTokenException("Unexpected xml element hash: 0x" + elementHash.ToString("X8"));
                xmlTree.AddElement(xmlCookElement);

                ElementType token = (ElementType)FileTools.ByteArrayToUShort(_buffer, ref _offset);
                switch (token)
                {
                    case ElementType.UnknownPTypeD_0x0D00:                              // 0x0D00
                        // nothing to do                                                // found in paths
                        break;                                                          // only token then next element


                    case ElementType.String:                                            // 0x0200
                    case ElementType.StringArrayVariable:                         // 0x0207   // not sure of structure as non-default, but as default has same as String
                        String strValue = _ReadByteString();                            // default value
                        if (strValue != null) _offset++; // +1 for \0

                        Debug.Assert((String)xmlCookElement.DefaultValue == strValue);
                        break;

                    case ElementType.StringArrayFixed:                                // 0x0206
                        String strArrValue = _ReadByteString();                         // default value
                        if (strArrValue != null) _offset++; // +1 for \0
                        Int32 strArrCount = _ReadInt32(null, null);

                        Debug.Assert((String)xmlCookElement.DefaultValue == strArrValue);
                        Debug.Assert(xmlCookElement.Count == strArrCount);
                        break;


                    case ElementType.Int32:                                             // 0x0000
                    case ElementType.Int32ArrayVariable:                                      // 0x0007
                    case ElementType.NonCookedInt32:                                    // 0x0700
                    case ElementType.Int32_0x0A00:                                      // 0x0A00
                        Int32 defaultInt32 = _ReadInt32(null, null);                    // default value

                        Debug.Assert((Int32)xmlCookElement.DefaultValue == defaultInt32);
                        break;


                    case ElementType.Int32ArrayFixed:                               // 0x0006   // found in colorsets.xml.cooked
                        UInt32 defaultDoubleWord = _ReadUInt32(null, null);
                        Int32 arraySize = _ReadInt32(null, null);

                        Debug.Assert((UInt32)xmlCookElement.DefaultValue == defaultDoubleWord);
                        Debug.Assert(xmlCookElement.Count == arraySize);
                        break;


                    case ElementType.Float:                                             // 0x0100
                    case ElementType.FloatArrayVariable:                          // 0x0107   // not sure of structure as non-default, but as default has same as Float
                    case ElementType.FloatTripletArrayVariable:                                      // 0x0500
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

                    case ElementType.ExcelIndexArrayFixed:                            // 0x0905
                        Int32 excelTableArrCode = _ReadInt32(null, null);               // excel table code
                        Int32 excelTableArrCount = _ReadInt32(null, null);

                        Debug.Assert(xmlCookElement.ExcelTableCode == excelTableArrCode);
                        Debug.Assert(xmlCookElement.Count == excelTableArrCount);
                        break;


                    case ElementType.FloatArrayFixed:                                        // 0x0106
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


                    case ElementType.ByteArrayVariable:                                         // 0x1007   // found in textures
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