using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Reanimator;

namespace launcher
{
    public partial class Hardcore : Form
    {
        List<String> _paths;
        List<Unit> _characters;

        readonly string _characterFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Hellgate\\Save\\Singleplayer";

        public Hardcore()
        {
            InitializeComponent();
        }

        private void Hardcore_Load(object sender, EventArgs e)
        {
            _paths = new List<String>();
            _characters = new List<Unit>();
            _paths.AddRange(Directory.GetFiles(_characterFolder, "*.hg1"));

            foreach (string path in _paths)
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    BitBuffer bitBuffer = new BitBuffer(FileTools.StreamToByteArray(stream)) {DataByteOffset = 0x2028};

                    Unit unit = new Unit(bitBuffer);

                    _characters.Add(unit);

                    String listString = "[" + (Reanimator.Forms.UnitHelpFunctions.GetSimpleValue(unit, Reanimator.Forms.ItemValueNames.level.ToString()) - 8) + "] ";
                    listString += unit.ToString();
                    characterListBox.Items.Add(listString);
                }
            }
        }
    }
}
