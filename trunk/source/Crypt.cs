using System;
using System.Security.Cryptography;

namespace Reanimator
{
    static class Crypt
    {
        ////// IDX String Crypt Stuffs //////
        private static readonly SHA1 SHA1Crypto = new SHA1CryptoServiceProvider();

        /// <summary>
        /// Generates the SHA1 hash key for 2 given strings and returns the first 4 bytes from each as a UInt64.
        /// </summary>
        /// <param name="str1">The first string to be hashed (low).</param>
        /// <param name="str2">The second string to be hashed (high).</param>
        /// <returns>The hashed strings first 4 bytes eash as a UInt64.</returns>
        public static UInt64 GetStringsSHA1UInt64(String str1, String str2)
        {
            UInt32 result1 = GetStringSHA1UInt32(str1);
            UInt32 result2 = GetStringSHA1UInt32(str2);
            return result1 | (UInt64)result2 << 32;
        }

        /// <summary>
        /// Generates the SHA1 hash key for a given string and returns the first 4 bytes as a UInt32.
        /// </summary>
        /// <param name="str">The string the be hashed.</param>
        /// <returns>The first 4 bytes of the hash as a UInt32.</returns>
        public static UInt32 GetStringSHA1UInt32(String str)
        {
            byte[] result = SHA1Crypto.ComputeHash(FileTools.StringToASCIIByteArray(str));
            return BitConverter.ToUInt32(result, 0);
        }


        ////// String Crypt Stuffs //////
        private const Int32 StringHashSize = 256;
        private static readonly UInt32[] StringHash = new UInt32[StringHashSize];
        private static bool _needStringHash = true;
        private const UInt32 StringKey1 = 0x80000000;
        private const Int32 StringKey2 = 0x4C11DB7;

        public static UInt32 GetStringHash(String str)
        {
            if (String.IsNullOrEmpty(str)) return 0;
            if (_needStringHash) GenerateStringHash();

            /* String Hashing Algorithm - hellgate_sp_dx9_x64.exe (1.18074.70.4256)
             * .text:000000014004C2F0 movzx   eax, byte ptr [r9]
             * .text:000000014004C2F4 mov     edx, edi
             * .text:000000014004C2F6 mov     ecx, edi
             * .text:000000014004C2F8 shr     rdx, 18h
             * .text:000000014004C2FC shl     ecx, 8
             * .text:000000014004C2FF add     r9, 1
             * .text:000000014004C303 xor     rdx, rax
             * .text:000000014004C306 mov     edi, [r10+rdx*4]
             * .text:000000014004C30A xor     edi, ecx
             * .text:000000014004C30C sub     r8d, 1
             * .text:000000014004C310 jnz     short loc_14004C2F0
             */

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
            /* String Hash Key Generation - hellgate_sp_dx9_x64.exe (1.18074.70.4256)
             * .text:0000000140035E30 loc_140035E30:
             * .text:0000000140035E30 mov     ecx, r8d
             * .text:0000000140035E33 shl     ecx, 18h
             * .text:0000000140035E36 mov     eax, ecx
             * .text:0000000140035E38 and     eax, 80000000h
             * .text:0000000140035E3D neg     eax
             * .text:0000000140035E3F lea     eax, [rcx+rcx]
             * .text:0000000140035E42 sbb     edx, edx
             * .text:0000000140035E44 and     edx, 4C11DB7h 
             * .text:0000000140035E4A xor     edx, eax
             * .text:0000000140035E4C mov     eax, edx
             * .text:0000000140035E4E and     eax, 80000000h
             * .text:0000000140035E53 neg     eax
             * .text:0000000140035E55 lea     eax, [rdx+rdx]
             * .text:0000000140035E58 sbb     ecx, ecx
             * .text:0000000140035E5A and     ecx, 4C11DB7h 
             * .text:0000000140035E60 xor     ecx, eax
             * .text:0000000140035E62 mov     eax, ecx
             * .text:0000000140035E64 and     eax, 80000000h
             * .text:0000000140035E69 neg     eax
             * .text:0000000140035E6B lea     eax, [rcx+rcx]
             * .text:0000000140035E6E sbb     edx, edx
             * .text:0000000140035E70 and     edx, 4C11DB7h 
             * .text:0000000140035E76 xor     edx, eax
             * .text:0000000140035E78 mov     eax, edx
             * .text:0000000140035E7A and     eax, 80000000h
             * .text:0000000140035E7F neg     eax
             * .text:0000000140035E81 lea     eax, [rdx+rdx]
             * .text:0000000140035E84 sbb     ecx, ecx
             * .text:0000000140035E86 and     ecx, 4C11DB7h 
             * .text:0000000140035E8C xor     ecx, eax
             * .text:0000000140035E8E mov     eax, ecx
             * .text:0000000140035E90 and     eax, 80000000h
             * .text:0000000140035E95 neg     eax
             * .text:0000000140035E97 lea     eax, [rcx+rcx]
             * .text:0000000140035E9A sbb     edx, edx
             * .text:0000000140035E9C and     edx, 4C11DB7h 
             * .text:0000000140035EA2 xor     edx, eax
             * .text:0000000140035EA4 mov     eax, edx
             * .text:0000000140035EA6 and     eax, 80000000h
             * .text:0000000140035EAB neg     eax
             * .text:0000000140035EAD lea     eax, [rdx+rdx]
             * .text:0000000140035EB0 sbb     ecx, ecx
             * .text:0000000140035EB2 and     ecx, 4C11DB7h 
             * .text:0000000140035EB8 xor     ecx, eax
             * .text:0000000140035EBA mov     eax, ecx
             * .text:0000000140035EBC and     eax, 80000000h
             * .text:0000000140035EC1 neg     eax
             * .text:0000000140035EC3 lea     eax, [rcx+rcx]
             * .text:0000000140035EC6 sbb     edx, edx
             * .text:0000000140035EC8 and     edx, 4C11DB7h 
             * .text:0000000140035ECE xor     edx, eax
             * .text:0000000140035ED0 mov     eax, edx
             * .text:0000000140035ED2 and     eax, 80000000h
             * .text:0000000140035ED7 neg     eax
             * .text:0000000140035ED9 lea     eax, [rdx+rdx]
             * .text:0000000140035EDC sbb     ecx, ecx
             * .text:0000000140035EDE add     r8d, 1
             * .text:0000000140035EE2 add     r9, 4
             * .text:0000000140035EE6 and     ecx, 4C11DB7h 
             * .text:0000000140035EEC xor     ecx, eax
             * .text:0000000140035EEE cmp     r8d, 100h
             * .text:0000000140035EF5 mov     [r9-4], ecx
             * .text:0000000140035EF9 jl      loc_140035E30
             */

            for (int i = 0; i < StringHashSize; i++)
            {
                Int32 hashValue = i << 0x18;

                Int32 hashSalt1 = hashValue*2;
                Int32 hashSalt2 = 0;
                if ((UInt32)hashValue >= StringKey1) hashSalt2--;

                hashSalt2 &= StringKey2;
                hashSalt2 ^= hashSalt1;
                hashSalt1 = hashSalt2*2;
                hashValue = 0;
                if ((UInt32)hashSalt2 >= StringKey1) hashValue--;

                hashValue &= StringKey2;
                hashValue ^= hashSalt1;
                hashSalt1 = hashValue*2;
                hashSalt2 = 0;
                if ((UInt32)hashValue >= StringKey1) hashSalt2--;

                hashSalt2 &= StringKey2;
                hashSalt2 ^= hashSalt1;
                hashSalt1 = hashSalt2*2;
                hashValue = 0;
                if ((UInt32)hashSalt2 >= StringKey1) hashValue--;

                hashValue &= StringKey2;
                hashValue ^= hashSalt1;
                hashSalt1 = hashValue*2;
                hashSalt2 = 0;
                if ((UInt32)hashValue >= StringKey1) hashSalt2--;

                hashSalt2 &= StringKey2;
                hashSalt2 ^= hashSalt1;
                hashSalt1 = hashSalt2*2;
                hashValue = 0;
                if ((UInt32)hashSalt2 >= StringKey1) hashValue--;

                hashValue &= StringKey2;
                hashValue ^= hashSalt1;
                hashSalt1 = hashValue*2;
                hashSalt2 = 0;
                if ((UInt32)hashValue >= StringKey1) hashSalt2--;

                hashSalt2 &= StringKey2;
                hashSalt2 ^= hashSalt1;
                hashSalt1 = hashSalt2*2;
                hashValue = 0;
                if ((UInt32)hashSalt2 >= StringKey1) hashValue--;

                hashValue &= StringKey2;
                hashValue ^= hashSalt1;
                StringHash[i] = (UInt32)hashValue;
            }

            _needStringHash = false;
        }



        ////// IDX Crypt Stuffs //////
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