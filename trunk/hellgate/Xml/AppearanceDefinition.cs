using System;
using Hellgate.Excel;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "APPEARANCE_DEFINITION")]
    public class AppearanceDefinition
    {
        [XmlCookedAttribute(
            Name = "nInitialized",
            DefaultValue = 0,
            ElementType = ElementType.Int32_0x0A00)]
        public Int32 Initialized;

        [XmlCookedAttribute(
            Name = "dwFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.Int32,
            CustomType = ElementType.Unsigned)]
        public UInt32 Flags;

        [XmlCookedAttribute(
            Name = "pszModel",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String Model;

        [XmlCookedAttribute(
            Name = "nModelDefId",
            DefaultValue = -1,
            ElementType = ElementType.Int32_0x0A00)]
        public Int32 ModelDefId;

        [XmlCookedAttribute(
            Name = "pLoader",
            DefaultValue = null,
            ElementType = ElementType.Pointer)]
        public Object Loader;

        [XmlCookedAttribute(
            Name = "pMeshBinding",
            DefaultValue = null,
            ElementType = ElementType.Pointer)]
        public Object MeshBinding;

        [XmlCookedAttribute(
            Name = "pGrannyModel",
            DefaultValue = null,
            ElementType = ElementType.Pointer)]
        public Object GrannyModel;

        [XmlCookedAttribute(
            Name = "nBoneCount",
            DefaultValue = 0,
            ElementType = ElementType.Int32_0x0A00)]
        public Int32 BoneCount;

        [XmlCookedAttribute(
            Name = "pszRagdoll",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String Ragdoll;

        [XmlCookedAttribute(
            Name = "pszHavokShape",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String HavokShape;

        [XmlCookedAttribute(
            Name = "pszSkeleton",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String Skeleton;

        [XmlCookedAttribute(
            Name = "pszNeck",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String Neck;

        [XmlCookedAttribute(
            Name = "pszSpineTop",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String SpineTop;

        [XmlCookedAttribute(
            Name = "pszSpineBottom",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String SpineBottom;

        [XmlCookedAttribute(
            Name = "pszSpineSidesTop",
            DefaultValue = null,
            ElementType = ElementType.StringArrayFixed,
            Count = 2,
            IsTestCentre = true)]
        public String[] SpineSidesTops;

        [XmlCookedAttribute(
            Name = "pszAimBone",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String AimBone;

        [XmlCookedAttribute(
            Name = "pszLeftFootBone",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String LeftFootBone;

        [XmlCookedAttribute(
            Name = "pszRightFootBone",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String RightFootBone;

        [XmlCookedAttribute(
            Name = "pszLeftWeaponBone",
            DefaultValue = null,
            ElementType = ElementType.StringArrayFixed,
            Count = 2)]
        public String[] LeftWeaponBones;

        [XmlCookedAttribute(
            Name = "pszRightWeaponBone",
            DefaultValue = null,
            ElementType = ElementType.StringArrayFixed,
            Count = 2)]
        public String[] RightWeaponBones;

        [XmlCookedAttribute(
            Name = "nNeck",
            DefaultValue = -1,
            ElementType = ElementType.Int32_0x0A00)]
        public Int32 NeckId;

        [XmlCookedAttribute(
            Name = "nSpineTop",
            DefaultValue = -1,
            ElementType = ElementType.Int32_0x0A00)]
        public Int32 SpineTopId;

        [XmlCookedAttribute(
            Name = "pnSpineSidesTop",
            DefaultValue = -1,
            ElementType = ElementType.Int32Array_0x0A06,
            Count = 2,
            IsTestCentre = true)]
        public Int32[] SpineSidesTopIds;

        [XmlCookedAttribute(
            Name = "nSpineBottom",
            DefaultValue = -1,
            ElementType = ElementType.Int32_0x0A00)]
        public Int32 SpineBottomId;

        [XmlCookedAttribute(
            Name = "nAimBone",
            DefaultValue = -1,
            ElementType = ElementType.Int32_0x0A00)]
        public Int32 AimBoneId;

        [XmlCookedAttribute(Name = "vNeckAim", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "vNeckAim.fX",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "vNeckAim.fY",
            DefaultValue = -1.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "vNeckAim.fZ",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 NeckAim = new Vector3();

        [XmlCookedAttribute(
            Name = "fMinFacingTargetZ",
            DefaultValue = -100.0f,
            ElementType = ElementType.Float,
            IsResurrection = true)]
        public float MinFacingTargetZ;

        [XmlCookedAttribute(Name = "vAimOffset", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "vAimOffset.fX",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "vAimOffset.fY",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "vAimOffset.fZ",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AimOffset = new Vector3();

        [XmlCookedAttribute(
            Name = "pvMuzzleOffset",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatArrayFixed,
            Count = 6)]
        public float[] MuzzleOffset;

        [XmlCookedAttribute(
            Name = "nWardrobeBaseId",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.WARDROBE_LAYER)] // 25649 WARDROBE_LAYER
        public Wardrobe WardrobeBase;
        public Int32 WardrobeBaseIndex;

        [XmlCookedAttribute(
            Name = "pnWardrobeLayerIds",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndexArrayFixed,
            TableCode = Xls.TableCodes.WARDROBE_LAYER, // 25649 WARDROBE_LAYER
            Count = 10)]
        public Wardrobe[] WardrobeLayers;
        public int[] WardrobeLayerIndices;

        [XmlCookedAttribute(
            Name = "pnWardrobeLayerParams",
            DefaultValue = 0,
            ElementType = ElementType.Int32ArrayFixed,
            Count = 10)]
        public Int32[] WardrobeLayerParams;

        [XmlCookedAttribute(
            Name = "nWardrobeAppearanceGroup",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.WARDROBE_APPEARANCE_GROUP)] // 21809 WARDROBE_APPEARANCE_GROUP
        public WardrobeAppearanceGroup WardrobeAppearanceGroup;
        public int WardroveAppearanceGroupIndex;

        [XmlCookedAttribute(
            Name = "fHeadTurnTotalLimit",
            DefaultValue = 0,
            ElementType = ElementType.NonCookedInt32)] // fHeadTurnTotalLimit should be float??
        public Int32 HeadTurnTotalLimit;

        [XmlCookedAttribute(
            Name = "nHeadTurnTotalLimitDegrees",
            DefaultValue = 100,
            ElementType = ElementType.Int32)]
        public Int32 HeadTurnTotalLimitDegrees;

        [XmlCookedAttribute(
            Name = "fHeadTurnBoneLimit",
            DefaultValue = 0,
            ElementType = ElementType.NonCookedInt32)] // fHeadTurnBoneLimit should be float??
        public Int32 HeadTurnBoneLimit;

        [XmlCookedAttribute(
            Name = "nHeadTurnBoneLimitDegrees",
            DefaultValue = 100,
            ElementType = ElementType.Int32)]
        public Int32 HeadTurnBoneLimitDegrees;

        [XmlCookedAttribute(
            Name = "fHeadTurnSpeed",
            DefaultValue = 0.05f,
            ElementType = ElementType.Float)]
        public float HeadTurnSpeed;

        [XmlCookedAttribute(
            Name = "fHeadTurnPercentVsTorso",
            DefaultValue = 0.5f,
            ElementType = ElementType.Float)]
        public float HeadTurnPercentVsTorso;

        [XmlCookedAttribute(
            Name = "fTurnSpeed",
            DefaultValue = 0.75f,
            ElementType = ElementType.Float)]
        public float TurnSpeed;

        [XmlCookedAttribute(
            Name = "fSpinSpeed",
            DefaultValue = 0.15f,
            ElementType = ElementType.Float)]
        public float SpinSpeed;

        [XmlCookedAttribute(
            Name = "pszTextureOverrides",
            DefaultValue = null,
            ElementType = ElementType.StringArrayFixed,
            Count = 7)]
        public String[] TextureOverrides;

        [XmlCookedAttribute(
            Name = "pnTextureOverrides",
            DefaultValue = -1,
            ElementType = ElementType.Int32Array_0x0A06,
            Count = 14)]
        public Int32[] TextureOverridesIds;

        [XmlCookedAttribute(
            Name = "pAnimations",
            DefaultValue = 0,
            ElementType = ElementType.TableArrayVariable,
            ChildType = typeof(AnimationDefinition))]
        public AnimationDefinition[] Animations;

        [XmlCookedAttribute(
            Name = "tInitAnimation",
            DefaultValue = null,
            ElementType = ElementType.Table,
            ChildType = typeof(AnimationDefinition))]
        public AnimationDefinition InitAnimation;

        [XmlCookedAttribute(
            Name = "dwViewFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.Int32,
            CustomType = ElementType.Unsigned)]
        public UInt32 ViewFlags;

        [XmlCookedAttribute(
            Name = "nWeaponAnimationGroupId",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.ANIMATION_GROUP)] // 26928 ANIMATION_GROUP
        public AnimationGroups WeaponAnimationGroup;
        public Int32 WeaponAnimationGroupIndex;

        [XmlCookedAttribute(
            Name = "pGrannyFile",
            DefaultValue = null,
            ElementType = ElementType.Pointer)]
        public Object GrannyFile;

        [XmlCookedAttribute(
            Name = "nViewCameraMode",
            DefaultValue = 5,
            ElementType = ElementType.Int32)]
        public Int32 ViewCameraMode;

        [XmlCookedAttribute(
            Name = "pInventoryViews",
            DefaultValue = 0,
            ElementType = ElementType.TableArrayVariable,
            ChildType = typeof(InventoryViewInfo))]
        public InventoryViewInfo[] InventoryViews;

        [XmlCookedAttribute(
            Name = "fMaxRopeBendXY",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float MaxRopeBendXY;

        [XmlCookedAttribute(
            Name = "fMaxRopeBendZ",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float MaxRopeBendZ;

        [XmlCookedAttribute(
            Name = "pszBoneNames",
            DefaultValue = "0", // was null in SP/TC clients...
            ElementType = ElementType.StringArrayVariable)]
        public String[] BoneNames;

        [XmlCookedAttribute(
            Name = "pszWeightGroups",
            DefaultValue = null,
            ElementType = ElementType.StringArrayVariable)]
        public String[] WeightGroups;

        [XmlCookedAttribute(
            Name = "pfWeights",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatArrayVariable)]
        public float[] Weights;

        [XmlCookedAttribute(
            Name = "pAnimGroups",
            DefaultValue = null,
            ElementType = ElementType.Pointer)]
        public Object AnimGroups;

        [XmlCookedAttribute(
            Name = "nAnimGroupCount",
            DefaultValue = 0,
            ElementType = ElementType.Int32_0x0A00)]
        public Int32 AnimGroupCount;

        [XmlCookedAttribute(
            Name = "nGrid_FileNameOffset",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 GridFileNameOffset;

        [XmlCookedAttribute(
            Name = "pszCopyTemplate",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String CopyTemplate;

        [XmlCookedAttribute(
            Name = "pszAnimationFilePrefix",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String AnimationFilePrefix;

        [XmlCookedAttribute(
            Name = "fPreviewScale",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float PreviewScale;

        [XmlCookedAttribute(
            Name = "nPreviewHeight",
            DefaultValue = 127,
            ElementType = ElementType.Int32)]
        public Int32 PreviewHeight;

        [XmlCookedAttribute(
            Name = "nPreviewWeight",
            DefaultValue = 127,
            ElementType = ElementType.Int32)]
        public Int32 PreviewWeight;

        [XmlCookedAttribute(
            Name = "nPreviewWardrobeBody",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.WARDROBE_BODY)] // 25905 WARDROBE_BODY
        public WardrobeBody PreviewWardrobeBody;

        [XmlCookedAttribute(
            Name = "fLODScreensize",
            DefaultValue = -1.0f,
            ElementType = ElementType.Float)]
        public float LODScreensize;

    }
}