using System;

namespace Hellgate
{
    public abstract class HellgateFile
    {
        public const String Extension = "";                 // the native extension of the file. e.g rule_pmt01.drl -> ".drl"
        public const String ExtensionDeserialised = "";     // the human-readable extension of the file. e.g. rule_pmt01.drl.xml -> ".drl.xml"

        /// <summary>
        /// Parses a files byte array deserialising the object.
        /// </summary>
        /// <param name="fileBytes">The bytes to parse.</param>
        public abstract void ParseFileBytes(byte[] fileBytes);

        /// <summary>
        /// Generates a native file byte array of the object.
        /// </summary>
        /// <returns>The byte array to save.</returns>
        public abstract byte[] ToByteArray();

        /// <summary>
        /// Exports the object as a human-readable document. e.g. As .xml or .txt, etc.
        /// </summary>
        /// <returns>The byte array to save.</returns>
        public abstract byte[] ExportAsDocument();
    }
}