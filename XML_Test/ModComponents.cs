using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace XML_Test
{
  /// <summary>
  /// Small static class to handle serialization and deserialization
  /// </summary>
  public static class Utilities
  {
    public static void Serialize(Revival revival, string path)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Revival));
      TextWriter tw = new StreamWriter(path);
      serializer.Serialize(tw, revival);
      tw.Close();
    }

    public static Revival Deserialize(string path)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Revival));
      TextReader tr = new StreamReader(path);
      Revival revival = (Revival)serializer.Deserialize(tr);
      tr.Close();

      return revival;
    }
  }

  /// <summary>
  /// Basic mod class, can contain multiple mods
  /// </summary>
  [Serializable]
  public class Revival
  {
    // A list of mods
    public List<Modification> modifications;

    public Revival()
    {
      modifications = new List<Modification>();
    }
  }

  /// <summary>
  /// A mod package. Contains idx files to modify
  /// </summary>
  [Serializable]
  public class Modification
  {
    // Mod name
    [XmlAttribute("id")]
    public string id;
    // Mod version
    [XmlAttribute("version")]
    public string version;
    // Mod description
    public string description;
    // A list of idx files
    public List<Index> indices;

    public Modification()
    {
      indices = new List<Index>();
    }
  }

  /// <summary>
  /// An idx file. Contains txt.cooked files to modify
  /// </summary>
  [Serializable]
  public class Index
  {
    // Name of the idx file to open
    [XmlAttribute("id")]
    public string id;
    // A list of txt.cooked files
    public List<File> files;

    public Index()
    {
      files = new List<File>();
    }
  }

  /// <summary>
  /// A txt.cooked file. Contains cells to modify
  /// </summary>
  [Serializable]
  public class File
  {
    // Name of the txt.cooked file
    [XmlAttribute("id")]
    public string id;
    // A list of cells
    public List<Modify> modifications;

    public File()
    {
      modifications = new List<Modify>();
    }
  }

  /// <summary>
  /// A cell that has to be modified
  /// </summary>
  [Serializable]
  public class Modify
  {
    // The row in which the cell is located
    [XmlAttribute("row")]
    public int row;
    // The column in which the cell is located
    [XmlAttribute("col")]
    public int col;
    // The new data value for the cell
    public int data;
  }
}
