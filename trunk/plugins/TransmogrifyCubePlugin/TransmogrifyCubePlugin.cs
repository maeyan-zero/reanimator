using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginInterface;
using System.Windows.Forms;

namespace TransmogrifyCubePlugin
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
      name = "Transmogrify Plugin";
      description = "Adds an option to create items or add attributes by using all kinds of materials.";
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

          ToolStripMenuItem transmogrifyItem = new ToolStripMenuItem("Transmogrify Items");
          transmogrifyItem.Click += new EventHandler(transmogrifyItem_Click);

          toolsMenu.DropDownItems.Add(transmogrifyItem);

          if (showMessageWhenSuccesfullyLoaded)
          {
            host.ShowMessage("TransmogrifyCubePlugin.dll loaded!");
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

    void transmogrifyItem_Click(object sender, EventArgs e)
    {
      host.ShowMessage("Not implemented yet");
    }
  }
}
