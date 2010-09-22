﻿using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class InteractMenuRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 interaction;//index
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Strings")]
        public Int32 stringTitle;
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Strings")]
        public Int32 stringToolTip;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string frameIcon;
        public Int32 menuButton;

    }
}