using System;

namespace Hellgate.Xml
{
    internal class AppearanceDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "nInitialized",
                DefaultValue = 0,
                ElementType = ElementType.Int32_0x0A00
            },
            new XmlCookElement
            {
                Name = "dwFlags",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "pszModel",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nModelDefId",
                DefaultValue = -1,
                ElementType = ElementType.Int32_0x0A00
            },
            new XmlCookElement
            {
                Name = "pLoader",
                DefaultValue = null,
                ElementType = ElementType.UnknownPTypeD_0x0D00
            },
            new XmlCookElement
            {
                Name = "pMeshBinding",
                DefaultValue = null,
                ElementType = ElementType.UnknownPTypeD_0x0D00
            },
            new XmlCookElement
            {
                Name = "pGrannyModel",
                DefaultValue = null,
                ElementType = ElementType.UnknownPTypeD_0x0D00
            },
            new XmlCookElement
            {
                Name = "nBoneCount",
                DefaultValue = 0,
                ElementType = ElementType.Int32_0x0A00
            },
            new XmlCookElement
            {
                Name = "pszRagdoll",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "pszHavokShape",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "pszSkeleton",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "pszNeck",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "pszSpineTop",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "pszSpineBottom",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "pszAimBone",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "pszLeftFootBone",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "pszRightFootBone",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "pszLeftWeaponBone",
                DefaultValue = null,
                ElementType = ElementType.StringArray_0x0206,
                Count = 2
            },
            new XmlCookElement
            {
                Name = "pszRightWeaponBone",
                DefaultValue = null,
                ElementType = ElementType.StringArray_0x0206,
                Count = 2
            },
            new XmlCookElement
            {
                Name = "nNeck",
                DefaultValue = -1,
                ElementType = ElementType.Int32_0x0A00
            },
            new XmlCookElement
            {
                Name = "nSpineTop",
                DefaultValue = -1,
                ElementType = ElementType.Int32_0x0A00
            },
            new XmlCookElement
            {
                Name = "nSpineBottom",
                DefaultValue = -1,
                ElementType = ElementType.Int32_0x0A00
            },
            new XmlCookElement
            {
                Name = "nAimBone",
                DefaultValue = -1,
                ElementType = ElementType.Int32_0x0A00
            },
            new XmlCookElement
            {
                Name = "vNeckAim.fX",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vNeckAim.fY",
                DefaultValue = -1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vNeckAim.fZ",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vAimOffset.fX",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vAimOffset.fY",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vAimOffset.fZ",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "pvMuzzleOffset",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatArray,
                Count = 6
            },
            new XmlCookElement
            {
                Name = "nWardrobeBaseId",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 25649 // WARDROBE_LAYER
            },
            new XmlCookElement
            {
                Name = "pnWardrobeLayerIds",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndexArray_0x0905,
                ExcelTableCode = 25649, // WARDROBE_LAYER
                Count = 5
            },
            new XmlCookElement
            {
                Name = "pnWardrobeLayerParams",
                DefaultValue = (UInt32)0,
                ElementType = ElementType.RGBADoubleWordArray,
                Count = 5
            },
            new XmlCookElement
            {
                Name = "nWardrobeAppearanceGroup",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 21809 // WARDROBE_APPEARANCE_GROUP
            },
            new XmlCookElement
            {
                Name = "fHeadTurnTotalLimit",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nHeadTurnTotalLimitDegrees",
                DefaultValue = 100,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fHeadTurnBoneLimit",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nHeadTurnBoneLimitDegrees",
                DefaultValue = 100,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fHeadTurnSpeed",
                DefaultValue = 0.05f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fHeadTurnPercentVsTorso",
                DefaultValue = 0.5f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fTurnSpeed",
                DefaultValue = 0.75f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fSpinSpeed",
                DefaultValue = 0.15f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "pszTextureOverrides",
                DefaultValue = null,
                ElementType = ElementType.StringArray_0x0206,
                Count = 7
            },
            new XmlCookElement
            {
                Name = "pnTextureOverrides",
                DefaultValue = -1,
                ElementType = ElementType.Int32Array_0x0A06,
                Count = 14
            },
            new XmlCookElement
            {
                Name = "pAnimations",
                DefaultValue = 0,
                ElementType = ElementType.TableCount,
                ChildType = typeof (AnimationDefinition)
            },
            new XmlCookElement
            {
                Name = "tInitAnimation",
                DefaultValue = null,
                ElementType = ElementType.Table,
                ChildType = typeof (AnimationDefinition)
            },
            new XmlCookElement
            {
                Name = "dwViewFlags",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nWeaponAnimationGroupId",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 26928 // ANIMATION_GROUP
            },
            new XmlCookElement
            {
                Name = "pGrannyFile",
                DefaultValue = null,
                ElementType = ElementType.UnknownPTypeD_0x0D00
            },
            new XmlCookElement
            {
                Name = "nViewCameraMode",
                DefaultValue = 5,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "pInventoryViews",
                DefaultValue = 0,
                ElementType = ElementType.TableCount,
                ChildType = typeof (InventoryViewInfo)
            },
            new XmlCookElement
            {
                Name = "fMaxRopeBendXY",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fMaxRopeBendZ",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "pszBoneNames",
                DefaultValue = null,
                ElementType = ElementType.StringArrayUnknown_0x0207
            },
            new XmlCookElement
            {
                Name = "pszWeightGroups",
                DefaultValue = null,
                ElementType = ElementType.StringArrayUnknown_0x0207
            },
            new XmlCookElement
            {
                Name = "pfWeights",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatArrayUnknown_0x0107
            },
            new XmlCookElement
            {
                Name = "pAnimGroups",
                DefaultValue = null,
                ElementType = ElementType.UnknownPTypeD_0x0D00
            },
            new XmlCookElement
            {
                Name = "nAnimGroupCount",
                DefaultValue = 0,
                ElementType = ElementType.Int32_0x0A00
            },
            new XmlCookElement
            {
                Name = "nGrid_FileNameOffset",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "pszCopyTemplate",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "pszAnimationFilePrefix",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "fPreviewScale",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "nPreviewHeight",
                DefaultValue = 127,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nPreviewWeight",
                DefaultValue = 127,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nPreviewWardrobeBody",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 25905 // WARDROBE_BODY
            },
            new XmlCookElement
            {
                Name = "fLODScreensize",
                DefaultValue = -1.0f,
                ElementType = ElementType.Float
            },
        };

        public AppearanceDefinition()
        {
            RootElement = "APPEARANCE_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}