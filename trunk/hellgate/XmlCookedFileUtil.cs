using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using Revival.Common;
using FieldDelegate = Revival.Common.ObjectDelegator.FieldDelegate;

namespace Hellgate
{
    public partial class XmlCookedFile
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class FileHeader
        {
            public UInt32 MagicWord;
            public Int32 Version;
            public UInt32 XmlRootDefinition;
            public Int32 XmlRootElementCount;
        }

        #region Init
        private static void _GenerateXmlDefinitions()
        {
            // generate definitions
            foreach (Type xmlType in XmlCookedDefinition.DefinitionTypes)
            {
                Dictionary<uint, XmlCookedElement> xmlElements = _GetDefinitionElements(xmlType);
                if (xmlElements.Count == 0) throw new Exceptions.InvalidXmlElement(xmlType.Name, "The XML class doesn't have an valid XML Elements defined.");

                XmlCookedAttribute[] query = (XmlCookedAttribute[])xmlType.GetCustomAttributes(typeof(XmlCookedAttribute), true);
                if (query.Length != 1 || String.IsNullOrEmpty(query[0].Name)) throw new Exceptions.InvalidXmlElement(xmlType.FullName, "The XML class doesn't have a valid XmlCookedAttribute associated with it.");

                XmlCookedAttribute xmlAttribute = query[0];
                xmlAttribute.NameHash = Crypt.GetStringHash(xmlAttribute.Name);

                XmlDefinitions.Add(xmlAttribute.NameHash, new XmlCookedDefinition(xmlAttribute, xmlElements, xmlType));
            }

            // find/assign child name hashes
            foreach (XmlCookedDefinition xmlDefinition in XmlDefinitions.Values)
            {
                foreach (XmlCookedElement xmlElement in xmlDefinition.Elements.Values)
                {
                    if (xmlElement.XmlAttribute.ChildType == null) continue;
                    if (xmlElement.XmlAttribute.CustomType == ElementType.Object) continue;

                    Type childType = xmlElement.XmlAttribute.ChildType;
                    foreach (XmlCookedDefinition xmlDef in XmlDefinitions.Values.Where(xmlDef => xmlDef.XmlObjectType == childType))
                    {
                        xmlElement.XmlAttribute.ChildTypeHash = xmlDef.Hash;
                        break;
                    }

                    Debug.Assert(xmlElement.XmlAttribute.ChildTypeHash != 0);
                }
            }
        }

        private static XmlCookedDefinition _GetXmlDefinition(uint rootHash)
        {
            XmlCookedDefinition xmlDefinition;
            return !XmlDefinitions.TryGetValue(rootHash, out xmlDefinition) ? null : xmlDefinition;
        }

        private static Dictionary<uint, XmlCookedElement> _GetDefinitionElements(Type type)
        {
            FieldInfo[] xmlFields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            ObjectDelegator objectDelegator = new ObjectDelegator(xmlFields);

            Dictionary<uint, XmlCookedElement> xmlElements = new Dictionary<uint, XmlCookedElement>();
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

                XmlCookedElement xmlElement = new XmlCookedElement(xmlAttribute, fieldDelegate);

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
                    xmlElements.Add(xAttribute.NameHash, new XmlCookedElement(xAttribute, _xVectorDelegate));
                    xmlElements.Add(yAttribute.NameHash, new XmlCookedElement(yAttribute, _yVectorDelegate));
                    xmlElements.Add(zAttribute.NameHash, new XmlCookedElement(zAttribute, _zVectorDelegate));
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
                        XmlCookedElement flagElement = new XmlCookedElement(flagsAttribute, fieldDelegate);

                        xmlElements.Add(flagsAttribute.NameHash, flagElement);
                    }

                    continue;
                }

                xmlAttribute.NameHash = _GetNameHash(xmlAttribute);

                if (xmlAttribute.CustomType == ElementType.Object)
                {
                    Dictionary<uint, XmlCookedElement> objectFields = _GetDefinitionElements(xmlAttribute.ChildType);
                    foreach (XmlCookedElement element in objectFields.Values)
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
        #endregion

        #region Shared Functions
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
        #endregion

        #region ParseCookedData helper functions
        private static void _SetFieldDefault(XmlCookedElement xmlElement, Object obj, Object defaultValue)
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

        private void _ParseCookedInt32(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            Object innerTextObj;
            switch (xmlCookedAttribute.CustomType)
            {
                case ElementType.Bool:
                    {
                        int int32Val = StreamTools.ReadInt32(_buffer, ref _offset);
                        bool boolValue = (int32Val == 0) ? false : true;
                        fieldDelegate.SetValue(currObj, boolValue);
                        innerTextObj = boolValue;
                    }
                    break;

                case ElementType.Unsigned:
                    {
                        UInt32 uint32Val = StreamTools.ReadUInt32(_buffer, ref _offset);
                        fieldDelegate.SetValue(currObj, uint32Val);
                        innerTextObj = uint32Val;
                    }
                    break;

                default:
                    {
                        Int32 int32Val = StreamTools.ReadInt32(_buffer, ref _offset);
                        fieldDelegate.SetValue(currObj, int32Val);
                        innerTextObj = int32Val;
                    }
                    break;
            }

            if (_generateXml && xmlRoot != null)
            {
                XmlNode int32Element = XmlDoc.CreateElement(xmlCookedAttribute.Name);
                xmlRoot.AppendChild(int32Element);
                int32Element.InnerText = innerTextObj.ToString();
            }

            _offset += xmlCookedAttribute.FlagOffsetChange;
        }

        private void _ParseCookedInt32ArrayFixed(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            Object arrayObj = null;
            if (xmlCookedAttribute.CustomType == ElementType.Unsigned)
            {
                UInt32[] uint32Array = new UInt32[xmlCookedAttribute.Count];
                for (int intIndex = 0; intIndex < xmlCookedAttribute.Count; intIndex++)
                {
                    uint32Array[intIndex] = StreamTools.ReadUInt32(_buffer, ref _offset);

                    if (!_generateXml || xmlRoot == null) continue;
                    XmlNode arrElement = XmlDoc.CreateElement(xmlCookedAttribute.Name);
                    xmlRoot.AppendChild(arrElement);
                    arrElement.InnerText = uint32Array[intIndex].ToString();
                }
            }
            else
            {
                Int32[] int32Array = new Int32[xmlCookedAttribute.Count];
                for (int intIndex = 0; intIndex < xmlCookedAttribute.Count; intIndex++)
                {
                    int32Array[intIndex] = StreamTools.ReadInt32(_buffer, ref _offset);

                    if (!_generateXml || xmlRoot == null) continue;
                    XmlNode arrElement = XmlDoc.CreateElement(xmlCookedAttribute.Name);
                    xmlRoot.AppendChild(arrElement);
                    arrElement.InnerText = int32Array[intIndex].ToString();
                }
            }

            fieldDelegate.SetValue(currObj, arrayObj);
        }

        private void _ParseCookedInt32ArrayVariable(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            int count = StreamTools.ReadInt32(_buffer, ref _offset);
            Debug.Assert(count >= 0);
            Int32[] int32Arr = new Int32[count];

            if (_generateXml && xmlRoot != null)
            {
                XmlNode int32ArrCountEle = XmlDoc.CreateElement(xmlCookedAttribute.Name + "Count");
                xmlRoot.AppendChild(int32ArrCountEle);
                int32ArrCountEle.InnerText = count.ToString();
            }

            for (int int32Index = 0; int32Index < count; int32Index++)
            {
                int32Arr[int32Index] = StreamTools.ReadInt32(_buffer, ref _offset);

                if (!_generateXml || xmlRoot == null) continue;
                XmlNode arrElement = XmlDoc.CreateElement(xmlCookedAttribute.Name);
                xmlRoot.AppendChild(arrElement);
                arrElement.InnerText = int32Arr[int32Index].ToString();
            }

            fieldDelegate.SetValue(currObj, int32Arr);
        }

        private void _ParseCookedFloat(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
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

            XmlNode floatElement = XmlDoc.CreateElement(xmlCookedAttribute.Name);
            xmlRoot.AppendChild(floatElement);
            floatElement.InnerText = (isNegativeZero ? "-" : "") + floatValue.ToString("R");
        }

        private void _ParseCookedFloatArrayFixed(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            float[] floatArray = new float[xmlCookedAttribute.Count];

            for (int fIndex = 0; fIndex < xmlCookedAttribute.Count; fIndex++)
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

                XmlNode floatElement = XmlDoc.CreateElement(xmlCookedAttribute.Name);
                xmlRoot.AppendChild(floatElement);
                floatElement.InnerText = (isNegativeZero ? "-" : "") + floatArray[fIndex].ToString("R");
            }

            if (xmlCookedAttribute.CustomType == ElementType.Vector3)
            {
                if (floatArray.Length != 3) throw new Exceptions.InvalidXmlElement(xmlCookedAttribute.Name, "The Vector3 Object does not have 3 elements within the FloatArrayFixed.");

                Vector3 vector3Obj = new Vector3(floatArray[0], floatArray[1], floatArray[2]);
                fieldDelegate.SetValue(currObj, vector3Obj);
            }
            else
            {
                fieldDelegate.SetValue(currObj, floatArray);
            }
        }

        private void _ParseCookedFloatArrayVariable(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            int floatArrCount = StreamTools.ReadInt32(_buffer, ref _offset);

            if (_generateXml && xmlRoot != null)
            {
                XmlNode countElement = XmlDoc.CreateElement(xmlCookedAttribute.Name + "Count");
                countElement.InnerText = floatArrCount.ToString();
                xmlRoot.AppendChild(countElement);
            }

            for (int floatArrIndex = 0; floatArrIndex < floatArrCount; floatArrIndex++)
            {
                _ParseCookedFloat(xmlCookedAttribute, fieldDelegate, currObj, xmlRoot);
            }
        }

        private void _ParseCookedFloatTripletArrayVariable(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            int fTripCount = StreamTools.ReadInt32(_buffer, ref _offset);
            Debug.Assert(fTripCount != 0);

            if (xmlCookedAttribute.CustomType != ElementType.Vector3) // I guess we could also have it as a float[][3] array, but fuck that
            {
                throw new NotImplementedException("case ElementType.FloatTripletArrayVariable && if (xmlAttribute.CustomType != ElementType.Vector3)");
            }

            if (_generateXml && xmlRoot != null)
            {
                XmlNode countElement = XmlDoc.CreateElement(xmlCookedAttribute.Name + "Count");
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
                    XmlNode fTripElement = XmlDoc.CreateElement(xmlCookedAttribute.Name);
                    fTripElement.InnerText = String.Format("{0}{1}, {2}{3}, {4}{5}", // todo: for memory there is a flag to enable showing the sign - check me
                                                (isNegativeZeroX ? "-" : ""), fTripArray[fTripIndex].X,
                                                (isNegativeZeroY ? "-" : ""), fTripArray[fTripIndex].Y,
                                                (isNegativeZeroZ ? "-" : ""), fTripArray[fTripIndex].Z);
                    xmlRoot.AppendChild(fTripElement);
                }
                else
                {
                    XmlNode fTripElement = XmlDoc.CreateElement(xmlCookedAttribute.Name);
                    fTripElement.InnerText = fTripArray[fTripIndex].ToString();
                    xmlRoot.AppendChild(fTripElement);
                }
            }

            fieldDelegate.SetValue(currObj, fTripArray);
        }

        private void _ParseCookedFloatQuadArrayVariable(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            int fQuadCount = StreamTools.ReadInt32(_buffer, ref _offset);
            Debug.Assert(fQuadCount != 0);

            if (xmlCookedAttribute.CustomType != ElementType.Vector4) // I guess we could also have it as a float[][4] array, but fuck that
            {
                throw new NotImplementedException("case ElementType.FloatQuadArrayVariable && if (xmlAttribute.CustomType != ElementType.Vector4)");
            }

            if (_generateXml && xmlRoot != null)
            {
                XmlNode countElement = XmlDoc.CreateElement(xmlCookedAttribute.Name + "Count");
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
                XmlNode fQuadElement = XmlDoc.CreateElement(xmlCookedAttribute.Name);
                fQuadElement.InnerText = fQuadArray[fQuadIndex].ToString();
                xmlRoot.AppendChild(fQuadElement);
            }

            fieldDelegate.SetValue(currObj, fQuadArray);
        }

        private void _ParseCookedString(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            int charCount = StreamTools.ReadInt32(_buffer, ref _offset);

            byte b = _buffer[_offset];
            bool overrideTypeAsByteArray = ((b < 0x20 || b > 0x7F) && b != 0x00); // if not a valid string, then treat as data
            for (int i = 4; i < charCount && !overrideTypeAsByteArray; i += 4) // check every 4th byte (don't want to waste too much time checking every byte)
            {
                b = _buffer[_offset + i];
                overrideTypeAsByteArray = ((b < 0x20 || b > 0x7F) && b != 0x00);
            }
            bool isByteArray = (xmlCookedAttribute.IsByteArray && charCount > 0) || overrideTypeAsByteArray;

            String str = String.Empty;
            if (xmlCookedAttribute.IsByteArray)
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
            XmlElement stringElement = XmlDoc.CreateElement(xmlCookedAttribute.Name);
            xmlRoot.AppendChild(stringElement);
            stringElement.InnerText = str;
            if (xmlCookedAttribute.IsByteArray)
            {
                stringElement.SetAttribute("isByteArray", "true");
            }
            else if (overrideTypeAsByteArray)
            {
                stringElement.SetAttribute("overrideTypeAsByteArray", "true");
            }
        }

        private void _ParseCookedStringArrayFixed(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            for (int strElementIndex = 0; strElementIndex < xmlCookedAttribute.Count; strElementIndex++)
            {
                _ParseCookedString(xmlCookedAttribute, fieldDelegate, currObj, xmlRoot);
            }
        }

        private void _ParseCookedStringArrayVariable(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            int stringArrCount = StreamTools.ReadInt32(_buffer, ref _offset);

            if (_generateXml && xmlRoot != null)
            {
                XmlNode countElement = XmlDoc.CreateElement(xmlCookedAttribute.Name + "Count");
                countElement.InnerText = stringArrCount.ToString();
                xmlRoot.AppendChild(countElement);
            }

            for (int strElementIndex = 0; strElementIndex < stringArrCount; strElementIndex++)
            {
                _ParseCookedString(xmlCookedAttribute, fieldDelegate, currObj, xmlRoot);
            }
        }

        // Table
        // TableArrayFixed
        // TableArrayVariable
        private void _ParseCookedTables(XmlCookedAttribute xmlAttribute, XmlCookedTree xmlTree, Object currObject, XmlNode xmlRoot)
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

                        root = XmlDoc.CreateElement(xmlTree.TwinRoot.Definition.Attributes.Name);
                        xmlRoot.AppendChild(root);
                    }

                    Object table = _ParseCookedData(xmlTree.TwinRoot, xmlAttribute.ChildType, root, desc);
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

                    root = XmlDoc.CreateElement(xmlTree.TwinRoot.Definition.Attributes.Name);
                    xmlRoot.AppendChild(root);
                }

                objs[i] = _ParseCookedData(xmlTree.TwinRoot, xmlAttribute.ChildType, root, desc);
            }
            xmlAttribute.FieldDelegate.SetValue(currObject, objs);
        }

        // ExcelIndex
        // ExcelIndexArrayFixed
        private Object _ParseCookedExcelIndex(XmlCookedAttribute xmlAttribute, XmlNode xmlRoot, XmlNode xmlDesc)
        {
            String excelString;
            byte strLen = _buffer[_offset++];
            if (strLen == 0xFF) excelString = null;
            else if (strLen == 0x00) excelString = String.Empty;
            else excelString = FileTools.ByteArrayToStringASCII(_buffer, ref _offset, strLen);

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

            return row;
        }

        private void _ParseCookedFlag(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            UInt32 currFlags = (uint)fieldDelegate.GetValue(currObj);
            if (currFlags == 0)
            {
                currFlags = StreamTools.ReadUInt32(_buffer, ref _offset);
                fieldDelegate.SetValue(currObj, currFlags);
            }

            if (!_generateXml || xmlRoot == null) return;
            bool flagged = false;
            switch (xmlCookedAttribute.ElementType)
            {
                case ElementType.BitFlag:
                    flagged = (currFlags & (1 << xmlCookedAttribute.BitFlagIndex)) > 0;
                    break;

                case ElementType.Flag:
                    flagged = (currFlags & xmlCookedAttribute.FlagMask) > 0;
                    break;
            }

            String innerText = flagged ? "1" : "0";

            XmlNode flagElement = XmlDoc.CreateElement(xmlCookedAttribute.Name);
            xmlRoot.AppendChild(flagElement);
            flagElement.InnerText = innerText;
        }

        private void _ParseCookedBitFlag(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            UInt32 currBitFlags = (UInt32)fieldDelegate.GetValue(currObj);
            if (currBitFlags == 0)
            {
                currBitFlags = StreamTools.ReadUInt32(_buffer, ref _offset);
                fieldDelegate.SetValue(currObj, currBitFlags);
            }

            if (!_generateXml || xmlRoot == null) return;
            bool flagged = false;
            switch (xmlCookedAttribute.ElementType)
            {
                case ElementType.BitFlag:
                    flagged = (currBitFlags & (1 << xmlCookedAttribute.BitFlagIndex)) > 0;
                    break;

                case ElementType.Flag:
                    flagged = (currBitFlags & xmlCookedAttribute.FlagMask) > 0;
                    break;
            }

            String innerText = flagged ? "1" : "0";

            XmlNode flagElement = XmlDoc.CreateElement(xmlCookedAttribute.Name);
            xmlRoot.AppendChild(flagElement);
            flagElement.InnerText = innerText;
        }

        private void _ParseCookedByteArrayVariable(XmlCookedAttribute xmlCookedAttribute, FieldDelegate fieldDelegate, Object currObj, XmlNode xmlRoot)
        {
            Int32 byteCount = FileTools.ByteArrayToInt32(_buffer, ref _offset);
            byte[] byteArray = new byte[byteCount];
            Buffer.BlockCopy(_buffer, _offset, byteArray, 0, byteCount);
            fieldDelegate.SetValue(currObj, byteArray);

            _offset += byteCount;

            if (!_generateXml || xmlRoot == null || byteCount <= 0) return;
            String value = BitConverter.ToString(byteArray);

            XmlNode bytesElement = XmlDoc.CreateElement(xmlCookedAttribute.Name);
            xmlRoot.AppendChild(bytesElement);
            bytesElement.InnerText = value;
        }
        #endregion

        #region ParseXml_ helper functions
        /// <summary>
        /// An element can be TestCentre AND Resurrection, so we must be careful and not double-count them.
        /// This will determine based on this.XmlCookedFile cooking options as to whether or not to include the element in the cooking process.
        /// </summary>
        /// <param name="xmlCookedElement">The XmlCookedElement to test.</param>
        /// <returns>True if the element should be included, false otherwise.</returns>
        private bool _IncludeElement(XmlCookedElement xmlCookedElement)
        {
            /* this looks like excess "ifs", but is done to make it easier (much easier than before) to read */

            // if only want resurrection (and SP) elements
            if (CookExcludeTestCentre && !CookExcludeResurrection)
            {
                if (xmlCookedElement.IsTestCentre && !xmlCookedElement.IsResurrection) return false;
            }

            // if only want test centre (and SP) elements
            if (!CookExcludeTestCentre && CookExcludeResurrection)
            {
                if (!xmlCookedElement.IsTestCentre && xmlCookedElement.IsResurrection) return false;
            }

            // if only want SP elements (excluding test centre and resurrection elements)
            if (CookExcludeTestCentre && CookExcludeResurrection)
            {
                if (xmlCookedElement.IsTestCentre && xmlCookedElement.IsResurrection) return false;
            }

            // else we must want all elments
            return true;
        }

        /// <summary>
        /// An element can be TestCentre AND Resurrection, so we must be careful and not double-count them.
        /// Thus we must check each combination type and base the count on indirect accumulation.
        /// </summary>
        /// <param name="xmlDefinition">The XML Definition that needs a count.</param>
        /// <returns>The number of applicable elements to be written based on XmlCookedFile cooking options.</returns>
        private int _GetElementCountToWrite(XmlCookedDefinition xmlDefinition)
        {
            // only resurrection elements
            if (CookExcludeTestCentre && !CookExcludeResurrection)
            {
                return xmlDefinition.SinglePlayerElementCount + xmlDefinition.ResurrectionElementCount;
            }

            // only test centre elements
            if (!CookExcludeTestCentre && CookExcludeResurrection)
            {
                return xmlDefinition.SinglePlayerElementCount + xmlDefinition.TestCentreElementCount;
            }

            // test centre and resurrection elements (i.e. all elements)
            if (!CookExcludeTestCentre && !CookExcludeResurrection)
            {
                return xmlDefinition.Count;
            }

            // only single player elements
            return xmlDefinition.SinglePlayerElementCount;
        }
        #endregion

        #region ParseXmlData helper functions
        private void _ParseXmlInt32(String elementText, XmlCookedElement xmlCookElement)
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

        private void _ParseXmlInt32ArrayFixed<T>(XmlCookedElement xmlCookElement, XmlNode xmlNode)
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

        private void _ParseXmlInt32ArrayVariable(XmlCookedElement xmlCookElement, XmlNode xmlNode, String elementText)
        {
            int arrayCount = int.Parse(elementText);
            bool allDefault = true;
            List<Int32> elements = _ParseXmlGetArrayElementValues<Int32>(xmlCookElement, xmlNode, ref allDefault, arrayCount);
            //if (allDefault) return;       // written even if all default

            Int32 count = elements.Count;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, count);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, elements.ToArray().ToByteArray());
        }

        private void _ParseXmlFloat(String elementText, XmlCookedElement xmlCookElement)
        {
            float floatValue = Convert.ToSingle(elementText);
            if (floatValue == 0 && elementText.Contains("-0")) floatValue = -1.0f * 0.0f;
            if ((float)xmlCookElement.Default == floatValue) return;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, floatValue);
        }

        private void _ParseXmlFloatArrayFixed(XmlCookedElement xmlCookElement, XmlNode xmlNode)
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

        private void _ParseXmlFloatArrayVariable(XmlCookedElement xmlCookElement, XmlNode xmlNode, String elementText)
        {
            int arrayCount = int.Parse(elementText);
            bool allDefault = true;
            List<float> elements = _ParseXmlGetArrayElementValues<float>(xmlCookElement, xmlNode, ref allDefault, arrayCount);
            //if (allDefault) return;       // written even if all default

            Int32 count = elements.Count;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, count);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, elements.ToArray().ToByteArray());
        }

        private void _ParseXmlFloatTripletArrayVariable(String elementText, XmlCookedElement xmlCookElement, XmlNode xmlNode)
        {
            // todo: add default check (particle has defaults)
            _ParseXmlFloatMultipleArrayVariable(elementText, xmlCookElement, xmlNode, 3);
        }

        private void _ParseXmlFloatQuadArrayVariable(String elementText, XmlCookedElement xmlCookElement, XmlNode xmlNode)
        {
            // todo: add default check (particle has defaults)
            _ParseXmlFloatMultipleArrayVariable(elementText, xmlCookElement, xmlNode, 4);
        }

        private void _ParseXmlFloatMultipleArrayVariable(String elementText, XmlCookedElement xmlCookElement, XmlNode xmlNode, int floatCount)
        {
            int arrayCount = int.Parse(elementText);
            bool allDefault = true;
            List<String> elements = _ParseXmlGetArrayElementValues<String>(xmlCookElement, xmlNode, ref allDefault, arrayCount, false);

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

        private void _ParseXmlString(String elementText, XmlCookedElement xmlCookElement, XmlElement xmlElement)
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

        private void _ParseXmlStringArrayFixed(XmlCookedElement xmlCookElement, XmlNode xmlNode)
        {
            int arrayCount = xmlCookElement.XmlAttribute.Count;
            bool allDefault = true;
            List<String> strings = _ParseXmlGetArrayElementValues<String>(xmlCookElement, xmlNode, ref allDefault, arrayCount);
            if (allDefault) return;

            for (int i = 0; i < arrayCount && i < strings.Count; i++)
            {
                Int32 strLen = strings[i].Length;
                FileTools.WriteToBuffer(ref _buffer, ref _offset, strLen + 1); // \0
                FileTools.WriteToBuffer(ref _buffer, ref _offset, FileTools.StringToASCIIByteArray(strings[i]));
                _offset++; // \0
            }
        }

        private void _ParseXmlStringArrayVariable(XmlCookedElement xmlCookElement, XmlNode xmlNode, String elementText)
        {
            int arrayCount = int.Parse(elementText);
            List<XmlElement> elements = _ParseXmlGetArrayElements(xmlCookElement, xmlNode);

            Int32 elementCount = elements.Count;
            FileTools.WriteToBuffer(ref _buffer, ref _offset, elementCount);

            for (int i = 0; i < arrayCount && i < elementCount; i++)
            {
                _ParseXmlString(elements[i].InnerText, xmlCookElement, elements[i]);
            }
        }

        private int _ParseXmlTable(XmlCookedElement xmlCookElement, XmlNode xmlElement)
        {
            XmlCookedDefinition xmlTableDefinition = _GetXmlDefinition(xmlCookElement.XmlAttribute.ChildTypeHash);

            if (xmlElement == null) // ElementType.Table must be cooked even if null
            {
                return _ParseXmlData(xmlTableDefinition, xmlElement);
            }

            XmlNode xmlTable = xmlElement.NextSibling;
            return _ParseXmlData(xmlTableDefinition, xmlTable);
        }

        private int _ParseXmlTableArrayFixed(XmlCookedElement xmlCookElement, XmlNode xmlNode)
        {
            return _ParseXmlTableArray(xmlCookElement, xmlNode, xmlCookElement.XmlAttribute.Count);
        }

        private int _ParseXmlTableArrayVariable(String elementText, XmlCookedElement xmlCookElement, XmlNode xmlNode)
        {
            int tableCount = String.IsNullOrEmpty(elementText) ? 0 : Convert.ToInt32(elementText);
            FileTools.WriteToBuffer(ref _buffer, ref _offset, tableCount); // table count is always written
            if (xmlNode == null) return 0;

            return _ParseXmlTableArray(xmlCookElement, xmlNode.NextSibling, tableCount);
        }

        private int _ParseXmlTableArray(XmlCookedElement xmlCookElement, XmlNode xmlNode, int tableCount)
        {
            XmlCookedDefinition xmlTableCountDefinition = _GetXmlDefinition(xmlCookElement.XmlAttribute.ChildTypeHash);
            int tablesAdded = 0;

            for (int i = 0; i < tableCount; i++)
            {
                XmlNode tableNode = xmlNode.NextSibling;
                if (tableNode == null || tableNode.Name != xmlTableCountDefinition.Attributes.Name) return -1;

                if (_ParseXmlData(xmlTableCountDefinition, tableNode) == -1) return -1;
                tablesAdded++;

                xmlNode = tableNode.NextSibling;
                if (xmlNode == null) break;
            }

            if (tablesAdded < tableCount) return -1;
            return tablesAdded;
        }

        private void _ParseXmlExcelIndex(String elementText, XmlCookedElement xmlCookElement)
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
                excelString = _fileManager.GetExcelStringFromRowIndex(xmlCookElement.XmlAttribute.TableCode, index);
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

        private void _ParseXmlExcelIndexArrayFixed(XmlCookedElement xmlCookElement, XmlNode xmlNode)
        {
            int arrayCount = xmlCookElement.XmlAttribute.Count;
            bool allDefault = true;
            List<String> elements = _ParseXmlGetArrayElementValues<String>(xmlCookElement, xmlNode, ref allDefault, arrayCount);

            for (int i = 0; i < arrayCount; i++)
            {
                String elementText = i < elements.Count ? elements[i] : String.Empty;
                _ParseXmlExcelIndex(elementText, xmlCookElement);
            }
        }

        private void _ParseXmlFlag(String elementText, XmlCookedAttribute xmlAttribute, IList<int> flagOffsets)
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

        private void _ParseXmlByteArrayVariable(String elementText, XmlCookedElement xmlCookElement)
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

        private static List<T> _ParseXmlGetArrayElementValues<T>(XmlCookedElement xmlCookElement, XmlNode xmlNode, ref bool allDefault, int arrayCount, bool checkDefault = true)
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

        private static List<XmlElement> _ParseXmlGetArrayElements(XmlCookedElement xmlCookElement, XmlNode xmlNode)
        {
            //List<XmlElement> elements = new List<XmlElement>();

            //foreach (XmlElement xmlChildNode in xmlNode.ChildNodes)
            //{
            //    if (xmlChildNode.Name != xmlCookElement.Name) continue;

            //    elements.Add(xmlChildNode);
            //}

            //return elements;

            return xmlNode.ChildNodes.Cast<XmlElement>().Where(xmlChildNode => xmlChildNode.Name == xmlCookElement.Name).ToList();
        }
        #endregion
    }
}