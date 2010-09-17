using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EffectsFiles
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String FXOName;
        [ExcelAttribute(SortID = 2)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String fxFile;
        public Int32 folder;
        public Int32 subFolder;
        public Int32 undefined1;
        public Single rangeForFallBack;
        public Int32 distanceForFallBack;
        public Int32 undefined2;
        public Int32 undefined3;
        [ExcelAttribute(IsBitmask = true)]
        public BitMask01 bitmask01;
        public Int32 vertexFormat;
        public Int32 techniqueGroup;
        public Int32 sBranchDepthUS;
        public Int32 sBranchDepthPS;

        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            stateFromEffect = (1 << 0),
            castShadow = (1 << 1),
            receiveShadow = (1 << 2),
            animated = (1 << 3),
            renderToZ = (1 << 4),
            forceAlphaPass = (1 << 5),
            alphaBlend = (1 << 6),
            alphaTest = (1 << 7),
            checkFormat = (1 << 8),
            fragments = (1 << 9),
            forceLightMap = (1 << 10),
            needsNormal = (1 << 11),
            compressTexCoord = (1 << 12),
            specularLUT = (1 << 13),
            useBGSHCoefs = (1 << 14),
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
