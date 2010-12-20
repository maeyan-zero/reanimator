using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hellgate
{
    public abstract class HellgateFile
    {
        public const String Extension = "";
        public const String ExtensionDeserialised = "";

        public abstract void ParseFileBytes(byte[] fileBytes);
        public abstract byte[] ToByteArray();
        public abstract byte[] ExportAsDocument();
    }
}
