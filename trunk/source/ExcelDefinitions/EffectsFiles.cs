using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EffectsFilesRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string FXOName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string fxFile;
        public Int32 folder;
        public Int32 subFolder;
        public Int32 undefined1;
        public float rangeForFallBack;
        public Int32 distanceForFallBack;
        public Int32 undefined2;
        public Int32 undefined3;
        public Int32 bitmask;/*0 bit statefromeffect
	1 bit castshadow
	2 bit receiveshadow
	3 bit animated
	4 bit rendertoz
	5 bit forcealphapass
	6 bit alphablend
	7 bit lphatest
	8 bit checkformat
	9 bit fragments
	A bit forcelightmap
	B bit needs normal
	C bit compress tex coord
	D bit specular LUT
	E bit useBGSHCoefs
	F bit usegloballights
	10 bit backuptransspecular
	11 bit emitsgpuparticles
	12 bit is screen effect
	13 bit loadalltechniques
	14 bit receiverain
	15 bit one particle system
	16 bit uses portals
	17 bit requires havok fx
	18 bit directionalInSH
	19 bit emissivediffuse*/
        public Int32 vertexFormat;
        public Int32 techniqueGroup;
        public Int32 sBranchDepthUS;
        public Int32 sBranchDepthPS;
    }
}
