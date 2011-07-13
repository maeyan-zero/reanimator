using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using System.IO;
using Revival.Common;
using FieldDelegate = Revival.Common.ObjectDelegator.FieldDelegate;

namespace Hellgate
{
    public partial class XmlCookedFile : HellgateFile
    {
        public new const String Extension = ".xml.cooked";
        public new const String ExtensionDeserialised = ".xml";
        private const UInt32 FileMagicWord = 0x6B304F43; // 'CO0k'
        private const Int32 RequiredVersion = 8;
        private const UInt32 DataMagicWord = 0x41544144;

        public XmlDocument XmlDoc { get; private set; }
        public Object XmlObject { get; private set; }
        public XmlCookedDefinition Definition { get; private set; }

        public String FileName { get; private set; }
        public bool CookExcludeTestCentre { get; set; }
        public bool CookExcludeResurrection { get; set; }
        public bool ThrowOnMissingExcelString { get; set; }
        public HashSet<String> ExcelStringsMissing { get; private set; }
        public bool HasExcelStringsMissing { get { return (ExcelStringsMissing != null); } }
        public bool HasTestCentreElements { get; private set; }
        public bool HasResurrectionElements { get; private set; }

        private static readonly Dictionary<uint, XmlCookedDefinition> XmlDefinitions = new Dictionary<uint, XmlCookedDefinition>(XmlCookedDefinition.DefinitionTypes.Length);
        private static FieldDelegate _xVectorDelegate;
        private static FieldDelegate _yVectorDelegate;
        private static FieldDelegate _zVectorDelegate;

        private readonly FileManager _fileManager;
        private int _offset;
        private byte[] _buffer;
        private bool _generateXml;

        public XmlCookedFile(FileManager fileManager, String fileName = null)
        {
            Thread.CurrentThread.CurrentCulture = Common.EnglishUSCulture;
            CookExcludeTestCentre = true;
            CookExcludeResurrection = true;
            ThrowOnMissingExcelString = false;
            HasTestCentreElements = false;
            HasResurrectionElements = false;

            if (_xVectorDelegate == null)
            {
                lock (XmlDefinitions)
                {
                    if (_xVectorDelegate == null)
                    {
                        ObjectDelegator vectorDelegates = new ObjectDelegator(typeof(Vector3).GetFields(BindingFlags.Public | BindingFlags.Instance));

                        _xVectorDelegate = vectorDelegates.GetFieldDelegate("X");
                        _yVectorDelegate = vectorDelegates.GetFieldDelegate("Y");
                        _zVectorDelegate = vectorDelegates.GetFieldDelegate("Z");

                        _GenerateXmlDefinitions();
                    }
                }
            }

            _fileManager = fileManager;
            FileName = fileName;
        }

        /// <summary>
        /// Uncooks a file byte array to XmlDoc.<br />
        /// (automatically determines .xml definition)
        /// </summary>
        /// <param name="fileBytes">The file bytes to uncook.</param>
        public override void ParseFileBytes(byte[] fileBytes)
        {
            ParseFileBytes(fileBytes, true);
        }

        public byte[] ParseFileBytes(byte[] fileBytes, bool generateXml = false)
        {
            if (fileBytes[0] == 0x43) // 'C' from C00k header
            {
                //_xmlCookedObject = new XmlCookedObject(fileManager);
                XmlObject = _ParseCookedBytes(fileBytes, 0, generateXml);
                return null;
            }

            return _ParseXmlBytes(fileBytes, null);
        }

        public byte[] GenerateXmlFromCooked(byte[] fileBytes)
        {
            XmlObject = _ParseCookedBytes(fileBytes, 0, true);
            return null;
        }

        public Object LoadCookedFile(byte[] fileBytes)
        {
            XmlObject = _ParseCookedBytes(fileBytes, 0, false);
            return XmlObject;
        }

        public byte[] GenerateCookedFromXml(byte[] fileBytes)
        {
            return _ParseXmlBytes(fileBytes, null);
        }

        // todo: delete me?
        public T GenerateObject<T>(byte[] fileBytes, int offset = 0) where T : new()
        {
            if (fileBytes == null) throw new ArgumentNullException("fileBytes", "File bytes cannot be null!");

            _buffer = fileBytes;
            _offset = offset;

            /* Header Structure
             * UInt32           Magic Word          'CO0k'
             * Int32            Version             Must be 8
             * UInt32           Root XML Element
             * Int32            Element Count
             */

            // check file infos
            FileHeader header = FileTools.ByteArrayToStructure<FileHeader>(_buffer, ref _offset);

            if (header.MagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();
            if (header.Version != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();

            XmlCookedDefinition xmlDefinition = _GetXmlDefinition(header.XmlRootDefinition);
            if (xmlDefinition == null) throw new Exceptions.NotSupportedFileDefinitionException();
            if (xmlDefinition.Count < header.XmlRootElementCount) throw new Exceptions.NotSupportedXmlElementCount(xmlDefinition.Attributes.Name);

            XmlCookedTree xmlTree = new XmlCookedTree(xmlDefinition, header.XmlRootElementCount);
            _ParseCookedDefinition(xmlTree);

            UInt32 dataMagicWord = FileTools.ByteArrayToUInt32(_buffer, ref _offset);
            if (dataMagicWord != DataMagicWord) throw new Exceptions.UnexpectedTokenException("'DATA' Token MagicWord expected but not found!");

            return (T)_ParseCookedData(xmlTree, typeof(T), null, null);
        }

        /// <summary>
        /// Cooks an XML document to a .xml.cooked.
        /// </summary>
        /// <param name="xmlDocument">An XML Document to Cook.</param>
        /// <returns>The cooked bytes, or null upon failure.</returns>
        public byte[] CookXmlDocument(XmlDocument xmlDocument)
        {
            return _ParseXmlBytes(null, xmlDocument);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="offset"></param>
        /// <param name="generateXml"></param>
        /// <returns></returns>
        private Object _ParseCookedBytes(byte[] fileBytes, int offset = 0, bool generateXml = false)
        {
            if (fileBytes == null) throw new ArgumentNullException("fileBytes", "File bytes cannot be null!");

            _buffer = fileBytes;
            _offset = offset;
            _generateXml = generateXml;

            /* Header Structure
             * UInt32           Magic Word          'CO0k'
             * Int32            Version             Must be 8
             * UInt32           Root XML Element
             * Int32            Element Count
             */

            // check file infos
            FileHeader header = FileTools.ByteArrayToStructure<FileHeader>(_buffer, ref _offset);

            if (header.MagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();
            if (header.Version != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();

            XmlCookedDefinition xmlDefinition = _GetXmlDefinition(header.XmlRootDefinition);
            Definition = xmlDefinition;
            if (xmlDefinition == null) throw new Exceptions.NotSupportedFileDefinitionException();
            if (xmlDefinition.Count < header.XmlRootElementCount) throw new Exceptions.NotSupportedXmlElementCount(xmlDefinition.Attributes.Name);

            XmlCookedTree xmlTree = new XmlCookedTree(xmlDefinition, header.XmlRootElementCount);
            _ParseCookedDefinition(xmlTree);

            UInt32 dataMagicWord = FileTools.ByteArrayToUInt32(_buffer, ref _offset);
            if (dataMagicWord != DataMagicWord) throw new Exceptions.UnexpectedTokenException("'DATA' Token MagicWord expected but not found!");

            XmlNode root = null;
            if (_generateXml)
            {
                XmlDoc = new XmlDocument();
                root = XmlDoc.CreateElement(xmlDefinition.Attributes.Name);
                XmlDoc.AppendChild(root);
            }

            return _ParseCookedData(xmlTree, xmlDefinition.XmlObjectType, root, null);
        }

        private void _ParseCookedDefinition(XmlCookedTree xmlTree)
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

            for (int i = 0; i < xmlTree.Count && _offset < _buffer.Length; i++)
            {
                uint elementHash = StreamTools.ReadUInt32(_buffer, ref _offset);

                XmlCookedElement xmlElement = xmlTree.GetElement(elementHash);
                if (xmlElement == null) throw new Exceptions.UnexpectedTokenException(String.Format("Unexpected XML Element Hash: 0x{0:X8} in XML Definition {1}.", elementHash, xmlTree.Definition));
                xmlTree.AddElement(xmlElement);

                if (xmlElement.IsTestCentre) HasTestCentreElements = true;
                if (xmlElement.IsResurrection) HasResurrectionElements = true;

                XmlCookedAttribute xmlAttribute = xmlElement.XmlAttribute;

                ElementType token = (ElementType)StreamTools.ReadUInt16(_buffer, ref _offset);
                switch (token)
                {
                    case ElementType.Pointer:                                      // 0x0D00
                        // nothing to do                                                        // found in paths
                        break;                                                                  // only token then next element


                    case ElementType.String:                                                    // 0x0200
                    case ElementType.StringArrayFixed:                                          // 0x0206
                    case ElementType.StringArrayVariable:                                       // 0x0207
                        byte strLen = _buffer[_offset++];
                        String strValue = null;
                        if (strLen > 0)
                        {
                            strValue = StreamTools.ReadStringAscii(_buffer, ref _offset, strLen, true);
                        }

                        Debug.Assert((String)xmlElement.XmlAttribute.DefaultValue == strValue);
                        xmlElement.Default = strValue;

                        if (token == ElementType.StringArrayFixed)
                        {
                            Int32 strArrCount = StreamTools.ReadInt32(_buffer, ref _offset);
                            Debug.Assert(xmlElement.XmlAttribute.Count == strArrCount);
                        }
                        break;


                    case ElementType.Int32:                                                     // 0x0000
                    case ElementType.Int32ArrayVariable:                                        // 0x0007
                    case ElementType.NonCookedInt32:                                            // 0x0700
                    case ElementType.NonCookedInt3207:                                          // 0x0707
                    case ElementType.Int32_0x0A00:                                              // 0x0A00
                        Int32 defaultInt32 = StreamTools.ReadInt32(_buffer, ref _offset);       // default value

                        if (xmlAttribute.CustomType == ElementType.Bool)
                        {
                            bool boolValue = (defaultInt32 == 0) ? false : true;
                            xmlElement.Default = boolValue;
                            Debug.Assert((bool)xmlAttribute.DefaultValue == boolValue);
                        }
                        else if (xmlAttribute.CustomType == ElementType.Unsigned)
                        {
                            UInt32 defaultUInt32 = (UInt32)defaultInt32;
                            xmlElement.Default = defaultUInt32;
                            Debug.Assert((UInt32)xmlAttribute.DefaultValue == defaultUInt32);
                        }
                        else
                        {
                            xmlElement.Default = defaultInt32;
                            Debug.Assert((Int32)xmlAttribute.DefaultValue == defaultInt32);
                        }
                        break;


                    case ElementType.Int32ArrayFixed:                                           // 0x0006   // found in colorsets and appearance definition
                        if (xmlAttribute.CustomType == ElementType.Unsigned)
                        {
                            UInt32 uint32 = StreamTools.ReadUInt32(_buffer, ref _offset);
                            xmlElement.Default = uint32;
                            Debug.Assert((UInt32)xmlAttribute.DefaultValue == uint32);

                        }
                        else
                        {
                            Int32 int32 = StreamTools.ReadInt32(_buffer, ref _offset);
                            xmlElement.Default = int32;
                            Debug.Assert((Int32)xmlAttribute.DefaultValue == int32);
                        }

                        Int32 arraySize = StreamTools.ReadInt32(_buffer, ref _offset);
                        Debug.Assert(xmlAttribute.Count == arraySize);
                        break;


                    case ElementType.Float:                                                     // 0x0100
                    case ElementType.FloatArrayVariable:                                        // 0x0107   // not sure of structure as non-default, but as default has same as Float
                    case ElementType.FloatTripletArrayVariable:                                 // 0x0500
                    case ElementType.FloatQuadArrayVariable:                                    // 0x0600   //materials "tScatterColor"
                    case ElementType.NonCookedFloat:                                            // 0x0800
                        float defaultFloat = StreamTools.ReadFloat(_buffer, ref _offset);       // default value
                        xmlElement.Default = defaultFloat;

                        Debug.Assert((float)xmlAttribute.DefaultValue == defaultFloat);
                        //if ((float)xmlAttribute.DefaultValue != defaultFloat) // seen in MP PathNode definitions only (not completely implemented - no point)
                        //{
                        //    Debug.WriteLine("Warning: DefaultValue {0} changed to {1} on Element {2}", xmlAttribute.DefaultValue, defaultFloat, xmlAttribute.Name);
                        //});
                        break;


                    case ElementType.Flag:                                                      // 0x0B01
                        bool defaultFlagged = StreamTools.ReadInt32(_buffer, ref _offset) != 0; // default value
                        UInt32 bitMask = StreamTools.ReadUInt32(_buffer, ref _offset);          // bit mask
                        xmlElement.Default = defaultFlagged;

                        Debug.Assert((bool)xmlAttribute.DefaultValue == defaultFlagged);
                        Debug.Assert(xmlAttribute.FlagMask == bitMask);
                        break;


                    case ElementType.BitFlag:                                                   // 0x0C02
                        Int32 bitIndex = StreamTools.ReadInt32(_buffer, ref _offset);           // bit index
                        Int32 bitCount = StreamTools.ReadInt32(_buffer, ref _offset);           // bit count
                        xmlElement.Default = false;

                        Debug.Assert(xmlAttribute.BitFlagIndex == bitIndex);
                        Debug.Assert(xmlAttribute.BitFlagCount == bitCount);
                        break;


                    case ElementType.ExcelIndex:                                                // 0x0903
                        Xls.TableCodes tableCode = (Xls.TableCodes)StreamTools.ReadInt32(_buffer, ref _offset); // excel table code

                        Debug.Assert(xmlAttribute.TableCode == tableCode);
                        break;

                    case ElementType.ExcelIndexArrayFixed:                                      // 0x0905
                        Xls.TableCodes excelTableArrCode = (Xls.TableCodes)StreamTools.ReadInt32(_buffer, ref _offset); // excel table code
                        Int32 excelTableArrCount = StreamTools.ReadInt32(_buffer, ref _offset);

                        Debug.Assert(xmlAttribute.TableCode == excelTableArrCode);
                        Debug.Assert(xmlAttribute.Count == excelTableArrCount);
                        break;


                    case ElementType.FloatArrayFixed:                                           // 0x0106
                        float defaultFloatArr = StreamTools.ReadFloat(_buffer, ref _offset);    // default value
                        Int32 arrayCount = StreamTools.ReadInt32(_buffer, ref _offset);         // count
                        xmlElement.Default = defaultFloatArr;

                        Debug.Assert((float)xmlAttribute.DefaultValue == defaultFloatArr);
                        Debug.Assert(xmlAttribute.Count == arrayCount);
                        break;


                    case ElementType.Table:                                                     // 0x0308
                    case ElementType.TableArrayFixed:                                           // 0x0309
                    case ElementType.TableArrayVariable:                                        // 0x030A
                        if (token == ElementType.TableArrayFixed)
                        {
                            Int32 tableArrayCount = StreamTools.ReadInt32(_buffer, ref _offset); // table count
                            Debug.Assert(xmlAttribute.Count == tableArrayCount);
                        }

                        UInt32 definitionHash = StreamTools.ReadUInt32(_buffer, ref _offset);       // table string hash
                        Int32 elementCount = StreamTools.ReadInt32(_buffer, ref _offset);       // table element count

                        XmlCookedDefinition tableXmlDefition = _GetXmlDefinition(definitionHash);
                        if (tableXmlDefition == null) throw new Exceptions.NotSupportedFileDefinitionException();
                        if (tableXmlDefition.Count < elementCount) throw new Exceptions.NotSupportedXmlElementCount(tableXmlDefition.Attributes.Name);

                        if (elementCount == -1)
                        {
                            xmlTree.AddExistingTree(definitionHash);
                        }
                        else
                        {
                            XmlCookedTree xmlChildTree = new XmlCookedTree(tableXmlDefition, xmlElement, xmlTree, elementCount);
                            xmlTree.AddTree(xmlChildTree);
                            _ParseCookedDefinition(xmlChildTree);
                        }
                        break;


                    case ElementType.Int32Array_0x0A06:                                         // 0x0A06
                        Int32 int32ArrDef = StreamTools.ReadInt32(_buffer, ref _offset);
                        Int32 int32ArrCount = StreamTools.ReadInt32(_buffer, ref _offset);
                        xmlElement.Default = int32ArrDef;

                        Debug.Assert((Int32)xmlAttribute.DefaultValue == int32ArrDef);
                        Debug.Assert(xmlAttribute.Count == int32ArrCount);
                        break;


                    case ElementType.ByteArrayVariable:                                         // 0x1007   // found in textures
                        Int32 unknown = StreamTools.ReadInt32(_buffer, ref _offset);
                        xmlElement.Default = unknown;

                        Debug.Assert((Int32)xmlAttribute.DefaultValue == unknown);
                        break;


                    default:
                        throw new Exceptions.UnexpectedTokenException("Unexpected .xml.cooked definition array token: 0x" + ((ushort)token).ToString("X4"));
                }
            }
        }

        private Object _ParseCookedData(XmlCookedTree xmlTree, Type objType, XmlNode xmlRoot, XmlNode xmlDesc)
        {
            Object obj = Activator.CreateInstance(objType);
            int elementCount = xmlTree.Count;

            // bitField info
            int bitFieldOffset = _offset;
            int bitFieldByteCount = (elementCount - 1 >> 3) + 1; // -1 as 16 >> 3 = 2 + 1 = 3, but should only be 2 bytes
            _offset += bitFieldByteCount;

            Dictionary<uint, XmlCookedElement> elements = xmlTree.Definition.Elements;
            int i = -1;
            foreach (XmlCookedElement xmlElement in elements.Values)
            {
                if (xmlElement.IsCustomOrigin) continue;

                XmlCookedAttribute xmlAttribute = xmlElement.XmlAttribute;
                FieldDelegate fieldDelegate = xmlElement.FieldDelegate;
                Object currObj = obj;

                if (xmlAttribute.ParentNames != null)
                {
                    foreach (String parentName in xmlAttribute.ParentNames)
                    {
                        XmlCookedElement parentElement = xmlTree.GetElement(parentName);
                        if (parentElement == null) throw new Exceptions.InvalidXmlElement(parentName, "The XML Element field delegates count not be found!");

                        FieldDelegate parentDelegate = parentElement.FieldDelegate;
                        currObj = parentDelegate.GetValue(currObj);
                    }
                }

                // if the element is not present in the file, then set to standard default value
                if (!xmlTree.ContainsElement(xmlElement.NameHash))
                {
                    _SetFieldDefault(xmlElement, currObj, xmlAttribute.DefaultValue);
                    continue;
                }

                i++;

                // if the element is present in the file's definition, but is not present in this tree section, then set to file default value
                if (!_TestBit(_buffer, bitFieldOffset, i))
                {
                    _SetFieldDefault(xmlElement, currObj, xmlElement.Default);
                    continue;
                }

                switch (xmlAttribute.ElementType)
                {
                    case ElementType.Int32:                         // 0x0000
                        _ParseCookedInt32(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.Int32ArrayFixed:               // 0x0006
                        _ParseCookedInt32ArrayFixed(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.Int32ArrayVariable:            // 0x0007
                        _ParseCookedInt32ArrayVariable(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.Float:                         // 0x0100
                        _ParseCookedFloat(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.FloatArrayFixed:               // 0x0106
                        _ParseCookedFloatArrayFixed(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.FloatArrayVariable:            // 0x0107
                        _ParseCookedFloatArrayVariable(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.FloatTripletArrayVariable:     // 0x0500       found in AppearanceDefinition and children, appears on types like tSelfIllumation, tSelfIllumationBlend, etc
                        _ParseCookedFloatTripletArrayVariable(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.FloatQuadArrayVariable:        // 0x0600       found in Screen FX and EnvironmentDefinition
                        _ParseCookedFloatQuadArrayVariable(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.String:                        // 0x0200
                        _ParseCookedString(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.StringArrayFixed:              // 0x0206
                        _ParseCookedStringArrayFixed(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.StringArrayVariable:           // 0x0207
                        _ParseCookedStringArrayVariable(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.Table:                         // 0x0308
                    case ElementType.TableArrayFixed:               // 0x0309
                    case ElementType.TableArrayVariable:            // 0x030A
                        _ParseCookedTables(xmlAttribute, xmlTree.GetTree(i), currObj, xmlRoot);
                        break;

                    case ElementType.ExcelIndex:                    // 0x0903
                        Object singleExcelRow = _ParseCookedExcelIndex(xmlAttribute, xmlRoot, xmlDesc);
                        fieldDelegate.SetValue(currObj, singleExcelRow);
                        break;

                    case ElementType.ExcelIndexArrayFixed:          // 0x0905
                        Object[] excelRows = new Object[xmlAttribute.Count];
                        for (int j = 0; j < xmlAttribute.Count; j++)
                        {
                            Object excelRow = _ParseCookedExcelIndex(xmlAttribute, xmlRoot, xmlDesc);
                            excelRows[j] = excelRow;
                        }
                        fieldDelegate.SetValue(currObj, excelRows);
                        break;

                    case ElementType.Flag:                          // 0x0B01
                        _ParseCookedFlag(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.BitFlag:                       // 0x0C02
                        _ParseCookedBitFlag(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.ByteArrayVariable:
                        _ParseCookedByteArrayVariable(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.NonCookedFloat:
                    case ElementType.NonCookedInt3207:
                    case ElementType.NonCookedInt32:
                        throw new Exceptions.InvalidXmlDocument("Encountered not-cooked XML Element = " + xmlAttribute.ElementType);

                    default:
                        throw new Exceptions.InvalidXmlElement(xmlAttribute.Name, "The XML Element doesn't have a valid ElementType set, or the ElementType is not yet supported. ElementType = " + xmlAttribute.ElementType);
                }
            }

            return obj;
        }

        private byte[] _ParseXmlBytes(byte[] xmlBytes, XmlDocument xmlDocument)
        {
            if (xmlBytes == null && (xmlDocument == null || xmlDocument.HasChildNodes)) return null;

            if (xmlDocument == null)
            {
                xmlDocument = new XmlDocument();
                using (MemoryStream memoryStream = new MemoryStream(xmlBytes))
                {
                    xmlDocument.Load(memoryStream);
                }
            }

            //if (_xmlDefinitions == null) throw new Exceptions.NotInitializedException();
            if (!xmlDocument.HasChildNodes) return null;

            XmlNode rootElement = xmlDocument.FirstChild;
            UInt32 xmlDefinitionHash = Crypt.GetStringHash(rootElement.Name);

            XmlCookedDefinition xmlDefinition = _GetXmlDefinition(xmlDefinitionHash);
            if (xmlDefinition == null) throw new Exceptions.NotSupportedFileDefinitionException();

            _offset = 0;
            _buffer = new byte[1024];


            // header details
            FileHeader xmlCookFileHeader = new FileHeader
            {
                MagicWord = FileMagicWord,
                Version = RequiredVersion,
                XmlRootDefinition = xmlDefinitionHash,
                XmlRootElementCount = _GetElementCountToWrite(xmlDefinition)
            };
            FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlCookFileHeader);

            // write default element array
            List<UInt32> cookedDefinitions = new List<UInt32>();
            int bytesWritten = _ParseXmlDefinition(xmlDefinition, cookedDefinitions);
            if (bytesWritten == 0) return null;


            // write data segment
            FileTools.WriteToBuffer(ref _buffer, ref _offset, DataMagicWord);
            XmlNode xmlNode = xmlDocument.FirstChild;
            bytesWritten = _ParseXmlData(xmlDefinition, xmlNode);
            if (bytesWritten == -1) return null;

            byte[] cookedBytes = new byte[_offset];
            Buffer.BlockCopy(_buffer, 0, cookedBytes, 0, _offset);

            return cookedBytes;
        }

        private int _ParseXmlDefinition(XmlCookedDefinition xmlDefinition, ICollection<UInt32> cookedDefinitions)
        {
            cookedDefinitions.Add(xmlDefinition.Hash);

            int offsetStart = _offset;
            foreach (XmlCookedElement xmlElement in xmlDefinition.Elements.Values)
            {
                if (!_IncludeElement(xmlElement)) continue;
                if (xmlElement.IsCustomOrigin) continue;
                XmlCookedAttribute xmlAttribute = xmlElement.XmlAttribute;

                // write name hash and token
                FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlAttribute.NameHash);
                FileTools.WriteToBuffer(ref _buffer, ref _offset, (ushort)xmlAttribute.ElementType);

                switch (xmlAttribute.ElementType)
                {
                    // 0 bytes
                    case ElementType.Pointer:          // 0x0D00
                        break;

                    // 4 bytes
                    case ElementType.Int32:                         // 0x0000
                    case ElementType.Int32ArrayVariable:            // 0x0007
                    case ElementType.Float:                         // 0x0100
                    case ElementType.FloatArrayVariable:            // 0x0107
                    case ElementType.FloatTripletArrayVariable:     // 0x0500
                    case ElementType.FloatQuadArrayVariable:        // 0x0600
                    case ElementType.NonCookedInt32:                // 0x0700
                    case ElementType.NonCookedFloat:                // 0x0800
                    case ElementType.Int32_0x0A00:                  // 0x0A00
                    case ElementType.ByteArrayVariable:             // 0x1007       // found in TEXTURE_DEFINITION child BLEND_RUN
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlAttribute.DefaultValue);
                        break;

                    // 1 + x bytes
                    case ElementType.String:                        // 0x0200
                    case ElementType.StringArrayVariable:           // 0x0207       // AppearanceDefinition - not tested DefaultValue != null
                        String strDefault = xmlAttribute.DefaultValue as String;
                        byte strLen = (byte)((strDefault == null) ? 0 : strDefault.Length);
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, strLen);

                        if (xmlAttribute.DefaultValue == null) continue;

                        byte[] stringBytes = FileTools.StringToASCIIByteArray(xmlAttribute.DefaultValue.ToString());
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, stringBytes);
                        _offset++; // \0
                        break;

                    // 1 + x bytes + 4 bytes
                    case ElementType.StringArrayFixed:              // 0x0206
                        String strArrDefault = xmlAttribute.DefaultValue as String;
                        byte strArrStrLen = (byte)((strArrDefault == null) ? 0 : strArrDefault.Length);
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, strArrStrLen);

                        if (xmlAttribute.DefaultValue != null)
                        {
                            byte[] strArrStringBytes = FileTools.StringToASCIIByteArray(xmlAttribute.DefaultValue.ToString());
                            FileTools.WriteToBuffer(ref _buffer, ref _offset, strArrStringBytes);
                            _offset++; // \0
                        }

                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlAttribute.Count);
                        break;

                    // 8 bytes
                    case ElementType.Flag:                          // 0x0B01
                        Int32 defaultFlagged = (bool)xmlAttribute.DefaultValue ? 1 : 0;
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, defaultFlagged);
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlAttribute.FlagMask);
                        break;

                    // 8 bytes
                    case ElementType.BitFlag:                       // 0x0C02
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlAttribute.BitFlagIndex);
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlAttribute.BitFlagCount);
                        break;

                    // 8 bytes
                    case ElementType.Table:                         // 0x0308
                    case ElementType.TableArrayFixed:               // 0x0309
                    case ElementType.TableArrayVariable:            // 0x030A
                        if (xmlAttribute.ElementType == ElementType.TableArrayFixed)
                        {
                            FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlAttribute.Count);
                        }

                        XmlCookedDefinition xmlChildDefinition = _GetXmlDefinition(xmlAttribute.ChildTypeHash);
                        if (xmlChildDefinition == null) throw new Exceptions.NotSupportedFileDefinitionException();

                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlAttribute.ChildTypeHash);

                        // make sure we haven't already cooked it
                        if (cookedDefinitions.Contains(xmlAttribute.ChildTypeHash))
                        {
                            const Int32 alreadyCooked = -1;
                            FileTools.WriteToBuffer(ref _buffer, ref _offset, alreadyCooked);
                            break;
                        }

                        int elementCount = _GetElementCountToWrite(xmlChildDefinition);
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, elementCount);

                        int bytesWritten = _ParseXmlDefinition(xmlChildDefinition, cookedDefinitions);
                        if (bytesWritten == 0) return 0;
                        break;

                    // 4 bytes
                    case ElementType.ExcelIndex:                    // 0x0903
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, (UInt32)xmlAttribute.TableCode);
                        break;

                    // 8 bytes
                    case ElementType.ExcelIndexArrayFixed:          // 0x0905
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, (UInt32)xmlAttribute.TableCode);
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlAttribute.Count);
                        break;

                    // 8 bytes
                    case ElementType.Int32ArrayFixed:               // 0x0006       // colorsets.xml.cooked (pdwColors)
                    case ElementType.FloatArrayFixed:               // 0x0106
                    case ElementType.Int32Array_0x0A06:             // 0x0A06       // AppearanceDefinition
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlAttribute.DefaultValue);
                        FileTools.WriteToBuffer(ref _buffer, ref _offset, xmlAttribute.Count);
                        break;

                    default:
                        Debug.Assert(false, "ElementType not set!");
                        break;
                }
            }

            return _offset - offsetStart;
        }

        private int _ParseXmlData(XmlCookedDefinition xmlDefinition, XmlNode xmlNode)
        {
            int offsetStart = _offset;
            int elementCount = _GetElementCountToWrite(xmlDefinition);

            // bitField info
            int bitFieldOffset = _offset;
            int bitFieldByteCount = (elementCount - 1 >> 3) + 1; // -1 as 16 >> 3 = 2 + 1 = 3, but should only be 2 bytes, and +1 as 7 >> 3 = 0
            _offset += bitFieldByteCount;

            int[] flagOffsets = new[] { -1, -1, -1 };
            int[] bitFlagOffsets = new[] { -1, -1, -1 };
            int bitIndex = -1;
            foreach (XmlCookedElement xmlCookElement in xmlDefinition.Elements.Values)
            {
                if (!_IncludeElement(xmlCookElement)) continue;
                if (xmlCookElement.IsCustomOrigin) continue;

                XmlCookedAttribute xmlAttribute = xmlCookElement.XmlAttribute;

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

                        case ElementType.TableArrayVariable:
                            const Int32 zeroTables = 0;
                            FileTools.WriteToBuffer(ref _buffer, ref _offset, zeroTables);
                            _FlagBit(_buffer, bitFieldOffset, bitIndex);
                            break;
                    }

                    continue;
                }

                String elementName = xmlCookElement.Name;
                if (xmlCookElement.ElementType == ElementType.Int32ArrayVariable ||
                    xmlCookElement.ElementType == ElementType.StringArrayVariable ||
                    xmlCookElement.ElementType == ElementType.FloatArrayVariable ||
                    xmlCookElement.ElementType == ElementType.TableArrayVariable ||
                    xmlCookElement.ElementType == ElementType.FloatTripletArrayVariable ||
                    xmlCookElement.ElementType == ElementType.FloatQuadArrayVariable)
                {
                    elementName += "Count";
                }

                XmlElement xmlElement = xmlNode[elementName];
                String elementText = String.Empty;
                if (xmlElement == null &&
                    xmlCookElement.ElementType != ElementType.Int32ArrayVariable &&    // need count written even if null/empty
                    xmlCookElement.ElementType != ElementType.StringArrayVariable &&    // need count written even if null/empty
                    xmlCookElement.ElementType != ElementType.FloatArrayVariable &&     // need count written even if null/empty
                    xmlCookElement.ElementType != ElementType.Table &&                  // want table even if null/empty
                    xmlCookElement.ElementType != ElementType.TableArrayVariable &&     // want tables even if null/empty
                    xmlCookElement.ElementType != ElementType.ExcelIndex &&             // want excel index even if null/empty
                    xmlCookElement.ElementType != ElementType.ExcelIndexArrayFixed &&   // want excel index even if null/empty
                    xmlCookElement.ElementType != ElementType.ByteArrayVariable         // need count written even if null/empty
                    ) continue;

                if (xmlElement != null)
                {
                    elementText = xmlElement.InnerText;
                }

                switch (xmlCookElement.ElementType)
                {
                    case ElementType.NonCookedInt32:                    // 0x0700
                    case ElementType.NonCookedFloat:                    // 0x0800
                    case ElementType.Pointer:              // 0x0D00
                        //int bp = 0;     // not cooked.... (I think...)
                        continue;

                    case ElementType.Int32:                             // 0x0000
                        _ParseXmlInt32(elementText, xmlCookElement);
                        _offset += xmlAttribute.FlagOffsetChange;
                        break;

                    case ElementType.Int32ArrayFixed:                   // 0x0006       // found in ColorSets and AppearanceDefinition
                        if (xmlCookElement.XmlAttribute.CustomType == ElementType.Unsigned)
                        {
                            _ParseXmlInt32ArrayFixed<UInt32>(xmlCookElement, xmlNode);
                        }
                        else
                        {
                            _ParseXmlInt32ArrayFixed<Int32>(xmlCookElement, xmlNode);
                        }
                        break;

                    case ElementType.Int32ArrayVariable:                // 0x0007       // found in RoomPathNodeSet
                        _ParseXmlInt32ArrayVariable(xmlCookElement, xmlNode, elementText);
                        break;

                    case ElementType.Float:                             // 0x0100
                        _ParseXmlFloat(elementText, xmlCookElement);
                        break;

                    case ElementType.FloatArrayFixed:                   // 0x0106
                        _ParseXmlFloatArrayFixed(xmlCookElement, xmlNode);
                        break;

                    case ElementType.FloatArrayVariable:                // 0x0107
                        _ParseXmlFloatArrayVariable(xmlCookElement, xmlNode, elementText);
                        break;

                    case ElementType.String:                            // 0x0200
                        _ParseXmlString(elementText, xmlCookElement, xmlElement);
                        break;

                    case ElementType.StringArrayFixed:                  // 0x0206
                        _ParseXmlStringArrayFixed(xmlCookElement, xmlNode);
                        break;

                    case ElementType.StringArrayVariable:               // 0x0207
                        _ParseXmlStringArrayVariable(xmlCookElement, xmlNode, elementText);
                        break;

                    case ElementType.Table:                             // 0x0308
                        if (_ParseXmlTable(xmlCookElement, xmlElement) == -1) return -1;
                        break;

                    case ElementType.TableArrayFixed:                   // 0x0309
                        if (_ParseXmlTableArrayFixed(xmlCookElement, xmlElement) == -1) return -1;
                        break;

                    case ElementType.TableArrayVariable:                // 0x030A
                        if (_ParseXmlTableArrayVariable(elementText, xmlCookElement, xmlElement) == -1) return -1; // we found less tables than we were supposed to cook
                        break;

                    case ElementType.FloatTripletArrayVariable:         // 0x0500
                        _ParseXmlFloatTripletArrayVariable(elementText, xmlCookElement, xmlNode);
                        break;

                    case ElementType.FloatQuadArrayVariable:            // 0x0600
                        _ParseXmlFloatQuadArrayVariable(elementText, xmlCookElement, xmlNode);
                        break;

                    case ElementType.ExcelIndex:                        // 0x0903
                        _ParseXmlExcelIndex(elementText, xmlCookElement);
                        break;

                    case ElementType.ExcelIndexArrayFixed:              // 0x0905
                        _ParseXmlExcelIndexArrayFixed(xmlCookElement, xmlNode);
                        break;

                    case ElementType.Flag:                              // 0x0B01
                        _ParseXmlFlag(elementText, xmlAttribute, flagOffsets);
                        break;

                    case ElementType.BitFlag:                           // 0x0C02
                        _ParseXmlFlag(elementText, xmlAttribute, bitFlagOffsets);
                        break;

                    case ElementType.ByteArrayVariable:                 // 0x1007
                        _ParseXmlByteArrayVariable(elementText, xmlCookElement);
                        break;

                    default:
                        throw new Exceptions.InvalidXmlElement(xmlCookElement.Name, "ElementType not set.\n\n" + xmlCookElement.ElementType);
                }

                _FlagBit(_buffer, bitFieldOffset, bitIndex);
            }

            return _offset - offsetStart;
        }

        /// <summary>
        /// Save the uncooked XML Document.
        /// </summary>
        /// <param name="path">The path to save the XML Document.</param>
        public void SaveXmlDocument(String path)
        {
            if (XmlDoc == null || !XmlDoc.HasChildNodes || String.IsNullOrEmpty(path)) return;

            String directory = Path.GetDirectoryName(path);
            if (String.IsNullOrEmpty(directory)) throw new DirectoryNotFoundException("The path supplied does not have a vaild directory.\n\n" + path);

            Directory.CreateDirectory(directory);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            XmlDoc.Save(path);
        }

        /// <summary>
        /// Generates a .xml.cooked byte array of the inner Object.
        /// [incomplete implementation]
        /// </summary>
        /// <returns>The Object as a .xml.cooked byte array.</returns>
        public override byte[] ToByteArray()
        {
            // todo: this should export as .xml.cooked, but would require going from an object to .cooked, at the moment we only do .xml -> .xml.cooked
            throw new NotImplementedException("Use ParseXmlBytes to get as .cooked for now.");
        }

        /// <summary>
        /// Exports the XmlDocument as a byte array for direct saving.
        /// Can use SaveXmlDocument(path) for easier use, but ExportAsDocument() is present for base override.
        /// </summary>
        /// <returns>The XmlDocument as an XML byte array.</returns>
        public override byte[] ExportAsDocument()
        {
            if (XmlDoc == null || !XmlDoc.HasChildNodes) return null;

            MemoryStream memoryStream = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
                NewLineOnAttributes = true,
                Encoding = new UTF8Encoding(false)
            };

            XmlWriter xmlWriter = XmlWriter.Create(memoryStream, settings);
            XmlDoc.WriteTo(xmlWriter);
            xmlWriter.Close();

            return memoryStream.ToArray();
        }
    }
}