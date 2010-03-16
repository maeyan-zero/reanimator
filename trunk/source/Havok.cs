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
        
        public Havok(BinaryReader binReader)
        {

        }

        enum IndexType
        {
            INDEX_TYPE_TRI_LIST
        }

        class hkVertex
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
