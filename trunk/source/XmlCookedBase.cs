using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Reanimator
{
    abstract class XmlCookedBase
    {
        private class XmlUnknownElement
        {
            public UInt32 Value { get; private set; }
            public ushort Token { get; private set; }
            public Object[] Objects { get; private set; }

            public XmlUnknownElement(UInt32 value, ushort token, params object[] objects)
            {
                Value = value;
                Token = token;
                Objects = objects;
            }
        }

        public XmlDocument XmlDoc { get; private set; }

        private readonly List<XmlUnknownElement> _unknownArray;

        protected byte[] Data;
        protected int ReadOffset;
        protected int WriteOffset;

        protected XmlCookedBase()
        {
            XmlDoc = new XmlDocument();
            _unknownArray = new List<XmlUnknownElement>();
        }

        protected abstract bool CookDataSegment(byte[] buffer);
        protected abstract bool ParseDataSegment(XmlElement dataElement);

        public byte[] Cook()
        {
            byte[] buffer = new byte[1024];
            WriteOffset = 0;

            FileTools.WriteToBuffer(ref buffer, ref WriteOffset, XmlCookedFile.FileHeadToken);     // 'CO0k'
            FileTools.WriteToBuffer(ref buffer, ref WriteOffset, XmlCookedFile.RequiredVersion);   // 8

            foreach (XmlUnknownElement xmlUnknownElement in _unknownArray)
            {
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlUnknownElement.Value);
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlUnknownElement.Token);

                foreach (Object obj in xmlUnknownElement.Objects)
                {
                    if (obj == null) // string from 0x0200
                    {
                        WriteOffset++;
                        continue;
                    }
                    FileTools.WriteToBuffer(ref buffer, ref WriteOffset, obj);
                }
            }

            FileTools.WriteToBuffer(ref buffer, ref WriteOffset, XmlCookedFile.DataSegmentToken);   // 'DATA'

            if (!CookDataSegment(buffer)) return null;

            byte[] data = new byte[WriteOffset];
            Buffer.BlockCopy(buffer, 0, data, 0, WriteOffset);

            return data;
        }

        public String Blah;

        public bool ParseData(byte[] data)
        {
            if (data == null) return false;
            Data = data;
            ReadOffset = 0;

            int fileHeadToken = FileTools.ByteArrayTo<Int32>(Data, ref ReadOffset);
            if (fileHeadToken != XmlCookedFile.FileHeadToken) return false;  // 'CO0k'

            int version = FileTools.ByteArrayTo<Int32>(Data, ref ReadOffset);
            if (version != XmlCookedFile.RequiredVersion) return false;

            XmlElement mainElement = XmlDoc.CreateElement("CO0k");
            XmlDoc.AppendChild(mainElement);

            XmlElement versionElement = XmlDoc.CreateElement("version");
            versionElement.InnerText = "8";
            mainElement.AppendChild(versionElement);


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

            XmlElement unknownArray = XmlDoc.CreateElement("unknownArray");
            mainElement.AppendChild(unknownArray);

            while (ReadOffset < Data.Length)
            {
                uint unknown = FileTools.ByteArrayTo<UInt32>(Data, ref ReadOffset);

                if (unknown == XmlCookedFile.DataSegmentToken) break;     // 'DATA'

                ushort token = FileTools.ByteArrayTo<ushort>(Data, ref ReadOffset);

                XmlElement arrayElement = XmlDoc.CreateElement("0x" + unknown.ToString("X8"));
                arrayElement.SetAttribute("token", "0x" + token.ToString("X4"));
                unknownArray.AppendChild(arrayElement);

                switch (token)
                {
                    case 0x0200:    // skills
                        String str = ReadByteString(arrayElement, "str", false);
                        if (str != null)
                        {
                            ReadOffset++; // need to include \0 as it isn't included in the strLen byte for some reason
                        }
                        _AddUnknown(unknown, token, str);

                        break;

                    case 0x0003:    // skills
                    //case 0x0005:
                    //case 0x00C0:
                    //case 0x008D: // found in particle effects
                        short srt = ReadShort(arrayElement, "short");
                        _AddUnknown(unknown, token, srt);
                        break;

                    case 0x0000:    // skills
                    case 0x0309:    // skills
                    case 0x0700:    // skills
                    case 0x0903:    // skills
                    //case 0x0A00:   // particle effects
                        int int0 = ReadInt32(arrayElement, "int");
                        _AddUnknown(unknown, token, int0);
                        break;

                    case 0x0100:    // skills
                    //case 0x0600:
                    ////case 0x0500: // found in particle effects
                        float f = ReadFloat(arrayElement, "float");
                        _AddUnknown(unknown, token, f);
                        break;

                    case 0x0B01:    // skills
                        int int1 = ReadInt32(arrayElement, "int");
                        UInt32 bitfield = ReadBitField(arrayElement, "bitField");
                        _AddUnknown(unknown, token, int1, bitfield);
                        break;

                    case 0x0308:    // skills
                    case 0x030A:    // skills
                    ////case 0x0106: // found in particle effects
                        int int2 = ReadInt32(arrayElement, "int1");
                        int int3 = ReadInt32(arrayElement, "int2");
                        _AddUnknown(unknown, token, int2, int3);
                        break;

                    case 0x0C02:    // skills
                        int int4 = ReadInt32(arrayElement, "index");
                        int int5 = ReadInt32(arrayElement, "int");
                        _AddUnknown(unknown, token, int4, int5);
                        break;

                    default:
                        MessageBox.Show("Unexpected unknownArray token!\n\ntoken = 0x" + token.ToString("X4"), "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Debug.Assert(false);
                        return false;
                }
            }

            XmlElement dataElement = XmlDoc.CreateElement("DATA");
            mainElement.AppendChild(dataElement);

            if (!ParseDataSegment(dataElement)) return false;

            if (ReadOffset != Data.Length)
            {
                MessageBox.Show("Entire file not parsed!\noffset != data.Length\n\noffset = " + ReadOffset +
                                "\ndata.Length = " + Data.Length, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private void _AddUnknown(UInt32 unknown, ushort token, params Object[] objects)
        {
            _unknownArray.Add(new XmlUnknownElement(unknown, token, objects));
        }

        protected Int32 ReadInt32(XmlNode parentNode, String elementName)
        {
            Int32 value = FileTools.ByteArrayTo<Int32>(Data, ref ReadOffset);

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = value.ToString();
            element.SetAttribute("asHex", "0x" + value.ToString("X8"));
            parentNode.AppendChild(element);

            return value;
        }

        protected float ReadFloat(XmlNode parentNode, String elementName)
        {
            float value = FileTools.ByteArrayTo<float>(Data, ref ReadOffset);

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = value.ToString("F5");
            parentNode.AppendChild(element);

            return value;
        }

        private short ReadShort(XmlNode parentNode, String elementName)
        {
            short value = FileTools.ByteArrayTo<short>(Data, ref ReadOffset);

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = value.ToString();
            element.SetAttribute("asHex", "0x" + value.ToString("X4"));
            parentNode.AppendChild(element);

            return value;
        }

        protected UInt32 ReadBitField(XmlNode parentNode, String elementName)
        {
            UInt32 value = FileTools.ByteArrayTo<UInt32>(Data, ref ReadOffset);

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = Convert.ToString(value, 2).PadLeft(32, '0');
            element.SetAttribute("asHex", "0x" + value.ToString("X8"));
            parentNode.AppendChild(element);

            return value;
        }

        protected String ReadByteString(XmlNode parentNode, String elementName, bool mustExist)
        {
            String value = null;
            byte strLen = FileTools.ByteArrayTo<byte>(Data, ref ReadOffset);
            if (strLen == 0xFF || strLen == 0x00)
            {
                if (mustExist)
                {
                    value = String.Empty;
                }
            }
            else
            {
                value = FileTools.ByteArrayToStringAnsi(Data, ref ReadOffset, strLen);
            }

            if (value == null) return null;

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = value;
            parentNode.AppendChild(element);

            return value;
        }

        protected String ReadZeroString(XmlNode parentNode, String elementName)
        {
            Int32 strLen = FileTools.ByteArrayTo<Int32>(Data, ref ReadOffset);
            Debug.Assert(strLen != 0);

            String value = FileTools.ByteArrayToStringAnsi(Data, ReadOffset);
            ReadOffset += strLen;

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = value;
            parentNode.AppendChild(element);

            return value;
        }

        protected void WriteByteString(ref byte[] buffer, String str, bool mustExist)
        {
            WriteByteString(ref buffer, str, mustExist, false);
        }

        protected void WriteByteString(ref byte[] buffer, String str, bool mustExist, bool use255)
        {
            Byte strLen = 0;
            if (str != null)
            {
                strLen = (byte)str.Length;
            }

            if (strLen != 0)
            {
                byte[] strBytes = FileTools.StringToASCIIByteArray(str);
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, strLen);
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, strBytes);
            }
            else
            {
                if (mustExist)
                {
                    if (use255)
                    {
                        strLen = 0xFF;
                    }

                    FileTools.WriteToBuffer(ref buffer, ref WriteOffset, strLen);
                }
            }
        }

        protected void WriteZeroString(ref byte[] buffer, String str)
        {
            Int32 strLen = str.Length;
            FileTools.WriteToBuffer(ref buffer, ref WriteOffset, strLen+1); // +1 for \0
            byte[] strBytes = FileTools.StringToASCIIByteArray(str);
            FileTools.WriteToBuffer(ref buffer, ref WriteOffset, strBytes);
            WriteOffset++; // \0
        }

        protected static bool HasChildNode(XmlNode xmlNode, String nodeName)
        {
            return xmlNode != null && xmlNode.ChildNodes.Cast<XmlNode>().Any(node => node.Name == nodeName);
        }
    }
}
