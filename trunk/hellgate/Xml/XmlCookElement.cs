using System;

namespace Hellgate.Xml
{
    public enum ElementType : ushort
    {
        Int32 = 0x0000,
        Int32ArrayFixed = 0x0006,           // found in AppearanceDefinition (pnWardrobeLayerParams) and ColorDefinition (pdwColors)
        Int32ArrayVariable = 0x0007,

        Float = 0x0100,
        FloatArrayFixed = 0x0106,
        FloatArrayVariable = 0x0107,        // found in AppearanceDefinition

        String = 0x0200,
        StringArrayFixed = 0x0206,          // found in AppearanceDefinition, like any other array type but as String
        StringArrayVariable = 0x0207,       // found in AppearanceDefinition and Material

        Table = 0x0308,
        TableArrayFixed = 0x0309,           // found in EnvironmentDefinition
        TableArrayVariable = 0x030A,

        FloatTripletArrayVariable = 0x0500, // found in AppearanceDefinition and children, appears on types like tSelfIllumation, tSelfIllumationBlend, etc
        FloatQuadArrayVariable = 0x0600,    // found in Screen FX

        NonCookedInt32 = 0x0700,
        NonCookedInt3207 = 0x0707,          // found in TCv4 Path files; probably variable array version - not cooked though so doesn't really matter
        NonCookedFloat = 0x0800,

        ExcelIndex = 0x0903,
        ExcelIndexArrayFixed = 0x0905,      // found in AppearanceDefinition, like any other array type but as ExcelIndex

        Int32_0x0A00 = 0x0A00,              // found in AppearanceDefinition and children; appears to be treated like normal Int32
        Int32Array_0x0A06 = 0x0A06,         // found in AppearanceDefinition, like any other array type but as Int32_0x0A00; name usually "pnName"
        Flag = 0x0B01,
        BitFlag = 0x0C02,
        Pointer = 0x0D00,                   // found in RoomPath and AppearanceDefinition and children; no known use/occurance - name usually "pName" (assumed to be a pointer)

        ByteArrayVariable = 0x1007,         // found in BlendRun (TextureDefinition child)

        Vector4 = 0xFFFB,                   // custom type for Vector4 object
        Unsigned = 0xFFFC,                  // custom type for Int32ArrayFixed -> UInt32ArrayFixed (but can/will be applicable for other Int->UInt types etc)
        Bool = 0xFFFD,                      // custom type for Int32 -> Bool
        Vector3 = 0xFFFE,                   // custom type for Vector3 object
        Object = 0xFFFF                     // custom type for objects (e.g. ConditionDef, AttachmentDef)
    }

    public class XmlCookElement
    {
        public String Name;
        public String TrueName;             // some element names have illegal xml chars in them (bad FSS), so we need the "true" name if we want the correct string hash
        public UInt32 NameHash;
        public Object DefaultValue;
        public Type ChildType;
        public UInt32 ChildTypeHash;
        public UInt32 ExcelTableCode;
        public ElementType ElementType;
        public Int32 FlagId;
        public Int32 FlagOffsetChange;      // EnvironmentDefinition has a dwDefFlags first, which actually reads in the following 5 flag elements causing an offset error.
        public UInt32 FlagMask;
        public Int32 BitFlagIndex;
        public Int32 BitFlagCount;          // used for BitIndex, total field BitCount
        public Int32 Count;                 // general array-type count
        public bool TreatAsData;
        public bool IsTestCentre;           // TestCentre elements only
        public bool IsResurrection;         // Resurrection elements only
        public UInt32 HashOverride;         // Elements with unknown strings
    }
}