using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reanimator
{
    class BitBuffer
    {
        byte[] data;
        int dataByteSize = 0;
        int dataBitSize = 0;

        int dataByteOffset = 0;
        public int DataByteOffset
        {
            set
            {
                if (value >= 0 && value <= dataByteSize)
                {
                    dataByteOffset = value;
                }
            }
            get
            {
                return dataByteOffset;
            }
        }
 
        int dataBitOffset = 0;
        public int DataBitOffset
        {
            set
            {
                if (value >= 0 && value <= dataBitSize)
                {
                    dataBitOffset = value;
                }
            }
            get
            {
                return dataBitOffset;
            }
        }

        public BitBuffer(byte[] dataIn)
        {
            data = dataIn;
            dataByteSize = dataIn.Length;
            dataBitSize = dataByteSize * 8;
        }

        public int ReadBits(int bitCount)
        {
            int bitsToRead = bitCount;
            int byteOffset = dataBitOffset >> 3;
            int b = data[dataByteOffset + byteOffset];

            int offsetBitsInThisByte = dataBitOffset & 0x07;
            int bitsToUseFromByte = 0x08 - offsetBitsInThisByte;

            int bitOffset = bitCount;
            if (bitsToUseFromByte < bitCount)
                bitOffset = bitsToUseFromByte;

            b >>= offsetBitsInThisByte;
            bitsToRead -= bitOffset;

            int cleanBits = 0x01 << bitOffset;
            cleanBits--;
            b &= (byte)cleanBits;

            int bytesStillToRead = bitsToRead + 0x07;
            bytesStillToRead >>= 3;

            int ret = b;
            for (int i = bytesStillToRead; i > 0; i--)
            {
                int bitLevel = (i - 1) * 8;

                b = data[dataByteOffset + byteOffset + i];
                int bitsRead = 0x08;

                if (i == bytesStillToRead)
                {
                    cleanBits = bitsToRead - bitLevel;
                    bitsRead = cleanBits;
                    cleanBits = 0x01 << cleanBits;
                    cleanBits--;
                    b &= (byte)cleanBits;
                }

                b <<= bitOffset + bitLevel;
                ret |= b;
                bitsToRead -= bitsRead;
            }

            dataBitOffset += bitCount;

            return ret;
        }
    }
}
