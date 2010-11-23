using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Revival.Common;

namespace Reanimator
{
    public class Model
    {
        Int32 type;
        Int32 minorVersion;
        Int32 majorVersion;
        Table[] fileStructure;

        List<Index> _index;
        List<Table> _indexMap;
        List<Geometry> _geometry;
#pragma warning disable 169
        Reserved _reserved;
#pragma warning restore 169
        Footer _footer;

        public string Id { get; private set; }

        public List<Index> index
        {
            get
            {
                return _index;
            }
        }

        public List<Geometry> geometry
        {
            get
            {
                return _geometry;
            }
        }

        public Model(BinaryReader binReader)
        {
            Id = "model";
            _index = new List<Index>();
            _indexMap = new List<Table>();
            _geometry = new List<Geometry>();

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
                        _index.Add(GetIndex(binReader));
                        break;
                    case (uint)Structure.Geometry:
                        _geometry.Add(GetGeometry(binReader));
                        break;
                    case (uint)Structure.Reserved:// skip this for now
                        binReader.ReadBytes((int)structure.size); 
                        //reserved = GetReserved(binReader);
                        break;
                    case (uint)Structure.Footer:
                        _footer = GetFooter(binReader);
                        break;
                }
            }
            binReader.ReadBytes(8); // this should bring to EOF
            binReader.Close();
        }

        // 1ST TIER FUNCTIONS

        Index GetIndex(BinaryReader binReader)
        {
            Index getIndex = new Index
            {
                zone = (Zone) binReader.ReadUInt32(),
                unknown02 = binReader.ReadUInt32(),
                unknown03 = binReader.ReadUInt32(),
                unknown04 = binReader.ReadUInt32()
            };
            binReader.ReadBytes(12); // nulls
            getIndex.triangle = GetTriangles(binReader);
            binReader.ReadBytes(8); // nulls
            getIndex.triangles = binReader.ReadUInt16();
            getIndex.positions = binReader.ReadUInt16();
            getIndex.unknown05 = binReader.ReadUInt16();
            binReader.ReadBytes(8); // nulls
            getIndex.diffuse = GetString(binReader);
            getIndex.normal = GetString(binReader);
            getIndex.glow = GetString(binReader);
            getIndex.specular = GetString(binReader);
            getIndex.light = GetString(binReader);
            binReader.ReadBytes(38); // nulls
            binReader.ReadBytes(8);// Another 8 nulls... this shouldnt be according to old code

            return getIndex;
        }

        Geometry GetGeometry(BinaryReader binReader)
        {
            Geometry getGeometry = new Geometry();
            getGeometry.unknown01 = binReader.ReadUInt32();
            getGeometry.coordinates = binReader.ReadUInt32();
            getGeometry.detail = (Detail)binReader.ReadUInt32();
            binReader.ReadBytes(8); // nulls
            getGeometry.unknown04 = binReader.ReadUInt32();
            getGeometry.position = GetCoordinates(binReader, getGeometry.detail);
            getGeometry.uv = GetUVs(binReader);
            if (getGeometry.detail != Detail.Simple) getGeometry.normal = GetCoordinates(binReader, getGeometry.detail);
            return getGeometry;
        }

        Reserved GetReserved(BinaryReader binReader)
        {
            Reserved reserved = new Reserved();
            binReader.ReadInt32(); //null
            binReader.ReadInt32(); // 01
            while (binReader.ReadInt32() != 0x01) { } // Read until the marker has been reached
            binReader.ReadInt32(); //null
            reserved.title = GetString(binReader);
            binReader.ReadInt32(); //null
            reserved.unknown = binReader.ReadBytes(64);
            return reserved;
        }

        Footer GetFooter(BinaryReader binReader)
        {
            Footer footer = new Footer();
            binReader.ReadInt32(); //null
            footer.path = GetString(binReader);
            footer.unknown = binReader.ReadBytes(64);
            return footer;
        }

        //
        // 2ND TIER FUNCTIONS
        //

        String GetString(BinaryReader binReader)
        {
            byte[] byteString = new byte[binReader.ReadUInt32()];
            byteString = binReader.ReadBytes(byteString.Length);
            return byteString.ToString();
        }

        Coordinate[] GetCoordinates(BinaryReader binReader, Detail detail)
        {
            UInt32 size;
            Coordinate[] coordinate;
            switch (detail)
            {
                case Detail.Simple:
                case Detail.Extended:
                    size = binReader.ReadUInt32() / (uint)Marshal.SizeOf(typeof(Coordinate));
                    coordinate = new Coordinate[size];
                    FileTools.BinaryToArray<Coordinate>(binReader, coordinate);
                    return coordinate;
                case Detail.Full:
                    size = binReader.ReadUInt32() / (uint)Marshal.SizeOf(typeof(Extended));
                    coordinate = new Extended[size];
                    FileTools.BinaryToArray<Extended>(binReader, (Extended[])coordinate);
                    return coordinate;
                default:
                    return null;
            }
        }

        Triangle[] GetTriangles(BinaryReader binReader)
        {
            UInt32 size = binReader.ReadUInt32() / (uint)Marshal.SizeOf(typeof(Triangle));
            Triangle[] triangle = new Triangle[size];
            binReader.ReadBytes(12); // nulls
            FileTools.BinaryToArray<Triangle>(binReader, triangle);
            return triangle;
        }

        UV[] GetUVs(BinaryReader binReader)
        {
            UInt32 size = binReader.ReadUInt32() / (uint)Marshal.SizeOf(typeof(UV));
            UV[] uv = new UV[size];
            FileTools.BinaryToArray<UV>(binReader, uv);
            return uv;
        }

        //
        // PRIMARY STRUCTURES:
        //

        public class Index
        {
            public Zone zone { get; set; }
            public UInt32 unknown02 { get; set; }
            public UInt32 unknown03 { get; set; }
            public UInt32 unknown04 { get; set; }
            public Triangle[] triangle { get; set; }
            public UInt16 positions { get; set; }
            public UInt16 triangles { get; set; }
            public UInt16 unknown05 { get; set; }
            public String diffuse { get; set; }
            public String normal { get; set; }
            public String glow { get; set; }
            public String specular { get; set; }
            public String light { get; set; }
            public Coordinate[] dimension { get; set; }
        }

        public class Geometry
        {
            public UInt32 unknown01 { get; set; }
            public UInt32 coordinates { get; set; }
            public Detail detail { get; set; }
            public UInt32 unknown04 { get; set; } // is this shape? 
            public UV[] uv { get; set; }
            public Coordinate[] position { get; set; }
            public Coordinate[] normal { get; set; }
        }

        class Reserved
        {
            public String title { get; set; }
            public byte[] unknown { get; set; }
        }

        class Footer
        {
            public String path { get; set; }
            public byte[] unknown { get; set; }
        }

        //
        // ENUMERATIONS:
        //

        public enum Type : uint
        {
            Unit = 0x1B0061E1,
            Background = 0xCAFE1515,
            Player = 0x43342ABF
        }

        public enum Detail : uint
        {
            Simple = 0x03,
            Extended = 0x01,
            Full = 0x04
        }

        public enum Zone : uint
        {
            Collision = 0x0026,
            Origin = 0x0020,
            Render = 0x0420
        }

        public enum Structure : uint
        {
            UnitIndex = 0x02,
            PlayerIndex = 0x08,
            Geometry = 0x05,
            IndexMap = 0x09,
            Reserved = 0x07,
            Footer = 0x06
        }

        public enum Shape : uint
        {
            Triangle = 0x03,
            Square = 0x04
        }

        //
        // INTERNAL STRUCTURES:
        //

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class Table
        {
            public UInt32 id { get; set; }
            public UInt32 offset { get; set; }
            public UInt32 size { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class Triangle
        {
            public UInt16 coordinate01 { get; set; }
            public UInt16 coordinate02 { get; set; }
            public UInt16 coordinate03 { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class Coordinate
        {
            public Single positionX { get; set; }
            public Single positionY { get; set; }
            public Single positionZ { get; set; }

            new public String ToString()
            {
                return positionX.ToString() + " " + positionY.ToString() + " " + positionZ.ToString();
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class Extended : Coordinate
        {
            public Single tanX { get; set; }
            public Single tanY { get; set; }
            public Single tanZ { get; set; }
            public UInt32 rgb { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class UV
        {
            public Single s { get; set; }
            public Single t { get; set; }

            new public string ToString()
            {
                return s.ToString() + " " + t.ToString();
            }
        }
    }
}
