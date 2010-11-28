using System;
using System.Collections.Generic;

namespace Hellgate.Xml
{
    public class XmlDefinition
    {
        public String RootElement;
        public UInt32 RootHash;
        public String Value;
        public readonly List<XmlCookElement> Elements;
        public Int32[] BitFields;
        public UInt32[] BitFlags;
        public bool NeedToReadBitFlags;
        public int BitFlagsWriteOffset;

        public XmlDefinition()
        {
            Elements = new List<XmlCookElement>();
        }

        public XmlCookElement this[int index]
        {
            get { return Elements[index]; }
        }

        public int Count
        {
            get { return Elements.Count; }
        }

        public void ResetFields()
        {
            NeedToReadBitFlags = true;
            BitFlagsWriteOffset = -1;
            if (BitFields != null)
            {
                for (int i = 0; i < BitFields.Length; i++)
                {
                    BitFields[i] = -1;
                }
            }
            if (BitFlags != null)
            {
                BitFlags = new UInt32[BitFlags.Length];
            }
        }
    }
}