using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reanimator.Forms.HeroEditorFunctions
{
    public partial class SingleSkillControl : UserControl
    {
        Skill _skill;

        public SingleSkillControl(Skill skill, AtlasImageLoader imageLoader, Point distanceToNeighbor)
        {
            InitializeComponent();

            _skill = skill;

            InitSkillPanel(imageLoader, distanceToNeighbor);
            SetButtonImages(imageLoader);
        }

        private void SetButtonImages(AtlasImageLoader imageLoader)
        {
            b_up.BackgroundImage = imageLoader.GetImage("button add");
            b_down.BackgroundImage = imageLoader.GetImage("button shift");
        }

        private void InitSkillPanel(AtlasImageLoader imageLoader, Point distanceToNeighbor)
        {
            this.toolTip1.ToolTipTitle = _skill.Name;
            this.toolTip1.SetToolTip(p_icon, _skill.Description);

            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = imageLoader.GetImage("icon panel big");

            p_icon.BackgroundImageLayout = ImageLayout.Stretch;
            p_icon.BackgroundImage = imageLoader.GetImage(_skill.IconName);

            this.Location = new Point(_skill.Position.X * distanceToNeighbor.X, _skill.Position.Y * distanceToNeighbor.Y);

            UpdateControls();
        }

        private void UpdateControls()
        {
            l_values.Text = _skill.CurrentLevel + "/" + _skill.MaxLevel;
            b_up.Visible = _skill.CurrentLevel != _skill.MaxLevel;
            b_down.Visible = _skill.CurrentLevel != 0;
        }

        private void b_up_Click(object sender, EventArgs e)
        {
            _skill.CurrentLevel++;
            UpdateControls();
        }

        private void b_down_Click(object sender, EventArgs e)
        {
            int level = _skill.CurrentLevel - 1;

            if (level < 0)
            {
                level = 0;
            }

            _skill.CurrentLevel = level;
            UpdateControls();
        }
    }

    public class GameIconHandler
    {
        List<GameIcon> _icon;

        public GameIconHandler()
        {
            _icon = new List<GameIcon>();
        }

        public List<GameIcon> Icons
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
            }
        }

        public Bitmap GetIconByName(string name)
        {
            try
            {
                if (_icon.Count > 0)
                {
                    GameIcon icon = _icon.Find(tmp => tmp.Name == name);
                    if(icon.Image != null)
                    {
                        return icon.Image;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }

    public class GameIcon
    {
        Bitmap _bitmap;
        string _iconName;

        public GameIcon(string path, string iconName)
        {
            _bitmap = new Bitmap(path);
            _iconName = iconName;
        }

        public GameIcon(Bitmap bmp, string iconName)
        {
            _bitmap = bmp;
            _iconName = iconName;
        }

        public Bitmap Image
        {
            get
            {
                return _bitmap;
            }
        }

        public string Name
        {
            get
            {
                return _iconName;
            }
        }
    }
}
