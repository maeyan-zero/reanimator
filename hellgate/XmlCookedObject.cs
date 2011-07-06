using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using Hellgate.Xml;
using Revival.Common;
using FieldDelegate = Revival.Common.ObjectDelegator.FieldDelegate;

namespace Hellgate
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = true)]
    public class XmlCookedAttribute : Attribute
    {
        public String Name;
        public String TrueName;             // some element names have illegal xml chars in them (bad FSS), so we need the "true" name if we want the correct string hash
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
        public bool IsByteArray;
        public bool IsTestCentre;           // TestCentre elements only
        public bool IsResurrection;         // Resurrection elements only
        public UInt32 HashOverride;         // Elements with unknown strings
        public FieldDelegate FieldDelegate;
        public List<String> ParentNames;    // CustomType elements with parents (e.g. Objects or fX+fY+fZ->Vector3)
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
            public readonly XmlElement Parent;

            public ElementType ElementType { get { return XmlAttribute.ElementType; } }
            public bool IsTestCentre { get { return XmlAttribute.IsTestCentre; } }
            public bool IsResurrection { get { return XmlAttribute.IsResurrection; } }
            public String Name { get { return XmlAttribute.Name; } }
            public uint NameHash { get { return XmlAttribute.NameHash; } }

            // set to true for elements that should *not* be read from an XML file (e.g. the "root" element of 3xFloat -> 1xVector3)
            public bool IsCustomOrigin { get; set; }

            public Object GetValue(Object obj)
            {
                return FieldDelegate.GetValue(obj);
            }

            public Object Default;

            public XmlElement(XmlCookedTree xmlTree)
            {
                XmlTree = xmlTree;
            }

            public XmlElement(XmlCookedAttribute xmlAttribute, FieldDelegate fieldDelegate, XmlElement parent = null)
            {
                XmlAttribute = xmlAttribute;
                FieldDelegate = fieldDelegate;
                Parent = parent;
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
            public readonly Type XmlObjectType;
            public readonly int SinglePlayerElementCount;
            public readonly int TestCentreElementCount;
            public readonly int ResurrectionElementCount;

            public int Count { get { return Elements.Count; } }
            public UInt32 RootHash { get { return RootElement.NameHash; } }

            public XmlDefinition(XmlCookedAttribute rootElement, Dictionary<uint, XmlElement> xmlElements, Type xmlObjectType)
            {
                RootElement = rootElement;
                Elements = xmlElements;
                XmlObjectType = xmlObjectType;

                foreach (XmlElement xmlElement in xmlElements.Values)
                {
                    if (xmlElement.IsCustomOrigin) continue;

                    if (xmlElement.IsResurrection) ResurrectionElementCount++;
                    if (xmlElement.IsTestCentre) TestCentreElementCount++;
                    if (!xmlElement.IsResurrection && !xmlElement.IsTestCentre) SinglePlayerElementCount++;
                }
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
            typeof (ConditionDefinition),
            
            // AI Behavior
            typeof (AIDefinition),
            typeof (AIBehaviorDefinitionTable),
            typeof (AIBehaviorDefinition),

            // Appearance
            typeof (AppearanceDefinition),
            typeof (AnimationDefinition),
            typeof (AnimEvent),
            typeof (InventoryViewInfo),

            // Colorsets
            typeof (ColorSetDefinition),
            typeof (ColorDefinition),

            // Demo Levels
            typeof (DemoLevelDefinition),

            // Environments
            typeof (EnvironmentDefinition),
            typeof (EnvLightDefinition),

            // Lights
            typeof (LightDefinition),

            // Materials
            typeof (Material),

            // Particle System
            typeof (ParticleSystemDefinition),

            // Room Layouts
            typeof (RoomPathNodeDefinition),
            typeof (RoomPathNodeSet),
            typeof (RoomPathNode),
            typeof (RoomLayoutGroupDefinition),
            typeof (RoomLayoutGroup),

            // Room Paths
            typeof (RoomPathNodeConnection),
            typeof (RoomPathNodeConnectionRef),

            // Screen Effects
            typeof (ScreenEffectDefinition),

            // Skill Events
            typeof (SkillEventsDefinition),
            typeof (SkillEventHolder),
            typeof (SkillEvent),

            // Skyboxes
            typeof (SkyboxDefinition),
            typeof (SkyboxModel),

            // Sound Effects
            typeof (SoundEffectDefinition),
            typeof (SoundEffect),

            // Sounds Reverbs
            typeof (SoundReverbDefinition),
            typeof (FmodReverbProperties),

            // State Events
            typeof (StateDefinition),
            typeof (StateEvent),

            // Textures
            typeof (TextureDefinition),
            typeof (BlendRLE),
            typeof (BlendRun)
        };
        private static readonly Dictionary<uint, XmlDefinition> XmlDefinitions = new Dictionary<uint, XmlDefinition>(XmlDefinitionTypes.Length);
        private static FieldDelegate _xVectorDelegate;
        private static FieldDelegate _yVectorDelegate;
        private static FieldDelegate _zVectorDelegate;

        private readonly FileManager _fileManager;
        private byte[] _buffer;
        private int _offset;
        //private Parse _parseTo;
        private bool _generateXml;

        public XmlDocument XmlDoc { get; private set; }
        public XmlDefinition Definition;
        public bool HasTestCentreElements;
        public bool HasResurrectionElements;

        public enum Parse
        {
            ToXml,
            ToObj,
            ToXmlAndObj
        }

        public XmlCookedObject(FileManager fileManager)
        {
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
        }

        public Object ParseFileBytes(byte[] fileBytes, int offset = 0, bool generateXml = false)
        {
            if (fileBytes == null) throw new ArgumentNullException("fileBytes", "File bytes cannot be null!");

            _buffer = fileBytes;
            _offset = offset;
            //_parseTo = parseTo;
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

            XmlDefinition xmlDefinition = GetXmlDefinition(header.XmlRootDefinition);
            Definition = xmlDefinition;
            if (xmlDefinition == null) throw new Exceptions.NotSupportedFileDefinitionException();
            if (xmlDefinition.Count < header.XmlRootElementCount) throw new Exceptions.NotSupportedXmlElementCount(xmlDefinition.RootElement.Name);

            XmlCookedTree xmlTree = new XmlCookedTree(xmlDefinition, header.XmlRootElementCount);
            _ReadXmlDefinition(xmlTree);

            UInt32 dataMagicWord = FileTools.ByteArrayToUInt32(_buffer, ref _offset);
            if (dataMagicWord != DataMagicWord) throw new Exceptions.UnexpectedTokenException("'DATA' Token MagicWord expected but not found!");

            XmlNode root = null;
            //if (_parseTo == Parse.ToXml || _parseTo == Parse.ToXmlAndObj) // this is equivalent to (_parseTo != Parse.ToObj), but it reads better using ==
            if (_generateXml)
            {
                XmlDoc = new XmlDocument();
                root = XmlDoc.CreateElement(xmlDefinition.RootElement.Name);
                XmlDoc.AppendChild(root);
            }

            return _ReadXmlData(xmlTree, xmlDefinition.XmlObjectType, root, null);
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

            XmlDefinition xmlDefinition = GetXmlDefinition(header.XmlRootDefinition);
            if (xmlDefinition == null) throw new Exceptions.NotSupportedFileDefinitionException();
            if (xmlDefinition.Count < header.XmlRootElementCount) throw new Exceptions.NotSupportedXmlElementCount(xmlDefinition.RootElement.Name);

            XmlCookedTree xmlTree = new XmlCookedTree(xmlDefinition, header.XmlRootElementCount);
            _ReadXmlDefinition(xmlTree);

            UInt32 dataMagicWord = FileTools.ByteArrayToUInt32(_buffer, ref _offset);
            if (dataMagicWord != DataMagicWord) throw new Exceptions.UnexpectedTokenException("'DATA' Token MagicWord expected but not found!");

            return (T)_ReadXmlData(xmlTree, typeof(T), null, null);
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

                        UInt32 stringHash = StreamTools.ReadUInt32(_buffer, ref _offset);       // table string hash
                        Int32 elementCount = StreamTools.ReadInt32(_buffer, ref _offset);       // table element count

                        XmlDefinition tableXmlDefition = GetXmlDefinition(stringHash);
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

        private Object _ReadXmlData(XmlCookedTree xmlTree, Type objType, XmlNode xmlRoot, XmlNode xmlDesc)
        {
            Object obj = Activator.CreateInstance(objType);
            int elementCount = xmlTree.Count;

            // bitField info
            int bitFieldOffset = _offset;
            int bitFieldByteCount = (elementCount - 1 >> 3) + 1; // -1 as 16 >> 3 = 2 + 1 = 3, but should only be 2 bytes
            _offset += bitFieldByteCount;

            XmlCookedFile.XmlFlag[] flags = new XmlCookedFile.XmlFlag[3];

            Dictionary<uint, XmlElement> elements = xmlTree.Definition.Elements;
            int i = -1;
            foreach (XmlElement xmlElement in elements.Values)
            {
                if (xmlElement.IsCustomOrigin) continue;

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
                        Object innerTextObj;
                        if (xmlAttribute.CustomType == ElementType.Bool)
                        {
                            int int32Val = StreamTools.ReadInt32(_buffer, ref _offset);
                            bool boolValue = (int32Val == 0) ? false : true;
                            fieldDelegate.SetValue(currObj, boolValue);
                            innerTextObj = boolValue;
                        }
                        else if (xmlAttribute.CustomType == ElementType.Unsigned)
                        {
                            UInt32 uint32Val = StreamTools.ReadUInt32(_buffer, ref _offset);
                            fieldDelegate.SetValue(currObj, uint32Val);
                            innerTextObj = uint32Val;
                        }
                        else
                        {
                            Int32 int32Val = StreamTools.ReadInt32(_buffer, ref _offset);
                            fieldDelegate.SetValue(currObj, int32Val);
                            innerTextObj = int32Val;
                        }

                        if (_generateXml && xmlRoot != null)
                        {
                            XmlNode int32Element = XmlDoc.CreateElement(xmlAttribute.Name);
                            xmlRoot.AppendChild(int32Element);
                            int32Element.InnerText = innerTextObj.ToString();
                        }

                        _offset += xmlAttribute.FlagOffsetChange;
                        break;

                    case ElementType.Int32ArrayFixed:               // 0x0006
                        Object arrayObj = null;
                        if (xmlAttribute.CustomType == ElementType.Unsigned)
                        {
                            UInt32[] uint32Array = new UInt32[xmlAttribute.Count];
                            for (int intIndex = 0; intIndex < xmlAttribute.Count; intIndex++)
                            {
                                uint32Array[intIndex] = StreamTools.ReadUInt32(_buffer, ref _offset);

                                if (!_generateXml || xmlRoot == null) continue;
                                XmlNode arrElement = XmlDoc.CreateElement(xmlAttribute.Name);
                                xmlRoot.AppendChild(arrElement);
                                arrElement.InnerText = uint32Array[intIndex].ToString();
                            }
                        }
                        else
                        {
                            Int32[] int32Array = new Int32[xmlAttribute.Count];
                            for (int intIndex = 0; intIndex < xmlAttribute.Count; intIndex++)
                            {
                                int32Array[intIndex] = StreamTools.ReadInt32(_buffer, ref _offset);

                                if (!_generateXml || xmlRoot == null) continue;
                                XmlNode arrElement = XmlDoc.CreateElement(xmlAttribute.Name);
                                xmlRoot.AppendChild(arrElement);
                                arrElement.InnerText = int32Array[intIndex].ToString();
                            }
                        }

                        fieldDelegate.SetValue(currObj, arrayObj);
                        break;

                    case ElementType.Int32ArrayVariable:            // 0x0007
                        int count = StreamTools.ReadInt32(_buffer, ref _offset);
                        Debug.Assert(count >= 0);
                        int[] int32Arr = new int[count];

                        if (_generateXml && xmlRoot != null)
                        {
                            XmlNode int32ArrCountEle = XmlDoc.CreateElement(xmlAttribute.Name + "Count");
                            xmlRoot.AppendChild(int32ArrCountEle);
                            int32ArrCountEle.InnerText = count.ToString();
                        }

                        for (int int32Index = 0; int32Index < count; int32Index++)
                        {
                            int32Arr[int32Index] = StreamTools.ReadInt32(_buffer, ref _offset);

                            if (!_generateXml || xmlRoot == null) continue;
                            XmlNode arrElement = XmlDoc.CreateElement(xmlAttribute.Name);
                            xmlRoot.AppendChild(arrElement);
                            arrElement.InnerText = int32Arr[int32Index].ToString();
                        }

                        fieldDelegate.SetValue(currObj, int32Arr);
                        break;

                    case ElementType.Float:                         // 0x0100
                        _ReadFloat(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        //float floatValue = StreamTools.ReadFloat(_buffer, ref _offset);
                        //fieldDelegate.SetValue(currObj, floatValue);

                        //if (_generateXml && xmlRoot != null)
                        //{
                        //    // is the float value a negative zero? - this is done to enable byte-for-byte checking for test cases
                        //    bool isNegativeZero = false;
                        //    if (floatValue == 0)
                        //    {
                        //        if (_TestBit(_buffer, _offset - 1, 7))
                        //        {
                        //            isNegativeZero = true;
                        //        }
                        //    }

                        //    XmlNode floatElement = XmlDoc.CreateElement(xmlAttribute.Name);
                        //    xmlRoot.AppendChild(floatElement);
                        //    floatElement.InnerText = (isNegativeZero ? "-" : "") + floatValue.ToString("R");
                        //}
                        break;

                    case ElementType.FloatArrayFixed:               // 0x0106
                        float[] floatArray = new float[xmlAttribute.Count];

                        for (int fIndex = 0; fIndex < xmlAttribute.Count; fIndex++)
                        {
                            floatArray[fIndex] = StreamTools.ReadFloat(_buffer, ref _offset);

                            if (!_generateXml || xmlRoot == null) continue;

                            // is the float value a negative zero? - this is done to enable byte-for-byte checking for test cases
                            bool isNegativeZero = false;
                            if (floatArray[fIndex] == 0)
                            {
                                if (_TestBit(_buffer, _offset - 1, 7))
                                {
                                    isNegativeZero = true;
                                }
                            }

                            XmlNode floatElement = XmlDoc.CreateElement(xmlAttribute.Name);
                            xmlRoot.AppendChild(floatElement);
                            floatElement.InnerText = (isNegativeZero ? "-" : "") + floatArray[fIndex].ToString("R");
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

                    case ElementType.FloatArrayVariable:            // 0x0107
                        int floatArrCount = StreamTools.ReadInt32(_buffer, ref _offset);

                        if (_generateXml && xmlRoot != null)
                        {
                            XmlNode countElement = XmlDoc.CreateElement(xmlElement.Name + "Count");
                            countElement.InnerText = floatArrCount.ToString();
                            xmlRoot.AppendChild(countElement);
                        }

                        for (int floatArrIndex = 0; floatArrIndex < floatArrCount; floatArrIndex++)
                        {
                            _ReadFloat(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        }
                        break;

                    case ElementType.FloatTripletArrayVariable:     // 0x0500       found in AppearanceDefinition and children, appears on types like tSelfIllumation, tSelfIllumationBlend, etc
                        int fTripCount = StreamTools.ReadInt32(_buffer, ref _offset);
                        Debug.Assert(fTripCount != 0);

                        if (xmlAttribute.CustomType != ElementType.Vector3) // I guess we could also have it as a float[][3] array, but fuck that
                        {
                            throw new NotImplementedException("case ElementType.FloatTripletArrayVariable && if (xmlAttribute.CustomType != ElementType.Vector3)");
                        }

                        if (_generateXml && xmlRoot != null)
                        {
                            XmlNode countElement = XmlDoc.CreateElement(xmlElement.Name + "Count");
                            countElement.InnerText = fTripCount.ToString();
                            xmlRoot.AppendChild(countElement);
                        }

                        Vector3[] fTripArray = new Vector3[fTripCount];
                        for (int fTripIndex = 0; fTripIndex < fTripCount; fTripIndex++)
                        {
                            fTripArray[fTripIndex] = new Vector3
                            {
                                X = StreamTools.ReadFloat(_buffer, ref _offset),
                                Y = StreamTools.ReadFloat(_buffer, ref _offset),
                                Z = StreamTools.ReadFloat(_buffer, ref _offset)
                            };

                            if (!_generateXml || xmlRoot == null) continue;

                            // this is pretty gross, but is done to enable byte-for-byte checking for test cases todo: remove me or add option to disable?
                            bool isNegativeZeroX = false;
                            bool isNegativeZeroY = false;
                            bool isNegativeZeroZ = false;
                            if (fTripArray[fTripIndex].Z == 0 && _TestBit(_buffer, _offset - 1, 7)) isNegativeZeroZ = true;
                            if (fTripArray[fTripIndex].Y == 0 && _TestBit(_buffer, _offset - 5, 7)) isNegativeZeroY = true;
                            if (fTripArray[fTripIndex].X == 0 && _TestBit(_buffer, _offset - 9, 7)) isNegativeZeroX = true;

                            if (isNegativeZeroX || isNegativeZeroY || isNegativeZeroZ)
                            {
                                XmlNode fTripElement = XmlDoc.CreateElement(xmlElement.Name);
                                fTripElement.InnerText = String.Format("{0}{1}, {2}{3}, {4}{5}", // todo: for memory there is a flag to enable showing the sign - check me
                                                            (isNegativeZeroX ? "-" : ""), fTripArray[fTripIndex].X,
                                                            (isNegativeZeroY ? "-" : ""), fTripArray[fTripIndex].Y,
                                                            (isNegativeZeroZ ? "-" : ""), fTripArray[fTripIndex].Z);
                                xmlRoot.AppendChild(fTripElement);
                            }
                            else
                            {
                                XmlNode fTripElement = XmlDoc.CreateElement(xmlElement.Name);
                                fTripElement.InnerText = fTripArray[fTripIndex].ToString();
                                xmlRoot.AppendChild(fTripElement);
                            }
                        }

                        break;

                    case ElementType.FloatQuadArrayVariable:        // 0x0600       found in Screen FX and EnvironmentDefinition
                        int fQuadCount = StreamTools.ReadInt32(_buffer, ref _offset);
                        Debug.Assert(fQuadCount != 0);

                        if (xmlAttribute.CustomType != ElementType.Vector4) // I guess we could also have it as a float[][4] array, but fuck that
                        {
                            throw new NotImplementedException("case ElementType.FloatQuadArrayVariable && if (xmlAttribute.CustomType != ElementType.Vector4)");
                        }

                        if (_generateXml && xmlRoot != null)
                        {
                            XmlNode countElement = XmlDoc.CreateElement(xmlElement.Name + "Count");
                            countElement.InnerText = fQuadCount.ToString();
                            xmlRoot.AppendChild(countElement);
                        }

                        Vector4[] fQuadArray = new Vector4[fQuadCount];
                        for (int fQuadIndex = 0; fQuadIndex < fQuadCount; fQuadIndex++)
                        {
                            fQuadArray[fQuadIndex] = new Vector4
                            {
                                X = StreamTools.ReadFloat(_buffer, ref _offset),
                                Y = StreamTools.ReadFloat(_buffer, ref _offset),
                                Z = StreamTools.ReadFloat(_buffer, ref _offset),
                                W = StreamTools.ReadFloat(_buffer, ref _offset)
                            };

                            if (!_generateXml || xmlRoot == null) continue;
                            XmlNode fQuadElement = XmlDoc.CreateElement(xmlElement.Name);
                            fQuadElement.InnerText = fQuadArray[fQuadIndex].ToString();
                            xmlRoot.AppendChild(fQuadElement);
                        }
                        break;

                    case ElementType.String:                        // 0x0200
                        _ReadString(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        break;

                    case ElementType.StringArrayFixed:              // 0x0206
                        for (int strElementIndex = 0; strElementIndex < xmlAttribute.Count; strElementIndex++)
                        {
                            _ReadString(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        }
                        break;

                    case ElementType.StringArrayVariable:           // 0x0207
                        int stringArrCount = StreamTools.ReadInt32(_buffer, ref _offset);

                        if (_generateXml && xmlRoot != null)
                        {
                            XmlNode countElement = XmlDoc.CreateElement(xmlElement.Name + "Count");
                            countElement.InnerText = stringArrCount.ToString();
                            xmlRoot.AppendChild(countElement);
                        }

                        for (int strElementIndex = 0; strElementIndex < stringArrCount; strElementIndex++)
                        {
                            _ReadString(xmlAttribute, fieldDelegate, currObj, xmlRoot);
                        }
                        break;

                    case ElementType.Table:                         // 0x0308
                    case ElementType.TableArrayFixed:               // 0x0309
                    case ElementType.TableArrayVariable:            // 0x030A
                        _ReadTables(xmlAttribute, xmlTree.GetTree(i), currObj, xmlRoot);
                        break;

                    case ElementType.ExcelIndex:                    // 0x0903
                        Object singleExcelRow = _ReadExcelIndex(xmlAttribute, xmlRoot, xmlDesc);
                        fieldDelegate.SetValue(currObj, singleExcelRow);
                        break;

                    case ElementType.ExcelIndexArrayFixed:          // 0x0905
                        Object[] excelRows = new Object[xmlAttribute.Count];
                        for (int j = 0; j < xmlAttribute.Count; j++)
                        {
                            Object excelRow = _ReadExcelIndex(xmlAttribute, xmlRoot, xmlDesc);
                            excelRows[j] = excelRow;
                        }
                        fieldDelegate.SetValue(currObj, excelRows);
                        break;

                    case ElementType.Flag:                          // 0x0B01
                        UInt32 currFlags = (uint)fieldDelegate.GetValue(currObj);
                        if (currFlags == 0)
                        {
                            currFlags = StreamTools.ReadUInt32(_buffer, ref _offset);
                            fieldDelegate.SetValue(currObj, currFlags);
                        }

                        if (_generateXml && xmlRoot != null)
                        {
                            bool flagged = false;
                            switch (xmlElement.ElementType)
                            {
                                case ElementType.BitFlag:
                                    flagged = (currFlags & (1 << xmlAttribute.BitFlagIndex)) > 0;
                                    break;

                                case ElementType.Flag:
                                    flagged = (currFlags & xmlAttribute.FlagMask) > 0;
                                    break;
                            }

                            String innerText = flagged ? "1" : "0";

                            XmlNode flagElement = XmlDoc.CreateElement(xmlAttribute.Name);
                            xmlRoot.AppendChild(flagElement);
                            flagElement.InnerText = innerText;
                        }
                        break;

                    case ElementType.BitFlag:                       // 0x0C02
                        UInt32 currBitFlags = (UInt32)fieldDelegate.GetValue(currObj);
                        if (currBitFlags == 0)
                        {
                            currBitFlags = StreamTools.ReadUInt32(_buffer, ref _offset);
                            fieldDelegate.SetValue(currObj, currBitFlags);
                        }

                        if (_generateXml && xmlRoot != null)
                        {
                            bool flagged = false;
                            switch (xmlElement.ElementType)
                            {
                                case ElementType.BitFlag:
                                    flagged = (currBitFlags & (1 << xmlAttribute.BitFlagIndex)) > 0;
                                    break;

                                case ElementType.Flag:
                                    flagged = (currBitFlags & xmlAttribute.FlagMask) > 0;
                                    break;
                            }

                            String innerText = flagged ? "1" : "0";

                            XmlNode flagElement = XmlDoc.CreateElement(xmlAttribute.Name);
                            xmlRoot.AppendChild(flagElement);
                            flagElement.InnerText = innerText;
                        }
                        break;

                    case ElementType.ByteArrayVariable:
                        Int32 byteCount = FileTools.ByteArrayToInt32(_buffer, ref _offset);
                        byte[] byteArray = new byte[byteCount];
                        Buffer.BlockCopy(_buffer, _offset, byteArray, 0, byteCount);
                        fieldDelegate.SetValue(currObj, byteArray);
                        
                        _offset += byteCount;

                        if (_generateXml && xmlRoot != null && byteCount > 0)
                        {
                            String value = BitConverter.ToString(byteArray);

                            XmlNode bytesElement = XmlDoc.CreateElement(xmlAttribute.Name);
                            xmlRoot.AppendChild(bytesElement);
                            bytesElement.InnerText = value;
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

        private void _ReadFloat(XmlCookedAttribute xmlAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            float floatValue = StreamTools.ReadFloat(_buffer, ref _offset);
            fieldDelegate.SetValue(currObj, floatValue);

            if (!_generateXml || xmlRoot == null) return;
            // is the float value a negative zero? - this is done to enable byte-for-byte checking for test cases
            bool isNegativeZero = false;
            if (floatValue == 0)
            {
                if (_TestBit(_buffer, _offset - 1, 7))
                {
                    isNegativeZero = true;
                }
            }

            XmlNode floatElement = XmlDoc.CreateElement(xmlAttribute.Name);
            xmlRoot.AppendChild(floatElement);
            floatElement.InnerText = (isNegativeZero ? "-" : "") + floatValue.ToString("R");
        }

        private void _ReadString(XmlCookedAttribute xmlAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            int charCount = StreamTools.ReadInt32(_buffer, ref _offset);

            byte b = _buffer[_offset];
            bool overrideTypeAsByteArray = ((b < 0x20 || b > 0x7F) && b != 0x00); // if not a valid string, then treat as data
            for (int i = 4; i < charCount && !overrideTypeAsByteArray; i += 4) // check every 4th byte (don't want to waste too much time checking every byte)
            {
                b = _buffer[_offset + i];
                overrideTypeAsByteArray = ((b < 0x20 || b > 0x7F) && b != 0x00);
            }
            //byte b = _buffer[_offset];
            //bool overrideTypeAsByteArray = ((b < 0x20 || b > 0x7F) && b != 0x00); // if not a valid string, then treat as data
            bool isByteArray = (xmlAttribute.IsByteArray && charCount > 0) || overrideTypeAsByteArray;

            String str = String.Empty;
            if (xmlAttribute.IsByteArray)
            {
                byte[] data = new byte[charCount];
                Buffer.BlockCopy(_buffer, _offset, data, 0, charCount);
                fieldDelegate.SetValue(currObj, data);

                if (_generateXml && xmlRoot != null)
                {
                    str = BitConverter.ToString(data);
                }
            }
            else // is a string
            {
                if (isByteArray) // is a string type, but has non-ascii chars
                {
                    byte[] data = new byte[charCount];
                    Buffer.BlockCopy(_buffer, _offset, data, 0, charCount);
                    str = BitConverter.ToString(data);
                }
                else
                {
                    str = (charCount == 1) ? String.Empty : StreamTools.ReadStringAscii(_buffer, _offset, charCount - 1);
                }

                fieldDelegate.SetValue(currObj, str);
            }

            _offset += charCount;

            if (!_generateXml || xmlRoot == null) return;
            System.Xml.XmlElement stringElement = XmlDoc.CreateElement(xmlAttribute.Name);
            xmlRoot.AppendChild(stringElement);
            stringElement.InnerText = str;
            if (xmlAttribute.IsByteArray)
            {
                stringElement.SetAttribute("isByteArray", "true");
            }
            else if (overrideTypeAsByteArray)
            {
                stringElement.SetAttribute("overrideTypeAsByteArray", "true");
            }
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

            // a FloatArrayFixed has all elements the same default value 
            if (xmlAttribute.ElementType == ElementType.FloatArrayFixed && xmlAttribute.CustomType == ElementType.Vector3)
            {
                defaultValue = new Vector3((float)defaultValue, (float)defaultValue, (float)defaultValue);
            }

            xmlElement.FieldDelegate.SetValue(obj, defaultValue);
        }

        private void _ReadTables(XmlCookedAttribute xmlAttribute, XmlCookedTree xmlTree, Object currObject, XmlNode xmlRoot)
        {
            Debug.Assert(xmlAttribute.ChildType != null);

            XmlNode root = null;
            int count;
            switch (xmlAttribute.ElementType)
            {
                case ElementType.TableArrayVariable:
                    count = StreamTools.ReadInt32(_buffer, ref _offset);
                    break;

                case ElementType.TableArrayFixed:
                    count = xmlAttribute.Count;
                    break;

                default: // case ElementType.Table
                    XmlNode desc = null;
                    if (_generateXml)
                    {
                        desc = XmlDoc.CreateElement(xmlAttribute.Name);
                        xmlRoot.AppendChild(desc);

                        root = XmlDoc.CreateElement(xmlTree.TwinRoot.Definition.RootElement.Name);
                        xmlRoot.AppendChild(root);
                    }

                    Object table = _ReadXmlData(xmlTree.TwinRoot, xmlAttribute.ChildType, root, desc);
                    xmlAttribute.FieldDelegate.SetValue(currObject, table);
                    return;
            }
            if (count == 0) return;

            if (_generateXml && xmlAttribute.ElementType == ElementType.TableArrayVariable)
            {
                XmlNode tableCountElement = XmlDoc.CreateElement(xmlAttribute.Name + "Count");
                xmlRoot.AppendChild(tableCountElement);
                tableCountElement.InnerText = count.ToString();
            }

            Object[] objs = (Object[])Activator.CreateInstance(xmlAttribute.FieldDelegate.FieldType, count);
            for (int i = 0; i < count; i++)
            {
                XmlNode desc = null;
                if (_generateXml)
                {
                    desc = XmlDoc.CreateElement(xmlAttribute.Name);
                    xmlRoot.AppendChild(desc);

                    root = XmlDoc.CreateElement(xmlTree.TwinRoot.Definition.RootElement.Name);
                    xmlRoot.AppendChild(root);
                }

                objs[i] = _ReadXmlData(xmlTree.TwinRoot, xmlAttribute.ChildType, root, desc);
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

        private Object _ReadExcelIndex(XmlCookedAttribute xmlAttribute, XmlNode xmlRoot, XmlNode xmlDesc)
        {
            String excelString = _ReadByteString();
            if (excelString == null)
            {
                // some files have a blank first entry, but filled second entry for whatever reason
                // to preserve the order, we must place blank/empty elements
                if (xmlAttribute.ElementType == ElementType.ExcelIndexArrayFixed && _generateXml && xmlRoot != null) 
                {
                    XmlNode excelElement = XmlDoc.CreateElement(xmlAttribute.Name);
                    xmlRoot.AppendChild(excelElement);
                }

                return null;
            }

            ExcelFile table = _fileManager.GetExcelTableFromCode(xmlAttribute.TableCode);

            Object row;
            if (!table.RowFromFirstString.TryGetValue(excelString, out row))
            {
                Debug.WriteLine("Warning: XML File has invalid Excel String '{0}' on table {1}", excelString, xmlAttribute.TableCode);
                return null;
            }

            if (_generateXml && xmlRoot != null)
            {
                XmlNode excelElement = XmlDoc.CreateElement(xmlAttribute.Name);
                xmlRoot.AppendChild(excelElement);
                excelElement.InnerText = table.Rows.IndexOf(row).ToString();

                if (xmlDesc != null)
                {
                    if (String.IsNullOrEmpty(xmlDesc.InnerText))
                    {
                        xmlDesc.InnerText = excelString;
                    }
                    else
                    {
                        xmlDesc.InnerText += ", " + excelString;
                    }
                }
            }

            //Object row = _fileManager.GetRowFromFirstString(xmlAttribute.TableCode, excelString);
            //if (row == null)
            //{
            //    Debug.WriteLine("Warning: XML File has invalid Excel String '{0}' on table {1}", excelString, xmlAttribute.TableCode);
            //    return null;
            //}

            return row;
        }

        private static void _GenerateXmlDefinitions()
        {
            // generate definitions
            foreach (Type xmlType in XmlDefinitionTypes)
            {
                Dictionary<uint, XmlElement> xmlElements = _GetDefinitionElements(xmlType);
                if (xmlElements.Count == 0) throw new Exceptions.InvalidXmlElement(xmlType.Name, "The XML class doesn't have an valid XML Elements defined.");

                XmlCookedAttribute[] query = (XmlCookedAttribute[])xmlType.GetCustomAttributes(typeof(XmlCookedAttribute), true);
                if (query.Length != 1 || String.IsNullOrEmpty(query[0].Name)) throw new Exceptions.InvalidXmlElement(xmlType.FullName, "The XML class doesn't have a valid XmlCookedAttribute associated with it.");

                XmlCookedAttribute xmlAttribute = query[0];
                xmlAttribute.NameHash = Crypt.GetStringHash(xmlAttribute.Name);

                XmlDefinitions.Add(xmlAttribute.NameHash, new XmlDefinition(xmlAttribute, xmlElements, xmlType));
            }

            // find/assign child name hashes
            foreach (XmlDefinition xmlDefinition in XmlDefinitions.Values)
            {
                foreach (XmlElement xmlElement in xmlDefinition.Elements.Values)
                {
                    if (xmlElement.XmlAttribute.ChildType == null) continue;
                    if (xmlElement.XmlAttribute.CustomType == ElementType.Object) continue;

                    Type childType = xmlElement.XmlAttribute.ChildType;
                    foreach (XmlDefinition xmlDef in XmlDefinitions.Values.Where(xmlDef => xmlDef.XmlObjectType == childType))
                    {
                        xmlElement.XmlAttribute.ChildTypeHash = xmlDef.RootHash;
                        break;
                    }

                    Debug.Assert(xmlElement.XmlAttribute.ChildTypeHash != 0);
                }
            }
        }

        public static XmlDefinition GetXmlDefinition(uint rootHash)
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

                XmlElement xmlElement = new XmlElement(xmlAttribute, fieldDelegate);

                // is it a Vector3 type?
                if (xmlAttribute.CustomType == ElementType.Vector3 && xmlAttributes.Length != 1)
                {
                    if (xmlAttribute.ElementType != ElementType.Float) throw new Exceptions.InvalidXmlElement(xmlAttribute.Name, "The XML Element Parent does not have a valid ElementType defined in definition = " + type.FullName);
                    if (xmlAttributes.Length != 4) throw new Exceptions.InvalidXmlElement(xmlAttribute.Name, "The XML Element series does not have the correct number of Attributes defined in definition = " + type.FullName);

                    XmlCookedAttribute xAttribute = null;
                    XmlCookedAttribute yAttribute = null;
                    XmlCookedAttribute zAttribute = null;
                    foreach (XmlCookedAttribute floatAttrib in xmlAttributes)
                    {
                        if (floatAttrib.Name.EndsWith("fX") || floatAttrib.Name.Contains("pfRed")) xAttribute = floatAttrib;
                        else if (floatAttrib.Name.EndsWith("fY") || floatAttrib.Name.Contains("pfGreen")) yAttribute = floatAttrib;
                        else if (floatAttrib.Name.EndsWith("fZ") || floatAttrib.Name.Contains("pfBlue")) zAttribute = floatAttrib;
                        else continue;

                        floatAttrib.NameHash = _GetNameHash(floatAttrib);
                        floatAttrib.CustomType = ElementType.Vector3;

                        if (floatAttrib.ParentNames == null) floatAttrib.ParentNames = new List<String>(); // this is kind of gross - but meh for now
                        floatAttrib.ParentNames.Insert(0, xmlAttribute.Name);
                    }

                    if (xAttribute == null || yAttribute == null || zAttribute == null) throw new Exceptions.InvalidXmlElement(xmlAttribute.Name, "The XML Element does not have all 3 vector names defined in definition = " + type.FullName);

                    xmlElement.IsCustomOrigin = true;
                    xmlElements.Add(xAttribute.NameHash, new XmlElement(xAttribute, _xVectorDelegate, xmlElement));
                    xmlElements.Add(yAttribute.NameHash, new XmlElement(yAttribute, _yVectorDelegate, xmlElement));
                    xmlElements.Add(zAttribute.NameHash, new XmlElement(zAttribute, _zVectorDelegate, xmlElement));
                }

                // is it a Flags Enum?
                if (xmlAttribute.ElementType == ElementType.Flag || xmlAttribute.ElementType == ElementType.BitFlag)
                {
                    FieldInfo[] flagFields = fieldDelegate.FieldType.GetFields(BindingFlags.Static | BindingFlags.Public); // need order as defined order - Enum.GetValues provides by value order
                    flagFields = flagFields.OrderBy(f => f.MetadataToken).ToArray(); // order by defined order - GetFields does not guarantee ordering

                    foreach (FieldInfo flagInfo in flagFields)
                    {
                        Object value = flagInfo.GetValue(null);
                        String valueStr = value.ToString();
                        MemberInfo memberInfo = fieldDelegate.FieldType.GetMember(valueStr).FirstOrDefault();
                        Debug.Assert(memberInfo != null);
                        XmlCookedAttribute xmlFlagAttributes = (XmlCookedAttribute)memberInfo.GetCustomAttributes(typeof(XmlCookedAttribute), false).FirstOrDefault();

                        XmlCookedAttribute flagsAttribute;
                        if (xmlFlagAttributes == null)
                        {
                            flagsAttribute = new XmlCookedAttribute
                            {
                                Name = valueStr,
                                ElementType = xmlAttribute.ElementType,
                                DefaultValue = false
                            };
                        }
                        else // if a flag has attributes, then we want to *extend* upon them, not replace them (e.g. has IsTestCentre etc)
                        {
                            flagsAttribute = xmlFlagAttributes;
                            flagsAttribute.Name = valueStr;
                            flagsAttribute.ElementType = xmlAttribute.ElementType;
                            flagsAttribute.DefaultValue = false;
                        }

                        flagsAttribute.NameHash = _GetNameHash(flagsAttribute);

                        if (xmlAttribute.ElementType == ElementType.Flag)
                        {
                            flagsAttribute.FlagId = xmlAttribute.FlagId;
                            flagsAttribute.FlagMask = (uint)value;
                        }
                        else
                        {
                            int index = 0;
                            uint valueInt = (uint)value;
                            for (; valueInt > 1; valueInt >>= 1, index++) { }

                            flagsAttribute.BitFlagIndex = index;
                            flagsAttribute.BitFlagCount = flagFields.Length;
                        }
                        XmlElement flagElement = new XmlElement(flagsAttribute, fieldDelegate);

                        xmlElements.Add(flagsAttribute.NameHash, flagElement);
                    }

                    continue;
                }

                xmlAttribute.NameHash = _GetNameHash(xmlAttribute);

                if (xmlAttribute.CustomType == ElementType.Object)
                {
                    Dictionary<uint, XmlElement> objectFields = _GetDefinitionElements(xmlAttribute.ChildType);
                    foreach (XmlElement element in objectFields.Values)
                    {
                        if (element.XmlAttribute.ParentNames == null) element.XmlAttribute.ParentNames = new List<String>(); // this is kind of gross - but meh for now
                        element.XmlAttribute.ParentNames.Insert(0, xmlAttribute.Name);

                        xmlElements.Add(element.NameHash, element);
                    }

                    xmlElement.IsCustomOrigin = true;
                }

                xmlAttribute.FieldDelegate = fieldDelegate;
                xmlElements.Add(xmlAttribute.NameHash, xmlElement);
            }

            return xmlElements;
        }

        private static uint _GetNameHash(XmlCookedAttribute xmlAttribute)
        {
            if (!String.IsNullOrEmpty(xmlAttribute.TrueName))
            {
                return Crypt.GetStringHash(xmlAttribute.TrueName);
            }

            return Crypt.GetStringHash(xmlAttribute.Name);
        }

        private static bool _TestBit(IList<byte> bitField, int byteOffset, int bitOffset)
        {
            byteOffset += bitOffset >> 3;
            bitOffset &= 0x07;

            return (bitField[byteOffset] & (1 << bitOffset)) >= 1;
        }
    }
}