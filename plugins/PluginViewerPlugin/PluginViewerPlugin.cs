using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginInterface;
using System.Windows.Forms;

namespace PluginViewer
{
  public class PlugIn : IPlugin
  {
    private string name;
    private string description;
    private string hglDirectory;
    private Form parent;
    private IPluginHost host;
    private MenuStrip hostMenu;

    public PlugIn()
    {
      name = "Plugin Viewer";
      description = "Adds an option to display all loaded plugins.";
    }

    public string Name
    {
      get
      {
        return name;
      }
    }

    public string Description
    {
      get
      {
        return description;
      }
    }

    public string HGLDirectory
    {
      get
      {
        return hglDirectory;
      }
      set
      {
        this.hglDirectory = value;
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
        return host;
      }
      set
      {
        this.host = value;

        host.Register(this);
      }
    }

    public System.Windows.Forms.MenuStrip HostMenu
    {
      get
      {
        return hostMenu;
      }
      set
      {
        this.hostMenu = value;
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

          ToolStripMenuItem pluginViewer = new ToolStripMenuItem("Show Plugins");
          pluginViewer.Click += new EventHandler(pluginViewer_Click);

          toolsMenu.DropDownItems.Add(pluginViewer);

          if (showMessageWhenSuccesfullyLoaded)
          {
            host.ShowMessage("PluginViewer.dll loaded!");
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

    void pluginViewer_Click(object sender, EventArgs e)
    {
      PluginViewerForm viewer = new PluginViewerForm(this.host.GetPluginList());
      viewer.Text = this.name;
      viewer.MdiParent = parent;
      viewer.Show();
    }
  }
}
