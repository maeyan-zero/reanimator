using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Hellgate
{
    public static class StreamTools
    {
        public static void WriteByte(byte[] buffer, ref int offset, byte val)
        {
            buffer[offset++] = val;
        }

        public static void WriteByte(byte[] buffer, ref int offset, int val)
        {
            buffer[offset++] = (byte)val;
        }

        public static void WriteShort(byte[] buffer, ref int offset, short val)
        {
            buffer[offset++] = (byte)(val);
            buffer[offset++] = (byte)(val >> 8);
        }

        public static void WriteShort(byte[] buffer, ref int offset, int val)
        {
            buffer[offset++] = (byte)(val);
            buffer[offset++] = (byte)(val >> 8);
        }

        public static void WriteInt32(byte[] buffer, ref int offset, Int32 val)
        {
            buffer[offset++] = (byte)(val);
            buffer[offset++] = (byte)(val >> 8);
            buffer[offset++] = (byte)(val >> 16);
            buffer[offset++] = (byte)(val >> 24);
        }

        public static void WriteUInt32(byte[] buffer, ref int offset, UInt32 val)
        {
            buffer[offset++] = (byte)(val);
            buffer[offset++] = (byte)(val >> 8);
            buffer[offset++] = (byte)(val >> 16);
            buffer[offset++] = (byte)(val >> 24);
        }

        public static unsafe void WriteFloat(byte[] buffer, ref int offset, float val)
        {
            int floatInt = *(int*)&val;
            buffer[offset++] = (byte)floatInt;
            buffer[offset++] = (byte)(floatInt >> 8);
            buffer[offset++] = (byte)(floatInt >> 16);
            buffer[offset++] = (byte)(floatInt >> 24);
        }

        public static void WriteInt64(byte[] buffer, ref int offset, Int64 val)
        {
            buffer[offset++] = (byte)(val);
            buffer[offset++] = (byte)(val >> 8);
            buffer[offset++] = (byte)(val >> 16);
            buffer[offset++] = (byte)(val >> 24);
            buffer[offset++] = (byte)(val >> 32);
            buffer[offset++] = (byte)(val >> 40);
            buffer[offset++] = (byte)(val >> 48);
            buffer[offset++] = (byte)(val >> 56);
        }

        public static void WriteUInt64(byte[] buffer, ref int offset, UInt64 val)
        {
            buffer[offset++] = (byte)(val);
            buffer[offset++] = (byte)(val >> 8);
            buffer[offset++] = (byte)(val >> 16);
            buffer[offset++] = (byte)(val >> 24);
            buffer[offset++] = (byte)(val >> 32);
            buffer[offset++] = (byte)(val >> 40);
            buffer[offset++] = (byte)(val >> 48);
            buffer[offset++] = (byte)(val >> 56);
        }

        public static void WriteByteArray(byte[] buffer, ref int offset, byte[] vals)
        {
            Buffer.BlockCopy(vals, 0, buffer, offset, vals.Length);
            offset += vals.Length;
        }

        public static void WriteStringAscii(byte[] buffer, ref int offset, String str, bool includeZero)
        {
            byte[] strBytes = Encoding.ASCII.GetBytes(str);
            Buffer.BlockCopy(strBytes, 0, buffer, offset, strBytes.Length);
            offset += strBytes.Length;
            if (includeZero) offset++;
        }

        public static void WriteStringUnicode(byte[] buffer, ref int offset, String str, bool includeZero)
        {
            byte[] strBytes = Encoding.Unicode.GetBytes(str);
            Buffer.BlockCopy(strBytes, 0, buffer, offset, strBytes.Length);
            offset += strBytes.Length;
            if (includeZero) offset += 2;
        }

        /// <summary>
        /// Reads a byte from a bytea array incrementing the offset byte the size of a byte.
        /// </summary>
        /// <param name="buffer">The byte array containing the byte.</param>
        /// <param name="offset">The initial offset within the byte array.</param>
        /// <returns>The byte value.</returns>
        public static byte ReadByte(byte[] buffer, ref int offset)
        {
            return buffer[offset++];
        }

        /// <summary>
        /// Converts an array of bytes to an Int16 from a given offset.<br />
        /// <i>offset</i> is incremented by the size of an Int16.
        /// </summary>
        /// <param name="buffer">The byte array containing the Int16.</param>
        /// <param name="offset">The initial offset within the byte array.</param>
        /// <returns>The Int16 value.</returns>
        public static Int16 ReadInt16(byte[] buffer, ref int offset)
        {
            Int16 value = BitConverter.ToInt16(buffer, offset);
            offset += 2;
            return value;
        }

        /// <summary>
        /// Converts an array of bytes to an UInt16 from a given offset.<br />
        /// <i>offset</i> is incremented by the size of an UInt16.
        /// </summary>
        /// <param name="buffer">The byte array containing the UInt16.</param>
        /// <param name="offset">The initial offset within the byte array.</param>
        /// <returns>The Int16 value.</returns>
        public static UInt16 ReadUInt16(byte[] buffer, ref int offset)
        {
            UInt16 value = BitConverter.ToUInt16(buffer, offset);
            offset += 2;
            return value;
        }

        /// <summary>
        /// Converts an array of bytes to an Int32 from a given offset.<br />
        /// <i>offset</i> is incremented by the size of an Int32.
        /// </summary>
        /// <param name="buffer">The byte array containing the Int32.</param>
        /// <param name="offset">The initial offset within the byte array.</param>
        /// <returns>The Int32 value.</returns>
        public static Int32 ReadInt32(byte[] buffer, ref int offset)
        {
            Int32 value = BitConverter.ToInt32(buffer, offset);
            offset += 4;
            return value;
        }

        /// <summary>
        /// Converts an array of bytes to a UInt32 from a given offset.<br />
        /// <i>offset</i> is incremented by the size of a UInt32.
        /// </summary>
        /// <param name="buffer">The byte array containing the UInt32.</param>
        /// <param name="offset">The initial offset within the byte array.</param>
        /// <returns>The UInt32 value.</returns>
        public static UInt32 ReadUInt32(byte[] buffer, ref int offset)
        {
            UInt32 value = BitConverter.ToUInt32(buffer, offset);
            offset += 4;
            return value;
        }

        /// <summary>
        /// Converts an array of bytes to a UInt32 from a given offset.<br />
        /// </summary>
        /// <param name="buffer">The byte array containing the UInt32.</param>
        /// <param name="offset">The initial offset within the byte array.</param>
        /// <returns>The UInt32 value.</returns>
        public static UInt32 ReadUInt32(byte[] buffer, int offset)
        {
            return BitConverter.ToUInt32(buffer, offset);
        }

        /// <summary>
        /// Converts an array of bytes to a Float from a given offset.<br />
        /// <i>offset</i> is incremented by the size of a Float.
        /// </summary>
        /// <param name="byteArray">The byte array containing the Float.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted Float value.</returns>
        public static unsafe float ReadFloat(byte[] byteArray, ref int offset)
        {
            float fValue;
            fixed (byte* b = &byteArray[offset])
            {
                fValue = *(((float*)b));
            }
            offset += 4;
            return fValue;
        }

        /// <summary>
        /// Converts an array of bytes to a Float from a given offset.<br />
        /// </summary>
        /// <param name="byteArray">The byte array containing the Float.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted Float value.</returns>
        public static unsafe float ReadFloat(byte[] byteArray, int offset)
        {
            float fValue;
            fixed (byte* b = &byteArray[offset])
            {
                fValue = *(((float*) b));
            }
            return fValue;
        }

        /// <summary>
        /// Converts an array of bytes to an Int64 from a given offset.<br />
        /// <i>offset</i> is incremented by the size of an Int64.
        /// </summary>
        /// <param name="buffer">The byte array containing the Int64.</param>
        /// <param name="offset">The initial offset within the byte array.</param>
        /// <returns>The Int64 value.</returns>
        public static Int64 ReadInt64(byte[] buffer, ref int offset)
        {
            Int64 value = BitConverter.ToInt64(buffer, offset);
            offset += 8;
            return value;
        }

        public static String ReadStringAscii(byte[] buffer, int offset, int charCount)
        {
            String str = Encoding.ASCII.GetString(buffer, offset, charCount);
            return str;
        }

        public static String ReadStringAscii(byte[] buffer, ref int offset, int charCount, bool includeZero)
        {
            String str = Encoding.ASCII.GetString(buffer, offset, charCount);
            offset += str.Length;
            if (includeZero) offset++;
            return str;
        }

        /// <summary>
        /// Reads a Unicode string from a byte array.
        /// </summary>
        /// <param name="buffer">The byte array containing the Unicode String.</param>
        /// <param name="offset">The initial offset within the byte array.</param>
        /// <param name="charCount">The number of <b>UNICODE</b> characters (NOT bytes), NOT including zero character.</param>
        /// <param name="includeZero">Set to true to increment the offset to include the zero.</param>
        /// <returns>The Unicode bytes as a String</returns>
        public static String ReadStringUnicode(byte[] buffer, ref int offset, int charCount, bool includeZero)
        {
            int byteCount = charCount * 2;
            String str = Encoding.Unicode.GetString(buffer, offset, byteCount);
            offset += byteCount;
            if (includeZero) offset += 2;
            return str;
        }
    }

    public static class ExtensionMethods
    {
        public static T DeepClone<T>(this T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
