using System;
using PluginInterface;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PluginTest
{
  // A basic Plugin. The class name MUST be "PlugIn"!
  class PlugIn : GenericPlugin
  {
    public PlugIn()
    {
      name = "Test";
      description = "A small test plugin.";
    }

    public override void InitializePlugIn(bool showMessageWhenSuccesfullyLoaded)
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

        base.InitializePlugIn(showMessageWhenSuccesfullyLoaded);
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
