using System;
using Hellgate.Excel;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "ANIMATION_DEFINITION")]
    public class AnimationDefinition
    {
        [XmlCookedAttribute(
            Name = "dwFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.Int32,
            CustomType = ElementType.Unsigned)]
        public UInt32 Flags;

        [XmlCookedAttribute(
            Name = "nUnitMode",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.UNITMODES)] // 26160 UNITMODES
        public UnitModes UnitMode;
        public int UnitModeIndex;

        [XmlCookedAttribute(
            Name = "nGroup",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.ANIMATION_GROUP)] // 26928 ANIMATION_GROUP
        public AnimationGroups Group;
        public int GroupIndex;

        [XmlCookedAttribute(
            Name = "pszFile",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String File;

        [XmlCookedAttribute(
            Name = "nFileIndex",
            DefaultValue = -1,
            ElementType = ElementType.Int32)]
        public Int32 FileIndex;

        [XmlCookedAttribute(
            Name = "fDuration",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float Duration;

        [XmlCookedAttribute(
            Name = "fVelocity",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float Velocity;

        [XmlCookedAttribute(
            Name = "fStartOffset",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float StartOffset;

        [XmlCookedAttribute(
            Name = "fTurnSpeed",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float TurnSpeed;

        [XmlCookedAttribute(
            Name = "fEaseIn",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float EaseIn;

        [XmlCookedAttribute(
            Name = "fEaseOut",
            DefaultValue = 0.5f,
            ElementType = ElementType.Float)]
        public float EaseOut;

        [XmlCookedAttribute(
            Name = "fStanceFadeTimePercent",
            DefaultValue = 0.75f,
            ElementType = ElementType.Float)]
        public float StanceFadeTimePercent;

        [XmlCookedAttribute(
            Name = "nBoneWeights",
            DefaultValue = -1,
            ElementType = ElementType.Int32)]
        public Int32 BoneWeights;

        [XmlCookedAttribute(
            Name = "nWeight",
            DefaultValue = 1,
            ElementType = ElementType.Int32)]
        public Int32 Weight;

        [XmlCookedAttribute(
            Name = "nStartStance",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.ANIMATION_STANCE)] // 26672 ANIMATION_STANCE
        public AnimationStance StartStance1;
        public int StartStance1Index;

        [XmlCookedAttribute(
            Name = "nStartStance2",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.ANIMATION_STANCE)] // 26672 ANIMATION_STANCE
        public AnimationStance StartStance2;
        public int StartStance2Index;

        [XmlCookedAttribute(
            Name = "nStartStance3",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.ANIMATION_STANCE)] // 26672 ANIMATION_STANCE
        public AnimationStance StartStance3;
        public int StartStance3Index;

        [XmlCookedAttribute(
            Name = "nEndStance",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.ANIMATION_STANCE)] // 26672 ANIMATION_STANCE
        public AnimationStance EndStance;
        public int EndStanceIndex;

        [XmlCookedAttribute(
            Name = "nPriorityBoost",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 PriorityBoost;

        [XmlCookedAttribute(
            Name = "tRagdollBlend",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] RagdollBlends;

        [XmlCookedAttribute(
            Name = "tRagdollPower",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] RagdollPowers;

        [XmlCookedAttribute(
            Name = "tSelfIllumation",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] SelfIllumations;

        [XmlCookedAttribute(
            Name = "tSelfIllumationBlend",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] SelfIllumationBlends;

        [XmlCookedAttribute(
            Name = "pBinding",
            DefaultValue = null,
            ElementType = ElementType.Pointer)]
        public Object Binding;

        [XmlCookedAttribute(
            Name = "pGrannyAnimation",
            DefaultValue = null,
            ElementType = ElementType.Pointer)]
        public Object GrannyAnimation;

        [XmlCookedAttribute(
            Name = "pGrannyFile",
            DefaultValue = null,
            ElementType = ElementType.Pointer)]
        public Object GrannyFile;

        [XmlCookedAttribute(
            Name = "pEvents",
            DefaultValue = 0,
            ElementType = ElementType.TableArrayVariable,
            ChildType = typeof (AnimEvent))]
        public AnimEvent[] Events;

        [XmlCookedAttribute(
            Name = "nPreviewMode",
            DefaultValue = -1,
            ElementType = ElementType.Int32_0x0A00)]
        public Int32 PreviewMode;

        [XmlCookedAttribute(
            Name = "pNextInGroup",
            DefaultValue = null,
            ElementType = ElementType.Pointer)]
        public AnimationDefinition NextInGroup; // guess that this is the "next AnimationDefinition in group"

        [XmlCookedAttribute(
            Name = "nAnimationCondition",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndexArrayFixed,
            TableCode = Xls.TableCodes.ANIMATION_CONDITION, // 26416 ANIMATION_CONDITION
            Count = 4)]
        public AnimationCondition AnimationCondition;
        public int AnimationConditionIndex;

        [XmlCookedAttribute(
            Name = "tCondition",
            DefaultValue = null,
            ElementType = ElementType.Table,
            ChildType = typeof (ConditionDefinition))]
        public ConditionDefinition Condition;
    }
}