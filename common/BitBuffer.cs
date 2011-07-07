using System;

namespace Revival.Common
{
    public class BitBuffer
    {
        private bool _internalBuffer;
        private byte[] _buffer;
        
        public int Offset;
        public int BitOffset;
        public int BytesUsed { get { return (BitOffset >> 3) + ((BitOffset % 8) == 0 ? 0 : 1); } }
        public int Length { get { return _buffer.Length; } }

        private int BitByteOffset { get { return (BitOffset >> 3); } }
        private int _startOffset;
        private int _maxBytes;

        public void SetBuffer(byte[] buffer, int offset, int maxBytes)
        {
            _buffer = buffer;
            Offset = _startOffset = offset;
            _maxBytes = maxBytes;
            BitOffset = 0;
            _internalBuffer = false;
        }

        public void CreateBuffer()
        {
            _buffer = new byte[1024];
            Offset = _startOffset = 0;
            BitOffset = 0;
            _internalBuffer = true;
            _maxBytes = 0;
        }

        public void FreeBuffer()
        {
            _buffer = null;
            Offset = _startOffset = 0;
            BitOffset = 0;
            _internalBuffer = false;
            _maxBytes = 0;
        }

        public bool ReadBool()
        {
            return (ReadBits(1) != 0);
        }

        public char ReadChar()
        {
            return (char)ReadBits(8);
        }

        public byte ReadByte()
        {
            return (byte)ReadBits(8);
        }

        public Int16 ReadInt16()
        {
            return (Int16)ReadBits(16);
        }

        public UInt16 ReadUInt16()
        {
            return (UInt16)ReadBits(16);
        }

        public unsafe float ReadFloat()
        {
            int val = ReadBits(32);
            return (*(float*)&val);
        }

        public Int32 ReadInt32()
        {
            return ReadBits(32);
        }

        public UInt32 ReadUInt32()
        {
            return (UInt32)ReadBits(32);
        }

        public int ReadBitsShift(int bitCount)
        {
            int val = ReadBits(bitCount);
            int shift = (1 << (bitCount - 1));
            return val - shift;
        }

        public Int64 ReadInt64()
        {
            Int64 low = ReadUInt32();
            Int64 high = ReadUInt32();

            high <<= 32;
            high |= low;

            return high;
        }

        public UInt64 ReadUInt64()
        {
            UInt64 low = ReadUInt32();
            UInt64 high = ReadUInt32();

            high <<= 32;
            high |= low;

            return high;
        }

        public Int64 ReadNonStandardFunc() // todo: this is how this function works in ASM - this might be reading in a double value, so until we've tested all usages, leaving it as "ReadNonStandardFunc"
        {
            byte[] ret = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                ret[i] = (byte)ReadBits(8);
            }

            return BitConverter.ToInt64(ret, 0); ;
        }

        public int ReadBits(int bitCount)
        {
            int bitsToRead = bitCount;
            int b = _buffer[Offset + BitByteOffset];

            int offsetBitsInThisByte = BitOffset & 0x07;
            int bitsToUseFromByte = 0x08 - offsetBitsInThisByte;

            int bitOffset = bitCount;
            if (bitsToUseFromByte < bitCount) bitOffset = bitsToUseFromByte;

            b >>= offsetBitsInThisByte;
            bitsToRead -= bitOffset;

            // clean any excess bits we don't want
            b &= ((0x01 << bitOffset) - 1);

            int bytesStillToRead = bitsToRead + 0x07;
            bytesStillToRead >>= 3;

            int ret = b;
            for (int i = bytesStillToRead; i > 0; i--)
            {
                int bitLevel = (i - 1) * 8;

                b = _buffer[Offset + BitByteOffset + i];
                int bitsRead = 0x08;

                if (i == bytesStillToRead)
                {
                    int cleanBits = bitsToRead - bitLevel;
                    bitsRead = cleanBits;
                    cleanBits = 0x01 << cleanBits;
                    cleanBits--;
                    b &= (byte)cleanBits;
                }

                b <<= bitOffset + bitLevel;
                ret |= b;
                bitsToRead -= bitsRead;
            }

            BitOffset += bitCount;

            return ret;
        }

        public void WriteBool(bool value)
        {
            WriteBits(value ? 1 : 0, 1);
        }

        public void WriteByte(int value)
        {
            WriteBits(value, 8);
        }

        public void WriteByte(byte value)
        {
            WriteBits(value, 8);
        }

        public void WriteInt16(Int16 value)
        {
            WriteBits(value, 16);
        }

        public void WriteUInt16(UInt16 value)
        {
            WriteBits(value, 16);
        }

        public void WriteInt16(Int32 value)
        {
            WriteBits(value, 16);
        }

        public void WriteUInt16(UInt32 value)
        {
            WriteBits((Int32)value, 16);
        }

        public unsafe void WriteFloat(float value)
        {
            int intVal = *(int*)&value;
            WriteBits(intVal, 32);
        }

        public void WriteInt32(Int32 value)
        {
            WriteBits(value, 32);
        }

        public void WriteUInt32(UInt32 value)
        {
            WriteBits((Int32)value, 32);
        }

        public void WriteUInt64(UInt64 value)
        {
            WriteBits((Int32)value, 32); // low
            WriteBits((Int32)(value >> 32), 32); // high
        }

        public void WriteNonStandardFunc(Int64 val)
        {
            byte[] byteArray = BitConverter.GetBytes(val);
            for (int i = 0; i < 8; i++)
            {
                WriteBits(byteArray[i], 8);
            }
        }

        public void WriteBitsShift(int value, int bitCount)
        {
            int shift = (1 << (bitCount - 1));
            value += shift;
            WriteBits(value, bitCount);
        }

        public void WriteBits(int value, int bitCount)
        {
            WriteBits(value, bitCount, BitOffset, true);
        }

        public void WriteBits(int value, int bitCount, int bitOffset, bool incrementBitOffset = false)
        {
            int currByteOffset = bitOffset >> 3;
            if (_internalBuffer && currByteOffset > _buffer.Length - 10)
            {
                byte[] newData = new byte[_buffer.Length + 1024];
                System.Buffer.BlockCopy(_buffer, 0, newData, 0, _buffer.Length);
                _buffer = newData;
            }

            int bitsToWrite = bitCount;
            int offsetBitsInFirstByte = bitOffset & 0x07;
            int bitByteOffset = 0x08 - offsetBitsInFirstByte;

            int bitsInFirstByte = bitCount;
            if (bitByteOffset < bitCount) bitsInFirstByte = bitByteOffset;

            int bytesToWriteTo = (bitsToWrite + 0x07 + offsetBitsInFirstByte) >> 3;
            if (_maxBytes > 0 && incrementBitOffset && Offset + currByteOffset + bytesToWriteTo > _startOffset + _maxBytes) throw new IndexOutOfRangeException("Written byte count has exceeded the maximum byte count of " + _maxBytes);

            for (int i = 0; i < bytesToWriteTo; i++, currByteOffset++)
            {
                int bitLevel = 0;
                if (offsetBitsInFirstByte > 0 && i > 0)
                {
                    bitLevel = 8 - offsetBitsInFirstByte;
                }
                if (offsetBitsInFirstByte > 0 && i >= 2)
                {
                    bitLevel += (i - 1) * 8;
                }
                else if (offsetBitsInFirstByte == 0 && i >= 1)
                {
                    bitLevel += i * 8;
                }

                int toWrite = (value >> bitLevel);
                if (i == 0)
                {
                    toWrite &= ((1 << bitsInFirstByte) - 1);
                    toWrite <<= offsetBitsInFirstByte;
                    bitsToWrite -= bitsInFirstByte;
                }
                else if (i == bytesToWriteTo - 1 && offsetBitsInFirstByte > 0)
                {
                    toWrite &= ((1 << bitsToWrite) - 1);
                }
                else
                {
                    bitsToWrite -= 8;
                }

                _buffer[Offset + currByteOffset] |= (byte)toWrite;
            }

            if (incrementBitOffset)
            {
                BitOffset += bitCount;
            }
        }

        public byte[] GetBuffer()
        {
            if (!_internalBuffer) return _buffer;

            Array.Resize(ref _buffer, BytesUsed);
            return _buffer;
        }
    }
}