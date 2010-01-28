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
  public class PlugIn : GenericPlugin
  {
    public PlugIn()
    {
      name = "Client Patcher";
      description = "Adds an option to patch the SP client to support \"Hardcore\" mode.";
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

          ToolStripMenuItem clientPatcher = new ToolStripMenuItem("Client Patcher...");
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
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "EXE Files (*.exe)|*.exe|All Files (*.*)|*.*";
      openFileDialog.InitialDirectory = host.GetHGLDirectory() + "\\SP_x64";
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
        this.host.ShowMessage("Hardcore patch applied!");
      }
      else
      {
        this.host.ShowMessage("Failed to apply Hardcore patch!", "Error");
      }
      clientFile.Dispose();
    }
  }
}
