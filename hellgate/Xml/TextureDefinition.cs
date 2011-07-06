using System;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "TEXTURE_DEFINITION")]
    public class TextureDefinition
    {
        [XmlCookedAttribute(
            Name = "nFormat",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int Format;

        [XmlCookedAttribute(
            Name = "tBinding",
            DefaultValue = 0,
            ElementType = ElementType.NonCookedInt32)]
        public int Binding;

        [XmlCookedAttribute(
            Name = "nFileSize",
            DefaultValue = 0,
            ElementType = ElementType.NonCookedInt32)]
        public int FileSize;

        [XmlCookedAttribute(
            Name = "nMipMapLevels",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int MipMapLevels;

        [XmlCookedAttribute(
            Name = "nArraySize",
            DefaultValue = 1,
            ElementType = ElementType.Int32)]
        public int ArraySize;

        [XmlCookedAttribute(
            Name = "nMipMapUsed",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int MipMapUsed;

        [XmlCookedAttribute(
            Name = "pszMaterialName",
            DefaultValue = "Default",
            ElementType = ElementType.String)]
        public String MaterialName;

        [XmlCookedAttribute(
            Name = "nMeshLODPriority",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int MeshLODPriority;

        [XmlCookedAttribute(
            Name = "nBlendWidth",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int BlendWidth;

        [XmlCookedAttribute(
            Name = "nBlendHeight",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int BlendHeight;

        [XmlCookedAttribute(
            Name = "pBlendRLE",
            DefaultValue = 0,
            ElementType = ElementType.TableArrayVariable,
            ChildType = typeof(BlendRLE))]
        public BlendRLE[] BlendRLE;

        [XmlCookedAttribute(
            Name = "nFramesU",
            DefaultValue = 1,
            ElementType = ElementType.Int32)]
        public int FramesU;

        [XmlCookedAttribute(
            Name = "nFramesV",
            DefaultValue = 1,
            ElementType = ElementType.Int32)]
        public int FramesV;

        [XmlCookedAttribute(
            Name = "nWidth",
            DefaultValue = 0,
            ElementType = ElementType.NonCookedInt32)]
        public int Width;

        [XmlCookedAttribute(
            Name = "nHeight",
            DefaultValue = 0,
            ElementType = ElementType.NonCookedInt32)]
        public int Height;

        [XmlCookedAttribute(
            Name = "dwConvertFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.Int32,
            CustomType = ElementType.Unsigned)]
        public uint ConvertFlags;

        [XmlCookedAttribute(
            Name = "fBlurFactor",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float BlurFactor;

        [XmlCookedAttribute(
            Name = "nSharpenFilter",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int SharpenFilter;

        [XmlCookedAttribute(
            Name = "nSharpenPasses",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int SharpenPasses;

        [XmlCookedAttribute(
            Name = "nMIPFilter",
            DefaultValue = 6,
            ElementType = ElementType.Int32)]
        public int MIPFilter;
    }
}