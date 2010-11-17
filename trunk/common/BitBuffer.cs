using System;

namespace Revival.Common
{
    public class BitBuffer
    {
        byte[] _data;
        readonly int _dataByteSize;
        readonly int _dataBitSize;

        int _dataByteOffset;
        public int DataByteOffset
        {
            set
            {
                if (value >= 0 && value <= _dataByteSize)
                {
                    _dataByteOffset = value;
                }
            }

            get { return _dataByteOffset; }
        }

        int _dataBitOffset;
        public int DataBitOffset
        {
            set
            {
                if (value >= 0 && value <= _dataBitSize)
                {
                    _dataBitOffset = value;
                }
            }

            get { return _dataBitOffset; }
        }

        public int Length
        {
            get { return _data.Length; }
        }

        public BitBuffer()
        {
            _data = new byte[1024];
        }

        public BitBuffer(byte[] dataIn)
        {
            _data = dataIn;
            _dataByteSize = dataIn.Length;
            _dataBitSize = _dataByteSize * 8;
        }

        public int ReadBits(int bitCount)
        {
            int bitsToRead = bitCount;
            int byteOffset = _dataBitOffset >> 3;
            int b = _data[_dataByteOffset + byteOffset];

            int offsetBitsInThisByte = _dataBitOffset & 0x07;
            int bitsToUseFromByte = 0x08 - offsetBitsInThisByte;

            int bitOffset = bitCount;
            if (bitsToUseFromByte < bitCount)
                bitOffset = bitsToUseFromByte;

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

                b = _data[_dataByteOffset + byteOffset + i];
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

            _dataBitOffset += bitCount;

            return ret;
        }

        public void WriteBits(int value, int bitCount)
        {
            WriteBits(value, bitCount, _dataBitOffset, true);
        }

        public void WriteBits(int value, int bitCount, int dataBitOffset)
        {
            WriteBits(value, bitCount, dataBitOffset, false);
        }

        public void WriteBits(int value, int bitCount, int dataBitOffset, bool setIncrementOffset)
        {
            int byteOffset = dataBitOffset >> 3;
            if (byteOffset > _data.Length - 10)
            {
                byte[] newData = new byte[_data.Length + 1024];
                Buffer.BlockCopy(_data, 0, newData, 0, _data.Length);
                _data = newData;
            }

            int bitsToWrite = bitCount;
            int offsetBitsInFirstByte = dataBitOffset & 0x07;
            int bitByteOffset = 0x08 - offsetBitsInFirstByte;

            int bitsInFirstByte = bitCount;
            if (bitByteOffset < bitCount)
                bitsInFirstByte = bitByteOffset;

            int bytesToWriteTo = (bitsToWrite + 0x07 + offsetBitsInFirstByte) >> 3;

            for (int i = 0; i < bytesToWriteTo; i++, byteOffset++)
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

                _data[_dataByteOffset + byteOffset] |= (byte)toWrite;
            }

            if (setIncrementOffset)
            {
                _dataBitOffset += bitCount;
            }
        }

        public byte[] GetData()
        {
            int byteCount = (_dataBitOffset >> 3) + 1;
            byte[] saveData = new byte[byteCount];
            Buffer.BlockCopy(_data, 0, saveData, 0, byteCount);
            return saveData;
        }
    }
}