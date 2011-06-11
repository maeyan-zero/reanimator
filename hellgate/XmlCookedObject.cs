using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Hellgate.Xml;
using Revival.Common;
using FieldDelegate = Revival.Common.ObjectDelegator.FieldDelegate;

namespace Hellgate
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = true)]
    public class XmlCookedAttribute : Attribute
    {
        public String Name;
        public UInt32 NameHash;
        public Object DefaultValue;
        public Type ChildType;
        public UInt32 ChildTypeHash;
        public Xls.TableCodes TableCode;
        public ElementType ElementType;
        public ElementType CustomType;
        public Int32 FlagId;
        public Int32 FlagOffsetChange;      // EnvironmentDefinition has a dwDefFlags first, which actually reads in the following 5 flag elements causing an offset error.
        public UInt32 FlagMask;
        public Int32 BitFlagIndex;
        public Int32 BitFlagCount;          // used for BitIndex, total field BitCount
        public Int32 Count;                 // general array-type count
        public bool TreatAsData;
        public bool IsTestCentre;           // TestCentre elements only
        public bool IsResurrection;         // Resurrection elements only
        public UInt32 HashOverride;         // Elements with unknown strings
        public FieldDelegate FieldDelegate;
        public List<String> ParentNames;
    }

    public class XmlCookedObject
    {
        private const UInt32 FileMagicWord = 0x6B304F43; // 'CO0k'
        private const Int32 RequiredVersion = 8;
        private const UInt32 DataMagicWord = 0x41544144;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class FileHeader
        {
            public UInt32 MagicWord;
            public Int32 Version;
            public UInt32 XmlRootDefinition;
            public Int32 XmlRootElementCount;
        }

        public class XmlElement
        {
            public readonly XmlCookedAttribute XmlAttribute;
            public readonly FieldDelegate FieldDelegate;
            public readonly XmlCookedTree XmlTree;

            public uint NameHash { get { return XmlAttribute.NameHash; } }
            public bool IsTestCentre { get { return XmlAttribute.IsTestCentre; } }
            public bool IsResurrection { get { return XmlAttribute.IsResurrection; } }

            public Object Default;

            public XmlElement(XmlCookedTree xmlTree)
            {
                XmlTree = xmlTree;
            }

            public XmlElement(XmlCookedAttribute xmlAttribute, FieldDelegate fieldDelegate)
            {
                XmlAttribute = xmlAttribute;
                FieldDelegate = fieldDelegate;
            }

            public override string ToString()
            {
                return XmlAttribute.Name;
            }
        }

        public class XmlDefinition
        {
            public readonly Dictionary<uint, XmlElement> Elements;

            public readonly XmlCookedAttribute RootElement;

            public int Count { get { return Elements.Count; } }

            public XmlDefinition(XmlCookedAttribute rootElement, Dictionary<uint, XmlElement> xmlElements)
            {
                RootElement = rootElement;
                Elements = xmlElements;
            }

            public XmlElement GetElement(uint nameHash)
            {
                XmlElement xmlElement;
                return !Elements.TryGetValue(nameHash, out xmlElement) ? null : xmlElement;
            }

            public XmlElement GetElement(String name)
            {
                return GetElement(Crypt.GetStringHash(name));
            }

            public override string ToString()
            {
                return RootElement.Name;
            }
        }

        private static readonly Type[] XmlDefinitionTypes =
        {
            // Shared
            typeof (XmlConditionDefinition),

            // AI Behavior
            typeof (XmlAIDefinition),
            typeof (XmlAIBehaviorDefinitionTable),
            typeof (XmlAIBehaviorDefinition),

            // Room Layouts
            typeof (XmlRoomPathNodeDefinition),
            typeof (XmlRoomPathNodeSet),
            typeof (XmlRoomPathNode),
            typeof (XmlRoomLayoutGroupDefinition),
            typeof (XmlRoomLayoutGroup),

            // Room Paths
            typeof (XmlRoomPathNodeConnection),
            typeof (XmlRoomPathNodeConnectionRef),

            // Skill Events
            typeof (XmlSkillEventsDefinition),
            typeof (XmlSkillEventHolder),
            typeof (XmlSkillEvent),

            // State Events
            typeof (XmlStateDefinition),
            typeof (XmlStateEvent)
        };
        private static readonly Dictionary<uint, XmlDefinition> XmlDefinitions = new Dictionary<uint, XmlDefinition>(XmlDefinitionTypes.Length);
        private static FieldDelegate _xVectorDelegate;
        private static FieldDelegate _yVectorDelegate;
        private static FieldDelegate _zVectorDelegate;

        private readonly FileManager _fileManager;
        private byte[] _buffer;
        private int _offset;

        public bool HasTestCentreElements;
        public bool HasResurrectionElements;

        public XmlCookedObject(FileManager fileManager)
        {
            if (_xVectorDelegate == null)
            {
                lock (XmlDefinitions)
                {
                    if (_xVectorDelegate == null)
                    {
                        ObjectDelegator vectorDelegates = new ObjectDelegator(typeof (Vector3).GetFields(BindingFlags.Public | BindingFlags.Instance));

                        _xVectorDelegate = vectorDelegates.GetFieldDelegate("X");
                        _yVectorDelegate = vectorDelegates.GetFieldDelegate("Y");
                        _zVectorDelegate = vectorDelegates.GetFieldDelegate("Z");

                        _GenerateXmlDefinitions();
                    }
                }
            }

            _fileManager = fileManager;
        }

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

            XmlDefinition xmlDefinition = _GetXmlDefinition(header.XmlRootDefinition);
            if (xmlDefinition == null) throw new Exceptions.NotSupportedFileDefinitionException();
            if (xmlDefinition.Count < header.XmlRootElementCount) throw new Exceptions.NotSupportedXmlElementCount(xmlDefinition.RootElement.Name);

            XmlCookedTree xmlTree = new XmlCookedTree(xmlDefinition, header.XmlRootElementCount);
            _ReadXmlDefinition(xmlTree);

            UInt32 dataMagicWord = FileTools.ByteArrayToUInt32(_buffer, ref _offset);
            if (dataMagicWord != DataMagicWord) throw new Exceptions.UnexpectedTokenException("'DATA' Token MagicWord expected but not found!");

            return (T)_ReadXmlData(xmlTree, typeof(T));
        }

        private void _ReadXmlDefinition(XmlCookedTree xmlTree)
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

                XmlElement xmlElement = xmlTree.GetElement(elementHash);
                if (xmlElement == null) throw new Exceptions.UnexpectedTokenException(String.Format("Unexpected XML Element Hash: 0x{0:X8} in XML Definition {1}.", elementHash, xmlTree.Definition));
                xmlTree.AddElement(xmlElement);

                if (xmlElement.IsTestCentre) HasTestCentreElements = true;
                if (xmlElement.IsResurrection) HasResurrectionElements = true;

                XmlCookedAttribute xmlAttribute = xmlElement.XmlAttribute;

                ElementType token = (ElementType)StreamTools.ReadUInt16(_buffer, ref _offset);
                switch (token)
                {
                    case ElementType.UnknownPTypeD_0x0D00:                                      // 0x0D00
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
                        else
                        {
                            xmlElement.Default = defaultInt32;

                            Debug.Assert((Int32)xmlAttribute.DefaultValue == defaultInt32);
                        }
                        break;


                    case ElementType.Int32ArrayFixed:                                           // 0x0006   // found in colorsets.xml.cooked
                        UInt32 defaultDoubleWord = StreamTools.ReadUInt32(_buffer, ref _offset);
                        Int32 arraySize = StreamTools.ReadInt32(_buffer, ref _offset);
                        xmlElement.Default = defaultDoubleWord;

                        Debug.Assert((UInt32)xmlAttribute.DefaultValue == defaultDoubleWord);
                        Debug.Assert(xmlAttribute.Count == arraySize);
                        break;


                    case ElementType.Float:                                                     // 0x0100
                    case ElementType.FloatArrayVariable:                                        // 0x0107   // not sure of structure as non-default, but as default has same as Float
                    case ElementType.FloatTripletArrayVariable:                                 // 0x0500
                    case ElementType.FloatQuadArrayVariable:                                    // 0x0600   //materials "tScatterColor"
                    case ElementType.NonCookedFloat:                                            // 0x0800
                        float defaultFloat = StreamTools.ReadFloat(_buffer, ref _offset);       // default value
                        xmlElement.Default = defaultFloat;

                        Debug.Assert((float) xmlAttribute.DefaultValue == defaultFloat);
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

                        UInt32 stringHash = StreamTools.ReadUInt32(_buffer, ref _offset);       // table string hash
                        Int32 elementCount = StreamTools.ReadInt32(_buffer, ref _offset);       // table element count

                        XmlDefinition tableXmlDefition = _GetXmlDefinition(stringHash);
                        if (tableXmlDefition == null) throw new Exceptions.NotSupportedFileDefinitionException();
                        if (tableXmlDefition.Count < elementCount) throw new Exceptions.NotSupportedXmlElementCount(tableXmlDefition.RootElement.Name);

                        if (elementCount == -1)
                        {
                            xmlTree.AddExistingTree(stringHash);
                        }
                        else
                        {
                            XmlCookedTree xmlChildTree = new XmlCookedTree(tableXmlDefition, xmlElement, xmlTree, elementCount);
                            xmlTree.AddTree(xmlChildTree);
                            _ReadXmlDefinition(xmlChildTree);
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

        private Object _ReadXmlData(XmlCookedTree xmlTree, Type objType)
        {
            Object obj = Activator.CreateInstance(objType);
            int elementCount = xmlTree.Count;

            // bitField info
            int bitFieldOffset = _offset;
            int bitFieldByteCount = (elementCount - 1 >> 3) + 1; // -1 as 16 >> 3 = 2 + 1 = 3, but should only be 2 bytes
            _offset += bitFieldByteCount;

            Dictionary<uint, XmlElement> elements = xmlTree.Definition.Elements;
            int i = -1;
            foreach (XmlElement xmlElement in elements.Values)
            {
                XmlCookedAttribute xmlAttribute = xmlElement.XmlAttribute;
                FieldDelegate fieldDelegate = xmlElement.FieldDelegate;
                Object currObj = obj;
                
                if (xmlAttribute.ParentNames != null)
                {
                    foreach (String parentName in xmlAttribute.ParentNames)
                    {
                        XmlElement parentElement = xmlTree.GetElement(parentName);
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
                        int int32Val = StreamTools.ReadInt32(_buffer, ref _offset);

                        if (xmlAttribute.CustomType == ElementType.Bool)
                        {
                            bool boolValue = (int32Val == 0) ? false : true;
                            fieldDelegate.SetValue(currObj, boolValue);
                        }
                        else
                        {
                            fieldDelegate.SetValue(currObj, int32Val);
                        }
                        break;

                    case ElementType.Float:                         // 0x0100
                        float floatValue = StreamTools.ReadFloat(_buffer, ref _offset);
                        fieldDelegate.SetValue(currObj, floatValue);
                        break;

                    case ElementType.FloatArrayFixed:               // 0x0106
                        float[] floatArray = new float[xmlAttribute.Count];
                        for (int fIndex = 0; fIndex < xmlAttribute.Count; fIndex++)
                        {
                            floatArray[fIndex] = StreamTools.ReadFloat(_buffer, ref _offset);
                        }

                        if (xmlAttribute.CustomType == ElementType.Vector3)
                        {
                            if (floatArray.Length != 3) throw new Exceptions.InvalidXmlElement(xmlAttribute.Name, "The Vector3 Object does not have 3 elements within the FloatArrayFixed. Definition = " + xmlTree.Definition);

                            Vector3 vector3Obj = new Vector3(floatArray[0], floatArray[1], floatArray[2]);
                            fieldDelegate.SetValue(currObj, vector3Obj);
                        }
                        else
                        {
                            fieldDelegate.SetValue(currObj, floatArray);
                        }
                        break;

                    case ElementType.String:                        // 0x0200
                        int charCount = StreamTools.ReadInt32(_buffer, ref _offset);
                        String str = StreamTools.ReadStringAscii(_buffer, ref _offset, charCount - 1, true);
                        fieldDelegate.SetValue(currObj, str);
                        break;

                    case ElementType.Table:                         // 0x0308
                    case ElementType.TableArrayFixed:               // 0x0309
                    case ElementType.TableArrayVariable:            // 0x030A
                        _ReadTables(xmlAttribute, xmlTree.GetTree(i), currObj);
                        break;

                    case ElementType.ExcelIndex:                    // 0x0903
                        Object singleExcelRow = _ReadExcelIndex(xmlAttribute);
                        fieldDelegate.SetValue(currObj, singleExcelRow);
                        break;

                    case ElementType.ExcelIndexArrayFixed:          // 0x0905
                        Object[] excelRows = new Object[xmlAttribute.Count];
                        for (int j = 0; j < xmlAttribute.Count; j++)
                        {
                            Object excelRow = _ReadExcelIndex(xmlAttribute);
                            excelRows[j] = excelRow;
                        }
                        fieldDelegate.SetValue(currObj, excelRows);
                        break;

                    case ElementType.Flag:                          // 0x0B01
                        uint currFlags = (uint)fieldDelegate.GetValue(currObj);
                        if (currFlags == 0)
                        {
                            uint flags = StreamTools.ReadUInt32(_buffer, ref _offset);
                            fieldDelegate.SetValue(currObj, flags);
                        }
                        break;

                    case ElementType.BitFlag:                       // 0x0C02
                        uint currBitFlags = (uint)fieldDelegate.GetValue(currObj);
                        if (currBitFlags == 0)
                        {
                            uint flags = StreamTools.ReadUInt32(_buffer, ref _offset);
                            fieldDelegate.SetValue(currObj, flags);
                        }
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

        private static void _SetFieldDefault(XmlElement xmlElement, Object obj, Object defaultValue)
        {
            XmlCookedAttribute xmlAttribute = xmlElement.XmlAttribute;

            if (xmlAttribute.ElementType == ElementType.Flag || xmlAttribute.ElementType == ElementType.BitFlag)
            {
                if ((bool)defaultValue) // only ever seen as false - if not false, new code needed
                {
                    throw new NotImplementedException("if ((bool)defaultValue)"); // doubt it'll ever happen - just being lazy
                }

                return; // false for a flag is default
            }

            if (xmlAttribute.ElementType == ElementType.FloatArrayFixed && xmlAttribute.CustomType == ElementType.Vector3)
            {
                defaultValue = new Vector3((float)defaultValue, (float)defaultValue, (float)defaultValue);
            }

            xmlElement.FieldDelegate.SetValue(obj, defaultValue);
        }

        private void _ReadTables(XmlCookedAttribute xmlAttribute, XmlCookedTree xmlTree, Object currObject)
        {
            Debug.Assert(xmlAttribute.ChildType != null);

            int count;
            switch (xmlAttribute.ElementType)
            {
                case ElementType.TableArrayVariable:
                    count = StreamTools.ReadInt32(_buffer, ref _offset);
                    break;

                case ElementType.TableArrayFixed:
                    count = xmlAttribute.Count;
                    break;

                default:
                    Object table = _ReadXmlData(xmlTree.TwinRoot, xmlAttribute.ChildType);
                    xmlAttribute.FieldDelegate.SetValue(currObject, table);
                    return;
            }
            if (count == 0) return;

            Object[] objs = (Object[])Activator.CreateInstance(xmlAttribute.FieldDelegate.FieldType, count);
            for (int i = 0; i < count; i++)
            {
                objs[i] = _ReadXmlData(xmlTree.TwinRoot, xmlAttribute.ChildType);
            }
            xmlAttribute.FieldDelegate.SetValue(currObject, objs);
        }

        private String _ReadByteString()
        {
            byte strLen = _buffer[_offset++];
            if (strLen == 0xFF) return null;
            if (strLen == 0x00) return String.Empty;

            return FileTools.ByteArrayToStringASCII(_buffer, ref _offset, strLen);
        }

        private Object _ReadExcelIndex(XmlCookedAttribute xmlAttribute)
        {
            String excelString = _ReadByteString();
            if (excelString == null) return null;

            Object row = _fileManager.GetRowFromFirstString(xmlAttribute.TableCode, excelString);
            if (row == null)
            {
                Debug.WriteLine("Warning: XML File has invalid Excel String '{0}' on table {1}", excelString, xmlAttribute.TableCode);
                return null;
            }

            return row;
        }

        private static void _GenerateXmlDefinitions()
        {
            foreach (Type xmlType in XmlDefinitionTypes)
            {
                Dictionary<uint, XmlElement> xmlElements = _GetDefinitionElements(xmlType);

                XmlCookedAttribute[] query = (XmlCookedAttribute[])xmlType.GetCustomAttributes(typeof(XmlCookedAttribute), true);
                if (query.Length != 1 || String.IsNullOrEmpty(query[0].Name)) throw new Exceptions.InvalidXmlElement(xmlType.FullName, "The XML class doesn't have a valid XmlCookedAttribute associated with it.");

                XmlCookedAttribute xmlAttribute = query[0];
                xmlAttribute.NameHash = Crypt.GetStringHash(xmlAttribute.Name);

                XmlDefinitions.Add(xmlAttribute.NameHash, new XmlDefinition(xmlAttribute, xmlElements));
            }
        }

        private static XmlDefinition _GetXmlDefinition(uint rootHash)
        {
            XmlDefinition xmlDefinition;
            return !XmlDefinitions.TryGetValue(rootHash, out xmlDefinition) ? null : xmlDefinition;
        }

        private static Dictionary<uint, XmlElement> _GetDefinitionElements(Type type)
        {
            FieldInfo[] xmlFields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            ObjectDelegator objectDelegator = new ObjectDelegator(xmlFields);

            Dictionary<uint, XmlElement> xmlElements = new Dictionary<uint, XmlElement>();
            foreach (FieldDelegate fieldDelegate in objectDelegator)
            {
                XmlCookedAttribute[] xmlAttributes = (XmlCookedAttribute[])fieldDelegate.Info.GetCustomAttributes(typeof(XmlCookedAttribute), true);
                if (xmlAttributes.Length == 0) continue;

                XmlCookedAttribute xmlAttribute = xmlAttributes[0];
                if (String.IsNullOrEmpty(xmlAttribute.Name)) throw new Exceptions.InvalidXmlElement("<NONAME>", "Encountered an XML Element with no Name within Type = " + type.FullName);

                // if we have more than one attribute, we have a CustomType - need to find "parent" attribute to confirm and get name
                if (xmlAttributes.Length > 1 && xmlAttribute.CustomType == ElementType.Int32) // ElementType.Int32 = 0x0000
                {
                    String elementName = xmlAttribute.Name;
                    xmlAttribute = xmlAttributes.FirstOrDefault(xmlAttrib => xmlAttrib.CustomType != ElementType.Int32);
                    if (xmlAttribute == null) throw new Exceptions.InvalidXmlElement(elementName, "Enountered XML Element with multiple attributes, but not CustomType found in definition = " + type.FullName);
                }

                // is it a Vector3 type?
                if (xmlAttribute.CustomType == ElementType.Vector3)
                {
                    if (xmlAttribute.ElementType == ElementType.Float)
                    {
                        Debug.Assert(xmlAttributes.Length == 4);

                        XmlCookedAttribute xAttribute = null;
                        XmlCookedAttribute yAttribute = null;
                        XmlCookedAttribute zAttribute = null;
                        foreach (XmlCookedAttribute floatAttrib in xmlAttributes)
                        {
                            if (floatAttrib.Name.EndsWith("fX")) xAttribute = floatAttrib;
                            else if (floatAttrib.Name.EndsWith("fY")) yAttribute = floatAttrib;
                            else if (floatAttrib.Name.EndsWith("fZ")) zAttribute = floatAttrib;
                            else continue;

                            floatAttrib.NameHash = Crypt.GetStringHash(floatAttrib.Name);
                            if (floatAttrib.ParentNames == null) floatAttrib.ParentNames = new List<String>(); // this is kind of gross - but meh for now
                            floatAttrib.ParentNames.Insert(0, xmlAttribute.Name);
                        }

                        if (xAttribute == null || yAttribute == null || zAttribute == null) throw new Exceptions.InvalidXmlElement(xmlAttribute.Name, "The XML Element does not have all 3 vector names defined in definition = " + type.FullName);

                        xmlElements.Add(xAttribute.NameHash, new XmlElement(xAttribute, _xVectorDelegate));
                        xmlElements.Add(yAttribute.NameHash, new XmlElement(yAttribute, _yVectorDelegate));
                        xmlElements.Add(zAttribute.NameHash, new XmlElement(zAttribute, _zVectorDelegate));
                    }
                }

                // is it a Flags Enum?
                if (xmlAttribute.ElementType == ElementType.Flag || xmlAttribute.ElementType == ElementType.BitFlag)
                {
                    Array flagValues = Enum.GetValues(fieldDelegate.FieldType);
                    foreach (Object value in flagValues)
                    {
                        XmlCookedAttribute flagsAttribute = new XmlCookedAttribute
                        {
                            Name = value.ToString(),
                            NameHash = Crypt.GetStringHash(value.ToString()),
                            ElementType = xmlAttribute.ElementType,
                            DefaultValue = false
                        };

                        if (xmlAttribute.ElementType == ElementType.Flag)
                        {
                            flagsAttribute.FlagId = xmlAttribute.FlagId;
                            flagsAttribute.FlagMask = (uint) value;
                        }
                        else
                        {
                            int index = 0;
                            uint valueInt = (uint) value;
                            for (; valueInt > 1; valueInt >>= 1, index++) { }

                            flagsAttribute.BitFlagIndex = index;
                            flagsAttribute.BitFlagCount = flagValues.Length;
                        }
                        XmlElement flagElement = new XmlElement(flagsAttribute, fieldDelegate);

                        xmlElements.Add(flagsAttribute.NameHash, flagElement);
                    }

                    continue;
                }

                xmlAttribute.NameHash = Crypt.GetStringHash(xmlAttribute.Name);

                if (xmlAttribute.CustomType == ElementType.Object)
                {
                    Dictionary<uint, XmlElement> objectFields = _GetDefinitionElements(xmlAttribute.ChildType);
                    foreach (XmlElement xmlElement in objectFields.Values)
                    {
                        if (xmlElement.XmlAttribute.ParentNames == null) xmlElement.XmlAttribute.ParentNames = new List<String>(); // this is kind of gross - but meh for now
                        xmlElement.XmlAttribute.ParentNames.Insert(0, xmlAttribute.Name);

                        xmlElements.Add(xmlElement.NameHash, xmlElement);
                    }
                }

                xmlAttribute.FieldDelegate = fieldDelegate;
                xmlElements.Add(xmlAttribute.NameHash, new XmlElement(xmlAttribute, fieldDelegate));
            }

            return xmlElements;
        }

        private static bool _TestBit(IList<byte> bitField, int byteOffset, int bitOffset)
        {
            byteOffset += bitOffset >> 3;
            bitOffset &= 0x07;

            return (bitField[byteOffset] & (1 << bitOffset)) >= 1;
        }
    }
}