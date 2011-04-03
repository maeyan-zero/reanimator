using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using Config = Revival.Common.Config;
using Hellgate;

namespace Reanimator.Forms.HeroEditorFunctions
{
    public class AtlasImageLoader
    {
        List<string> _loadedAtlas;
        Dictionary<string, Bitmap> _iconDictionary;

        public AtlasImageLoader()
        {
            _loadedAtlas = new List<string>();
            _iconDictionary = new Dictionary<string, Bitmap>();
        }

        public int ImageCount
        {
            get { return _iconDictionary.Count(); }
        }

        public void LoadAtlas(string filePath)
        {
            try
            {
                if (!_loadedAtlas.Contains(filePath))
                {
                    Atlas atlas = XmlUtilities<Atlas>.Deserialize(filePath);

                    if (atlas.file == null)
                    {
                        return;
                    }

                    _loadedAtlas.Add(filePath);
                    GameIcon[] icons = LoadImagesFromLocalFiles(atlas);

                    foreach(GameIcon icon in icons)
                    {
                        if (_iconDictionary.ContainsKey(icon.IconName))
                        {
                            icon.IconName += "_1";
                        }

                        _iconDictionary.Add(icon.IconName, icon.Bitmap);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void LoadAtlas(string filePath, FileManager fileManager)
        {
            try
            {
                if (!_loadedAtlas.Contains(filePath))
                {
                    fileManager.BeginAllDatReadAccess();

                    byte[] xmlFile = fileManager.GetFileBytes(filePath);
                    
                    Stream stream = new MemoryStream(xmlFile);
                    Atlas atlas = XmlUtilities<Atlas>.Deserialize(stream);
                    stream.Close();

                    if (atlas.file == null)
                    {
                        return;
                    }

                    _loadedAtlas.Add(filePath);
                    GameIcon[] icons = LoadImagesFromGameFiles(atlas, fileManager);

                    foreach (GameIcon icon in icons)
                    {
                        if(_iconDictionary.ContainsKey(icon.IconName))
                        {
                            icon.IconName += "_1";
                        }

                        _iconDictionary.Add(icon.IconName, icon.Bitmap);
                    }

                    fileManager.EndAllDatAccess();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static Bitmap TextureFromGameFile(string path, FileManager fileManager)
        {
            byte[] imageFile = fileManager.GetFileBytes(path);
            Stream stream = new MemoryStream(imageFile);
            FreeImageAPI.FreeImageBitmap bmp = new FreeImageAPI.FreeImageBitmap(stream);
            stream.Close();

            Bitmap image = bmp.ToBitmap();
            bmp.Dispose();

            return image;
        }

        private GameIcon[] LoadImagesFromLocalFiles(Atlas atlas)
        {
            if (atlas.file == null || atlas.file == string.Empty)
            {
                return null;
            }

            string path = Path.Combine(Config.HglDir, atlas.file);
            Bitmap bitmap = new Bitmap(path);

            return LoadImages(atlas, bitmap);
        }

        private GameIcon[] LoadImagesFromGameFiles(Atlas atlas, FileManager fileManager)
        {
            if (atlas.file == null || atlas.file == string.Empty)
            {
                return null;
            }

            string extension = Path.GetExtension(atlas.file);
            string path = atlas.file.Replace(extension, string.Empty) + ".dds";

            Bitmap bitmap = TextureFromGameFile(path, fileManager);

            return LoadImages(atlas, bitmap);
        }

        private GameIcon[] LoadImages(Atlas atlas, Bitmap bitmap)
        {
            try
            {
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

                        GameIcon icon = new GameIcon(frame.name, bmp);
                        icons.Add(icon);
                    }
                }

                bitmap.Dispose();

                return icons.ToArray();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public Bitmap GetImage(string name)
        {
            Bitmap bmp = _iconDictionary[name];

            return bmp;
        }

        public void ClearImageList()
        {
            _loadedAtlas.Clear();
            _iconDictionary.Clear();
        }

        public void SaveImagesToFolder(string folder)
        {
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                List<string> keys = new List<string>(_iconDictionary.Keys);

                foreach (string key in keys)
                {
                    _iconDictionary[key].Save(Path.Combine(folder, key + ".bmp"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<string> GetImageNames()
        {
            List<string> keys = new List<string>(_iconDictionary.Keys);

            return keys;
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
