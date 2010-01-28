using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginInterface;
using System.Windows.Forms;
using System.IO;

namespace ItemShopPlugin
{
  public class PlugIn : GenericPlugin
  {
    public PlugIn()
    {
      name = "Item Shop";
      description = "Adds an option to purchase special items that aren't available in game as well as new options for Reanimator (with your Palladium/Items).";
    }

    public void InitializePlugIn(bool showMessageWhenSuccesfullyLoaded)
    {
      try
      {
        ToolStripItem[] items = hostMenu.Items.Find("toolsMenu", true);
        ToolStripMenuItem toolsMenu;

        if (items.Length != 0)
        {
          toolsMenu = (ToolStripMenuItem)items[0];

          ToolStripMenuItem clientPatcher = new ToolStripMenuItem("Item Shop");
          clientPatcher.Click += new EventHandler(clientPatcher_Click);

          toolsMenu.DropDownItems.Add(clientPatcher);

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

    void clientPatcher_Click(object sender, EventArgs e)
    {
      ItemShopForm transfer = new ItemShopForm();
      transfer.Text = this.name;
      transfer.MdiParent = parent;
      transfer.Show();
    }
  }
}
