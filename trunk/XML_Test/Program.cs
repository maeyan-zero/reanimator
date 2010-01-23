using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace XML_Test
{
  class Program
  {
    static void Main(string[] args)
    {
      Revival revival = new Revival();
      Modification mod = revival.AddNewModification("Revival SP modification", "1.2.0", "This is the base SP modification developed by the Revival team.");
      Index index = mod.AddNewIndexFile("hellgate000");
      File file = index.AddNewCookedFile("gameglobals.txt.cooked");
      file.AddNewModifyEntry(1, 1, 20);

      Utilities.Serialize(revival, Directory.GetCurrentDirectory() + @"\Revival.xml");

      Revival rev = Utilities.Deserialize(Directory.GetCurrentDirectory() + @"\Revival.xml");
      rev.modifications.ToString();
    }
  }
}
