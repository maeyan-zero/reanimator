using System;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "ANIM_EVENT")]
    public class AnimEvent
    {
        [XmlCookedAttribute(
            Name = "eType",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 Type;

        [XmlCookedAttribute(
            Name = "fTime",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Time;

        [XmlCookedAttribute(
            Name = "fRandChance",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float RandChance;

        [XmlCookedAttribute(
            Name = "fParam",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Param;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.eType",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 AttachmentType;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.dwFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.Int32,
            CustomType = ElementType.Unsigned)]
        public UInt32 AttachmentFlags;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.nVolume",
            DefaultValue = 1,
            ElementType = ElementType.Int32)]
        public Int32 AttachmentVolume;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.pszAttached",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String AttachmentAttached;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.nAttachedDefId",
            DefaultValue = -1,
            ElementType = ElementType.Int32_0x0A00)]
        public Int32 AttachmentAttachedId;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.pszBone",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String AttachmentBone;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.nBoneId",
            DefaultValue = -1,
            ElementType = ElementType.Int32_0x0A00)]
        public Int32 AttachmentBoneId;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.vPosition",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatArrayFixed,
            Count = 3,
            CustomType = ElementType.Vector3)]
        public Vector3 AttachmentPosition;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.vNormal",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatArrayFixed,
            Count = 3,
            CustomType = ElementType.Vector3)]
        public Vector3 AttachmentNormal;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.fScale",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float,
            IsResurrection = true)]
        public float AttachmentScale;

        [XmlCookedAttribute(
            Name = "tCondition",
            DefaultValue = null,
            ElementType = ElementType.Table,
            ChildType = typeof(ConditionDefinition))]
        public ConditionDefinition Condition;
    }
}