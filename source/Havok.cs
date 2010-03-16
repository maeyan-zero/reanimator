using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Reanimator
{
    class Havok
    {
        FileHeader head;
        byte[] classDeclaration;
        byte[] environmentData;

        HavokClassManager manager;

        public Havok(BinaryReader binReader)
        {
            InitializeHavokClassManager();

            FileTools.BinaryToArray<FileHeader>(binReader, head);
            classDeclaration = binReader.ReadBytes(head.datalength);
            environmentData = binReader.ReadBytes(environmentLength);

        }

        public class HavokClassManager
        {
            List<HavokClass> _havokClass;

            public HavokClass HavokClassList
            {
                get
                {
                    return _havokClass;
                }
            }

            public void AddClass(string classid, uint token, Type type)
            {
                _havokClass.Add(new HavokClass(classid, token, type));
            }

            class HavokClass
            {
                string classid;
                uint token;
                Type type;

                public HavokClass(string classid, uint token, Type type)
                {
                    this.classid = classid;
                    this.token = token;
                    this.type = type;
                }
            }


        }

        void InitializeHavokClassManager()
        {
            manager = new HavokClassManager();

            manager.AddClass("hkClass", 0xEB9627A5, typeof(hkClass));
            manager.AddClass("hkClassMember", 0x4B28502E, typeof(hkClassMember));
            manager.AddClass("hkClassEnum", 0xA1179609, typeof(hkClassEnum));
            manager.AddClass("hkClassEnumItem", 0x6C8A6FCE, typeof(hkClassEnumItem));
            manager.AddClass("hkAnimationBinding", 0x39F89DE3, typeof(hkAnimationBinding));
            manager.AddClass("hkxVertexP4N4C1T2", 0x8AEB5D03, typeof(hkxVertexP4N4C1T2));
            manager.AddClass("hkxMaterial", 0x9C48433D, typeof(hkxMaterial));
            manager.AddClass("hkxIndexBuffer", 0x378C8A1C, typeof(hkxIndexBuffer));
            manager.AddClass("hkxTextureInplace", 0x4C134BF6, typeof(hkxTextureInplace));
            manager.AddClass("hkxTextureFile", 0x77EF1702, typeof(hkxTextureFile));
            manager.AddClass("hkxMeshSection", 0x6724D403, typeof(hkxMeshSection));
            manager.AddClass("hkInterleavedSkeletalAnimation", 0xFF541CC2, typeof(hkInterleavedSkeletalAnimation));
            manager.AddClass("hkSkeletalAnimation", 0x49C8AAB1, typeof(hkSkeletalAnimation));
            manager.AddClass("hkxVertexFormat", 0x94D19F37, typeof(hkxVertexFormat));
            manager.AddClass("hkxSkinBinding", 0x10C732C5, typeof(hkxSkinBinding));
            manager.AddClass("hkxScene", 0x36866F1C, typeof(hkxScene));
            manager.AddClass("hkMeshBindingMapping", 0xF4A6A64D, typeof(hkMeshBindingMapping));
            manager.AddClass("hkReferencedObject", 0x13111C3B, typeof(hkReferencedObject));
            manager.AddClass("hkxVertexP4N4T4B4W4I4Q4", 0xC675E385, typeof(hkxVertexP4N4T4B4W4I4Q4));
            manager.AddClass("hkxLight", 0x93A9928E, typeof(hkxLight));
            manager.AddClass("hkAnimationContainer", 0x6D6256F4, typeof(hkAnimationContainer));
            manager.AddClass("hkRootLevelContainer", 0x4EA398F5, typeof(hkRootLevelContainer));
            manager.AddClass("hkxAttribute", 0xC1A64D91, typeof(hkxAttribute));
            manager.AddClass("hkAnimatedReferenceFrame", 0xC1A64D91, typeof(hkAnimatedReferenceFrame));
            manager.AddClass("hkxNodeAnnotationData", 0x17951E52, typeof(hkxNodeAnnotationData));
            manager.AddClass("hkBaseObject", 0x008A70E0, typeof(hkBaseObject));
            manager.AddClass("hkMeshBinding", 0x9C31F988, typeof(hkMeshBinding));
            manager.AddClass("hkAnnotationTrack", 0x90C66F84, typeof(hkAnnotationTrack));
            manager.AddClass("hkxCamera", 0xAE5FC6D5, typeof(hkxCamera));
            manager.AddClass("hkxMaterialTextureStage", 0x9FBA85E0, typeof(hkxMaterialTextureStage));
            manager.AddClass("hkxMesh", 0xBD06CE6D, typeof(hkxMesh));
            manager.AddClass("hkBone", 0xF01140A7, typeof(hkBone));
            manager.AddClass("hkAnnotationTrackAnnotation", 0xCA881873, typeof(hkAnnotationTrackAnnotation));
            manager.AddClass("hkBoneAttachement", 0x9A3EDD8E, typeof(hkBoneAttachement));
            manager.AddClass("hkRootLevelContainerNamedVariant", 0x9C893A85, typeof(hkRootLevelContainerNamedVariant));
            manager.AddClass("hkSkeleton", 0x64615EA3, typeof(hkSkeleton));
            manager.AddClass("hkxNode", 0x9FC7620A, typeof(hkxNode));
            manager.AddClass("hkxAttributeGroup", 0x1CC06716, typeof(hkxAttributeGroup));
            manager.AddClass("hkxVertexBuffer", 0x54140657, typeof(hkxVertexBuffer));
        }

        class hkClass
        {
            hkClassMember classMember;
        }

        class hkClassMember
        {
            hkClassEnum classEnum;
        }

        class hkClassEnum
        {
            hkClassEnumItem classEnumItem;
        }

        class hkClassEnumItem
        {

        }

        class hkAnimationBinding
        {

        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class hkxVertexP4N4C1T2
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            float[] position;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            float[] normal;
            int diffuseA;
            float u;
            float v;
            int diffuseB;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class hkxVertexP4N4T4B4W4I4Q4
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            float[] position;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            float[] normal;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            float[] tangent;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            float[] binormal;
            uint weights;
            uint indices;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            short[] qu0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            short[] qu1;
        }

        class hkxMaterial
        {

        }

        class hkxIndexBuffer
        {

        }

        class hkxTextureInplace
        {

        }

        class hkxTextureFile
        {

        }

        class hkxMeshSection
        {

        }

        class hkInterleavedSkeletalAnimation
        {

        }

        class hkSkeletalAnimation
        {

        }

        class hkxVertexFormat
        {

        }

        class hkxSkinBinding
        {

        }

        class hkxScene
        {

        }

        class hkMeshBindingMapping
        {

        }

        class hkReferencedObject
        {

        }

        class hkxLight
        {

        }

        class hkAnimationContainer
        {

        }

        class hkRootLevelContainer
        {

        }

        class hkxAttribute
        {

        }

        class hkAnimatedReferenceFrame
        {

        }

        class hkxNodeAnnotationData
        {

        }

        class hkBaseObject
        {

        }

        class hkMeshBinding
        {

        }

        class hkAnnotationTrack
        {

        }

        class hkxCamera
        {

        }

        class hkxMaterialTextureStage
        {

        }

        class hkxMesh
        {

        }

        class hkBone
        {

        }

        class hkAnnotationTrackAnnotation
        {

        }

        class hkBoneAttachement
        {

        }

        class hkRootLevelContainerNamedVariant
        {

        }

        class hkSkeleton
        {

        }

        class hkxNode
        {

        }

        class hkxAttributeGroup
        {

        }

        class hkxVertexBuffer
        {

        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class FileHeader
        {
            int value01;
            int value02;
            int value03; // null
            int value04; // 4
            short value05;
            short value06;
            int value07;
            int value08;
            int value09;// null
            int value10;// null
            int value11;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            byte[] version;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            byte[] classnames;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            byte[] classnamesData;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            byte[] data;
            public int datalength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            byte[] dataData;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            byte[] types;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            byte[] typesData;
        }

        class hkaSkeleton
        {
            string name;
            int[] parentIndices;
            int[] bones;
            float[] referencePose;
            int floatSlots;
            int localFrames;
        }

        class hkaBone
        {
            string name;
            bool lockTranslation;
        }

        class hkaInterleavedUncompressedAnimation
        {

        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class hkVertexBuffer01
        {

        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class hkVertexBuffer02
        {

        }


        class hkIndex
        {
            IndexType indexType;
            short[] indices;
        }

        class hkMaterial
        {
            string name;
            int stages;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            float[] diffuseColor;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            float[] ambientColor;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            float[] speculatColor;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            float[] emissiveColor;
        }

        class hkaSkeleton
        {

        }
    }
}
