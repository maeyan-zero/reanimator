using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Config = Revival.Common.Config;
using Hellgate;

namespace Reanimator.Forms.HeroEditorFunctions
{
    public class AtlasImageLoader
    {
        private readonly List<String> _loadedAtlas;
        private readonly Dictionary<String, Bitmap> _iconDictionary;

        public AtlasImageLoader()
        {
            _loadedAtlas = new List<String>();
            _iconDictionary = new Dictionary<String, Bitmap>();
        }

        public int ImageCount
        {
            get { return _iconDictionary.Count(); }
        }

        public void LoadAtlas(String filePath)
        {
            if (_loadedAtlas.Contains(filePath)) return;

            Atlas atlas = XmlUtilities<Atlas>.Deserialize(filePath);
            if (atlas.file == null) return;

            _loadedAtlas.Add(filePath);
            IEnumerable<GameIcon> icons = _LoadImagesFromLocalFiles(atlas);

            foreach (GameIcon icon in icons)
            {
                if (_iconDictionary.ContainsKey(icon.IconName))
                {
                    icon.IconName += "_1";
                }

                _iconDictionary.Add(icon.IconName, icon.Bitmap);
            }
        }

        public void LoadAtlas(String filePath, FileManager fileManager)
        {
            if (_loadedAtlas.Contains(filePath)) return;

            fileManager.BeginAllDatReadAccess();
            byte[] xmlFile = fileManager.GetFileBytes(filePath);

            Atlas atlas;
            using (Stream stream = new MemoryStream(xmlFile))
            {
                atlas = XmlUtilities<Atlas>.Deserialize(stream);
                stream.Close();
            }
            if (atlas.file == null) return;

            _loadedAtlas.Add(filePath);
            IEnumerable<GameIcon> icons = _LoadImagesFromGameFiles(atlas, fileManager);

            foreach (GameIcon icon in icons)
            {
                if (_iconDictionary.ContainsKey(icon.IconName))
                {
                    icon.IconName += "_1";
                }

                _iconDictionary.Add(icon.IconName, icon.Bitmap);
            }

            fileManager.EndAllDatAccess();
        }

        public static Bitmap TextureFromGameFile(String path, FileManager fileManager)
        {
            byte[] imageFile = fileManager.GetFileBytes(path);
            Stream stream = new MemoryStream(imageFile);
            FreeImageAPI.FreeImageBitmap bmp = new FreeImageAPI.FreeImageBitmap(stream);
            stream.Close();

            Bitmap image = bmp.ToBitmap();
            bmp.Dispose();

            return image;
        }

        private IEnumerable<GameIcon> _LoadImagesFromLocalFiles(Atlas atlas)
        {
            if (String.IsNullOrEmpty(atlas.file)) return null;

            String path = Path.Combine(Config.HglDir, atlas.file);
            Bitmap bitmap = new Bitmap(path);

            return LoadImages(atlas, bitmap);
        }

        private IEnumerable<GameIcon> _LoadImagesFromGameFiles(Atlas atlas, FileManager fileManager)
        {
            if (String.IsNullOrEmpty(atlas.file)) return null;

            String extension = Path.GetExtension(atlas.file);
            Debug.Assert(extension != null);

            String path = atlas.file.Replace(extension, String.Empty) + ".dds";

            Bitmap bitmap = TextureFromGameFile(path, fileManager);

            return LoadImages(atlas, bitmap);
        }

        private IEnumerable<GameIcon> LoadImages(Atlas atlas, Bitmap bitmap)
        {
            List<GameIcon> icons = new List<GameIcon>();

            foreach (Frame frame in atlas.frames)
            {
                if (frame.w == 0 || frame.h == 0) continue;

                Bitmap bmp = new Bitmap((int)frame.w, (int)frame.h);
                Graphics g = Graphics.FromImage(bmp);

                Rectangle toDraw = new Rectangle(0, 0, (int)frame.w, (int)frame.h);

                g.Clear(Color.Transparent);
                g.DrawImage(bitmap, toDraw, frame.x, frame.y, frame.w, frame.h, GraphicsUnit.Pixel);
                g.Dispose();

                GameIcon icon = new GameIcon(frame.name, bmp);
                icons.Add(icon);
            }

            bitmap.Dispose();

            return icons.ToArray();
        }

        public Bitmap GetImage(String name)
        {
            if (_iconDictionary.ContainsKey(name))
            {
                Bitmap bmp = _iconDictionary[name];

                return bmp;
            }
            return null;
        }

        public void ClearImageList()
        {
            _loadedAtlas.Clear();
            _iconDictionary.Clear();
        }

        public void SaveImagesToFolder(String folder)
        {
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                List<String> keys = new List<String>(_iconDictionary.Keys);

                foreach (String key in keys)
                {
                    _iconDictionary[key].Save(Path.Combine(folder, key + ".bmp"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<String> GetImageNames()
        {
            List<String> keys = new List<String>(_iconDictionary.Keys);

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
