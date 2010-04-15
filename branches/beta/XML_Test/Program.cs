using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace XML_Test
{
  class Program
  {
    static void Main(string[] args)
    {
      Revival revival = new Revival();
      Modification mod = revival.AddNewModification("Revival SP modification", "1.2.0", "This is the base SP modification developed by the Revival team.");
      Index index1 = mod.AddNewIndexFile("hellgate000");
      File file1 = index1.AddNewCookedFile("gameglobals.txt.cooked");
      file1.AddNewModifyEntry(1, 1, 20);
      File file2 = index1.AddNewCookedFile("gamestats.txt.cooked");
      file2.AddNewModifyEntry(1, 2, 15);
      file2.AddNewModifyEntry(10, 3, 2);
      Index index2 = mod.AddNewIndexFile("hellgate123");
      File file3 = index2.AddNewCookedFile("abc.txt.cooked");
      file3.AddNewModifyEntry(0, 0, 12);
      file3.AddNewModifyEntry(3, 5, 1);
      file2.AddNewModifyEntry(4, 2, 43);

      //Serialize the mod file
      Utilities.Serialize(revival, Directory.GetCurrentDirectory() + @"\Revival.xml");

      //Deserialize the mod file
      Revival rev = Utilities.Deserialize(Directory.GetCurrentDirectory() + @"\Revival.xml");

      //Display the created file
      StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + @"\Revival.xml");
      Form1 form = new Form1(reader.ReadToEnd());
      Application.Run(form);
    }
  }
}
