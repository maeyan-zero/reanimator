using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SoundsRow
    {
        ExcelFile.TableHeader header;

        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 name; //pchar
        public Int32 undefined1;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 extension; //pchar
        public Int32 undefined2;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 lqExtension; //pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Int32[] undefined3;
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined6;
        public float weight1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined7;
        public Int32 fileName2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined8;
        public float weight2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined9;
        public Int32 fileName3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined10;
        public float weight3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined11;
        public Int32 fileName4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined12;
        public float weight4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined13;
        public Int32 fileName5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined14;
        public float weight5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined15;
        public Int32 fileName6;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined16;
        public float weight6;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined17;
        public Int32 fileName7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined18;
        public float weight7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined19;
        public Int32 fileName8;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined20;
        public float weight8;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined21;
        public Int32 fileName9;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined22;
        public float weight9;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined23;
        public Int32 fileName10;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined24;
        public float weight10;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined25;
        public Int32 fileName11;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined26;
        public float weight11;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined27;
        public Int32 fileName12;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined28;
        public float weight12;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined29;
        public Int32 fileName13;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined30;
        public float weight13;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined31;
        public Int32 fileName14;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public Int32[] undefined32;
        public float weight14;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] undefined33;
        public Int32 freqVar;
        public Int32 undefined34;
        public Int32 volVar;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Int32[] undefined35;
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
        public Int32 undefined43;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Int32[] vcas;//list
        public Int32 musicRef;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Int32[] undefined44;
        public Int32 stingerRef;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Int32[] undefined45;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 effects;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        public Int32[] undefined46;
    }

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
            loadAtStartup = 16384,
            useGlobalLights = 32768,
            backupTransSpecular = 65536,
            emitsGpuParticles = 131072,
            isScreenEffect = 262144,
            loadAllTechniques = 524288,
            receiveRain = 1048576,
            oneParticleSystem = 2097152,
            usesPortals = 4194304,
            requiresHavokFx = 8388608,
            directionalInSH = 16777216,
            emissivediffuse = 33554432
        }
    }
}