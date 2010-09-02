using System;
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

        public static void ByteArrayToStructure(byte[] byteArray, ref object obj, int offset, int length)
        {
            if (length == 0)
            {
                length = Marshal.SizeOf(obj);
            }

            IntPtr i = Marshal.AllocHGlobal(length);
            Marshal.Copy(byteArray, offset, i, length);

            obj = Marshal.PtrToStructure(i, obj.GetType());

            Marshal.FreeHGlobal(i);
        }

        public static object ByteArrayToStructure(byte[] byteArray, Type type, int offset)
        {
            object obj = Activator.CreateInstance(type);
            ByteArrayToStructure(byteArray, ref obj, offset, 0);
            return obj;
        }

        public static object ByteArrayToStructure(byte[] byteArray, Type type, int offset, int length)
        {
            object obj = Activator.CreateInstance(type);
            ByteArrayToStructure(byteArray, ref obj, offset, length);
            return obj;
        }

        public static Int32[] ByteArrayToInt32Array(byte[] byteArray, int offset, int count)
        {
            Int32[] int32Array = new Int32[count];

            IntPtr bytePtr = Marshal.UnsafeAddrOfPinnedArrayElement(byteArray, offset);
            Marshal.Copy(bytePtr, int32Array, 0, count);

            return int32Array;
        }

        public static Int32 ByteArrayToInt32(byte[] byteArray, int offset)
        {
            IntPtr bytePtr = Marshal.UnsafeAddrOfPinnedArrayElement(byteArray, offset);
            return Marshal.ReadInt32(bytePtr);
        }

        public static T ByteArrayTo<T>(byte[] byteArray, int offset)
        {
            IntPtr bytePtr = Marshal.UnsafeAddrOfPinnedArrayElement(byteArray, offset);
            return (T)Marshal.PtrToStructure(bytePtr, typeof(T));
        }

        public static T ByteArrayTo<T>(byte[] byteArray, ref int offset)
        {
            Debug.Assert(offset < byteArray.Length, "Error: offset < byteArray.Length");
            IntPtr bytePtr = Marshal.UnsafeAddrOfPinnedArrayElement(byteArray, offset);
            offset += Marshal.SizeOf(typeof(T));
            return (T)Marshal.PtrToStructure(bytePtr, typeof(T));
        }

        public static T[] ByteArrayToArray<T>(byte[] byteArray, ref int offset, int count)
        {
            Debug.Assert(offset <= byteArray.Length, "Error: offset > byteArray.Length");

            int sizeOfT = Marshal.SizeOf(typeof(T));
            int sizeOfBuffer = sizeOfT * count;

            Debug.Assert(offset + sizeOfBuffer <= byteArray.Length, "Error: offset + sizeOfBuffer > byteArray.Length");

            IntPtr buffer = Marshal.AllocCoTaskMem(sizeOfBuffer);
            Marshal.Copy(byteArray, offset, buffer, sizeOfBuffer);
            offset += sizeOfBuffer;

            T[] obj = new T[count];
            for (int i = 0, bufferOffset = 0; i < count; i++, bufferOffset += sizeOfT)
            {
                obj[i] = (T)Marshal.PtrToStructure(new IntPtr(buffer.ToInt32() + bufferOffset), typeof(T));
            }

            Marshal.FreeCoTaskMem(buffer);
            return obj;
        }

        public static String ByteArrayToStringAnsi(byte[] byteArray, int offset)
        {
            IntPtr bytePtr = Marshal.UnsafeAddrOfPinnedArrayElement(byteArray, offset);
            return Marshal.PtrToStringAnsi(bytePtr);
        }

        public static String ByteArrayToStringAnsi(byte[] byteArray, ref int offset, int len)
        {
            IntPtr bytePtr = Marshal.UnsafeAddrOfPinnedArrayElement(byteArray, offset);
            offset += len;
            return Marshal.PtrToStringAnsi(bytePtr, len);
        }

        public static String ByteArrayToStringUnicode(byte[] byteArray, int offset)
        {
            IntPtr bytePtr = Marshal.UnsafeAddrOfPinnedArrayElement(byteArray, offset);
            return Marshal.PtrToStringUni(bytePtr);
        }

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

        public static byte[] StructureToByteArray(Object obj)
        {
            int length = Marshal.SizeOf(obj);
            byte[] byteArray = new byte[length];
            IntPtr i = Marshal.AllocHGlobal(length);

            Marshal.StructureToPtr(obj, i, true);
            Marshal.Copy(i, byteArray, 0, length);
            Marshal.FreeHGlobal(i);

            return byteArray;
        }

        public static byte[] StringToUnicodeByteArray(String str)
        {
            return Encoding.Unicode.GetBytes(str);
        }

        public static byte[] StringToASCIIByteArray(String str)
        {
            return Encoding.ASCII.GetBytes(str);
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
                destination[i] = (T)ByteArrayToStructure(binReader.ReadBytes(Marshal.SizeOf(typeof(T))), typeof(T), 0);
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