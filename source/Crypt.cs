using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reanimator
{
    class Crypt
    {
        struct Encryption
        {
            public const int key1 = 0x10DCD;
            public const int key2 = 0xF4559D5;
            public const int key3 = 666;
        }

        public static byte Routine(int offset)
        {
            int blockSize = 32;
            int blockIndex = -1;
            int value = 0;
            byte[] bytes = new byte[sizeof(int)];
            byte[] table = new byte[blockSize * sizeof(int)];

            blockIndex = offset / (blockSize * sizeof(int)) * (blockSize * sizeof(int));
            value = blockIndex + Encryption.key3;

            for (int i = 0; i < blockSize; i++)
            {
                value = (value * Encryption.key1) + Encryption.key2;
                bytes = BitConverter.GetBytes(value);
                Array.Copy(bytes, 0, table, i * 4, 4);
            }

            return table[offset - blockIndex];
        }

        public static void Decrypt(byte[] indexBuffer)
        {
            for (int i = 0; i < indexBuffer.Length; i++)
            {
                indexBuffer[i] -= Routine(i);
            }
            Console.WriteLine("Finished Decrypting Index.");
        }

        public static void Encrypt(byte[] indexBuffer)
        {
            for (int i = 0; i < indexBuffer.Length; i++)
            {
                indexBuffer[i] += Routine(i);
            }
            Console.WriteLine("Finished Encrypting Index.");
        }
    }
}
