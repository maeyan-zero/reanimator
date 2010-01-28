using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginInterface;
using System.Windows.Forms;

namespace TransmogrifyCubePlugin
{
  public class PlugIn : GenericPlugin
  {
    public PlugIn()
    {
      name = "Transmogrify Plugin";
      description = "Adds an option to create items or add attributes by using all kinds of materials.";
    }

    public override void InitializePlugIn(bool showMessageWhenSuccesfullyLoaded)
    {
      try
      {
        ToolStripItem[] items = hostMenu.Items.Find("toolsMenu", true);
        ToolStripMenuItem toolsMenu;

        if (items.Length != 0)
        {
          toolsMenu = (ToolStripMenuItem)items[0];

          ToolStripMenuItem transmogrifyItem = new ToolStripMenuItem("Transmogrify Items");
          transmogrifyItem.Click += new EventHandler(transmogrifyItem_Click);

          toolsMenu.DropDownItems.Add(transmogrifyItem);

          base.InitializePlugIn(showMessageWhenSuccesfullyLoaded);
        }
        else
        {
          host.ShowMessage("Could not find menu entry \"toolsMenu!\"");
        }
      }
      catch (Exception ex)
      {
        host.ShowMessage(ex.Message);
      }
    }

    void transmogrifyItem_Click(object sender, EventArgs e)
    {
      host.ShowMessage("Not implemented yet");
    }
  }
}
