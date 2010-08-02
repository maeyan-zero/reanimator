using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EffectsFilesRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string FXOName;
        [ExcelOutput(SortId = 2)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string fxFile;
        public Int32 folder;
        public Int32 subFolder;
        public Int32 undefined1;
        public float rangeForFallBack;
        public Int32 distanceForFallBack;
        public Int32 undefined2;
        public Int32 undefined3;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public EffectsFiles.BitMask01 bitmask01;
        public Int32 vertexFormat;
        public Int32 techniqueGroup;
        public Int32 sBranchDepthUS;
        public Int32 sBranchDepthPS;

        public abstract class EffectsFiles
        {
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
        }
    }
}
