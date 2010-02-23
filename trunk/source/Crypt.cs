using System;

namespace Reanimator
{
    class Crypt
    {
        class CryptState
        {
            public const int Key1 = 0x10DCD;
            public const int Key2 = 0xF4559D5;
            public const int Key3 = 666;
            public const int BlockSize = 32;

            public Byte[] Data { get; private set; }
            public Byte[] Table { get; private set; }

            public UInt32 Offset { get; set; }
            public UInt32 BlockIndex { get; set; }
            public Int32 Size { get { return Data.Length; } }

            public CryptState(byte[] indexBuffer)
            {
                Data = indexBuffer;
                BlockIndex = 0xFFFFFFFF;
                Table = new Byte[CryptState.BlockSize * sizeof(Int32)];
            }
        }

        static byte GetCryptByte(CryptState cryptState)
        {
            UInt32 value = cryptState.Offset / (CryptState.BlockSize * sizeof(Int32)) * (CryptState.BlockSize * sizeof(Int32));

            if (cryptState.BlockIndex != value)
            {
                cryptState.BlockIndex = value;
                value += CryptState.Key3;
                for (int i = 0; i < CryptState.BlockSize; i++)
                {
                    value = (value * CryptState.Key1) + CryptState.Key2;
                    byte[] bytes = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(bytes, 0, cryptState.Table, i * sizeof(UInt32), bytes.Length);
                }
            }

            return cryptState.Table[cryptState.Offset - cryptState.BlockIndex];
        }

        public static void Decrypt(byte[] indexBuffer)
        {
            CryptState cryptState = new CryptState(indexBuffer);

            while (cryptState.Offset < cryptState.Size)
            {
                cryptState.Data[cryptState.Offset] -= Crypt.GetCryptByte(cryptState);
                cryptState.Offset++;
            }
        }

        public static void Encrypt(byte[] indexBuffer)
        {
            CryptState cryptState = new CryptState(indexBuffer);

            while (cryptState.Offset < cryptState.Size)
            {
                cryptState.Data[cryptState.Offset] += Crypt.GetCryptByte(cryptState);
                cryptState.Offset++;
            }
        }
    }
}