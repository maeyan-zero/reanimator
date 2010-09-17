using System;

namespace Hellgate
{
    public static class Crypt
    {
        private const Int32 StringHashSize = 256;
        private static readonly UInt32[] StringHash = new UInt32[StringHashSize];
        private static bool _needStringHash = true;
        private const UInt32 StringKey1 = 0x80000000;
        private const Int32 StringKey2 = 0x4C11DB7;

        public static UInt32 GetHash(String str)
        {
            if (String.IsNullOrEmpty(str)) return 0;
            if (_needStringHash) GenerateStringHash();

            UInt32 stringHash = 0;
            int strLen = str.Length;
            for (int i = 0; i < strLen; i++)
            {
                UInt32 hashIndex = stringHash >> 0x18;
                UInt32 hashSalt = stringHash << 0x08;
                hashIndex ^= str[i];
                stringHash = StringHash[hashIndex] ^ hashSalt;
            }

            return stringHash;
        }

        private static void GenerateStringHash()
        {
            for (int i = 0; i < StringHashSize; i++)
            {
                Int32 hashValue = i << 0x18;

                Int32 hashSalt1 = hashValue * 2;
                Int32 hashSalt2 = 0;
                if ((UInt32)hashValue >= StringKey1) hashSalt2--;

                hashSalt2 &= StringKey2;
                hashSalt2 ^= hashSalt1;
                hashSalt1 = hashSalt2 * 2;
                hashValue = 0;
                if ((UInt32)hashSalt2 >= StringKey1) hashValue--;

                hashValue &= StringKey2;
                hashValue ^= hashSalt1;
                hashSalt1 = hashValue * 2;
                hashSalt2 = 0;
                if ((UInt32)hashValue >= StringKey1) hashSalt2--;

                hashSalt2 &= StringKey2;
                hashSalt2 ^= hashSalt1;
                hashSalt1 = hashSalt2 * 2;
                hashValue = 0;
                if ((UInt32)hashSalt2 >= StringKey1) hashValue--;

                hashValue &= StringKey2;
                hashValue ^= hashSalt1;
                hashSalt1 = hashValue * 2;
                hashSalt2 = 0;
                if ((UInt32)hashValue >= StringKey1) hashSalt2--;

                hashSalt2 &= StringKey2;
                hashSalt2 ^= hashSalt1;
                hashSalt1 = hashSalt2 * 2;
                hashValue = 0;
                if ((UInt32)hashSalt2 >= StringKey1) hashValue--;

                hashValue &= StringKey2;
                hashValue ^= hashSalt1;
                hashSalt1 = hashValue * 2;
                hashSalt2 = 0;
                if ((UInt32)hashValue >= StringKey1) hashSalt2--;

                hashSalt2 &= StringKey2;
                hashSalt2 ^= hashSalt1;
                hashSalt1 = hashSalt2 * 2;
                hashValue = 0;
                if ((UInt32)hashSalt2 >= StringKey1) hashValue--;

                hashValue &= StringKey2;
                hashValue ^= hashSalt1;
                StringHash[i] = (UInt32)hashValue;
            }

            _needStringHash = false;
        }

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
                Table = new Byte[BlockSize * sizeof(Int32)];
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
                cryptState.Data[cryptState.Offset] -= GetCryptByte(cryptState);
                cryptState.Offset++;
            }
        }

        public static void Encrypt(byte[] indexBuffer)
        {
            CryptState cryptState = new CryptState(indexBuffer);

            while (cryptState.Offset < cryptState.Size)
            {
                cryptState.Data[cryptState.Offset] += GetCryptByte(cryptState);
                cryptState.Offset++;
            }
        }
    }
}