using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Reanimator.HeroEditorFunctions
{
    public class SkillButton : Button
    {
        Bitmap _normal;
        Bitmap _hover;
        Bitmap _clicked;

        public Bitmap ButtonNormal
        {
            get { return _normal; }
            set { _normal = value; }
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

    public class SkillPanel
    {
        static Atlas _skillPanel;
        static Bitmap _skillPanelBmp;
        static Atlas _skillTree;
        static Bitmap _skillTreeBmp;

        static SkillPanel()
        {
            try
            {
                _skillPanel = XmlUtilities<Atlas>.Deserialize("skillpanel_atlas.xml");
                _skillTree = XmlUtilities<Atlas>.Deserialize("skilltree_atlas.xml");
                _skillPanelBmp = new Bitmap(_skillPanel.file.Replace(@"data\uix\", string.Empty));
                _skillPanelBmp.MakeTransparent(Color.White);
                _skillTreeBmp = new Bitmap(_skillTree.file.Replace(@"data\uix\", string.Empty));
                _skillTreeBmp.MakeTransparent(Color.White);
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

                    g.Clear(Color.Blue);
                    g.DrawImage(_skillTreeBmp, toDraw, frame.x, frame.y, frame.w, frame.h, GraphicsUnit.Pixel);
                    g.Dispose();

                    return bmp;
                }
            }
            MessageBox.Show("No matching image found with name " + imageName + "!");
            return new Bitmap(64, 64); ;
        }
    }
}
