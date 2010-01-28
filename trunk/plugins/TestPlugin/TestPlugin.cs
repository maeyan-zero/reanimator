using System;
using PluginInterface;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PluginTest
{
  // A basic Plugin. The class name MUST be "PlugIn"!
  class PlugIn : IPlugin
  {
    private string name;
    private string description;
    // The current hgl main folder
    private string hglDirectory;
    // A reference to the parent form
    private Form parent;
    // A reference to the plugin host
    private IPluginHost host;
    // A reference to the option bar for easy access
    private MenuStrip hostMenu;

    public PlugIn()
    {
      name = "Test";
      description = "A small test plugin.";
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
        return host;
      }
      set
      {
        this.host = value;
        // Register the plugin in the host
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

    /// <summary>
    /// Initializes the plugin
    /// </summary>
    /// <param name="showSuccessMessage">True: Show a message window after the plugin was loaded successfully. False: Don't show this message</param>
    public void InitializePlugIn(bool showMessageWhenSuccesfullyLoaded)
    {
      try
      {
        // Gets the "File" entry of the options menu so we can add another sub entry
        ToolStripMenuItem file = (ToolStripMenuItem)hostMenu.Items.Find("fileMenu", true)[0];
        hostMenu.Items.Add("I am a plugin entry :)");

        List<ToolStripItem> open = new List<ToolStripItem>();
        // Search the "File" sub-entries for the "Open" option
        open.AddRange(file.DropDownItems.Find("openToolStripMenuItem", true));

        string text = "Hi alex2069 and maeyan, I'm a plugin entry! You can remove me by deleting my dll (PluginTest.dll)";

        // If the "Open" option doesn't exist yet, create it
        if (open.Count == 0)
        {
          ToolStripMenuItem newOpen = new ToolStripMenuItem("Open");
          newOpen.Name = "openToolStripMenuItem";
          open.Add(newOpen);

          file.DropDownItems.Add(newOpen);
        }

        // If it exists, just add the new button and register the click event
        ToolStripMenuItem entry = new ToolStripMenuItem(text);
        entry.Click += new EventHandler(entry_Click);
        ((ToolStripMenuItem)open[0]).DropDownItems.Add(entry);

        // When initialization is done display a success message
        if (showMessageWhenSuccesfullyLoaded)
        {
          host.ShowMessage("PluginTest successfully initialized!");
        }
      }
      catch (Exception ex)
      {
        host.ShowMessage(ex.Message);
      }
    }

    // When the plugin menu entry is clicked
    void entry_Click(object sender, EventArgs e)
    {
      host.ShowMessage("You clicked me!");
    }
  }
}
