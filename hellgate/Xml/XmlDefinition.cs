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
        public Int32[] Flags;
        public UInt32 FlagsBaseMask;          // mostly a hack to ensure 100% cooking fidelity; some bit fields are 0xFDFDFDFD with 0 at flags not high, 1 at flags high
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
            if (Flags != null)
            {
                for (int i = 0; i < Flags.Length; i++)
                {
                    Flags[i] = -1;
                }
            }
            if (BitFlags != null)
            {
                BitFlags = new UInt32[BitFlags.Length];
            }
        }
    }
}