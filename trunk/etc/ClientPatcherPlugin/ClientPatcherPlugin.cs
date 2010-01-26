using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginInterface;
using System.Windows.Forms;
using System.IO;
using Reanimator;

namespace ClientPatcherPlugin
{
  public class PlugIn : IPlugin
  {
    private string name;
    private IPluginHost host;
    private MenuStrip hostMenu;
    private string hglDirectory;

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
        return this.host;
      }
      set
      {
        this.host = value;
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

    public void InitializePlugIn()
    {
      try
      {
        ToolStripItem[] items = hostMenu.Items.Find("toolsMenu", true);
        ToolStripMenuItem toolsMenu;

        if (items.Length != 0)
        {
          toolsMenu = (ToolStripMenuItem)items[0];

          ToolStripMenuItem clientPatcher = new ToolStripMenuItem("Client Patcher...");
          clientPatcher.Click += new EventHandler(clientPatcher_Click);

          toolsMenu.DropDownItems.Add(clientPatcher);
          host.ShowMessage("ClientPatcherPlugin.dll loaded!");
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
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "EXE Files (*.exe)|*.exe|All Files (*.*)|*.*";
      openFileDialog.InitialDirectory = hglDirectory + "\\SP_x64";
      if (openFileDialog.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      FileStream clientFile;
      try
      {
        clientFile = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.ReadWrite);
      }
      catch (Exception)
      {
        return;
      }

      ClientPatcher clientPatcher = new ClientPatcher(FileTools.StreamToByteArray(clientFile));
      if (clientPatcher.ApplyHardcorePatch())
      {
        FileStream fileOut = new FileStream(openFileDialog.FileName + ".patched.exe", FileMode.Create);
        fileOut.Write(clientPatcher.Buffer, 0, clientPatcher.Buffer.Length);
        fileOut.Dispose();
        MessageBox.Show("Hardcore patch applied!");
      }
      else
      {
        MessageBox.Show("Failed to apply Hardcore patch!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      clientFile.Dispose();
    }
  }
}
