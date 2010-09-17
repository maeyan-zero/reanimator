using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SoundsRow
    {
        ExcelFile.TableHeader header;

        [ExcelFile.ExcelOutput(IsStringOffset = true, SortId = 1, RequiresDefault = true)]
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
        public BitMask01 bitmask01;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 directory;//pchar
        public Int32 undefined5;
        public Int32 fileName1;
        public Int32 undefined6a;
        public Int32 undefined6b;
        public Int32 undefined6c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined6d;//information
        public Int32 undefined6e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined6f;//information
        public Int32 undefined6g;
        public float weight1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined7;
        public Int32 fileName2;
        public Int32 undefined8a;
        public Int32 undefined8b;
        public Int32 undefined8c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined8d;//information
        public Int32 undefined8e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined8f;//information
        public Int32 undefined8g;
        public float weight2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined9;
        public Int32 fileName3;
        public Int32 undefined10a;
        public Int32 undefined10b;
        public Int32 undefined10c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined10d;//information
        public Int32 undefined10e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined10f;//information
        public Int32 undefined10g;
        public float weight3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined11;
        public Int32 fileName4;
        public Int32 undefined12a;
        public Int32 undefined12b;
        public Int32 undefined12c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined12d;//information
        public Int32 undefined12e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined12f;//information
        public Int32 undefined12g;
        public float weight4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined13;
        public Int32 fileName5;
        public Int32 undefined14a;
        public Int32 undefined14b;
        public Int32 undefined14c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined14d;//information
        public Int32 undefined14e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined14f;//information
        public Int32 undefined14g;
        public float weight5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined15;
        public Int32 fileName6;
        public Int32 undefined16a;
        public Int32 undefined16b;
        public Int32 undefined16c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined16d;//information
        public Int32 undefined16e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined16f;//information
        public Int32 undefined16g;
        public float weight6;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined17;
        public Int32 fileName7;
        public Int32 undefined18a;
        public Int32 undefined18b;
        public Int32 undefined18c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined18d;//information
        public Int32 undefined18e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined18f;//information
        public Int32 undefined18g;
        public float weight7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined19;
        public Int32 fileName8;
        public Int32 undefined20a;
        public Int32 undefined20b;
        public Int32 undefined20c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined20d;//information
        public Int32 undefined20e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined20f;//information
        public Int32 undefined20g;
        public float weight8;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined21;
        public Int32 fileName9;
        public Int32 undefined22a;
        public Int32 undefined22b;
        public Int32 undefined22c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined22d;//information
        public Int32 undefined22e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined22f;//information
        public Int32 undefined22g;
        public float weight9;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined23;
        public Int32 fileName10;
        public Int32 undefined24a;
        public Int32 undefined24b;
        public Int32 undefined24c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined24d;//information
        public Int32 undefined24e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined24f;//information
        public Int32 undefined24g;
        public float weight10;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined25;
        public Int32 fileName11;
        public Int32 undefined26a;
        public Int32 undefined26b;
        public Int32 undefined26c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined26d;//information
        public Int32 undefined26e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined26f;//information
        public Int32 undefined26g;
        public float weight11;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined27;
        public Int32 fileName12;
        public Int32 undefined28a;
        public Int32 undefined28b;
        public Int32 undefined28c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined28d;//information
        public Int32 undefined28e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined28f;//information
        public Int32 undefined28g;
        public float weight12;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined29;
        public Int32 fileName13;
        public Int32 undefined30a;
        public Int32 undefined30b;
        public Int32 undefined30c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined30d;//information
        public Int32 undefined30e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined30f;//information
        public Int32 undefined30g;
        public float weight13;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined31;
        public Int32 fileName14;
        public Int32 undefined32a;
        public Int32 undefined32b;
        public Int32 undefined32c;//information
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined32d;//information
        public Int32 undefined32e;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 undefined32f;//information
        public Int32 undefined32g;
        public float weight14;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] undefined33;
        public Int32 freqVar;
        public Int32 undefined34;
        public Int32 volVar;
        public Int32 undefined123;//needs labeling
        public Int32 undefined1234;//needs labeling
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
        public Int32 undefined46d;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] undefined47;

        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            nonblock = (1 << 0),
            stream = (1 << 1),
            is3D = (1 << 2),//it is in fact called just 3D, but that isn't liked.
            unk01 = (1 << 3),
            loops = (1 << 4),
            canCutoff = (1 << 5),
            highlander = (1 << 6),
            groupHighlander = (1 << 7),
            software = (1 << 8),
            headRelative = (1 << 9),
            isMusic = (1 << 10),
            dontRandomizeStart = (1 << 11),
            dontCrossfadeVariations = (1 << 12),
            unk02 = (1 << 13),
            loadAtStartup = (1 << 14),
            useGlobalLights = (1 << 15),
            backupTransSpecular = (1 << 16),
            emitsGpuParticles = (1 << 17),
            isScreenEffect = (1 << 18),
            loadAllTechniques = (1 << 19),
            receiveRain = (1 << 20),
            oneParticleSystem = (1 << 21),
            usesPortals = (1 << 22),
            requiresHavokFx = (1 << 23),
            directionalInSH = (1 << 24),
            emissivediffuse = (1 << 25)
        }
    }
}