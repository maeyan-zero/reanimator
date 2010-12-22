using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Revival.Common;
using System.Xml;

namespace Hellgate
{
    public class Model
    {
        private Int32 type;
        private Int32 minorVersion;
        private Int32 majorVersion;
        private Table[] fileStructure;
        private List<Index> _index;
        private List<Table> _indexMap; // Used by .m/player type models only
        private List<Geometry> _geometry;
        //private Reserved _reserved;
        private Footer _footer;
        public string Id { get; private set; }

        public Model(byte[] buffer, string modelId)
        {
            MemoryStream memStream = new MemoryStream(buffer);
            BinaryReader binReader = new BinaryReader(memStream);

            Id = modelId;
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
            memStream.Close();
        }

        public byte[] ExportCollada()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            xmlDocument.AppendChild(xmlDeclaration);

            XmlElement colladaElement = xmlDocument.CreateElement("COLLADA");
            colladaElement.SetAttribute("xmlns", "http://www.collada.org/2005/11/COLLADASchema");
            colladaElement.SetAttribute("version", "1.4.1");
            xmlDocument.AppendChild(colladaElement);
            //
            // Asset Section, contains meta data
            //
            XmlElement assetElement = xmlDocument.CreateElement("asset");
            colladaElement.AppendChild(assetElement);
                XmlElement contributorElement = xmlDocument.CreateElement("contributor");
                assetElement.AppendChild(contributorElement);
                    XmlElement authorElement = xmlDocument.CreateElement("author");
                    authorElement.InnerText = "Flagship Studios";
                    contributorElement.AppendChild(authorElement);
                    XmlElement authoringToolElement = xmlDocument.CreateElement("authoring_tool");
                    authoringToolElement.InnerText = "Autodesk 3ds Max 8";
                    contributorElement.AppendChild(authoringToolElement);
                    XmlElement commentsElement = xmlDocument.CreateElement("comments");
                    commentsElement.InnerText = "Ripped via Reanimator - Hellgate London conversion tools. www.hellgateaus.net";
                    contributorElement.AppendChild(commentsElement);
                XmlElement createdElement = xmlDocument.CreateElement("created");
                createdElement.InnerText = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                assetElement.AppendChild(createdElement);
                XmlElement modifiedElement = xmlDocument.CreateElement("modified");
                modifiedElement.InnerText = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                assetElement.AppendChild(modifiedElement);
                XmlElement unitElement = xmlDocument.CreateElement("unit");
                unitElement.SetAttribute("meter", "0.01");
                unitElement.SetAttribute("name", "centimeter");
                assetElement.AppendChild(unitElement);
                XmlElement upAxisElement = xmlDocument.CreateElement("up_axis");
                upAxisElement.InnerText = "Y_UP";
                assetElement.AppendChild(upAxisElement);
            //
            // Geometry Sections
            //
            XmlElement libraryGeometriesElement = xmlDocument.CreateElement("library_geometries");
            colladaElement.AppendChild(libraryGeometriesElement);
            XmlElement geometryElement = xmlDocument.CreateElement("geometry");
            string geoLib = String.Format("{0}-lib", Id);
            geometryElement.SetAttribute("id", geoLib);
            geometryElement.SetAttribute("name", Id);
            libraryGeometriesElement.AppendChild(geometryElement);
            XmlElement meshElement = xmlDocument.CreateElement("mesh");
            geometryElement.AppendChild(meshElement);

            for (int i = 0; i < _geometry.Count; i++)
            {
                XmlElement meshSource = CreateColladaSource(xmlDocument, _geometry[i].position, "positions" + i.ToString());
                meshElement.AppendChild(meshSource);
                meshSource = CreateColladaSource(xmlDocument, _geometry[i].normal, "normals" + i.ToString());
                meshElement.AppendChild(meshSource);
                meshSource = CreateColladaSource(xmlDocument, _geometry[i].uv, "uvmap" + i.ToString());
                meshElement.AppendChild(meshSource);

                // Verticies
                XmlElement verticiesElement = xmlDocument.CreateElement("vertices");
                verticiesElement.SetAttribute("id", String.Format("{0}-{1}-{2}", Id, "lib", "verticies" + i.ToString()));
                meshSource.AppendChild(verticiesElement);

                XmlElement inputElement = xmlDocument.CreateElement("input");
                inputElement.SetAttribute("semantic", "POSITION");
                inputElement.SetAttribute("source", String.Format("#{0}-{1}-{2}", Id, "lib", "positions" + i.ToString()));
                verticiesElement.AppendChild(inputElement);

                // Triangles
                XmlElement trianglesElement = xmlDocument.CreateElement("triangles");
                trianglesElement.SetAttribute("count", _index[i].triangle.Length.ToString());
                trianglesElement.SetAttribute("material", "default");
                meshSource.AppendChild(trianglesElement);

                inputElement = xmlDocument.CreateElement("input");
                inputElement.SetAttribute("offset", "0");
                inputElement.SetAttribute("semantic", "VERTEX");
                inputElement.SetAttribute("source", String.Format("#{0}-{1}-{2}", Id, "lib", "verticies" + i.ToString()));
                trianglesElement.AppendChild(inputElement);

                inputElement = xmlDocument.CreateElement("input");
                inputElement.SetAttribute("offset", "0");
                inputElement.SetAttribute("semantic", "NORMAL");
                inputElement.SetAttribute("source", String.Format("#{0}-{1}-{2}", Id, "lib", "normals" + i.ToString()));
                trianglesElement.AppendChild(inputElement);

                inputElement = xmlDocument.CreateElement("input");
                inputElement.SetAttribute("offset", "0");
                inputElement.SetAttribute("semantic", "TEXCOORD");
                inputElement.SetAttribute("source", String.Format("#{0}-{1}-{2}", Id, "lib", "uvmap" + i.ToString()));
                trianglesElement.AppendChild(inputElement);

                XmlElement pElement = xmlDocument.CreateElement("p");
                StringWriter pString = new StringWriter();
                foreach (Triangle t in _index[0].triangle)
                {
                    pString.Write(t.ToString());
                }
                pElement.InnerText = pString.ToString();
                trianglesElement.AppendChild(pElement);
            }

            //
            // Material
            //
            for (int i = 0; i < _index.Count; i++)
            {

            }



            MemoryStream ms = new MemoryStream();
            xmlDocument.Save(ms);
            byte[] arr = ms.ToArray();
            ms.Close();

            return arr;
        }

        public byte[] ExportObj()
        {
            return null;
            //    StringWriter sw = new StringWriter();

            //    sw.WriteLine("# OBJ Export Test");
            //    sw.WriteLine("o " + model.Id);
            //    sw.WriteLine();

            //    int indexLength = 0;

            //    for (int i = 0; i < model.index.Count; i++)
            //    {
            //        sw.WriteLine("g " + i);
            //        foreach (Model.Coordinate coordinate in model.geometry[i].position)
            //        {
            //            sw.WriteLine("v " + coordinate.ToString());
            //        }
            //        foreach (Model.UV uv in model.geometry[i].uv)
            //        {
            //            sw.WriteLine("vt " + uv.ToString());
            //        }
            //        foreach (Model.Coordinate coordinate in model.geometry[i].normal)
            //        {
            //            sw.WriteLine("vn " + coordinate.ToString());
            //        }
            //        // INDEX SWAP
            //        int swapIndex;
            //        if (model.index.Count > 1)
            //        {
            //            if (i < model.index.Count - 1)
            //            {
            //                swapIndex = i + 1;
            //            }
            //            else
            //            {
            //                swapIndex = 0;
            //            }
            //        }
            //        else
            //        {
            //            swapIndex = i;
            //        }
            //        // END INDEXSWAP
            //        foreach (Model.Triangle triangle in model.index[swapIndex].triangle)
            //        {
            //            sw.Write("f ");
            //            sw.Write((triangle.coordinate01 + 1 + indexLength) + "/");
            //            sw.Write((triangle.coordinate01 + 1 + indexLength) + "/");
            //            sw.Write((triangle.coordinate01 + 1 + indexLength) + " ");
            //            sw.Write((triangle.coordinate02 + 1 + indexLength) + "/");
            //            sw.Write((triangle.coordinate02 + 1 + indexLength) + "/");
            //            sw.Write((triangle.coordinate02 + 1 + indexLength) + " ");
            //            sw.Write((triangle.coordinate03 + 1 + indexLength) + "/");
            //            sw.Write((triangle.coordinate03 + 1 + indexLength) + "/");
            //            sw.Write((triangle.coordinate03 + 1 + indexLength).ToString());
            //            sw.WriteLine();
            //        }
            //        indexLength += model.index[swapIndex].triangles;
            //    }
            //    return sw.ToString();
            //}

            //public void Export()
            //{
            //    XmlWriterSettings wSettings = new XmlWriterSettings();
            //    wSettings.Indent = true;
            //    MemoryStream ms = new MemoryStream();
            //    XmlWriter xw = XmlWriter.Create(ms, wSettings);// Write Declaration

            //    //debug
            //    _modelID = "model_name_here";
            //    DateTime now = DateTime.Now;

            //    //
            //    // HEADER
            //    //

            //    xw.WriteStartDocument();
            //    xw.WriteStartElement("COLLADA");
            //    xw.WriteAttributeString("version", "1.4.1");
            //    //xw.WriteAttributeString("xmlns", "http://www.collada.org/2005/11/COLLADASchema");
            //    xw.WriteStartElement("asset");
            //    xw.WriteStartElement("contributor");
            //    xw.WriteStartElement("author");
            //    xw.WriteString("Ripped by Maeyan");
            //    xw.WriteEndElement();
            //    xw.WriteStartElement("authoring_tool");
            //    xw.WriteString("3ds Max 7.0.0");
            //    xw.WriteEndElement();
            //    xw.WriteStartElement("copyright");
            //    xw.WriteString("Flagship Studios");
            //    xw.WriteEndElement();
            //    xw.WriteEndElement();// End Contributor
            //    xw.WriteStartElement("created");
            //    xw.WriteValue(now - new TimeSpan(0, 0, 0));
            //    xw.WriteEndElement();
            //    xw.WriteStartElement("modified");
            //    xw.WriteValue(now - new TimeSpan(0, 0, 0));
            //    xw.WriteEndElement();
            //    xw.WriteStartElement("unit");
            //    xw.WriteAttributeString("meter", "0.01");
            //    xw.WriteAttributeString("name", "centimeter");
            //    xw.WriteEndElement();
            //    xw.WriteStartElement("up_axis");
            //    xw.WriteString("Y_UP");
            //    xw.WriteEndElement();
            //    xw.WriteEndElement();// End asset

            //    //
            //    // MATERIAL
            //    //

            //    xw.WriteStartElement("library_materials");
            //    xw.WriteStartElement("material");
            //    xw.WriteAttributeString("id", "default");
            //    xw.WriteStartElement("instance_effect");
            //    xw.WriteAttributeString("url", "#lambert-fx");
            //    xw.WriteEndElement();
            //    xw.WriteEndElement();
            //    xw.WriteEndElement();

            //    //
            //    // EFFECT
            //    //

            //    xw.WriteStartElement("library_effects");
            //    xw.WriteStartElement("effect");
            //    xw.WriteAttributeString("id", "lambert-fx");
            //    xw.WriteStartElement("profile_COMMON");
            //    xw.WriteStartElement("technique");
            //    xw.WriteAttributeString("sid", "common");
            //    xw.WriteStartElement("lambert");

            //    xw.WriteStartElement("emission");
            //    xw.WriteStartElement("color");
            //    xw.WriteString("0 0 0 1");
            //    xw.WriteEndElement(); // color
            //    xw.WriteEndElement(); // diffuse

            //    xw.WriteStartElement("ambient");
            //    xw.WriteStartElement("color");
            //    xw.WriteString("0 0 0 1");
            //    xw.WriteEndElement(); // color
            //    xw.WriteEndElement(); // diffuse

            //    xw.WriteStartElement("diffuse");
            //    xw.WriteStartElement("color");
            //    xw.WriteString("0.5 0.5 0.5 1");
            //    xw.WriteEndElement(); // color
            //    xw.WriteEndElement(); // diffuse

            //    xw.WriteStartElement("reflective");
            //    xw.WriteStartElement("color");
            //    xw.WriteString("1 1 1 1");
            //    xw.WriteEndElement(); // color
            //    xw.WriteEndElement(); // diffuse

            //    xw.WriteStartElement("reflectivity");
            //    xw.WriteStartElement("float");
            //    xw.WriteString("1");
            //    xw.WriteEndElement(); // color
            //    xw.WriteEndElement(); // diffuse

            //    xw.WriteStartElement("transparent");
            //    xw.WriteStartElement("color");
            //    xw.WriteString("0 0 0 1");
            //    xw.WriteEndElement(); // color
            //    xw.WriteEndElement(); // diffuse

            //    xw.WriteStartElement("transparency");
            //    xw.WriteStartElement("float");
            //    xw.WriteString("1");
            //    xw.WriteEndElement(); // color
            //    xw.WriteEndElement(); // diffuse

            //    xw.WriteStartElement("index_of_refraction");
            //    xw.WriteStartElement("float");
            //    xw.WriteString("0");
            //    xw.WriteEndElement(); // color
            //    xw.WriteEndElement(); // diffuse

            //    xw.WriteEndElement(); // lambert
            //    xw.WriteEndElement(); // Technique
            //    xw.WriteEndElement(); // profile_COMMON
            //    xw.WriteEndElement(); // Effect
            //    xw.WriteEndElement(); // Library Effects

            //    //
            //    // LIBRARY GEOMETRIES
            //    //

            //    xw.WriteStartElement("library_geometries");
            //    xw.WriteAttributeString("id", modelID);
            //    xw.WriteStartElement("geometry");
            //    xw.WriteStartElement("mesh");

            //    //
            //    // POSITIONS
            //    //

            //    Single[] positions = MergePositions();

            //    xw.WriteStartElement("source");
            //    xw.WriteAttributeString("id", modelID + "-positions");
            //    xw.WriteStartElement("float_array");
            //    xw.WriteAttributeString("id", modelID + "-positions-array");
            //    xw.WriteAttributeString("count", positions.Length.ToString());
            //    xw.WriteString(FileTools.ArrayToStringGeneric<Single>(positions, " "));
            //    xw.WriteEndElement();
            //    xw.WriteStartElement("technique_common");
            //    xw.WriteStartElement("accessor");
            //    xw.WriteAttributeString("stride", "3");
            //    xw.WriteAttributeString("source", "#" + modelID + "-positions-array");
            //    xw.WriteAttributeString("count", (positions.Length / 3).ToString());
            //    xw.WriteStartElement("param");
            //    xw.WriteAttributeString("name", "X");
            //    xw.WriteAttributeString("type", "float");
            //    xw.WriteEndElement();
            //    xw.WriteStartElement("param");
            //    xw.WriteAttributeString("name", "Y");
            //    xw.WriteAttributeString("type", "float");
            //    xw.WriteEndElement();
            //    xw.WriteStartElement("param");
            //    xw.WriteAttributeString("name", "Z");
            //    xw.WriteAttributeString("type", "float");
            //    xw.WriteEndElement();
            //    xw.WriteEndElement();// End Accessor
            //    xw.WriteEndElement();// End technique common
            //    xw.WriteEndElement();// End source

            //    //
            //    // UV
            //    //

            //    for (int i = 0; i < _geometry.Count; i++)
            //    {
            //        Single[] uvArray = GetUVArray(i);

            //        //if (index[i].zone == Zone.Render)
            //        {
            //            xw.WriteStartElement("source");
            //            xw.WriteAttributeString("id", modelID + "-uv-" + i.ToString());
            //            xw.WriteStartElement("float_array");
            //            xw.WriteAttributeString("id", modelID + "-uv-array");
            //            xw.WriteAttributeString("count", uvArray.Length.ToString());
            //            xw.WriteString(FileTools.ArrayToStringGeneric<Single>(uvArray, " "));
            //            xw.WriteEndElement();
            //            xw.WriteStartElement("technique_common");
            //            xw.WriteStartElement("accessor");
            //            xw.WriteAttributeString("stride", "2");
            //            xw.WriteAttributeString("source", "#" + modelID + "-uv-array");
            //            xw.WriteAttributeString("count", (uvArray.Length / 2).ToString());
            //            xw.WriteStartElement("param");
            //            xw.WriteAttributeString("name", "S");
            //            xw.WriteAttributeString("type", "float");
            //            xw.WriteEndElement();
            //            xw.WriteStartElement("param");
            //            xw.WriteAttributeString("name", "T");
            //            xw.WriteAttributeString("type", "float");
            //            xw.WriteEndElement();
            //            xw.WriteEndElement();// End Accessor
            //            xw.WriteEndElement();// End technique common
            //            xw.WriteEndElement();// End source
            //        }
            //    }

            //    xw.WriteStartElement("vertices");
            //    xw.WriteAttributeString("id", modelID + "-vertices");
            //    xw.WriteStartElement("input");
            //    xw.WriteAttributeString("semantic", "POSITION");
            //    xw.WriteAttributeString("source", "#" + modelID + "-positions");
            //    xw.WriteEndElement();
            //    xw.WriteEndElement();

            //    //
            //    // INDEX:
            //    //

            //    for (int i = 0; i < _index.Count; i++)
            //    {
            //        UInt16[] triangleArray = GetTriangleArray(i);

            //        xw.WriteStartElement("triangles");
            //        xw.WriteAttributeString("count", (triangleArray.Length / 3).ToString());
            //        xw.WriteAttributeString("material", "initialShadingGroup");
            //        xw.WriteStartElement("input");
            //        xw.WriteAttributeString("offset", "0");
            //        xw.WriteAttributeString("semantic", "VERTEX");
            //        xw.WriteAttributeString("source", "#" + modelID + "-vertices");
            //        xw.WriteEndElement();
            //        xw.WriteStartElement("input");
            //        xw.WriteAttributeString("offset", "0");
            //        xw.WriteAttributeString("semantic", "TEXCOORD");
            //        xw.WriteAttributeString("source", "#" + modelID + "-uv-" + i.ToString());
            //        xw.WriteEndElement();
            //        xw.WriteStartElement("p");
            //        xw.WriteString(FileTools.ArrayToStringGeneric<UInt16>(triangleArray, " "));
            //        xw.WriteEndElement();
            //        xw.WriteEndElement();
            //    }

            //    xw.WriteEndElement();// End mesh
            //    xw.WriteEndElement();// End geometry
            //    xw.WriteEndElement();// End library_geometries

            //    //
            //    // VISUAL SCENE:
            //    //

            //    xw.WriteStartElement("library_visual_scenes");
            //    xw.WriteStartElement("visual_scene");
            //    xw.WriteAttributeString("id", "scene");
            //    xw.WriteStartElement("node");
            //    xw.WriteAttributeString("id", modelID + "-geometry");
            //    xw.WriteStartElement("rotate");
            //    xw.WriteAttributeString("sid", "rotateZ");
            //    xw.WriteString("0 0 1 0");
            //    xw.WriteEndElement();
            //    xw.WriteStartElement("rotate");
            //    xw.WriteAttributeString("sid", "rotateY");
            //    xw.WriteString("0 1 0 0");
            //    xw.WriteEndElement();
            //    xw.WriteStartElement("rotate");
            //    xw.WriteAttributeString("sid", "rotateX");
            //    xw.WriteString("1 0 0 0");
            //    xw.WriteEndElement();
            //    xw.WriteStartElement("instance_geometry");
            //    xw.WriteAttributeString("url", "#" + modelID);
            //    xw.WriteStartElement("bind_material");
            //    xw.WriteStartElement("technique_common");
            //    xw.WriteStartElement("instance_material");
            //    xw.WriteAttributeString("target", "#default");
            //    xw.WriteAttributeString("symbol", "initialShadingGroup");
            //    xw.WriteEndElement(); // instance_material
            //    xw.WriteEndElement(); // technique_common
            //    xw.WriteEndElement(); // bind_material
            //    xw.WriteEndElement(); // instance_geometry
            //    xw.WriteEndElement(); // node
            //    xw.WriteEndElement(); // visual scene
            //    xw.WriteEndElement(); // library visual scenes

            //    xw.WriteStartElement("scene");
            //    xw.WriteStartElement("instance_visual_scene");
            //    xw.WriteAttributeString("url", "#scene");
            //    xw.WriteEndElement();
            //    xw.WriteEndElement();

            //    xw.WriteEndElement();// End COLLADA
            //    xw.WriteEndDocument();
            //    xw.Flush();

            //    FileStream stream = new FileStream(@"D:\\out.dae", FileMode.OpenOrCreate);
            //    stream.Write(ms.ToArray(), 0, ms.ToArray().Length);
            //    stream.Close();

            //}
        }

        private XmlElement CreateColladaSource<T>(XmlDocument xmlDocument, T[] source, string desc)
        {
            int positions = 0;
            string[] coordDesc = null;
            StringWriter elementsList = new StringWriter();

            if (typeof(T) == typeof(Coordinate))
            {
                positions = Coordinate.positions;
                coordDesc = Coordinate.coordDesc;
                Coordinate[] cSource = source as Coordinate[];
                foreach (Coordinate coordinate in cSource)
                {
                    elementsList.Write(coordinate.ToString());
                }
            }

            if (typeof(T) == typeof(UV))
            {
                positions = UV.positions;
                coordDesc = UV.coordDesc;
                UV[] uSource = source as UV[];
                foreach (UV coordinate in uSource)
                {
                    elementsList.Write(coordinate.ToString());
                }
            }
            
            int coordinates = source.Length;
            int totalCoordinates = positions * coordinates;

            XmlElement meshElement = xmlDocument.CreateElement("source");
            string sourceDesc = String.Format("{0}-{1}-{2}", Id, "lib", desc);
            meshElement.SetAttribute("id", sourceDesc);
            meshElement.SetAttribute("name", desc);

            XmlElement meshArrayElement = xmlDocument.CreateElement("float_array");
            string sourceDescArray = String.Format("{0}-array", sourceDesc);
            meshArrayElement.SetAttribute("id", sourceDescArray);
            string noElements = totalCoordinates.ToString();
            meshArrayElement.SetAttribute("count", noElements);
            meshArrayElement.InnerText = elementsList.ToString();
            meshElement.AppendChild(meshArrayElement);

            XmlElement techniqueCommonElement = xmlDocument.CreateElement("technique_common");
            meshElement.AppendChild(techniqueCommonElement);
            XmlElement accessorElement = xmlDocument.CreateElement("accessor");
            accessorElement.SetAttribute("count", coordinates.ToString());
            string sourceRef = String.Format("#{0}", sourceDescArray);
            accessorElement.SetAttribute("source", sourceRef);
            accessorElement.SetAttribute("stride", positions.ToString());
            techniqueCommonElement.AppendChild(accessorElement);

            for (int i = 0; i < positions; i++)
            {
                XmlElement paramElement = xmlDocument.CreateElement("param");
                paramElement.SetAttribute("name", coordDesc[i]);
                paramElement.SetAttribute("type", "float");
                techniqueCommonElement.AppendChild(paramElement);
            }

            return meshElement;
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
            if (getGeometry.detail != Detail.Simple)
            {
                getGeometry.normal = GetCoordinates(binReader, getGeometry.detail);
            }
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

            new public String ToString()
            {
                return String.Format("{0} {1} {2} ", coordinate01.ToString(), coordinate02.ToString(), coordinate03.ToString());
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class Coordinate
        {
            public static Int32 positions = 3;
            public static String[] coordDesc = new string[] { "X", "Y", "Z" };
            public Single positionX { get; set; }
            public Single positionY { get; set; }
            public Single positionZ { get; set; }

            new public String ToString()
            {
                return String.Format("{0} {1} {2} ", positionX.ToString(), positionY.ToString(), positionZ.ToString());
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
            public static Int32 positions = 2;
            public static String[] coordDesc = new string[] { "S", "T" };
            public Single s { get; set; }
            public Single t { get; set; }

            new public string ToString()
            {
                return String.Format("{0} {1} ", s.ToString() , t.ToString());
            }
        }
    }
}
