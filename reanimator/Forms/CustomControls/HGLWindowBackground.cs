using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Hellgate;
using Reanimator.Forms.HeroEditorFunctions;
using System.Windows.Forms;

namespace Reanimator.Forms.CustomControls
{
    class HGLWindowBackground
    {
        Image topLeft;
        Image topRight;

        Image bottomLeft;
        Image bottomRight;

        TextureBrush mainBrush;
        TextureBrush topBorderBrush;
        TextureBrush bottomBorderBrush;
        TextureBrush leftBorderBrush;
        TextureBrush rightBorderBrush;

        public HGLWindowBackground()
        {
        }

        public void SetBitmaps(FileManager fileManager, HGLWindowStyle windowStyle)
        {
            AtlasImageLoader loader;

            loader = new AtlasImageLoader();
            loader.LoadAtlas(@"data\uix\xml\main_atlas.xml", fileManager);

            List<Bitmap> imageList = new List<Bitmap>();

            imageList.Clear();
            imageList.Add(loader.GetImage(windowStyle + "_1_TL"));
            imageList.Add(loader.GetImage(windowStyle + "_2_TM"));
            imageList.Add(loader.GetImage(windowStyle + "_3_TR"));

            imageList.Add(loader.GetImage(windowStyle + "_4_ML"));
            imageList.Add(loader.GetImage(windowStyle + "_5_MM"));
            imageList.Add(loader.GetImage(windowStyle + "_6_MR"));

            imageList.Add(loader.GetImage(windowStyle + "_7_BL"));
            imageList.Add(loader.GetImage(windowStyle + "_8_BM"));
            imageList.Add(loader.GetImage(windowStyle + "_9_BR"));

            SetBitmaps(imageList);

            loader.ClearImageList();
            imageList.Clear();
        }

        public void SetBitmaps(List<Bitmap> bitmaps)
        {
            topLeft = bitmaps[0];
            topBorderBrush = new TextureBrush(bitmaps[1]);
            topRight = bitmaps[2];

            leftBorderBrush = new TextureBrush(bitmaps[3]);
            mainBrush = new TextureBrush(bitmaps[4]);
            rightBorderBrush = new TextureBrush(bitmaps[5]);

            bottomLeft = bitmaps[6];
            bottomBorderBrush = new TextureBrush(bitmaps[7]);
            bottomRight = bitmaps[8];
        }

        public Bitmap GenerateBackground(Size size)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bmp);

            GenerateBackground(g, size);

            g.Dispose();

            return bmp;
        }

        public void GenerateBackground(Control control)
        {
            Graphics g = control.CreateGraphics();
            Size size = control.ClientSize;

            GenerateBackground(g, size);

            g.Dispose();
        }

        private void GenerateBackground(Graphics g, Size size)
        {
            TextureBrush bottom = (TextureBrush)bottomBorderBrush.Clone();
            TextureBrush right = (TextureBrush)rightBorderBrush.Clone();
            bottom.TranslateTransform(0, size.Height - bottom.Image.Height);
            right.TranslateTransform(size.Width - right.Image.Width, 0);

            g.FillRegion(mainBrush, new Region(new Rectangle(topLeft.Width, topLeft.Height, size.Width - topLeft.Width - bottomRight.Width, size.Height - topLeft.Height - bottomRight.Height)));
            //e.Graphics.DrawLine(texturePenMain, this.Width / 2, 0, this.Width / 2, this.Height);

            //top border
            g.FillRectangle(topBorderBrush, new Rectangle(topLeft.Width, 0, size.Width - topLeft.Width - topRight.Width, topRight.Height));
            //bottom border
            g.FillRectangle(bottom, new Rectangle(bottomLeft.Width, size.Height - bottom.Image.Height, size.Width - bottomLeft.Width - bottomRight.Width, size.Height - bottom.Image.Height));

            //left border
            g.FillRectangle(leftBorderBrush, new Rectangle(0, topLeft.Height, leftBorderBrush.Image.Width, size.Height - topLeft.Height - bottomLeft.Height));
            //right border
            g.FillRectangle(right, new Rectangle(size.Width - right.Image.Width, topRight.Height, size.Width - right.Image.Width, size.Height - topRight.Height - bottomRight.Height));

            //upper left corner
            g.DrawImage(topLeft, 0, 0);
            //upper right corner
            g.DrawImage(topRight, size.Width - topRight.Width, 0);

            //lower left corner
            g.DrawImage(bottomLeft, 0, size.Height - bottomLeft.Height);
            //lower right corner
            g.DrawImage(bottomRight, size.Width - topRight.Width, size.Height - bottomLeft.Height);
        }
    }

    public enum HGLWindowStyle
    {
        genericbox,
        gentransbox,
        genhighlight,
        gendropshadowL,
        genglow
    }
}
