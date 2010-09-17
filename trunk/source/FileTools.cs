﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Reanimator
{
    public static class FileTools
    {
        /// <summary>
        /// Reads a Stream and converts it to a byte array.
        /// </summary>
        /// <param name="stream">The Stream the read from.</param>
        /// <returns>The read byte array.</returns>
        public static byte[] StreamToByteArray(Stream stream)
        {
            if (stream == null) return null;

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[1024];
                int bytes;
                while ((bytes = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, bytes);
                }
                byte[] output = ms.ToArray();
                return output;
            }
        }

        /// <summary>
        /// Converts an array of bytes to an Int32 from a given offset.<br />
        /// <i>offset</i> is incremented by the size of an Int32.
        /// </summary>
        /// <param name="byteArray">The byte array containing the Int32.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted Int32 value.</returns>
        public static Int32 ByteArrayToInt32(byte[] byteArray, ref int offset)
        {
            Int32 value = BitConverter.ToInt32(byteArray, offset);
            offset += 4;
            return value;
        }

        /// <summary>
        /// Converts an array of bytes to an Int32 from a given offset.
        /// </summary>
        /// <param name="byteArray">The byte array containing the Int32.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted Int32 value.</returns>
        public static Int32 ByteArrayToInt32(byte[] byteArray, int offset)
        {
            return BitConverter.ToInt32(byteArray, offset);
        }

        /// <summary>
        /// Converts an array of bytes to a UInt32 from a given offset.<br />
        /// <i>offset</i> is incremented by the size of a UInt32.
        /// </summary>
        /// <param name="byteArray">The byte array containing the UInt32.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted UInt32 value.</returns>
        public static UInt32 ByteArrayToUInt32(byte[] byteArray, ref int offset)
        {
            UInt32 value = BitConverter.ToUInt32(byteArray, offset);
            offset += 4;
            return value;
        }

        /// <summary>
        /// Converts an array of bytes to a Float from a given offset.<br />
        /// <i>offset</i> is incremented by the size of a Float.
        /// </summary>
        /// <param name="byteArray">The byte array containing the Float.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted Float value.</returns>
        public static float ByteArrayToFloat(byte[] byteArray, ref int offset)
        {
            float value = BitConverter.ToSingle(byteArray, offset);
            offset += 4;
            return value;
        }

        /// <summary>
        /// Converts an array of bytes to a UInt16 from a given offset.<br />
        /// <i>offset</i> is incremented by the size of a UInt16.
        /// </summary>
        /// <param name="byteArray">The byte array containing the UInt16.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted UInt16 value.</returns>
        public static UInt16 ByteArrayToUShort(byte[] byteArray, ref int offset)
        {
            UInt16 value = BitConverter.ToUInt16(byteArray, offset);
            offset += 2;
            return value;
        }

        /// <summary>
        /// Converts an array of bytes to an array of Int32 values.<br />
        /// <i>offset</i> is incremented by the size of the Int32 array.
        /// </summary>
        /// <param name="byteArray">The byte array containing the Int32 array.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <param name="count">The number of Int32 array elements.</param>
        /// <returns>The converted Int32 array.</returns>
        public static Int32[] ByteArrayToInt32Array(byte[] byteArray, ref int offset, int count)
        {
            Int32[] int32Array = new Int32[count];

            GCHandle pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            IntPtr bytePtr = new IntPtr((int)pinnedArray.AddrOfPinnedObject() + offset);
            Marshal.Copy(bytePtr, int32Array, 0, count);
            pinnedArray.Free();
            offset += count*4;

            return int32Array;
        }

        /// <summary>
        /// Converts an array of bytes to a structure.<br />
        /// <i>offset</i> is incremented by the size of the structure.
        /// </summary>
        /// <param name="byteArray">The byte array containing the structure.</param>
        /// <param name="type">The type of structure.</param>
        /// <param name="offset">The offset within the byte array to the structure.</param>
        /// <returns>The converted object.</returns>
        public static object ByteArrayToStructure(byte[] byteArray, Type type, ref int offset)
        {
            int structSize = Marshal.SizeOf(type);

            GCHandle hcHandle = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            IntPtr ptrStruct = Marshal.AllocHGlobal(structSize);
            Marshal.Copy(byteArray, offset, ptrStruct, structSize);
            object structure = Marshal.PtrToStructure(ptrStruct, type);
            Marshal.FreeHGlobal(ptrStruct);
            hcHandle.Free();

            offset += structSize;
            return structure;
        }

        /// <summary>
        /// Converts an array of bytes to a structure.<br />
        /// <i>offset</i> is incremented by the size of the structure.
        /// </summary>
        /// <typeparam name="T">The type of structure.</typeparam>
        /// <param name="byteArray">The byte array containing the structure.</param>
        /// <param name="offset">The offset within the byte array to the structure.</param>
        /// <returns>The converted object.</returns>
        public static T ByteArrayToStructure<T>(byte[] byteArray, ref int offset)
        {
            Type structType = typeof(T);
            int structSize = Marshal.SizeOf(structType);

            GCHandle hcHandle = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            IntPtr ptrStruct = new IntPtr((int)hcHandle.AddrOfPinnedObject() + offset);
            T structure = (T)Marshal.PtrToStructure(ptrStruct, structType);
            hcHandle.Free();

            offset += structSize;
            return structure;
        }

        /// <summary>
        /// Converts an array of bytes to a structure.
        /// </summary>
        /// <typeparam name="T">The type of structure.</typeparam>
        /// <param name="byteArray">The byte array containing the structure.</param>
        /// <param name="offset">The offset within the byte array to the structure.</param>
        /// <returns>The converted object.</returns>
        public static T ByteArrayToStructure<T>(byte[] byteArray, int offset)
        {
            return ByteArrayToStructure<T>(byteArray, ref offset);
        }

        /// <summary>
        /// Converts an array of bytes to an array of T values.<br />
        /// <i>offset</i> is incremented by the size of the T array.<br />
        /// <b>This function should not be used for standard types (e.g. Int32).<br />
        /// (The Marshal.Copy() function can do standard types)</b>
        /// </summary>
        /// <typeparam name="T">The type of array elements.</typeparam>
        /// <param name="byteArray">The byte array containing the T array.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <param name="count">The number of T array elements.</param>
        /// <returns>The converted T array.</returns>
        public static unsafe T[] ByteArrayToArray<T>(byte[] byteArray, ref int offset, int count)
        {
            Debug.Assert(offset <= byteArray.Length, "Error: offset > byteArray.Length");

            int sizeOfT = Marshal.SizeOf(typeof(T));
            int sizeOfBuffer = sizeOfT * count;
            Debug.Assert(offset + sizeOfBuffer <= byteArray.Length, "Error: offset + sizeOfBuffer > byteArray.Length");

            T[] obj = new T[count];
            fixed (byte* pData = byteArray)
            {
                for (int i = 0; i < count; i++)
                {
                    IntPtr addr = new IntPtr(pData + offset);
                    obj[i] = (T)Marshal.PtrToStructure(addr, typeof(T));
                    offset += sizeOfT;
                }
            }

            return obj;
        }

        /// <summary>
        /// Converts an array of bytes to an ASCII String from offset up to the
        /// first null character from offset or remaining bytes if null can't be found.
        /// </summary>
        /// <param name="byteArray">The byte array containing the ASCII String.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted ASCII String.</returns>
        public static String ByteArrayToStringASCII(byte[] byteArray, int offset)
        {
            // may not look as pretty, but much faster/safter than using Marshal string crap
            // get first null location etc
            int arrayLenth = byteArray.Length;
            int strLength = 0;
            for (int i = offset; i < arrayLenth; i++)
            {
                if (byteArray[i] != 0x00) continue;

                strLength = i - offset;
                break;
            }

            if (strLength == 0)
            {
                strLength = byteArray.Length - offset;
            }

            return Encoding.ASCII.GetString(byteArray, offset, strLength);
        }

        /// <summary>
        /// Converts an array of bytes to an ASCII String from offset up to the first null character.<br />
        /// <i>offset</i> is incremented by the <i>len</i> argument value.
        /// </summary>
        /// <param name="byteArray">The byte array containing the ANSI String.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <param name="len">The length of the string to convert up to.</param>
        /// <returns>The converted ASCII String.</returns>
        public static String ByteArrayToStringASCII(byte[] byteArray, ref int offset, int len)
        {
            String str = Encoding.ASCII.GetString(byteArray, offset, len);
            offset += len;
            return str;
        }

        /// <summary>
        /// Converts an array of bytes to a Unicode String from offset up to the first null character.<br />
        /// </summary>
        /// <param name="byteArray">The byte array containing the Unicode String.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted Unicode String.</returns>
        public static String ByteArrayToStringUnicode(byte[] byteArray, int offset)
        {
            // may not look as pretty, but much faster/safter than using Marshal string crap
            // get first null location etc
            int arrayLenth = byteArray.Length;
            int strLength = 0;
            for (int i = offset; i < arrayLenth; i++, i++)
            {
                if (byteArray[i] != 0x00) continue;

                strLength = i - offset;
                break;
            }

            if (strLength == 0)
            {
                strLength = byteArray.Length - offset;
            }

            String str = Encoding.Unicode.GetString(byteArray, offset, strLength);
            return str;
        }

        /// <summary>
        /// Converts a String into its Unicode byte array.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The converted byte array.</returns>
        public static byte[] StringToUnicodeByteArray(String str)
        {
            return Encoding.Unicode.GetBytes(str);
        }

        /// <summary>
        /// Converts a String into its ASCII byte array.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The converted byte array.</returns>
        public static byte[] StringToASCIIByteArray(String str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        /// Searches a byte array for a sequence of bytes.<br />
        /// <i>Uses 0x90 as a wild card.</i>
        /// </summary>
        /// <param name="byteArray">The byte array to search.</param>
        /// <param name="searchFor">The byte sequence to search for.</param>
        /// <returns>The index found or -1.</returns>
        public static int ByteArrayContains(byte[] byteArray, byte[] searchFor)
        {
            for (int i = 0; i < byteArray.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < searchFor.Length; j++)
                {
                    if (searchFor[j] == 0x90)
                    {
                        continue;
                    }

                    if (byteArray[i + j] == searchFor[j]) continue;

                    found = false;
                    break;
                }

                if (found)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Converts an Object into a byte array.
        /// </summary>
        /// <param name="obj">The Object to convert.</param>
        /// <returns>The converted byte array.</returns>
        public static byte[] StructureToByteArray(Object obj)
        {
            int length = Marshal.SizeOf(obj);
            byte[] byteArray = new byte[length];

            IntPtr intPtr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(obj, intPtr, false);
            Marshal.Copy(intPtr, byteArray, 0, length);
            Marshal.FreeHGlobal(intPtr);

            return byteArray;
        }




        public static byte[] IntArrayToByteArray(int[] source)
        {
            byte[] result = new byte[source.Length * sizeof(int)];

            for (int i = 0; i < source.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(source[i]), 0, result, i * sizeof(int), sizeof(int));
            }

            return result;
        }

        public static void WriteToBuffer(ref byte[] buffer, int offset, Object toWrite)
        {
            WriteToBuffer(ref buffer, ref offset, toWrite);
        }





        /// <summary>
        /// Serializes an object and appends it to the supplied buffer, increasing offset by object size.<br />
        /// If the buffer is too small the bufer size is increaed by the object size + 1024 bytes.
        /// </summary>
        /// <param name="buffer">A reference to a byte array (not null).</param>
        /// <param name="offset">A reference to the write offset (offset is increased by the size of object).</param>
        /// <param name="toWrite">A sersializable object to write.</param>
        public static void WriteToBuffer(ref byte[] buffer, ref int offset, Object toWrite)
        {
            byte[] toWriteBytes = toWrite as byte[] ?? StructureToByteArray(toWrite);

            WriteToBuffer(ref buffer, ref offset, toWriteBytes, toWriteBytes.Length, false);
        }

        public static void WriteToBuffer(ref byte[] buffer, int offset, byte[] toWriteBytes, int lengthToWrite, bool insert)
        {
            WriteToBuffer(ref buffer, ref offset, toWriteBytes, lengthToWrite, insert);
        }

        public static void WriteToBuffer(ref byte[] buffer, ref int offset, byte[] toWriteBytes, int lengthToWrite, bool insert)
        {
            byte[] insertBuffer = null;
            if (insert)
            {
                insertBuffer = new byte[buffer.Length - offset];
                Buffer.BlockCopy(buffer, offset, insertBuffer, 0, insertBuffer.Length);
            }

            if (offset + lengthToWrite > buffer.Length || insert)
            {
                byte[] newBuffer = new byte[buffer.Length + lengthToWrite + 1024];
                Buffer.BlockCopy(buffer, 0, newBuffer, 0, buffer.Length);
                buffer = newBuffer;
            }

            Buffer.BlockCopy(toWriteBytes, 0, buffer, offset, lengthToWrite);

            if (insert)
            {
                Buffer.BlockCopy(insertBuffer, 0, buffer, offset + lengthToWrite, insertBuffer.Length);
            }

            offset += lengthToWrite;
        }

        public static void BinaryToArray<T>(BinaryReader binReader, T[] destination)
        {
            for (int i = 0; i < destination.Length; i++)
            {
                byte[] byteArray = binReader.ReadBytes(Marshal.SizeOf(typeof (T)));
                int offset = 0;
                destination[i] = ByteArrayToStructure<T>(byteArray, ref offset);
            }
        }

        public static string ArrayToStringGeneric<T>(IList<T> array, string delimeter)
        {
            string outputString = "";

            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] is IList<T>)
                {
                    //Recursively convert nested arrays to string
                    outputString += ArrayToStringGeneric((IList<T>)array[i], delimeter);
                }
                else
                {
                    outputString += array[i];
                }

                if (i != array.Count - 1)
                    outputString += delimeter;
            }

            return outputString;
        }

        public static String SaveFileDiag(String fileExtension, String typeName, String defaultFileName, String initialDirectory)
        {
            // This little function is here because for some reason AddExtension = false doesn't seem to do shit.
            // So basically I just check it manually.

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                AddExtension = false,
                DefaultExt = fileExtension,
                FileName = defaultFileName,
                Filter = String.Format("{1} File(s) (*.{0})|*.{0}", fileExtension, typeName),
                InitialDirectory = initialDirectory
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                saveFileDialog.Dispose();
                return null;
            }
            String filePath = saveFileDialog.FileName;
            saveFileDialog.Dispose();

            // since AddExtension = false doesn't seem to do shit
            string replaceExtension = "." + fileExtension;
            while (filePath.Contains(replaceExtension))
            {
                filePath = filePath.Replace(replaceExtension, "");
            }
            filePath += replaceExtension;

            if (!filePath.Contains(fileExtension))
            {
                filePath += fileExtension;
            }

            return filePath;
        }

        public static string OpenFileDiag(String fileExtension, String typeName, String initialDirectory)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = fileExtension,
                Filter = String.Format("{1} File(s) (*.{0})|*.{0}", fileExtension, typeName),
                InitialDirectory = initialDirectory
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                openFileDialog.Dispose();
                return null;
            }
            String filePath = openFileDialog.FileName;
            openFileDialog.Dispose();

            return filePath;
        }

        public static bool WriteFile(String filePath, byte[] byteData)
        {
            DialogResult dr = DialogResult.Yes;
            while (dr == DialogResult.Yes)
            {
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        fs.Write(byteData, 0, byteData.Length);
                    }

                    return true;
                }
                catch (Exception e)
                {
                    dr = MessageBox.Show("Failed to write to file!\nTry Again?\n\n" + e, "Error",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Error);
                }
            }

            return false;
        }
    }
}