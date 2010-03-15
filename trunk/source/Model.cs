using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace Reanimator
{
    class Model
    {
        String modelID;

        Int32 type;
        Int32 minorVersion;
        Int32 majorVersion;
        Table[] fileStructure;

        List<Index> index;
        List<Table> indexMap;
        List<Geometry> geometry;
        Reserved reserved;
        Footer footer;

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
                    case (uint)Structure.Reserved:// skip this for now
                        binReader.ReadBytes((int)structure.size); 
                        //reserved = GetReserved(binReader);
                        break;
                    case (uint)Structure.Footer:
                        footer = GetFooter(binReader);
                        break;
                }
            }
            binReader.ReadBytes(8); // this should bring to EOF
            binReader.Close();
        }

        // 1ST TIER FUNCTIONS

        Index GetIndex(BinaryReader binReader)
        {
            Index index = new Index();
            index.zone = (Zone)binReader.ReadUInt32();
            index.unknown02 = binReader.ReadUInt32();
            index.unknown03 = binReader.ReadUInt32();
            index.unknown04 = binReader.ReadUInt32();
            binReader.ReadBytes(12); // nulls
            index.triangle = GetTriangles(binReader);
            binReader.ReadBytes(8); // nulls
            index.triangles = binReader.ReadUInt16();
            index.positions = binReader.ReadUInt16();
            index.unknown05 = binReader.ReadUInt16();
            binReader.ReadBytes(8); // nulls
            index.diffuse = GetString(binReader);
            index.normal = GetString(binReader);
            index.glow = GetString(binReader);
            index.specular = GetString(binReader);
            index.light = GetString(binReader);
            binReader.ReadBytes(38); // nulls
            binReader.ReadBytes(8);// Another 8 nulls... this shouldnt be according to old code
            return index;
        }

        Geometry GetGeometry(BinaryReader binReader)
        {
            Geometry geometry = new Geometry();
            geometry.unknown01 = binReader.ReadUInt32();
            geometry.coordinates = binReader.ReadUInt32();
            geometry.detail = (Detail)binReader.ReadUInt32();
            binReader.ReadBytes(8); // nulls
            geometry.unknown04 = binReader.ReadUInt32();
            geometry.position = GetCoordinates(binReader, geometry.detail);
            geometry.uv = GetUVs(binReader);
            if (geometry.detail != Detail.Simple) geometry.normal = GetCoordinates(binReader, geometry.detail);
            return geometry;
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
        // MISC HELPER FUNCTIONS:
        //

        public Single[] MergePositions()
        {
            int length = 0;
            foreach (Geometry g in geometry)
            {
                length += g.position.Length * 3;
            }

            Single[] positions = new Single[length];
            length = 0;
            foreach (Geometry g in geometry)
            {
                for (int i = 0; i < g.position.Length; i++)
                {
                    positions[length++] = g.position[i].positionX;
                    positions[length++] = g.position[i].positionY;
                    positions[length++] = g.position[i].positionZ;
                }
            }

            return positions;
        }

        public UInt16[] GetTriangleArray(int idx)
        {
            int length = 0;
            UInt16[] triangles = new UInt16[index[idx].triangles * 3];
            
            for (int i = 0; i < triangles.Length / 3; i++)
            {
                triangles[length++] = index[idx].triangle[i].coordinate01;
                triangles[length++] = index[idx].triangle[i].coordinate02;
                triangles[length++] = index[idx].triangle[i].coordinate03;
            }

            return triangles;
        }

        public Single[] GetUVArray(int idx)
        {
            int length = 0;
            Single[] uv = new Single[geometry[idx].uv.Length * 2];

            for (int i = 0; i < uv.Length / 2; i++)
            {
                uv[length++] = geometry[idx].uv[i].x;
                uv[length++] = geometry[idx].uv[i].y;
            }

            return uv;
        }

        //
        // EXPORT COLLADA:
        //

        public void Export()
        {
            XmlWriterSettings wSettings = new XmlWriterSettings();
            wSettings.Indent = true;
            MemoryStream ms = new MemoryStream();
            XmlWriter xw = XmlWriter.Create(ms, wSettings);// Write Declaration

            //debug
            modelID = "model_name_here";
            DateTime now = DateTime.Now;

            xw.WriteStartDocument();
            xw.WriteStartElement("COLLADA");
            xw.WriteAttributeString("version", "1.4.1");
            //xw.WriteAttributeString("xmlns", "http://www.collada.org/2005/11/COLLADASchema");
            xw.WriteStartElement("asset");
            xw.WriteStartElement("contributor");
            xw.WriteStartElement("author");
            xw.WriteString("Ripped by Maeyan");
            xw.WriteEndElement();
            xw.WriteStartElement("authoring_tool");
            xw.WriteString("3ds Max 9");
            xw.WriteEndElement();
            xw.WriteStartElement("copyright");
            xw.WriteString("Flagship Studios");
            xw.WriteEndElement();
            xw.WriteEndElement();// End Contributor
            xw.WriteStartElement("created");
            xw.WriteValue(now - new TimeSpan(0, 0, 0));
            xw.WriteEndElement();
            xw.WriteStartElement("modified");
            xw.WriteValue(now - new TimeSpan(0, 0, 0));
            xw.WriteEndElement();
            xw.WriteStartElement("unit");
            xw.WriteAttributeString("meter", "0.01");
            xw.WriteAttributeString("name", "centimeter");
            xw.WriteEndElement();
            xw.WriteStartElement("up_axis");
            xw.WriteString("Y_UP");
            xw.WriteEndElement();
            xw.WriteEndElement();// End asset

            //
            // MATERIAL
            //

            xw.WriteStartElement("library_materials");
            xw.WriteStartElement("material");
            xw.WriteAttributeString("id", "default");
            xw.WriteStartElement("instance_effect");
            xw.WriteAttributeString("url", "#default-fx");
            xw.WriteEndElement();
            xw.WriteEndElement();
            xw.WriteEndElement();

            //
            // EFFECT
            //

            xw.WriteStartElement("library_effects");
            xw.WriteStartElement("effect");
            xw.WriteAttributeString("id", "default-fx");
            xw.WriteStartElement("profile_COMMON");
            xw.WriteStartElement("technique");
            xw.WriteAttributeString("sid", "common");
            xw.WriteStartElement("phong");
            xw.WriteStartElement("diffuse");
            xw.WriteStartElement("color");
            xw.WriteString("0 0 0 1");
            xw.WriteEndElement(); // color
            xw.WriteEndElement(); // diffuse
            xw.WriteEndElement(); // phong
            xw.WriteEndElement(); // Technique
            xw.WriteEndElement(); // profile_COMMON
            xw.WriteEndElement(); // Effect
            xw.WriteEndElement(); // Library Effects

            xw.WriteStartElement("library_geometries");
            xw.WriteAttributeString("id", modelID);
            xw.WriteStartElement("geometry");
            xw.WriteStartElement("mesh");
            
            //
            // POSITIONS
            //

            Single[] positions = MergePositions();

            xw.WriteStartElement("source");
            xw.WriteAttributeString("id", modelID + "-positions");
            xw.WriteStartElement("float_array");
            xw.WriteAttributeString("id", modelID + "-positions-array");
            xw.WriteAttributeString("count", positions.Length.ToString());
            xw.WriteString(FileTools.ArrayToStringGeneric<Single>(positions, " "));
            xw.WriteEndElement();
            xw.WriteStartElement("technique_common");
            xw.WriteStartElement("accessor");
            xw.WriteAttributeString("stride", "3");
            xw.WriteAttributeString("source", "#" + modelID + "-positions-array");
            xw.WriteAttributeString("count", (positions.Length / 3).ToString());
            xw.WriteStartElement("param");
            xw.WriteAttributeString("name", "X");
            xw.WriteAttributeString("type", "float");
            xw.WriteEndElement();
            xw.WriteStartElement("param");
            xw.WriteAttributeString("name", "Y");
            xw.WriteAttributeString("type", "float");
            xw.WriteEndElement();
            xw.WriteStartElement("param");
            xw.WriteAttributeString("name", "Z");
            xw.WriteAttributeString("type", "float");
            xw.WriteEndElement();
            xw.WriteEndElement();// End Accessor
            xw.WriteEndElement();// End technique common
            xw.WriteEndElement();// End source

            //
            // UV
            //

            for (int i = 0; i < geometry.Count; i++)
            {
                Single[] uvArray = GetUVArray(i);

                //if (index[i].zone == Zone.Render)
                {
                    xw.WriteStartElement("source");
                    xw.WriteAttributeString("id", modelID + "-uv-" + i.ToString());
                    xw.WriteStartElement("float_array");
                    xw.WriteAttributeString("id", modelID + "-uv-array");
                    xw.WriteAttributeString("count", uvArray.Length.ToString());
                    xw.WriteString(FileTools.ArrayToStringGeneric<Single>(uvArray, " "));
                    xw.WriteEndElement();
                    xw.WriteStartElement("technique_common");
                    xw.WriteStartElement("accessor");
                    xw.WriteAttributeString("stride", "3");
                    xw.WriteAttributeString("source", "#" + modelID + "-uv-array");
                    xw.WriteAttributeString("count", (uvArray.Length / 2).ToString());
                    xw.WriteStartElement("param");
                    xw.WriteAttributeString("name", "S");
                    xw.WriteAttributeString("type", "float");
                    xw.WriteEndElement();
                    xw.WriteStartElement("param");
                    xw.WriteAttributeString("name", "T");
                    xw.WriteAttributeString("type", "float");
                    xw.WriteEndElement();
                    xw.WriteEndElement();// End Accessor
                    xw.WriteEndElement();// End technique common
                    xw.WriteEndElement();// End source
                }
            }

            xw.WriteStartElement("vertices");
            xw.WriteAttributeString("id", modelID + "-vertices");
            xw.WriteStartElement("input");
            xw.WriteAttributeString("semantic", "POSITION");
            xw.WriteAttributeString("source", "#" + modelID + "-positions");
            xw.WriteEndElement();
            xw.WriteEndElement();

            //
            // INDEX:
            //

            for (int i = 0; i < index.Count; i++)
            {
                UInt16[] triangleArray = GetTriangleArray(i);

                xw.WriteStartElement("triangles");
                xw.WriteAttributeString("count", (triangleArray.Length / 3).ToString());
                xw.WriteAttributeString("material", "defaultSG");
                xw.WriteStartElement("input");
                xw.WriteAttributeString("offset", "0");
                xw.WriteAttributeString("semantic", "VERTEX");
                xw.WriteAttributeString("source", "#" + modelID + "-vertices");
                xw.WriteEndElement();
                xw.WriteAttributeString("semantic", "TEXCOORD");
                xw.WriteAttributeString("source", "#" + modelID + "-uv-" + i.ToString());
                xw.WriteEndElement();
                xw.WriteStartElement("p");
                xw.WriteString(FileTools.ArrayToStringGeneric<UInt16>(triangleArray, " "));
                xw.WriteEndElement();
                xw.WriteEndElement();
            }

            xw.WriteEndElement();// End mesh
            xw.WriteEndElement();// End geometry
            xw.WriteEndElement();// End library_geometries

            //
            // VISUAL SCENE:
            //

            xw.WriteStartElement("library_visual_scenes");
            xw.WriteStartElement("visual_scene");
            xw.WriteAttributeString("id", "scene");
            xw.WriteStartElement("node");
            xw.WriteAttributeString("layer", "L1");
            xw.WriteAttributeString("id", modelID + "-geometry");
            xw.WriteStartElement("translate");
            xw.WriteString("0 0 0");
            xw.WriteEndElement();
            xw.WriteStartElement("rotate");
            xw.WriteAttributeString("sid", "rotateZ");
            xw.WriteString("0 0 1 0");
            xw.WriteEndElement();
            xw.WriteStartElement("rotate");
            xw.WriteAttributeString("sid", "rotateY");
            xw.WriteString("0 1 0 0");
            xw.WriteEndElement();
            xw.WriteStartElement("rotate");
            xw.WriteAttributeString("sid", "rotateX");
            xw.WriteString("1 0 0 0");
            xw.WriteEndElement();
            xw.WriteStartElement("instance_geometry");
            xw.WriteAttributeString("url", "#" + modelID);
            xw.WriteStartElement("bind_material");
            xw.WriteStartElement("technique_common");
            xw.WriteStartElement("instance_material");
            xw.WriteAttributeString("target", "#default");
            xw.WriteAttributeString("symbol", "defaultSG");
            xw.WriteEndElement(); // instance_material
            xw.WriteEndElement(); // technique_common
            xw.WriteEndElement(); // bind_material
            xw.WriteEndElement(); // instance_geometry
            xw.WriteEndElement(); // node
            xw.WriteEndElement(); // visual scene
            xw.WriteEndElement(); // library visual scenes

            xw.WriteStartElement("scene");
            xw.WriteStartElement("instance_visual_scene");
            xw.WriteAttributeString("url", "#scene");
            xw.WriteEndElement();
            xw.WriteEndElement();

            xw.WriteEndElement();// End COLLADA
            xw.WriteEndDocument();
            xw.Flush();

            FileStream stream = new FileStream(@"D:\\out.dae", FileMode.OpenOrCreate);
            stream.Write(ms.ToArray(), 0, ms.ToArray().Length);
            stream.Close();

        }

        //
        // PRIMARY STRUCTURES:
        //

        class Index
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

        class Geometry
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

        enum Shape : uint
        {
            Triangle = 0x03,
            Square = 0x04
        }

        //
        // INTERNAL STRUCTURES:
        //

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class Table
        {
            public UInt32 id { get; set; }
            public UInt32 offset { get; set; }
            public UInt32 size { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class Triangle
        {
            public UInt16 coordinate01 { get; set; }
            public UInt16 coordinate02 { get; set; }
            public UInt16 coordinate03 { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class Coordinate
        {
            public Single positionX { get; set; }
            public Single positionY { get; set; }
            public Single positionZ { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class Extended : Coordinate
        {
            public Single tanX { get; set; }
            public Single tanY { get; set; }
            public Single tanZ { get; set; }
            public UInt32 rgb { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class UV
        {
            public Single x { get; set; }
            public Single y { get; set; }
        }
    }
}
