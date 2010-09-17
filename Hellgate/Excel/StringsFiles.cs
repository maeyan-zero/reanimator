using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class StringFilesRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        [ExcelOutput(IsBool = true)]
        public Int32 isCommon;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 loadedbyGame;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 creditsFile;//bool
    }
}