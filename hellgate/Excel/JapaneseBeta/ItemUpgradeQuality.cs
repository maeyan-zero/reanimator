using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemUpgradeQualityBeta
    {
        ExcelFile.RowHeader header;
        public Int32 quality;
        public float weight;
    }
}
