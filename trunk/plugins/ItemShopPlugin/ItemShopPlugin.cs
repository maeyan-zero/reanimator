using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginInterface;
using System.Windows.Forms;
using System.IO;

namespace ItemShopPlugin
{
  public class PlugIn : IPlugin
  {
    private string name;
    private string description;
    private Form parent;
    private IPluginHost host;
    private MenuStrip hostMenu;
    private string hglDirectory;

    public PlugIn()
    {
      name = "Item Shop";
      description = "Adds an option to purchase special items that aren't available in game as well as new options for Reanimator (with your Palladium/Items).";
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    public string Description
    {
      get
      {
        return description;
      }
    }

    public Form Parent
    {
      set
      {
        this.parent = value;
      }
    }

    public IPluginHost Host
    {
      get
      {
        return this.host;
      }
      set
      {
        this.host = value;

        host.Register(this);
      }
    }

    public MenuStrip HostMenu
    {
      get
      {
        return this.hostMenu;
      }
      set
      {
        this.hostMenu = value;
      }
    }

    public string HGLDirectory
    {
      get
      {
        return this.hglDirectory;
      }
      set
      {
        this.hglDirectory = value;
      }
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

          if (showMessageWhenSuccesfullyLoaded)
          {
            host.ShowMessage("ItemShopPlugin.dll loaded!");
          }
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
