using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class StateEventTypesRow
    {
        ExcelFile.TableHeader header;

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
        public Int32 clientOnly;//bool
        public Int32 serverOnly;//bool
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public StateEventTypes.BitMask01 bitmask01;
        /*public Int32 bitMask1;/* bit uses force new, 0
	bit uses first person, 1
	bit uses add to center, 2
	bit uses control unit only, 3
	bit uses float, 4
	bit uses owned by control, 5
	bit uses set immediately, 6
	bit uses clear immediately, 7
	bit uses not control unit, 8
	bit uses on weapons, 9
	bit uses ignore camera, 10
	bit uses on clear, 11
	bit unknown, 12
	bit reapply on appearance load, 14
	bit uses share duration, 13
	bit uses check condition on weapons, 15  */
        public Int32 usesBone;//bool
        public Int32 usesBoneWeight;//bool
        public Int32 usesAttachments;//bool
        public Int32 addsRopesOnAddTarget;//bool
        public Int32 usesMaterialOverrideType;//bool
        public Int32 refreshStatsPointer;//bool
        public Int32 usesEnvironmentOverride;//bool
        public Int32 usesScreenEffects;//bool
        public Int32 canClearOnFree;//bool
        public Int32 controlUnitOnly;//bool
        public Int32 requiresLoadedGraphics;//bool
        public Int32 applyToAppearanceOverride;//bool
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] unknown2;
    }
    public abstract class StateEventTypes
    {
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