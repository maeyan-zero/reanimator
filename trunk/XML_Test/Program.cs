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
        Modification mod = new Modification();
          mod.id = "Revival SP modification";
          mod.version = "1.2.0";
          mod.description = "This is the base SP modification developed by the Revival team.";
          Index index = new Index();
            index.id = "hellgate000";
            File file = new File();
              file.id = "gameglobals.txt.cooked";
              Modify modify = new Modify();
                modify.row = 1;
                modify.col = 1;      
                modify.data = 20;
            file.modifications.Add(modify);
          index.files.Add(file);
        mod.indices.Add(index);
      revival.modifications.Add(mod);


      Utilities.Serialize(revival, Directory.GetCurrentDirectory() + @"\Revival.xml");

      Revival rev = Utilities.Deserialize(Directory.GetCurrentDirectory() + @"\Revival.xml");
      rev.modifications.ToString();
    }
  }
}
