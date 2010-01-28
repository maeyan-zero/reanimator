using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginInterface;
using System.Windows.Forms;

namespace ItemTransferPlugin
{
  public class PlugIn : GenericPlugin
  {
    public PlugIn()
    {
      name = "Item Transfer";
      description = "Adds an option to transfer items between characters.";
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

          ToolStripMenuItem transferItem = new ToolStripMenuItem("Transfer Items");
          transferItem.Click += new EventHandler(transferItem_Click);

          toolsMenu.DropDownItems.Add(transferItem);

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

    void transferItem_Click(object sender, EventArgs e)
    {
      ItemTransferForm transfer = new ItemTransferForm();
      transfer.Text = this.name;
      transfer.MdiParent = parent;
      transfer.Show();
    }
  }
}
