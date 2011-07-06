using System;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "INVENTORY_VIEW_INFO")]
    public class InventoryViewInfo
    {
        [XmlCookedAttribute(Name = "vCamFocus", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "vCamFocus.fX",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "vCamFocus.fY",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "vCamFocus.fZ",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 NeckAim = new Vector3();

        [XmlCookedAttribute(
            Name = "fCamRotation",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float CamRotation;

        [XmlCookedAttribute(
            Name = "fCamPitch",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float CamPitch;

        [XmlCookedAttribute(
            Name = "fCamDistance",
            DefaultValue = 3.0f,
            ElementType = ElementType.Float)]
        public float CamDistance;

        [XmlCookedAttribute(
            Name = "fCamFOV",
            DefaultValue = 1.047198f,
            ElementType = ElementType.Float)]
        public float CamFOV;

        [XmlCookedAttribute(
            Name = "pszEnvName",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String EnvName;

        [XmlCookedAttribute(
            Name = "pszBoneName",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String BoneName;

        [XmlCookedAttribute(
            Name = "nBone",
            DefaultValue = -1,
            ElementType = ElementType.Int32_0x0A00)]
        public Int32 Bone;
    }
}