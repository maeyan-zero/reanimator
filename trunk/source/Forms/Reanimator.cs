using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Reanimator.Forms;
using Reanimator.Excel;
using System.Threading;
using System.Runtime.InteropServices;
using PluginInterface;
using System.Reflection;

namespace Reanimator
{
    public partial class Reanimator : Form, IPluginHost
    {
      #region PLUGINS



      bool showInitializationMessage = true;
      private List<IPlugin> pluginList;

      public bool Register(IPlugin ipi)
      {
        pluginList.Add(ipi);
        return true;
      }

      public void ShowMessage(string message)
      {
        MessageBox.Show(message);
      }

      public void LoadPlugins()
      {
        string path = Application.StartupPath + @"\Plugins\";

        if (!Directory.Exists(path))
        {
          Directory.CreateDirectory(path);
        }
        string[] pluginFiles = Directory.GetFiles(path, "*.dll");

        for (int i = 0; i < pluginFiles.Length; i++)
        {
          string args = pluginFiles[i].Substring(
            pluginFiles[i].LastIndexOf("\\") + 1,
            pluginFiles[i].IndexOf(".dll") -
            pluginFiles[i].LastIndexOf("\\") - 1);

          Type ObjType = null;
          // load the dll
          try
          {
            // load it
            Assembly ass = null;
            ass = Assembly.LoadFile(pluginFiles[i]);
            if (ass != null)
            {
              ObjType = ass.GetType(args + ".PlugIn");
            }
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.Message);
          }
          try
          {
            // OK Lets create the object as we have the Report Type
            if (ObjType != null)
            {
              IPlugin plugin = (IPlugin)Activator.CreateInstance(ObjType);
              plugin.Host = this;
              plugin.HostMenu = this.menuStrip;
              plugin.HGLDirectory = Config.hglDir;
              plugin.InitializePlugIn(true);
            }
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.Message);
          }
        }
      }

      public IPlugin[] GetPluginList()
      {
        return pluginList.ToArray();
      }




      #endregion

        private Options options;
        private List<string> indexFilesOpen;
        private ExcelTables excelTables;

        private int childFormNumber = 0;
  
        public Reanimator()
        {
            options = new Options();
            indexFilesOpen = new List<string>();

            #region PLUGIN
            pluginList = new List<IPlugin>();
            #endregion

            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "HGL Files (*.idx, *.hg1, *.cooked)|*.idx;*.hg1;*.cooked|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                if (openFileDialog.FileName.EndsWith("idx"))
                {
                    OpenFile_IDX(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("hg1"))
                {
                    OpenFile_HG1(openFileDialog.FileName);
                }
                else if (openFileDialog.FileName.EndsWith("cooked"))
                {
                    OpenFile_COOKED(openFileDialog.FileName);
                }
            }

        }

        private void OpenIndexFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Index Files (*.idx)|*.idx|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Config.hglDir + "\\Data";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("idx"))
            {
                OpenFile_IDX(openFileDialog.FileName);
            }
        }

        private void OpenCharacterFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Character Files (*.hg1)|*.hg1|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Hellgate\\Save\\Singleplayer";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("hg1"))
            {
                OpenFile_HG1(openFileDialog.FileName);
            }
        }

        private void OpenCookedFile(object sender, EventArgs e)
        {
            MessageBox.Show("Testing function! Must open exceltablex.txt.cooked!\n I'd recommend extracting all dat files and keeping the dir structure for it to work correctly (this will be fixed in time).", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Cooked Files (*.cooked)|*.cooked|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Config.dataDirsRoot;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && openFileDialog.FileName.EndsWith("cooked"))
            {
                OpenFile_COOKED(openFileDialog.FileName);
            }
        }

        private bool OpenFile_IDX(string szFileName)
        {
            if (indexFilesOpen.Contains(szFileName))
            {
                return false;
            }

            FileStream indexFile;
            try
            {
                indexFile = new FileStream(szFileName, FileMode.Open);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to open file: " + szFileName + "\n\n" + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            Index index = new Index(FileTools.StreamToByteArray(indexFile));
            IndexExplorer indexExplorer = new IndexExplorer(indexFile, index);
            indexExplorer.dataGridView.DataSource = index.GetFileTable();
            indexExplorer.Text += ": " + szFileName;
            indexExplorer.MdiParent = this;
            indexExplorer.Show();
            indexExplorer.FormClosed += new FormClosedEventHandler(indexExplorer_FormClosed);

            indexFilesOpen.Add(indexFile.Name);

            return true;
        }

        private bool OpenFile_HG1(string szFileName)
        {
            if (excelTables == null)
            {
                MessageBox.Show("You must open the exceltable.txt.cooked file before viewing a character (dirty test implementation requirement)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            FileStream heroFile;
            try
            {
                heroFile = new FileStream(szFileName, FileMode.Open);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to open file: " + szFileName + "\n\n" + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            BitBuffer bitBuffer = new BitBuffer(FileTools.StreamToByteArray(heroFile));
            bitBuffer.DataByteOffset = 0x2028;

            Unit heroUnit = new Unit(bitBuffer);
            heroUnit.ReadUnit(ref heroUnit);

            HeroEditor heroEditor = new HeroEditor(heroUnit, excelTables, szFileName);
            heroEditor.Text = "Hero Editor: " + szFileName;
            heroEditor.MdiParent = this;
            heroEditor.Show();

            return true;
        }

        private void OpenFile_COOKED(string szFileName)
        {
            MessageBox.Show("Todo");
        }

        private void indexExplorer_FormClosed(object sender, FormClosedEventArgs e)
        {
            IndexExplorer indexExplorer = (IndexExplorer)sender;
            indexFilesOpen.Remove(indexExplorer.IndexFile.Name);
            indexExplorer.IndexFile.Dispose();
            indexExplorer.Dispose();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            options.ShowDialog(this);

            //update the Plugins with the new HGL path
            foreach (IPlugin plugin in pluginList)
            {
              plugin.HGLDirectory = Config.dataDirsRoot;
            }

            this.BringToFront();
        }

        #region CONVERTED TO PLUGIN
        //private void clientPatcherToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "EXE Files (*.exe)|*.exe|All Files (*.*)|*.*";
        //    openFileDialog.InitialDirectory = Config.hglDir + "\\SP_x64";
        //    if (openFileDialog.ShowDialog(this) != DialogResult.OK)
        //    {
        //        return;
        //    }

        //    FileStream clientFile;
        //    try
        //    {
        //        clientFile = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.ReadWrite);
        //    }
        //    catch (Exception)
        //    {
        //        return;
        //    }

        //    ClientPatcher clientPatcher = new ClientPatcher(FileTools.StreamToByteArray(clientFile));
        //    if (clientPatcher.ApplyHardcorePatch())
        //    {
        //        FileStream fileOut = new FileStream(openFileDialog.FileName + ".patched.exe", FileMode.Create);
        //        fileOut.Write(clientPatcher.Buffer, 0, clientPatcher.Buffer.Length);
        //        fileOut.Dispose();
        //        MessageBox.Show("Hardcore patch applied!");
        //    }
        //    else
        //    {
        //        MessageBox.Show("Failed to apply Hardcore patch!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    clientFile.Dispose();
        //}
        #endregion

        private void LoadExcelTables(object sender, EventArgs e)
        {
            Progress progress = (Progress)sender;
            FileStream excelFile;

            string excelFilePath = Config.dataDirsRoot + "\\data_common\\excel\\exceltables.txt.cooked";
            try
            {
                excelFile = new FileStream(excelFilePath, FileMode.Open);
                excelTables = new ExcelTables(FileTools.StreamToByteArray(excelFile));
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load exceltables!\nPlease ensure your directories are set correctly.\n\nFile: \n" + excelFilePath, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                progress.Close();
                progress.Dispose();
                return;
            }

            progress.ConfigBar(0, excelTables.Count, 1);
            progress.SetLoadingText("Loading in excel tables (" + excelTables.Count + ")...");
            excelTables.LoadTables(Config.dataDirsRoot + "\\data_common\\excel\\", progress);
            progress.Dispose();
        }

        // this fixes a weird windows API bug causing the ShowDialog to minimise the main client
        private void Reanimator_MouseClick(object sender, MouseEventArgs e)
        {
            this.Focus();
        }

        private void Reanimator_ResizeEnd(object sender, EventArgs e)
        {
            Config.clientHeight = this.Height;
            Config.clientWidth = this.Width;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct Header
        {
            public Int32 header;
            public Int32 unknown1;
            public Int32 count;
            public Int32 unknown2;
        }

        struct StringBlock
        {
            public int unknown1;
            public string stringId;
            public int unknown2;
            public string str;
            public int unknown3;
            public string language;
            public string usage;
            public string defaultString;
            public int blockIndex;
        }


        private void Reanimator_Load(object sender, EventArgs e)
        {
            /*
            FileStream test = new FileStream("strings_strings.xls.uni.cooked", FileMode.Open);
            byte[] bytes = FileTools.StreamToByteArray(test);

            int offset = 0;
            Header header = (Header)FileTools.ByteArrayToStructure(bytes, typeof(Header), 0);
            offset += Marshal.SizeOf(header);

            List<StringBlock> stringBlocks = new List<StringBlock>();
            for (int i = 0; i < header.count; i++)
            {
                StringBlock stringBlock;

                stringBlock.unknown1 = FileTools.ByteArrayToInt32(bytes, offset);
                offset += sizeof(Int32);

                int count = FileTools.ByteArrayToInt32(bytes, offset);
                offset += sizeof(Int32);
                stringBlock.stringId = FileTools.ByteArrayToStringAnsi(bytes, offset);
                offset += count+1;

                stringBlock.unknown2 = FileTools.ByteArrayToInt32(bytes, offset);
                offset += sizeof(Int32);

                count = FileTools.ByteArrayToInt32(bytes, offset);
                offset += sizeof(Int32);
                stringBlock.str = FileTools.ByteArrayToStringUnicode(bytes, offset);
                offset += count;

                if (offset >= 0x152A0)
                {
                    int breakpoint = 1;
                }

                stringBlock.unknown3 = FileTools.ByteArrayToInt32(bytes, offset);
                offset += sizeof(Int32);

                count = FileTools.ByteArrayToInt32(bytes, offset);
                offset += sizeof(Int32);
                stringBlock.language = FileTools.ByteArrayToStringUnicode(bytes, offset);
                offset += (count + 1) * 2;

                count = FileTools.ByteArrayToInt32(bytes, offset);
                offset += sizeof(Int32);
                stringBlock.usage = FileTools.ByteArrayToStringUnicode(bytes, offset);
                offset += (count + 1) * 2;

                if (stringBlock.usage == "Singular")
                {
                    count = FileTools.ByteArrayToInt32(bytes, offset);
                    offset += sizeof(Int32);
                    stringBlock.defaultString = FileTools.ByteArrayToStringUnicode(bytes, offset);
                    offset += (count + 1) * 2;
                }

                stringBlock.blockIndex = FileTools.ByteArrayToInt32(bytes, offset);
                offset += sizeof(Int32);
            }*/


            this.Height = Config.clientHeight;
            this.Width = Config.clientWidth;
            this.Show();
            this.Refresh();
            // this fixes a weird windows API bug causing the ShowDialog to minimise the main client
            this.OnMouseClick(new MouseEventArgs(MouseButtons.Left, 1, this.Left+10, this.Top+10, 0));

            Progress progress = new Progress();
            progress.Shown += new EventHandler(LoadExcelTables);
            progress.ShowDialog(this);

            #region PLUGIN
            try
            {
              LoadPlugins();
            }
            catch (Exception ex)
            {
              MessageBox.Show(ex.Message);
            }
            #endregion


        }
    }
}