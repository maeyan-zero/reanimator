using System;
using System.Collections.Generic;
using System.Linq;

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

        private int _countExcludeTCv4;
        public int CountExcludeTCv4
        {
            get
            {
                if (_countExcludeTCv4 == 0 && Count != 0)
                {
                    foreach (XmlCookElement xmlCookElement in Elements.Where(xmlCookElement => !xmlCookElement.IsTCv4))
                    {
                        _countExcludeTCv4++;
                    }
                }

                return _countExcludeTCv4;
            }
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