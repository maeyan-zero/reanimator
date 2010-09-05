using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SoundsTCv4Row
    {
        ExcelFile.TableHeader header;

        [ExcelFile.ExcelOutput(IsStringOffset = true, SortId = 1)]
        public Int32 name; //pchar
        public Int32 undefined1;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 extension; //pchar
        public Int32 undefined2;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 lqExtension; //pchar
        public Int32 undefined3a;
        public Int32 undefined3b;
        public Int32 pickType;//unk
        public Int32 language;//idx
        public Int32 volume;
        public float undefined2a;
        public float minRange;
        public float maxRange;
        public Int32 rollOffType;
        public Int32 reverbSend;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Sounds.BitMask01 bitmask01;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 directory;//pchar
        public Int32 undefined5;
        public Int32 fileName1;
        Int32 undefined6a;
        Int32 undefined6b;
        public Int32 undefined6c;//information
        public Int32 undefined6d;//information
        Int32 undefined6e;
        public Int32 undefined6f;//information
        Int32 undefined6g;
        public float weight1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined7;
        public Int32 fileName2;
        Int32 undefined8a;
        Int32 undefined8b;
        public Int32 undefined8c;//information
        public Int32 undefined8d;//information
        Int32 undefined8e;
        public Int32 undefined8f;//information
        Int32 undefined8g;
        public float weight2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined9;
        public Int32 fileName3;
        Int32 undefined10a;
        Int32 undefined10b;
        public Int32 undefined10c;//information
        public Int32 undefined10d;//information
        Int32 undefined10e;
        public Int32 undefined10f;//information
        Int32 undefined10g;
        public float weight3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined11;
        public Int32 fileName4;
        Int32 undefined12a;
        Int32 undefined12b;
        public Int32 undefined12c;//information
        public Int32 undefined12d;//information
        Int32 undefined12e;
        public Int32 undefined12f;//information
        Int32 undefined12g;
        public float weight4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined13;
        public Int32 fileName5;
        Int32 undefined14a;
        Int32 undefined14b;
        public Int32 undefined14c;//information
        public Int32 undefined14d;//information
        Int32 undefined14e;
        public Int32 undefined14f;//information
        Int32 undefined14g;
        public float weight5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined15;
        public Int32 fileName6;
        Int32 undefined16a;
        Int32 undefined16b;
        public Int32 undefined16c;//information
        public Int32 undefined16d;//information
        Int32 undefined16e;
        public Int32 undefined16f;//information
        Int32 undefined16g;
        public float weight6;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined17;
        public Int32 fileName7;
        Int32 undefined18a;
        Int32 undefined18b;
        public Int32 undefined18c;//information
        public Int32 undefined18d;//information
        Int32 undefined18e;
        public Int32 undefined18f;//information
        Int32 undefined18g;
        public float weight7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined19;
        public Int32 fileName8;
        Int32 undefined20a;
        Int32 undefined20b;
        public Int32 undefined20c;//information
        public Int32 undefined20d;//information
        Int32 undefined20e;
        public Int32 undefined20f;//information
        Int32 undefined20g;
        public float weight8;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined21;
        public Int32 fileName9;
        Int32 undefined22a;
        Int32 undefined22b;
        public Int32 undefined22c;//information
        public Int32 undefined22d;//information
        Int32 undefined22e;
        public Int32 undefined22f;//information
        Int32 undefined22g;
        public float weight9;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined23;
        public Int32 fileName10;
        Int32 undefined24a;
        Int32 undefined24b;
        public Int32 undefined24c;//information
        public Int32 undefined24d;//information
        Int32 undefined24e;
        public Int32 undefined24f;//information
        Int32 undefined24g;
        public float weight10;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined25;
        public Int32 fileName11;
        Int32 undefined26a;
        Int32 undefined26b;
        public Int32 undefined26c;//information
        public Int32 undefined26d;//information
        Int32 undefined26e;
        public Int32 undefined26f;//information
        Int32 undefined26g;
        public float weight11;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined27;
        public Int32 fileName12;
        Int32 undefined28a;
        Int32 undefined28b;
        public Int32 undefined28c;//information
        public Int32 undefined28d;//information
        Int32 undefined28e;
        public Int32 undefined28f;//information
        Int32 undefined28g;
        public float weight12;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined29;
        public Int32 fileName13;
        Int32 undefined30a;
        Int32 undefined30b;
        public Int32 undefined30c;//information
        public Int32 undefined30d;//information
        Int32 undefined30e;
        public Int32 undefined30f;//information
        Int32 undefined30g;
        public float weight13;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined31;
        public Int32 fileName14;
        Int32 undefined32a;
        Int32 undefined32b;
        public Int32 undefined32c;//information
        public Int32 undefined32d;//information
        Int32 undefined32e;
        public Int32 undefined32f;//information
        Int32 undefined32g;
        public float weight14;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined33;
        public Int32 freqVar;
        public Int32 undefined34;
        public Int32 volVar;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined35;
        public Int32 maxWithInRad;
        public float radius;
        public Int32 hardwarePriority;
        public Int32 gamePriority;
        public float fadeOutTime;
        public Int32 undefined40;
        public float fadeInTime;
        public Int32 undefined41;
        public float frontSend;
        public float centerSend;
        public float rearSend;
        public float sideSend;
        public Int32 lfeSend;//lfe ?
        public Int32 mixState;//idx
        public Int32 bus;
        public Int32 vcas1;//list
        public Int32 vcas2;
        public Int32 vcas3;
        public Int32 vcas4;
        public Int32 vcas5;
        public Int32 vcas6;
        public Int32 vcas7;
        public Int32 vcas8;
        public Int32 musicRef;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined44;
        public Int32 stingerRef;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined45;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 effects;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined46;
        [ExcelOutput(Exclude = true)]
        public Int32 ADSR_tcv4;
        [ExcelOutput(Exclude = true)]
        public Int32 undefined_TCV4_2;//these may not even be the new columns, but all of these undefined columns seem to be zero.
        [ExcelOutput(Exclude = true)]
        public Int32 undefined_TCV4_3;
        [ExcelOutput(Exclude = true)]
        public Int32 undefined_TCV4_4;
        public Int32 undefined46d;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        Int32[] undefined47;



        public abstract class Sounds
        {
            [FlagsAttribute]
            public enum BitMask01 : uint
            {
                nonblock = 1,
                stream = 2,
                is3D = 4,//it is in fact called just 3D, but that isn't liked.
                unk01 = 8,
                loops = 16,
                canCutoff = 32,
                highlander = 64,
                groupHighlander = 128,
                software = 256,
                headRelative = 512,
                isMusic = 1024,
                dontRandomizeStart = 2048,
                dontCrossfadeVariations = 4096,
                unk02 = 8192,
                loadAtStartup = 16384
            }
        }
    }
}