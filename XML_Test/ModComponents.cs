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
    [XmlArray("Modification-List")]
    public List<Modification> modifications;

    public Revival()
    {
      modifications = new List<Modification>();
    }

    /// <summary>
    /// Adds a new modification
    /// </summary>
    /// <param name="modID">The name of the mod</param>
    /// <param name="version">The version of the mod</param>
    /// <param name="description">The description of the mod</param>
    public Modification AddNewModification(string modID, string version, string description)
    {
      Modification modification = modifications.Find(mod => mod.id == modID && mod.version == version);

      if (modification == null)
      {
        modification = new Modification(modID, version, description);
        modifications.Add(modification);
      }

      return modification;
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
    [XmlArray("IDX-Files")]
    public List<Index> indices;

    public Modification() : this(string.Empty, string.Empty, string.Empty)
    {
    }

    public Modification(string modID, string version, string description)
    {
      indices = new List<Index>();
      this.id = modID;
      this.version = version;
      this.description = description;
    }

    /// <summary>
    /// Adds a new idx file
    /// </summary>
    /// <param name="idxID">The name of the idx file</param>
    /// <param name="fileID">The name of the txt.cooked file</param>
    public Index AddNewIndexFile(string idxID)
    {
      Index index = indices.Find(ind => ind.id == idxID);

      if (index == null)
      {
        index = new Index(idxID);
        indices.Add(index);
      }

      return index;
    }

      /// <summary>
    /// Adds a new idx file + txt.cooked file + modification entry
    /// </summary>
    /// <param name="idxID">The name of the idx file</param>
    /// <param name="fileID">The name of the txt.cooked file</param>
    /// <param name="row">The row number</param>
    /// <param name="column">The column number</param>
    /// <param name="value">The new value of the modified cell</param>
    public void AddNewIndexFileAndEntry(string idxID, string fileID, int row, int column, int value)
    {
      Index index = indices.Find(ind => ind.id == idxID);

      if (index == null)
      {
        index = new Index(idxID);
        indices.Add(index);
      }

      index.AddNewCookedFileAndEntry(fileID, row, column, value);
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
    [XmlArray("Cooked-Files")]
    public List<File> files;

    public Index() : this(string.Empty)
    {
    }

    public Index(string idxID)
    {
      this.id = idxID;
      files = new List<File>();
    }

    /// <summary>
    /// Adds a new txt.cooked file
    /// </summary>
    /// <param name="fileID">The name of the txt.cooked file</param>
    public File AddNewCookedFile(string fileID)
    {
      File file = files.Find(tmpFile => tmpFile.id == fileID);

      if (file == null)
      {
        file = new File(fileID);
        files.Add(file);
      }

      return file;
    }

    /// <summary>
    /// Adds a new txt.cooked file + modification entry
    /// </summary>
    /// <param name="fileID">The name of the txt.cooked file</param>
    /// <param name="row">The row number</param>
    /// <param name="column">The column number</param>
    /// <param name="value">The new value of the modified cell</param>
    public void AddNewCookedFileAndEntry(string fileID, int row, int column, int value)
    {
      File file = files.Find(tmpFile => tmpFile.id == fileID);

      if (file == null)
      {
        file = new File(fileID);
        files.Add(file);
      }

      file.AddNewModifyEntry(row, column, value);
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
    [XmlArray("Cells")]
    public List<Modify> modifies;

    public File() : this(string.Empty)
    {
    }

    public File(string fileID)
    {
      this.id = fileID;
      modifies = new List<Modify>();
    }

    /// <summary>
    /// Adds a new cell modification
    /// </summary>
    /// <param name="row">The row number</param>
    /// <param name="column">The column number</param>
    /// <param name="value">The new value of the modified cell</param>
    public void AddNewModifyEntry(int row, int column, int value)
    {
      Modify mod = modifies.Find(tmpMod => tmpMod.row == row && tmpMod.col == column);

      if (mod == null)
      {
        mod = new Modify(row, column);
        modifies.Add(mod);
      }

      mod.data = value;
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


    public Modify() : this(-1, -1, -1)
    {
    }

    public Modify(int row, int column)
    {
      this.row = row;
      this.col = column;
    }

    public Modify(int row, int column, int value)
    {
      this.row = row;
      this.col = column;
      this.data = value;
    }
  }
}
