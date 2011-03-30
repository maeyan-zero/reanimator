using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Reanimator.Forms.HeroEditorFunctions
{
//<file>data\uix\skilltree_atlas.png</file>
//<widthbasis>1600</widthbasis>
//<heightbasis>1200</heightbasis>
//<ws_widthbasis>1920</ws_widthbasis>
//<chunksize>0</chunksize>
//<renderpriority>2</renderpriority>

    [XmlRoot("atlas")]
    public class Atlas
    {
        public string file;
        public int widthbasis;
        public int heightbasis;
        public int ws_widthbasis;
        public int chunksize;
        public int renderpriority;

        [XmlElement("frame")]
        public Frame[] frames;
    }

    public class Frame
    {
        public string name;
        public float x;
        public float y;
        public float w;
        public float h;
        public float xoffs;
        public float yoffs;
        public int hasmask;
        public int red;
        public int green;
        public int blue;
        public int alpha;
    }
}
