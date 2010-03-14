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
                    case (uint)Structure.Reserved:
                        reserved = GetReserved(binReader);
                        break;
                    case (uint)Structure.Footer:
                        footer = GetFooter(binReader);
                        break;
                }
            }
            binReader.ReadBytes(8); // this should bring to EOF
            binReader.Close();
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

        Reserved GetReserved(BinaryReader binReader)
        {
            Reserved reserved = new Reserved();
            binReader.ReadInt32(); //null
            binReader.ReadInt32(); // 01
            // the following skips a butt load of reserved space
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

        String GetString(BinaryReader binReader)
        {
            byte[] byteString = new byte[binReader.ReadUInt32()];
            byteString = binReader.ReadBytes(byteString.Length);
            return byteString.ToString();
        }

        String GetMergedPositions()
        {
            string positions = "";
            foreach (Index idx in index)
            {
                for (int i = 0; i < idx.data.Length; i++)
                {
                    positions += idx.data[i].ToString() + " ";
                }
            }
            return positions;
        }

        public void Export()
        {
            XmlWriterSettings wSettings = new XmlWriterSettings();
            wSettings.Indent = true;
            MemoryStream ms = new MemoryStream();
            XmlWriter xw = XmlWriter.Create(ms, wSettings);// Write Declaration

            //debug
            modelID = "model_name_here";

            xw.WriteStartDocument();
            xw.WriteStartElement("COLLADA");
            xw.WriteStartElement("asset");
            xw.WriteStartElement("contributor");
            xw.WriteStartElement("author");
            xw.WriteString("Ripped by Maeyan, Mesh by Flagship Studio (c)");
            xw.WriteEndElement();
            xw.WriteStartElement("copyright");
            xw.WriteString("Ripped by Maeyan, Mesh by Flagship Studio (c)");
            xw.WriteEndElement();
            xw.WriteEndElement();// End Contributor
            xw.WriteStartElement("created");
            xw.WriteString(DateTime.Today.ToLongDateString());
            xw.WriteEndElement();
            xw.WriteStartElement("modified");
            xw.WriteString(DateTime.Today.ToLongDateString());
            xw.WriteEndElement();
            xw.WriteStartElement("unit");
            xw.WriteAttributeString("meter", "0.01");
            xw.WriteAttributeString("name", "centimeter");
            xw.WriteEndElement();
            xw.WriteStartElement("up_axis");
            xw.WriteString("Y_UP");
            xw.WriteEndElement();
            xw.WriteEndElement();// End asset

            xw.WriteStartElement("library_geometries");
            xw.WriteAttributeString("id", modelID);
            xw.WriteStartElement("geometry");
            xw.WriteStartElement("mesh");
            xw.WriteAttributeString("id", modelID + "-positions");
            xw.WriteStartElement("source");

            xw.WriteStartElement("float_array");
            xw.WriteString(GetMergedPositions());
            xw.WriteEndElement();

            xw.WriteStartElement("technique_common");
            xw.WriteAttributeString("stride", 3);
            xw.WriteAttributeString("source", "#" + modelID = "-positions-array");
            xw.WriteEndElement();

            xw.WriteStartElement("accessor");
            xw.WriteEndElement();

            xw.WriteEndElement();// End mesh
            xw.WriteEndElement();// End geometry
            xw.WriteEndElement();// End library_geometries

            xw.WriteEndElement();// End COLLADA
            xw.WriteEndDocument();
            xw.Flush();

            FileStream stream = new FileStream(@"D:\\out.xml", FileMode.OpenOrCreate);
            stream.Write(ms.ToArray(), 0, ms.ToArray().Length);
            stream.Close();

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

        struct Reserved
        {
            public String title { get; set; }
            public byte[] unknown { get; set; }
        }

        struct Footer
        {
            public String path { get; set; }
            public byte[] unknown { get; set; }
        }
    }
}
