using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EffectsFiles
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string FXOName;
        [ExcelOutput(SortColumnOrder = 2)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string fxFile;
        public Folder folder;
        public SubFolder subFolder;
        public Int32 undefined1;
        public float rangeToFallBack;
        [ExcelOutput(IsTableIndex = true, TableStringId = "EFFECTS")]
        public Int32 distanceFallBack;
        public Int32 undefined2;
        public Int32 undefined3;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public EffectsFiles.BitMask01 bitmask01;
        public VertexFormat vertexFormat;
        public TechniqueGroup techniqueGroup;
        public Int32 sBranchDepthUS;
        public Int32 sBranchDepthPS;

        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            stateFromEffect = 1,
            castShadow = 2,
            receiveShadow = 4,
            animated = 8,
            renderToZ = 16,
            forceAlphaPass = 32,
            alphaBlend = 64,
            alphaTest = 128,
            checkFormat = 256,
            fragments = 512,
            forceLightMap = 1024,
            needsNormal = 2048,
            compressTexCoord = 4096,
            specularLUT = 8192,
            useBGSHCoefs = 16384,
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

        public enum Folder
        {
            Common = 0,
            Hellgate = 1,
            Tugboat = 2
        }
        public enum SubFolder
        {
            Null = 0,
            OneX = 1
        }
        public enum VertexFormat
        {
            Null = -1,
            Rigid64 = 1,
            Rigid32 = 2,
            Rigid16 = 3,
            Animated = 4,
            Animated11 = 5,
            Particle = 6,
            Particale11 = 7
        }

        public enum TechniqueGroup
        {
            Null = -1,
            Model = 0,
            Particle = 1,
            Blur = 2,
            General = 3,
            List = 5
        }
    }
}
