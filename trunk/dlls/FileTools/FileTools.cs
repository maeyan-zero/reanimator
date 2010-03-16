using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace Reanimator
{
    public class FileTools
    {
        public static byte[] StreamToByteArray(Stream stream)
        {
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
            IntPtr bytePtr = Marshal.UnsafeAddrOfPinnedArrayElement(byteArray, offset);
            offset += Marshal.SizeOf(typeof(T));
            return (T)Marshal.PtrToStructure(bytePtr, typeof(T));
        }

        public static String ByteArrayToStringAnsi(byte[] byteArray, int offset)
        {
            IntPtr bytePtr = Marshal.UnsafeAddrOfPinnedArrayElement(byteArray, offset);
            return Marshal.PtrToStringAnsi(bytePtr);
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

                    if (byteArray[i + j] != searchFor[j])
                    {
                        found = false;
                        break;
                    }
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
            return UnicodeEncoding.Unicode.GetBytes(str);
        }

        public static byte[] StringToASCIIByteArray(String str)
        {
            return ASCIIEncoding.ASCII.GetBytes(str);
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

        public static void WriteToBuffer(ref byte[] buffer, ref int offset, Object toWrite)
        {
            byte[] toWriteBytes = toWrite as byte[];
            if (toWriteBytes == null)
            {
                toWriteBytes = FileTools.StructureToByteArray(toWrite);
            }

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

            if (insert && insertBuffer != null)
            {
                Buffer.BlockCopy(insertBuffer, 0, buffer, offset + lengthToWrite, insertBuffer.Length);
            }

            offset += lengthToWrite;
        }

        public static void BinaryToArray<T>(BinaryReader binReader, T[] destination)
        {
            for (int i = 0; i < destination.Length; i++)
            {
                destination[i] = (T)FileTools.ByteArrayToStructure(binReader.ReadBytes(Marshal.SizeOf(typeof(T))), typeof(T), 0);
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
                    outputString += ArrayToStringGeneric<T>((IList<T>)array[i], delimeter);
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
    }
}