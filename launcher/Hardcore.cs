using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Reanimator;

namespace launcher
{
    public partial class Hardcore : Form
    {
        List<string> paths;
        List<Unit> characters;

        readonly string character_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Hellgate\\Save\\Singleplayer";

        public Hardcore()
        {
            InitializeComponent();
        }

        private void Hardcore_Load(object sender, EventArgs e)
        {
            paths = new List<string>();
            characters = new List<Reanimator.Unit>();
            paths.AddRange(Directory.GetFiles(character_folder, "*.hg1"));

            foreach (string path in paths)
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    BitBuffer bit_buffer = new BitBuffer(FileTools.StreamToByteArray(stream));
                    bit_buffer.DataByteOffset = 0x2028;

                    Unit unit = new Unit(bit_buffer);
                    unit.ReadUnit(ref unit);

                    characters.Add(unit);

                    string list_string = "[" + (Reanimator.Forms.UnitHelpFunctions.GetSimpleValue(unit, Reanimator.Forms.ItemValueNames.level.ToString()) - 8) + "] ";
                    //unit.
                    list_string += unit.ToString();
                    characterListBox.Items.Add(list_string);
                }
            }
        }
    }
}
