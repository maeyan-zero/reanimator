using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Reanimator
{
    class XmlCookedFile
    {
        private XmlCookedBase _xmlCooked;

        public XmlCookedFile()
        {
            _xmlCooked = null;
        }

        public bool ParseData(byte[] data)
        {
            if (data == null) return false;

            const int offset = 8;
            if (offset >= data.Length - 4) return false;

            // this isn't actually (at least, I don't think it is) a type specifier, but seeing
            // as (from what I've seen so far) the unknown array is constant per-type, might as well use it as such
            uint cookedType = FileTools.ByteArrayTo<uint>(data, offset);

            switch(cookedType)
            {
                case 0x400053E7: // skills
                    _xmlCooked = new XmlCookedSkill();
                    break;

                case 0x3A048D4A: // states
                    _xmlCooked = new XmlCookedState();
                    break;

                default:
                    MessageBox.Show("Not implemented type!\nOnly skills and states supported.\n\nunknown = 0x" + cookedType.ToString("X8"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Debug.Assert(false);
                    return false;
            }

            return _xmlCooked.ParseData(data);
        }

        public void SaveXml(String path)
        {
            if (_xmlCooked.XmlDoc == null || String.IsNullOrEmpty(path)) return;

            _xmlCooked.XmlDoc.Save(path);
        }
    }
}