using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Reanimator
{
    class Model
    {
        /*
         * HEADER
         * int     type identifier
         * int     const 163 0xA3
         * int     const 1 0x01
         * int     no structs
         * FILE CONTENTS[no structs]
         * {
         * int      id
         * int      start offset
         * int      end offset
         * }
         * 
         **/
        Int32 type;
        Int32 minorVersion;
        Int32 majorVersion;
        Table[] fileStructure;

        List<Index> index;
        List<Table> indexMap;
        List<Geometry> geometry;

        public Model(BinaryReader binReader)
        {
            index = new List<Index>();
            indexMap = new List<Table>();
            geometry = new List<Geometry>();

            type = binReader.ReadInt32();
            minorVersion = binReader.ReadInt32();
            majorVersion = binReader.ReadInt32();

            fileStructure = new Table[binReader.ReadInt32()];
            FileTools.BinaryToArray<Table>(binReader, fileStructure);

            foreach (Table structure in fileStructure)
            {
                switch (structure.id)
                {
                    case (uint)Structure.UnitIndex:
                        index.Add(GetIndex(binReader));
                        break;
                    case (uint)Structure.Geometry:
                        geometry.Add(GetGeometry(binReader));
                        break;
                    case (uint)Structure.Reserved:
                        // This is a large, empty array.. probably reserved memory
                        // Actually, at the end of the array is a descriptive string of the item
                        // then 64 bytes of data
                        binReader.ReadBytes((int)structure.size);
                        break;
                    case (uint)Structure.Footer:
                        // contains the full path of the object
                        // followed by another ~64bytes of data
                        binReader.ReadBytes((int)structure.size);
                        break;
                }
            }
        }

        Index GetIndex(BinaryReader binReader)
        {
            Index index = new Index();
            index.unknown01 = binReader.ReadUInt32();
            index.unknown02 = binReader.ReadUInt32();
            index.unknown03 = binReader.ReadUInt32();
            index.unknown04 = binReader.ReadUInt32();
            binReader.ReadBytes(12); // nulls
            index.data = new UInt16[binReader.ReadUInt32() / sizeof(UInt16)];
            binReader.ReadBytes(12); // nulls
            for (int i = 0; i < index.data.Length; i++) index.data[i] = binReader.ReadUInt16();
            binReader.ReadBytes(8); // nulls
            index.unknown05 = binReader.ReadUInt16();
            index.unknown06 = binReader.ReadUInt16();
            index.unknown07 = binReader.ReadUInt16();
            binReader.ReadBytes(8); // nulls
            index.diffuse = GetString(binReader);
            index.normal = GetString(binReader);
            index.glow = GetString(binReader);
            index.specular = GetString(binReader);
            index.light = GetString(binReader);
            binReader.ReadBytes(38); // nulls
            // Another 8 nulls... this shouldnt be according to old code
            binReader.ReadBytes(8);
            return index;
        }

        Geometry GetGeometry(BinaryReader binReader)
        {
            Geometry geometry = new Geometry();
            geometry.unknown01 = binReader.ReadUInt32();
            geometry.unknown02 = binReader.ReadUInt32();
            geometry.detail = (Detail)binReader.ReadUInt32();
            binReader.ReadBytes(8); // nulls
            geometry.unknown04 = binReader.ReadUInt32();
            geometry.position = new Single[binReader.ReadUInt32() / sizeof(Single)];
            for (int i = 0; i < geometry.position.Length; i++) geometry.position[i] = binReader.ReadSingle();
            geometry.uv = new Single[binReader.ReadUInt32() / sizeof(Single)];
            for (int i = 0; i < geometry.uv.Length; i++) geometry.uv[i] = binReader.ReadSingle();
            if (geometry.detail != Detail.Simple) {
                geometry.normal = new Single[binReader.ReadUInt32() / sizeof(Single)];
                for (int i = 0; i < geometry.normal.Length; i++) geometry.normal[i] = binReader.ReadSingle();
            }
            return geometry;
        }

        String GetString(BinaryReader binReader)
        {
            byte[] byteString = new byte[binReader.ReadUInt32()];
            byteString = binReader.ReadBytes(byteString.Length);
            return byteString.ToString();
        }

        enum Type : uint
        {
            Unit = 0x1B0061E1,
            Background = 0xCAFE1515,
            Player = 0x43342ABF
        }

        enum Detail : uint
        {
            Simple = 0x03,
            Extended = 0x01,
            Full = 0x04
        }

        enum Zone : uint
        {
            Collision = 0x0026,
            Origin = 0x0020,
            Render = 0x0420
        }

        enum Structure : uint
        {
            UnitIndex = 0x02,
            PlayerIndex = 0x08,
            Geometry = 0x05,
            IndexMap = 0x09,
            Reserved = 0x07,
            Footer = 0x06
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct Table
        {
            public UInt32 id { get; set; }
            public UInt32 offset { get; set; }
            public UInt32 size { get; set; }
        }

        struct Index
        {
            public UInt32 unknown01 { get; set; }
            public UInt32 unknown02 { get; set; }
            public UInt32 unknown03 { get; set; }
            public UInt32 unknown04 { get; set; }
            public UInt16[] data { get; set; }
            public UInt16 unknown05 { get; set; }
            public UInt16 unknown06 { get; set; }
            public UInt16 unknown07 { get; set; }
            public String diffuse { get; set; }
            public String normal { get; set; }
            public String glow { get; set; }
            public String specular { get; set; }
            public String light { get; set; }
            public Single[] dimension { get; set; }
        }

        struct Geometry
        {
            public UInt32 unknown01 { get; set; }
            public UInt32 unknown02 { get; set; }
            public Detail detail { get; set; }
            public UInt32 unknown04 { get; set; }
            public Single[] uv { get; set; }
            public Single[] position { get; set; }
            public Single[] normal { get; set; }
        }
    }
}
