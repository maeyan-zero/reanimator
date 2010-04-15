using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Reanimator.Forms.HeroEditorFunctions
{
    public class ImageFromTextureHandler
    {
        static Atlas _skillPanel;
        static Bitmap _skillPanelBmp;
        static Atlas _skillTree;
        static Bitmap _skillTreeBmp;
        static Atlas _inventoryPanel;
        static Bitmap _inventoryPanelBmp;

        public ImageFromTextureHandler()
        {
            try
            {
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
                string currentDir = Directory.GetCurrentDirectory();
                if (files.Contains(currentDir + @"\skillpanel_atlas.xml") && files.Contains(currentDir + @"\skilltree_atlas.xml") && files.Contains(currentDir + @"\wsinventory_atlas.xml") &&
                    files.Contains(currentDir + @"\skillpanel_atlas.png") && files.Contains(currentDir + @"\skilltree_atlas.png") && files.Contains(currentDir + @"\wsinventory_atlas.png"))
                {
                    _skillPanel = XmlUtilities<Atlas>.Deserialize("skillpanel_atlas.xml");
                    _skillTree = XmlUtilities<Atlas>.Deserialize("skilltree_atlas.xml");
                    _inventoryPanel = XmlUtilities<Atlas>.Deserialize("wsinventory_atlas.xml");

                    _skillPanelBmp = new Bitmap(_skillPanel.file.Replace(@"data\uix\", string.Empty));
                    _skillPanelBmp.MakeTransparent(Color.White);

                    _skillTreeBmp = new Bitmap(_skillTree.file.Replace(@"data\uix\", string.Empty));
                    //_skillTreeBmp.MakeTransparent(Color.White);

                    _inventoryPanelBmp = new Bitmap(_inventoryPanel.file.Replace(@"data\uix\", string.Empty));
                    _inventoryPanelBmp.MakeTransparent(Color.White);
                }
                else
                {
                    MessageBox.Show("Make sure the following files are in the same directory as the Reanimator.exe (" + currentDir + "):\n" +
                    "\t- skillpanel_atlas.xml\n" +
                    "\t- skillpanel_atlas.png\n" +
                    "\t- skilltree_atlas.xml\n" +
                    "\t- skilltree_atlas.png\n",
                    //"\t- wsinventory_atlas.xml\n" +
                    //"\t- wsinventory_atlas.png\n",
                    "Files not found!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SkillPanel");
            }
        }

        public Bitmap GetImageFromSkillPanel(string imageName)
        {
            foreach(Frame frame in _skillPanel.frames)
            {
                if(frame.name == imageName)
                {
                    Bitmap bmp = new Bitmap((int)frame.w, (int)frame.h);
                    Graphics g = Graphics.FromImage(bmp);

                    Rectangle toDraw = new Rectangle(0, 0, (int)frame.w, (int)frame.h);

                    g.Clear(Color.Transparent);
                    g.DrawImage(_skillPanelBmp, toDraw, frame.x, frame.y, frame.w, frame.h, GraphicsUnit.Pixel);
                    g.Dispose();
                    return bmp;
                }
            }
            return null;
        }

        public Bitmap GetImageFromSkillTree(string imageName)
        {
            foreach (Frame frame in _skillTree.frames)
            {
                if(imageName == "Penetrating Shot Icon")
                {
                    imageName = "Homing Shot Icon";
                }
                if (frame.name == imageName)
                {
                    Bitmap bmp = new Bitmap((int)frame.w, (int)frame.h);
                    Graphics g = Graphics.FromImage(bmp);

                    Rectangle toDraw = new Rectangle(0, 0, (int)frame.w, (int)frame.h);

                    g.Clear(Color.Transparent);
                    g.DrawImage(_skillTreeBmp, toDraw, frame.x, frame.y, frame.w, frame.h, GraphicsUnit.Pixel);
                    g.Dispose();

                    return bmp;
                }
            }
            MessageBox.Show("No matching image found with name " + imageName + "!");
            return new Bitmap(64, 64); ;
        }

        public Bitmap GetImageFromInventoryPanel(string imageName)
        {
            foreach (Frame frame in _inventoryPanel.frames)
            {
                if (frame.name == imageName)
                {
                    Bitmap bmp = new Bitmap((int)frame.w, (int)frame.h);
                    Graphics g = Graphics.FromImage(bmp);

                    Rectangle toDraw = new Rectangle(0, 0, (int)frame.w, (int)frame.h);

                    g.Clear(Color.Transparent);
                    g.DrawImage(_inventoryPanelBmp, toDraw, frame.x, frame.y, frame.w, frame.h, GraphicsUnit.Pixel);
                    g.Dispose();
                    return bmp;
                }
            }
            return null;
        }
    }

    public class SkillButton : Button
    {
        Bitmap _normal;
        Bitmap _hover;
        Bitmap _clicked;

        public Bitmap ButtonNormal
        {
            get { return _normal; }
            set
            {
                _normal = value;
                this.BackgroundImage = _normal;
                this.Size = _normal.Size;
            }
        }

        public Bitmap ButtonHover
        {
            get { return _hover; }
            set { _hover = value; }
        }

        public Bitmap ButtonClicked
        {
            get { return _clicked; }
            set { _clicked = value; }
        }

        public SkillButton()
            : base()
        {
            this.MouseLeave += new EventHandler(SkillButton_MouseLeave);
            this.MouseEnter += new EventHandler(SkillButton_MouseEnter);
            this.MouseDown += new MouseEventHandler(SkillButton_MouseDown);
            this.MouseUp += new MouseEventHandler(SkillButton_MouseUp);

            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.CheckedBackColor = Color.Black;
            this.FlatAppearance.MouseDownBackColor = Color.Black;
            this.FlatAppearance.MouseOverBackColor = Color.Black;
        }

        public void SetButtonImages(Bitmap normal, Bitmap hover, Bitmap clicked)
        {
            _normal = normal;
            _hover = hover;
            _clicked = clicked;

            this.BackgroundImage = _normal;
            this.Size = normal.Size;
        }

        void SkillButton_MouseLeave(object sender, EventArgs e)
        {
            if (_normal != null)
            {
                this.BackgroundImage = _normal;
            }
        }

        void SkillButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (_hover != null)
            {
                this.BackgroundImage = _hover;
            }
        }

        void SkillButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (_clicked != null)
            {
                this.BackgroundImage = _clicked;
            }
        }

        void SkillButton_MouseEnter(object sender, EventArgs e)
        {
            if (_hover != null)
            {
                this.BackgroundImage = _hover;
            }
        }
    }
}
