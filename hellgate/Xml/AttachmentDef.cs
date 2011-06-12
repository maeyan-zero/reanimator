using System;
using Revival.Common;

namespace Hellgate.Xml
{
    public class AttachmentDef
    {
        [XmlCookedAttribute(
            Name = "tAttachmentDef.eType",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int Type;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.dwFlags",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int Flags;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.pszAttached",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String Attached;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.nVolume",
            DefaultValue = 1,
            ElementType = ElementType.Int32)]
        public int Volume;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.nAttachedDefId",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public int AttachedDefId;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.pszBone",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String Bone;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.nBoneId",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public int BoneId;

        [XmlCookedAttribute(Name = "vPosition", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAttachmentDef.vPosition.fX",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAttachmentDef.vPosition.fY",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAttachmentDef.vPosition.fZ",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 Position = new Vector3();

        [XmlCookedAttribute(
            Name = "tAttachmentDef.fRotation",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Rotation;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.fYaw",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Yaw;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.fPitch",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Pitch;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.fRoll",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Roll;

        [XmlCookedAttribute(Name = "vNormal", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAttachmentDef.vNormal.fX",
            DefaultValue = -1.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAttachmentDef.vNormal.fY",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAttachmentDef.vNormal.fZ",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 Normal = new Vector3();

        [XmlCookedAttribute(
            Name = "tAttachmentDef.fScale",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float,
            IsResurrection = true)]
        public float Scale;
    }
}