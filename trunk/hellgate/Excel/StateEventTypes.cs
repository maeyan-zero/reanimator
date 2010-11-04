using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class StateEventTypes
    {
        TableHeader header;
        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string paramText;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string setEventHandler;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] undefined1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string clearEventHandler;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] undefined2;
        public Int32 usesTable0;
        public Int32 usesTable1;
        public Int32 comboFilterFunction0;
        public Int32 comboFilterFunction1;
        [ExcelOutput(IsBool = true)]
        public Int32 clientOnly;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 serverOnly;//bool
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public StateEventTypes.BitMask01 bitmask01;
        [ExcelOutput(IsBool = true)]
        public Int32 usesBone;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesBoneWeight;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesAttachments;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 addsRopesOnAddTarget;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesMaterialOverrideType;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 refreshStatsPointer;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesEnvironmentOverride;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesScreenEffects;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 canClearOnFree;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 controlUnitOnly;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 requiresLoadedGraphics;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 applyToAppearanceOverride;//bool
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] unknown2;

        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            usesForceNew = 1,
            usesFirstPerson = 2,
            usesAddToCenter = 4,
            usesControlUnitOnly = 8,
            usesFloat = 16,
            usesOwnedByControl = 32,
            usesSetImmediately = 64,
            usesClearImmediately = 128,
            usesNotControlUnit = 256,
            usesOnWeapons = 512,
            usesIgnoreCamera = 1024,
            usesOnClear = 2048,
            unknown = 4096,
            reapplyOnAppearanceLoad = 8192,
            usesShareDuration = 16384,
            usesCheckConditionOnWeapons = 32768
        }
    }
}
