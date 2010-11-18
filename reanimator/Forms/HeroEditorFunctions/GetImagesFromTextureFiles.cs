using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace Reanimator.Forms.HeroEditorFunctions
{
    public class AtlasImageLoader
    {
        List<string> _loadedAtlas;
        GameIconHandler _images;

        public AtlasImageLoader()
        {
            _images = new GameIconHandler();
            _loadedAtlas = new List<string>();
        }

        public void LoadAtlas(string filePath)
        {
            try
            {
                if (!_loadedAtlas.Contains(filePath))
                {
                    _loadedAtlas.Add(filePath);
                    _images.Icons.AddRange(LoadImages(filePath));
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private GameIcon[] LoadImages(string xmlPath)
        {
            try
            {
                Atlas atlas = XmlUtilities<Atlas>.Deserialize(xmlPath);

                string path = Path.Combine(Config.HglDir, atlas.file);
                Bitmap bitmap = new Bitmap(path);
                List<GameIcon> icons = new List<GameIcon>();

                foreach (Frame frame in atlas.frames)
                {
                    if (frame.w != 0 && frame.h != 0)
                    {
                        Bitmap bmp = new Bitmap((int)frame.w, (int)frame.h);
                        Graphics g = Graphics.FromImage(bmp);

                        Rectangle toDraw = new Rectangle(0, 0, (int)frame.w, (int)frame.h);

                        g.Clear(Color.Transparent);
                        g.DrawImage(bitmap, toDraw, frame.x, frame.y, frame.w, frame.h, GraphicsUnit.Pixel);
                        g.Dispose();

                        GameIcon icon = new GameIcon(bmp, frame.name);
                        icons.Add(icon);

                        //bmp.Save(@"F:\icons\" + frame.name + ".bmp");
                    }
                }

                return icons.ToArray();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public Bitmap GetImage(string name)
        {

            Bitmap bmp = _images.GetIconByName(name);

            return bmp;
        }

        public void ClearImageList()
        {
            _loadedAtlas.Clear();
            _images.Icons.Clear();
        }
    }

    //public class SkillButton : Button
    //{
    //    Bitmap _normal;
    //    Bitmap _hover;
    //    Bitmap _clicked;

    //    public Bitmap ButtonNormal
    //    {
    //        get { return _normal; }
    //        set
    //        {
    //            _normal = value;
    //            this.BackgroundImage = _normal;
    //            this.Size = _normal.Size;
    //        }
    //    }

    //    public Bitmap ButtonHover
    //    {
    //        get { return _hover; }
    //        set { _hover = value; }
    //    }

    //    public Bitmap ButtonClicked
    //    {
    //        get { return _clicked; }
    //        set { _clicked = value; }
    //    }

    //    public SkillButton()
    //        : base()
    //    {
    //        this.MouseLeave += new EventHandler(SkillButton_MouseLeave);
    //        this.MouseEnter += new EventHandler(SkillButton_MouseEnter);
    //        this.MouseDown += new MouseEventHandler(SkillButton_MouseDown);
    //        this.MouseUp += new MouseEventHandler(SkillButton_MouseUp);

    //        this.BackgroundImageLayout = ImageLayout.Stretch;
    //        this.FlatStyle = FlatStyle.Flat;
    //        this.FlatAppearance.BorderSize = 0;
    //        this.FlatAppearance.CheckedBackColor = Color.Black;
    //        this.FlatAppearance.MouseDownBackColor = Color.Black;
    //        this.FlatAppearance.MouseOverBackColor = Color.Black;
    //    }

    //    public void SetButtonImages(Bitmap normal, Bitmap hover, Bitmap clicked)
    //    {
    //        _normal = normal;
    //        _hover = hover;
    //        _clicked = clicked;

    //        this.BackgroundImage = _normal;
    //        this.Size = normal.Size;
    //    }

    //    void SkillButton_MouseLeave(object sender, EventArgs e)
    //    {
    //        if (_normal != null)
    //        {
    //            this.BackgroundImage = _normal;
    //        }
    //    }

    //    void SkillButton_MouseUp(object sender, MouseEventArgs e)
    //    {
    //        if (_hover != null)
    //        {
    //            this.BackgroundImage = _hover;
    //        }
    //    }

    //    void SkillButton_MouseDown(object sender, MouseEventArgs e)
    //    {
    //        if (_clicked != null)
    //        {
    //            this.BackgroundImage = _clicked;
    //        }
    //    }

    //    void SkillButton_MouseEnter(object sender, EventArgs e)
    //    {
    //        if (_hover != null)
    //        {
    //            this.BackgroundImage = _hover;
    //        }
    //    }
    //}
}
