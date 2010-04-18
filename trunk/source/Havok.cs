using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

// "is never used" warning - driving me insane
#pragma warning disable 169

namespace Reanimator
{
    class Havok
    {
        byte[] head;

        HavokClassManager manager;
        List<hkObject> content;

        public Havok(BinaryReader binReader)
        {
            content = new List<hkObject>();
            InitializeHavokClassManager();
            head = binReader.ReadBytes(208);
            
            int classnameslen = BitConverter.ToInt32(head, 132);
            try
            {
                while (binReader.BaseStream.Position < classnameslen)
                {
                    uint token = binReader.ReadUInt32();
                    string classname = GetClassName(binReader);

                    Type type = manager.GetClass(token);
                    if (type != null)
                    {
                        content.Add((hkObject)Activator.CreateInstance(type));
                        if (type == typeof(hkxVertexBuffer)) // dirty fix
                        {
                            binReader.BaseStream.Position = classnameslen;
                        }
                    }
                    else
                    {
                        throw new Exception(classname);
                    }
                }
                binReader.ReadByte();
            }
            catch(Exception e)
            {
                System.Console.WriteLine("Undefined class: " + e.ToString());
            }
        }

        public class HavokClassManager
        {
            List<HavokClass> _havokClass;

            public List<HavokClass> HavokClassList
            {
                get
                {
                    return _havokClass;
                }
            }

            public HavokClassManager()
            {
                _havokClass = new List<HavokClass>();
            }

            public void AddClass(string classid, uint token, Type type)
            {
                _havokClass.Add(new HavokClass(classid, token, type));
            }

            public Type GetClass(string classid)
            {
                foreach (HavokClass havokClass in _havokClass)
                {
                    if (havokClass.ClassID == classid)
                    {
                        return havokClass.Type;
                    }
                }
                return null;
            }

            public Type GetClass(uint token)
            {
                foreach (HavokClass havokClass in _havokClass)
                {
                    if (havokClass.Token == token)
                    {
                        return havokClass.Type;
                    }
                }
                return null;
            }

            public class HavokClass
            {
                string _classid;
                uint _token;
                Type _type;

                public string ClassID
                {
                    get
                    {
                        return _classid;
                    }
                }

                public uint Token
                {
                    get
                    {
                        return _token;
                    }
                }

                public Type Type
                {
                    get
                    {
                        return _type;
                    }
                }

                public HavokClass(string classid, uint token, Type type)
                {
                    this._classid = classid;
                    this._token = token;
                    this._type = type;
                }
            }
        }

        void InitializeHavokClassManager()
        {
            manager = new HavokClassManager();

            manager.AddClass("hkClass", 0xA52796EB, typeof(hkClass));
            manager.AddClass("hkClassMember", 0x2E50284B, typeof(hkClassMember));
            manager.AddClass("hkClassEnum", 0x9617A10C, typeof(hkClassEnum));
            manager.AddClass("hkClassEnumItem", 0xCE6F8A6C, typeof(hkClassEnumItem));
            manager.AddClass("hkAnimationBinding", 0xE39DF839, typeof(hkAnimationBinding));
            manager.AddClass("hkxVertexP4N4C1T2", 0x035DEB8A, typeof(hkxVertexP4N4C1T2));
            manager.AddClass("hkxMaterial", 0x3D43489C, typeof(hkxMaterial));
            manager.AddClass("hkxIndexBuffer", 0x1C8A8C37, typeof(hkxIndexBuffer));
            manager.AddClass("hkxTextureInplace", 0xF64B134C, typeof(hkxTextureInplace));
            manager.AddClass("hkxTextureFile", 0x0217EF77, typeof(hkxTextureFile));
            manager.AddClass("hkxMeshSection", 0x03D42467, typeof(hkxMeshSection));
            manager.AddClass("hkInterleavedSkeletalAnimation", 0xC21C54FF, typeof(hkInterleavedSkeletalAnimation));
            manager.AddClass("hkSkeletalAnimation", 0xB1AAC849, typeof(hkSkeletalAnimation));
            manager.AddClass("hkxVertexFormat", 0x379FD194, typeof(hkxVertexFormat));
            manager.AddClass("hkxSkinBinding", 0xC532C710, typeof(hkxSkinBinding));
            manager.AddClass("hkxScene", 0x1C6F8636, typeof(hkxScene));
            manager.AddClass("hkMeshBindingMapping", 0x4DA6A6F4, typeof(hkMeshBindingMapping));
            manager.AddClass("hkReferencedObject", 0x3B1C1113, typeof(hkReferencedObject));
            manager.AddClass("hkxVertexP4N4T4B4W4I4Q4", 0x85E375C6, typeof(hkxVertexP4N4T4B4W4I4Q4));
            manager.AddClass("hkxLight", 0x8E92A993, typeof(hkxLight));
            manager.AddClass("hkAnimationContainer", 0xF456626D, typeof(hkAnimationContainer));
            manager.AddClass("hkRootLevelContainer", 0xF598A34E, typeof(hkRootLevelContainer));
            manager.AddClass("hkxAttribute", 0x914DA6C1, typeof(hkxAttribute));
            manager.AddClass("hkAnimatedReferenceFrame", 0xDA8C7D7D, typeof(hkAnimatedReferenceFrame));
            manager.AddClass("hkxNodeAnnotationData", 0x521E9517, typeof(hkxNodeAnnotationData));
            manager.AddClass("hkBaseObject", 0xE0708A00, typeof(hkBaseObject));
            manager.AddClass("hkMeshBinding", 0x88F9319C, typeof(hkMeshBinding));
            manager.AddClass("hkAnnotationTrack", 0x846FC690, typeof(hkAnnotationTrack));
            manager.AddClass("hkxCamera", 0xD5C65FAE, typeof(hkxCamera));
            manager.AddClass("hkxMaterialTextureStage", 0xE085BA9F, typeof(hkxMaterialTextureStage));
            manager.AddClass("hkxMesh", 0x6DCE06BD, typeof(hkxMesh));
            manager.AddClass("hkBone", 0xA74011F0, typeof(hkBone));
            manager.AddClass("hkAnnotationTrackAnnotation", 0x731888CA, typeof(hkAnnotationTrackAnnotation));
            manager.AddClass("hkBoneAttachement", 0x8BDD3E9A, typeof(hkBoneAttachement));
            manager.AddClass("hkRootLevelContainerNamedVariant", 0x853A899C, typeof(hkRootLevelContainerNamedVariant));
            manager.AddClass("hkSkeleton", 0xA35E6164, typeof(hkSkeleton));
            manager.AddClass("hkxNode", 0x0A62C79F, typeof(hkxNode));
            manager.AddClass("hkxAttributeGroup", 0x1667C01C, typeof(hkxAttributeGroup));
            manager.AddClass("hkxVertexBuffer", 0x57061454, typeof(hkxVertexBuffer));
            manager.AddClass("hkxAnimatedFloat", 0x9BB15AF4, typeof(hkxAnimatedFloat));
        }

        string GetClassName(BinaryReader binReader)
        {
            char[] classname = new char[64];
            binReader.ReadByte(); // 0x09
            for (int i = 0; ; i++)
            {
                classname[i] = (char)binReader.ReadByte();
                if (classname[i] == 0)
                {
                    break;
                }
            }
            return FileTools.ArrayToStringGeneric<char>(classname, ""); ;
        }

        class hkObject
        {

        }

        class hkClass : hkObject
        {
            hkClassMember classMember;
        }

        class hkClassMember : hkObject
        {
            hkClassEnum classEnum;
        }

        class hkClassEnum : hkObject
        {
            hkClassEnumItem classEnumItem;
        }

        class hkClassEnumItem : hkObject
        {

        }

        class hkAnimationBinding : hkObject
        {
            hkReferencedObject originalSkeletonName;
            int animation;
            int[] transformTrackToBoneIndices;
            int[] floatTrackToFloatSlotIndices;
            string blendHint;
        }

        //[StructLayout(LayoutKind.Sequential, Pack = 1)]
        class hkxVertexP4N4C1T2 : hkObject
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

        //[StructLayout(LayoutKind.Sequential, Pack = 1)]
        class hkxVertexP4N4T4B4W4I4Q4 : hkObject
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

        class hkxMaterial : hkObject
        {

        }

        class hkxIndexBuffer : hkObject
        {
            string indexType;
            short[] indices16;
            int[] indices32;
            int vertexBaseOffset;
            int length;
        }

        class hkxTextureInplace : hkObject
        {

        }

        class hkxTextureFile : hkObject
        {
            string filename;
            hkReferencedObject name;
            hkReferencedObject originalFilename;
        }

        class hkxMeshSection : hkObject
        {

        }

        class hkInterleavedSkeletalAnimation : hkObject
        {

        }

        class hkSkeletalAnimation : hkObject
        {

        }

        class hkxVertexFormat : hkObject
        {

        }

        class hkxSkinBinding : hkObject
        {

        }

        class hkxScene : hkObject
        {

        }

        class hkMeshBindingMapping : hkObject
        {

        }

        class hkReferencedObject : hkObject
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            int[] texture;
            string usageHint;
            int tcoordChannel;
        }

        class hkxLight : hkObject
        {

        }

        class hkAnimationContainer : hkObject
        {

        }

        class hkRootLevelContainer : hkObject
        {

        }

        class hkxAttribute : hkObject
        {

        }

        class hkAnimatedReferenceFrame : hkObject
        {

        }

        class hkxNodeAnnotationData : hkObject
        {

        }

        class hkBaseObject : hkObject
        {

        }

        class hkMeshBinding : hkObject
        {

        }

        class hkAnnotationTrack : hkObject
        {

        }

        class hkxCamera : hkObject
        {

        }

        class hkxMaterialTextureStage : hkObject
        {

        }

        class hkxMesh : hkObject
        {

        }

        class hkBone : hkObject
        {

        }

        class hkAnnotationTrackAnnotation : hkObject
        {

        }

        class hkBoneAttachement : hkObject
        {

        }

        class hkRootLevelContainerNamedVariant : hkObject
        {

        }

        class hkSkeleton : hkObject
        {

        }

        class hkxNode : hkObject
        {

        }

        class hkxAttributeGroup : hkObject
        {

        }

        class hkxVertexBuffer : hkObject
        {

        }

        class hkxAnimatedFloat : hkObject
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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte[] classnames;
            int value12;
            int classnameLength;

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
    }
}

#pragma warning restore 169