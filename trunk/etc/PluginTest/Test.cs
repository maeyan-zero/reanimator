using System;
using PluginInterface;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PluginTest
{
  class PlugIn : IPlugin
  {
    private string name;
    private IPluginHost host;
    private MenuStrip hostMenu;

    public PlugIn()
    {
      name = "Test";
    }

    public string Name
    {
      get
      {
        return this.name;
      }
      set
      {
        this.name = value;
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

    public MenuStrip HostMenu
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

    public string getName()
    {
      return name;
    }

    public void InitializePlugIn()
    {
      try
      {
        // Get the "file" entry
        ToolStripMenuItem file = (ToolStripMenuItem)hostMenu.Items.Find("fileMenu", true)[0];

        // Get the "Selected" entry, if it already exists
        List<ToolStripItem> select = new List<ToolStripItem>();
        select.AddRange(file.DropDownItems.Find("openToolStripMenuItem", true));

        // If it doesn't exist yet, create it
        if (select.Count == 0)
        {
          ToolStripMenuItem newSelect = new ToolStripMenuItem("Hi alex2069 and maeyan, I'm a plugin entry");
          newSelect.Name = "Test";
          file.DropDownItems.Add(newSelect);
          select.Add(newSelect);
        }

        // If it exists, add the button
        ((ToolStripMenuItem)select[0]).DropDownItems.Add(this.name);
      }
      catch (Exception ex)
      {
        host.ShowMessage(ex.Message);
      }
    }
  }
}
